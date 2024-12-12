using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.BUFF;
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;
using System.Text.RegularExpressions;
using System.Web;

namespace Steam_Authenticator
{
    internal class Appsetting
    {
        private static Appsetting instance;

        public static Appsetting Instance => instance;

        static Appsetting()
        {
            instance = new Appsetting();
        }

        [JsonIgnore]
        public SettingManifest AppSetting { get; private set; } = new SettingManifest();

        [JsonIgnore]
        public List<UserClient> Clients { get; private set; } = new List<UserClient>();

        [JsonIgnore]
        public List<BuffClient> BuffClients { get; private set; } = new List<BuffClient>();

        [JsonIgnore]
        public AppManifest Manifest { get; private set; } = new AppManifest();
    }

    public class Domain
    {
        public string SteamCommunity { get; set; }
        public string SteamApi { get; set; }
        public string SteamPowered { get; set; }
        public string SteamLogin { get; set; }
    }

    public class HostProxy
    {
        public string Address { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }

    public class UserClient
    {
        public static UserClient None = new UserClient(new User(), new SteamCommunityClient());

        public readonly SemaphoreSlim LoginConfirmLocker = new SemaphoreSlim(1, 1);
        public readonly SemaphoreSlim ConfirmationPopupLocker = new SemaphoreSlim(1, 1);

        private Action startLogin = null;
        private Action<bool> endLogin = null;

        public UserClient(User user, SteamCommunityClient client)
        {
            User = user;
            Client = client;
        }

        public SteamCommunityClient Client { get; set; }

        public User User { get; set; }

        public UserClient WithStartLogin(Action action)
        {
            startLogin = action;
            return this;
        }

        public UserClient WithEndLogin(Action<bool> action)
        {
            endLogin = action;
            return this;
        }

        /// <summary>
        /// 获取账号名
        /// </summary>
        /// <returns></returns>
        public string GetAccount()
        {
            if (!string.IsNullOrWhiteSpace(Client?.Account))
            {
                return Client.Account;
            }

            return User?.Account;
        }

        public async Task<bool> LoginAsync()
        {
            bool result = false;
            try
            {
                startLogin?.Invoke();

                if (string.IsNullOrWhiteSpace(User?.RefreshToken))
                {
                    result = false;
                    return result;
                }

                result = await Client.LoginAsync(User.RefreshToken);
                if (string.IsNullOrWhiteSpace(User.Account) && Client.Account != User.Account)
                {
                    User.Account = Client.Account;
                }

                Appsetting.Instance.Manifest.SaveSteamUser(User.SteamId, User);
                return result;
            }
            finally
            {
                endLogin?.Invoke(result);
            }
        }

        public async Task LogoutAsync()
        {
            User.RefreshToken = string.Empty;
            Appsetting.Instance.Manifest.SaveSteamUser(User.SteamId, User);
            try
            {
                endLogin?.Invoke(false);
                await Client.LogoutAsync();
            }
            catch
            {
            }
        }
    }

    public class BuffClient
    {
        public static BuffClient None = new BuffClient(new BuffUser());

        private Action<bool> startLogin = null;
        private Action<bool, bool> endLogin = null;

        public BuffClient(BuffUser user)
        {
            User = user;
        }

        public BuffUser User { get; private set; }

        public CookieCollection Cookies => Extension.GetCookies(User?.BuffCookies ?? "");

        public bool LoggedIn { get; set; }

        public BuffClient WithStartLogin(Action<bool> action)
        {
            startLogin = action;
            return this;
        }

        public BuffClient WithEndLogin(Action<bool, bool> action)
        {
            endLogin = action;
            return this;
        }

        public async Task RefreshAsync(bool relogin, CancellationToken cancellationToken = default)
        {
            try
            {
                startLogin?.Invoke(relogin);

                var buffResponse = await BuffApi.QueryUserInfo(cookies: Cookies, cancellationToken);

                if (string.IsNullOrWhiteSpace(buffResponse.Body?.data?.id))
                {
                    LoggedIn = false;
                    return;
                }

                LoggedIn = true;

                var newCookies = this.Cookies;
                newCookies.Add(buffResponse.Cookies);

                var buffUser = buffResponse.Body.data;
                User.Nickname = buffUser.nickname;
                User.Avatar = buffUser.avatar;
                User.BuffCookies = string.Join("; ", newCookies.Select(cookie => $"{cookie.Name}={HttpUtility.UrlEncode(cookie.Value)}"));

                Appsetting.Instance.Manifest.SaveBuffUser(User.UserId, User);
            }
            finally
            {
                endLogin?.Invoke(relogin, LoggedIn);
            }
        }

        public Task LogoutAsync()
        {
            User.BuffCookies = string.Empty;
            Appsetting.Instance.Manifest.SaveBuffUser(User.UserId, User);
            LoggedIn = false;
            endLogin?.Invoke(false, false);
            return Task.CompletedTask;
        }

        public async Task<IWebResponse<BuffResponse<List<SteamTradeResponse>>>> QuerySteamTrade()
        {
            return await BuffApi.QuerySteamTrade(cookies: Cookies);
        }
    }

    /// <summary>
    /// 接受报价
    /// </summary>
    public class AcceptOfferRuleSetting
    {
        public Rule OfferMessage { get; set; } = new Rule
        {
            Enabled = true,
            Type = RuleType.报价消息,
            Condition = ConditionType.正则匹配,
            Value = @"^(\d+)T(\d+),令牌确认请留意对方Steam昵称，等级，账户年资，确保一致!!!$"
        };

        public Rule AssetName { get; set; } = new Rule
        {
            Type = RuleType.饰品名称
        };

        public class Rule
        {
            public RuleType Type { get; set; }

            public bool Enabled { get; set; } = false;

            public ConditionType Condition { get; set; } = ConditionType.包含;

            public string Value { get; set; } = string.Empty;

            public bool Check(string input)
            {
                input = input.Trim();
                if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(input))
                {
                    return false;
                }

                var vaules = Value.Split(Environment.NewLine).Select(c => c.Trim()).Where(c => !string.IsNullOrWhiteSpace(c));
                if (!vaules.Any())
                {
                    return false;
                }
                switch (Condition)
                {
                    case ConditionType.等于:
                        return vaules.Any(value => input.Equals(value, StringComparison.OrdinalIgnoreCase));
                    case ConditionType.不等于:
                        return vaules.All(value => !input.Equals(value, StringComparison.OrdinalIgnoreCase));
                    case ConditionType.包含:
                        return vaules.Any(value => input.Contains(value, StringComparison.OrdinalIgnoreCase));
                    case ConditionType.不包含:
                        return vaules.All(value => !input.Contains(value, StringComparison.OrdinalIgnoreCase));
                    case ConditionType.正则匹配:
                        return vaules.Any(value => Regex.IsMatch(input, value, RegexOptions.IgnoreCase));
                }

                return false;
            }
        }

        public enum RuleType
        {
            报价消息 = 1,
            饰品名称 = 2
        }

        public enum ConditionType
        {
            等于 = 101,
            不等于 = 102,

            包含 = 201,
            不包含 = 202,

            正则匹配 = 301
        }
    }
}

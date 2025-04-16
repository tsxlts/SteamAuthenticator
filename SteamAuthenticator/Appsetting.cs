using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.BUFF;
using Steam_Authenticator.Model.ECO;
using Steam_Authenticator.Model.YouPin898;
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;

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
        public List<EcoClient> EcoClients { get; private set; } = new List<EcoClient>();

        [JsonIgnore]
        public List<YouPinClient> YouPinClients { get; private set; } = new List<YouPinClient>();

        [JsonIgnore]
        public AppManifest Manifest { get; private set; } = new AppManifest();

        [JsonIgnore]
        public string DownloadDirectory
        {
            get
            {
                var path = Path.Combine(AppContext.BaseDirectory, "download");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        [JsonIgnore]
        public string Install => Path.Combine(AppContext.BaseDirectory, "setup", "Install.exe");

        [JsonIgnore]
        public string SetupApplication => Path.Combine(AppContext.BaseDirectory, "!setup", "Install.exe");

        [JsonIgnore]
        public int LastTipVersion => 1;
    }

    public class Domain
    {
        public string SteamCommunity { get; set; }
        public string SteamApi { get; set; }
        public string SteamPowered { get; set; }
        public string SteamHelp { get; set; }
        public string SteamLogin { get; set; }
    }

    public class HostProxy
    {
        public string Address { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }

    public interface Client
    {
        public string Key { get; }
    }

    public class UserClient : Client
    {
        public static UserClient None = new UserClient(new User(), new SteamCommunityClient());

        public readonly SemaphoreSlim LoginConfirmLocker = new SemaphoreSlim(1, 1);

        private List<Offer> receivedOffers = new List<Offer>();
        private List<Offer> autoConfirmOffers = new List<Offer>();
        private Action startLogin = null;
        private Action<bool> endLogin = null;

        public UserClient(User user, SteamCommunityClient client)
        {
            User = user;
            Client = client;
        }

        public SteamCommunityClient Client { get; set; }

        public User User { get; set; }

        public string Key => User?.SteamId;

        public List<Offer> AutoConfirmOffers => autoConfirmOffers ?? new List<Offer>();

        /// <summary>
        /// 收到的报价
        /// </summary>
        public List<Offer> ReceivedOffers => receivedOffers ?? new List<Offer>();

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

        public void SetAutoConfirmOffers(List<Offer> offers)
        {
            autoConfirmOffers = offers;
        }

        public void SetReceivedOffers(List<Offer> offers)
        {
            receivedOffers = offers;
        }

        /// <summary>
        /// 获取账号名
        /// </summary>
        /// <returns></returns>
        public string GetAccount()
        {
            return User?.Account;
        }

        public async ValueTask<string> GetAccountAsync()
        {
            if (User == null)
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(User.Account))
            {
                return User.Account;
            }

            string account = await RefreshAccountAsync();
            return account;
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
                if (string.IsNullOrWhiteSpace(User.Account))
                {
                    await RefreshAccountAsync();
                }

                return result;
            }
            finally
            {
                endLogin?.Invoke(result);
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                endLogin?.Invoke(false);
                await Client.LogoutAsync();

                User.RefreshToken = string.Empty;
                Appsetting.Instance.Manifest.SaveSteamUser(User.SteamId, User);
            }
            catch
            {
            }
        }

        public async Task<string> RefreshAccountAsync()
        {
            if (Client == null || !Client.LoggedIn)
            {
                return null;
            }

            string account = await Client.GetAccountNameAsync();
            if (string.IsNullOrWhiteSpace(account))
            {
                return null;
            }

            User.Account = account;
            Appsetting.Instance.Manifest.SaveSteamUser(User.SteamId, User);

            return account;
        }
    }

    public class BuffClient : Client
    {
        public static BuffClient None = new BuffClient(new BuffUser());

        private Action<bool> startLogin = null;
        private Action<bool, bool> endLogin = null;

        public BuffClient(BuffUser user)
        {
            User = user;
        }

        public BuffUser User { get; private set; }

        public string Key => User?.UserId;

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
                User.SteamId = buffUser.steamid;
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

    public class EcoClient : Client
    {
        public static EcoClient None = new EcoClient(new EcoUser());

        private Action<bool> startLogin = null;
        private Action<bool, bool> endLogin = null;

        public EcoClient(EcoUser user)
        {
            User = user;
        }

        public EcoUser User { get; private set; }

        public string Key => User?.UserId;

        public bool LoggedIn => !string.IsNullOrWhiteSpace(Token);

        public string Token { get; set; }

        public async Task<bool> RefreshTokenAsync(bool relogin, CancellationToken cancellationToken = default)
        {
            try
            {
                startLogin?.Invoke(relogin);

                var refreshToken = await EcoApi.RefreshTokenAsync(User.ClientId, User.RefreshToken, cancellationToken);
                var refreshTokenData = refreshToken.Body?.StatusData?.ResultData;
                if (string.IsNullOrWhiteSpace(refreshTokenData?.Token))
                {
                    Token = null;
                    return false;
                }

                User.RefreshToken = refreshTokenData.RefreshToken;
                User.RefreshTokenExpireTime = refreshTokenData.RefreshTokenExpireDate;
                Token = refreshTokenData.Token;

                Appsetting.Instance.Manifest.SaveEcoUser(User.UserId, User);
                return true;
            }
            finally
            {
                endLogin?.Invoke(relogin, LoggedIn);
            }
        }

        public async Task RefreshClientAsync(CancellationToken cancellationToken = default)
        {
            var refreshTokenExpire = User.RefreshTokenExpireTime - DateTime.Now;
            if (TimeSpan.Zero < refreshTokenExpire && refreshTokenExpire < TimeSpan.FromMinutes(30) && !string.IsNullOrWhiteSpace(User.RefreshToken))
            {
                await RefreshTokenAsync(false, cancellationToken);
            }

            var userResponse = await EcoApi.QueryUserAsync(this, cancellationToken);
            var userData = userResponse?.StatusData?.ResultData;

            if (string.IsNullOrWhiteSpace(userData?.UserId))
            {
                return;
            }

            var steamUserResponse = await EcoApi.QuerySteamUserAsync(this, cancellationToken);
            var steamUserData = steamUserResponse?.StatusData?.ResultData;

            User.Nickname = userData.UserName;
            User.Avatar = userData.UserHead;
            User.SteamIds = steamUserData?.Select(c => c.SteamId).ToList() ?? new List<string>();

            Appsetting.Instance.Manifest.SaveEcoUser(User.UserId, User);
        }

        public async Task<EcoResponse<List<QueryOffersResponse>>> QueryOffers(string gameId = "730", CancellationToken cancellationToken = default)
        {
            return await EcoApi.QueryOffers(this, gameId, cancellationToken);
        }

        public Task LogoutAsync()
        {
            User.RefreshToken = null;
            Appsetting.Instance.Manifest.SaveEcoUser(User.UserId, User);

            Token = null;
            endLogin?.Invoke(false, false);
            return Task.CompletedTask;
        }

        public EcoClient WithStartLogin(Action<bool> action)
        {
            startLogin = action;
            return this;
        }

        public EcoClient WithEndLogin(Action<bool, bool> action)
        {
            endLogin = action;
            return this;
        }
    }

    public class YouPinClient : Client
    {
        public static YouPinClient None = new YouPinClient(new YouPinUser());

        private Action<bool> startLogin = null;
        private Action<bool, bool> endLogin = null;

        public YouPinClient(YouPinUser user)
        {
            User = user;
        }

        public YouPinUser User { get; private set; }

        public string Key => User?.UserId;

        public bool LoggedIn { get; set; }

        public async Task RefreshAsync(bool relogin, CancellationToken cancellationToken = default)
        {
            try
            {
                startLogin?.Invoke(relogin);

                var userRespnse = await YouPin898Api.GetUserInfo(User?.Token, cancellationToken);
                if (string.IsNullOrWhiteSpace(userRespnse.Body?.GetData()?.UserId))
                {
                    LoggedIn = false;
                    return;
                }

                LoggedIn = true;

                var user = userRespnse.Body.GetData();
                User.Nickname = user.NickName;
                User.Avatar = user.Avatar;
                User.SteamId = user.SteamId;

                Appsetting.Instance.Manifest.SaveYouPinUser(User.UserId, User);
            }
            finally
            {
                endLogin?.Invoke(relogin, LoggedIn);
            }
        }

        public async Task<IWebResponse<YouPin898Response<GetOfferListResponse>>> GetOfferList(CancellationToken cancellationToken = default)
        {
            return await YouPin898Api.GetOfferList(User?.Token, cancellationToken);
        }

        public Task LogoutAsync()
        {
            User.Token = null;
            Appsetting.Instance.Manifest.SaveYouPinUser(User.UserId, User);

            endLogin?.Invoke(false, false);
            return Task.CompletedTask;
        }

        public YouPinClient WithStartLogin(Action<bool> action)
        {
            startLogin = action;
            return this;
        }

        public YouPinClient WithEndLogin(Action<bool, bool> action)
        {
            endLogin = action;
            return this;
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

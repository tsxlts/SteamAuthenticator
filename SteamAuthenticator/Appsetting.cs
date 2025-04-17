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

    public interface IClient
    {
        public string Key { get; }
    }

    public interface IUserClient : IClient
    {
        public bool LoggedIn { get; }

        public Task<bool> LoginAsync(CancellationToken cancellationToken = default);

        public Task<bool> RefreshClientAsync(CancellationToken cancellationToken = default);

        public Task LogoutAsync(CancellationToken cancellationToken = default);

        public IUserClient WithStartLogin(Action action);

        public IUserClient WithEndLogin(Action<bool> action);
    }

    public abstract class BaseUserClient : IUserClient
    {
        protected Action startLogin = null;
        protected Action<bool> endLogin = null;

        public BaseUserClient(bool logged)
        {
            LoggedIn = logged;
        }

        public IUserClient WithStartLogin(Action action)
        {
            startLogin = action;
            return this;
        }

        public IUserClient WithEndLogin(Action<bool> action)
        {
            endLogin = action;
            return this;
        }

        public async Task<bool> LoginAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                startLogin?.Invoke();

                var logged = await InternalLoginAsync(cancellationToken);
                LoggedIn = logged;

                await RefreshClientAsync(cancellationToken);
                return logged;
            }
            finally
            {
                endLogin?.Invoke(LoggedIn);
            }
        }

        public async Task<bool> RefreshClientAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var logged = await InternalRefreshClientAsync(cancellationToken);
                LoggedIn = logged;

                return logged;
            }
            finally
            {
                endLogin?.Invoke(LoggedIn);
            }
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            LoggedIn = false;

            await InternalLogoutAsync(cancellationToken);

            endLogin?.Invoke(false);
        }

        protected abstract Task<bool> InternalLoginAsync(CancellationToken cancellationToken = default);

        protected abstract Task<bool> InternalRefreshClientAsync(CancellationToken cancellationToken = default);

        protected abstract Task InternalLogoutAsync(CancellationToken cancellationToken = default);

        public abstract string Key { get; }

        public bool LoggedIn { get; private set; }
    }

    public class UserClient : IClient
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

    public class BuffClient : BaseUserClient
    {
        public static BuffClient None = new BuffClient(new BuffUser(), false);

        public BuffClient(BuffUser user, bool logged) : base(logged)
        {
            User = user;
        }

        public BuffUser User { get; private set; }

        public CookieCollection Cookies => Extension.GetCookies(User?.BuffCookies ?? "");

        public override string Key => User?.UserId;

        protected override async Task<bool> InternalLoginAsync(CancellationToken cancellationToken = default)
        {
            var buffResponse = await BuffApi.QueryUserInfo(cookies: Cookies, cancellationToken);
            if (string.IsNullOrWhiteSpace(buffResponse.Body?.data?.id))
            {
                return false;
            }

            var newCookies = this.Cookies;
            newCookies.Add(buffResponse.Cookies);

            var buffUser = buffResponse.Body.data;
            User.SteamId = buffUser.steamid;
            User.Nickname = buffUser.nickname;
            User.Avatar = buffUser.avatar;
            User.BuffCookies = string.Join("; ", newCookies.Select(cookie => $"{cookie.Name}={HttpUtility.UrlEncode(cookie.Value)}"));

            Appsetting.Instance.Manifest.SaveBuffUser(User.UserId, User);
            return true;
        }

        protected override Task<bool> InternalRefreshClientAsync(CancellationToken cancellationToken = default)
        {
            return InternalLoginAsync(cancellationToken);
        }

        protected override Task InternalLogoutAsync(CancellationToken cancellationToken = default)
        {
            User.BuffCookies = string.Empty;
            Appsetting.Instance.Manifest.SaveBuffUser(User.UserId, User);
            return Task.CompletedTask;
        }

        public async Task<IWebResponse<BuffResponse<List<SteamTradeResponse>>>> QuerySteamTrade()
        {
            return await BuffApi.QuerySteamTrade(cookies: Cookies);
        }
    }

    public class EcoClient : BaseUserClient
    {
        public static EcoClient None = new EcoClient(new EcoUser(), false);

        public EcoClient(EcoUser user, bool logged) : base(logged)
        {
            User = user;
        }

        public EcoUser User { get; private set; }

        public override string Key => User?.UserId;

        public string Token { get; set; }

        protected override async Task<bool> InternalLoginAsync(CancellationToken cancellationToken = default)
        {
            return await RefreshTokenAsync(cancellationToken);
        }

        protected override async Task<bool> InternalRefreshClientAsync(CancellationToken cancellationToken = default)
        {
            var refreshTokenExpire = User.RefreshTokenExpireTime - DateTime.Now;
            if (TimeSpan.Zero < refreshTokenExpire && refreshTokenExpire < TimeSpan.FromMinutes(30) && !string.IsNullOrWhiteSpace(User.RefreshToken))
            {
                await RefreshTokenAsync(cancellationToken);
            }

            var userResponse = await EcoApi.QueryUserAsync(this, cancellationToken);
            if (userResponse?.StatusCode == "4001" || userResponse?.StatusData?.ResultCode == "4001")
            {
                return false;
            }

            var userData = userResponse?.StatusData?.ResultData;
            if (string.IsNullOrWhiteSpace(userData?.UserId))
            {
                return LoggedIn;
            }

            var steamUserResponse = await EcoApi.QuerySteamUserAsync(this, cancellationToken);
            var steamUserData = steamUserResponse?.StatusData?.ResultData;

            User.Nickname = userData.UserName;
            User.Avatar = userData.UserHead;
            User.SteamIds = steamUserData?.Select(c => c.SteamId).ToList() ?? new List<string>();
            Appsetting.Instance.Manifest.SaveEcoUser(User.UserId, User);

            return true;
        }

        protected override Task InternalLogoutAsync(CancellationToken cancellationToken = default)
        {
            User.RefreshToken = null;
            Appsetting.Instance.Manifest.SaveEcoUser(User.UserId, User);

            Token = null;
            return Task.CompletedTask;
        }

        public async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default)
        {
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

        public async Task<EcoResponse<List<QueryOffersResponse>>> QueryOffers(string gameId = "730", CancellationToken cancellationToken = default)
        {
            return await EcoApi.QueryOffers(this, gameId, cancellationToken);
        }
    }

    public class YouPinClient : BaseUserClient
    {
        public static YouPinClient None = new YouPinClient(new YouPinUser(), false);

        public YouPinClient(YouPinUser user, bool logged) : base(logged)
        {
            User = user;
        }

        public YouPinUser User { get; private set; }

        public override string Key => User?.UserId;

        protected override async Task<bool> InternalLoginAsync(CancellationToken cancellationToken = default)
        {
            var userRespnse = await YouPin898Api.GetUserInfo(User?.Token, cancellationToken);
            if (string.IsNullOrWhiteSpace(userRespnse.Body?.GetData()?.UserId))
            {
                return false;
            }

            var user = userRespnse.Body.GetData();
            User.Nickname = user.NickName;
            User.Avatar = user.Avatar;
            User.SteamId = user.SteamId;

            Appsetting.Instance.Manifest.SaveYouPinUser(User.UserId, User);
            return true;
        }

        protected override Task<bool> InternalRefreshClientAsync(CancellationToken cancellationToken = default)
        {
            return InternalLoginAsync(cancellationToken);
        }

        protected override Task InternalLogoutAsync(CancellationToken cancellationToken = default)
        {
            User.Token = null;
            Appsetting.Instance.Manifest.SaveYouPinUser(User.UserId, User);
            return Task.CompletedTask;
        }

        public async Task<IWebResponse<YouPin898Response<GetOfferListResponse>>> GetOfferList(CancellationToken cancellationToken = default)
        {
            return await YouPin898Api.GetOfferList(User?.Token, cancellationToken);
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

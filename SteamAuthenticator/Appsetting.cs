using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.BUFF;
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
        public static UserClient None = new UserClient(new User(), new SteamCommunityClient(), null);

        public readonly SemaphoreSlim LoginConfirmLocker = new SemaphoreSlim(1, 1);
        public readonly SemaphoreSlim ConfirmationPopupLocker = new SemaphoreSlim(1, 1);

        private Action startLogin = null;
        private Action<bool> endLogin = null;

        public UserClient(User user, SteamCommunityClient client, BuffClient buffClient)
        {
            User = user;
            Client = client;
            BuffClient = buffClient;
        }

        public SteamCommunityClient Client { get; set; }

        public User User { get; set; }

        public BuffClient BuffClient { get; private set; }

        public UserClient SetBuffClient(BuffClient buffClient)
        {
            BuffClient = buffClient;
            if (User != null)
            {
                User.BuffUser = buffClient?.User;
            }
            return this;
        }

        public UserClient SaveSetting(BuffUserSetting setting)
        {
            if (User?.BuffUser != null)
            {
                User.BuffUser.Setting = setting;
            }
            return this;
        }

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
                return result;
            }
            finally
            {
                endLogin?.Invoke(result);
            }
        }
    }

    public class BuffClient
    {
        public static BuffClient None = new BuffClient(new BuffUser());

        private Action startLogin = null;
        private Action<bool> endLogin = null;

        public BuffClient(BuffUser user)
        {
            User = user;
        }

        public BuffUser User { get; private set; }

        public bool LoggedIn { get; set; }

        public BuffClient WithStartLogin(Action action)
        {
            startLogin = action;
            return this;
        }

        public BuffClient WithEndLogin(Action<bool> action)
        {
            endLogin = action;
            return this;
        }

        public async Task<bool> LoginAsync()
        {
            bool result = false;
            try
            {
                startLogin?.Invoke();

                if (!(User.Cookies?.Any() ?? false))
                {
                    result = false;
                    return result;
                }

                var userInfo = await BuffApi.QueryUserInfo(cookies: User.Cookies);
                result = !string.IsNullOrWhiteSpace(userInfo.Body?.data?.id);

                LoggedIn = result;

                return result;
            }
            finally
            {
                endLogin?.Invoke(result);
            }
        }

        public async Task<IWebResponse<BuffResponse<List<SteamTradeResponse>>>> QuerySteamTrade()
        {
            return await BuffApi.QuerySteamTrade(cookies: User.Cookies);
        }
    }
}

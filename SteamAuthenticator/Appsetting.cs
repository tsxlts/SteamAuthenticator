using Newtonsoft.Json;
using Steam_Authenticator.Model;
using SteamKit;
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

        public CookieCollection BuffCookies { get; set; }

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

}

using Newtonsoft.Json;
using Steam_Authenticator.Model;
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
        public SteamCommunityClient CurrentClient { get; private set; } = new SteamCommunityClient();

        [JsonIgnore]
        public AppManifest Manifest { get; private set; } = new AppManifest();

        public void SetWebClient(SteamCommunityClient webClient)
        {
            CurrentClient = webClient;
        }
    }

    public class Domain
    {
        public string SteamCommunity { get; set; }
        public string SteamApi { get; set; }
        public string SteamPowered { get; set; }
        public string SteamLogin { get; set; }
    }


    public class ProxyAuth
    {
        public string AuthName { get; set; }
        public string EncryptKey { get; set; }
    }

}

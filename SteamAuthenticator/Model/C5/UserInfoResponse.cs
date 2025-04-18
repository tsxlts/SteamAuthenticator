
namespace Steam_Authenticator.Model.C5
{
    public class UserInfoResponse
    {
        public string uid { get; set; }

        public string avatar { get; set; }

        public string nickname { get; set; }

        public decimal balance { get; set; }

        public List<SteamUser> steamList { get; set; } = new List<SteamUser>();

        public class SteamUser
        {
            public string steamId { get; set; }
            public string avatar { get; set; }
        }
    }
}

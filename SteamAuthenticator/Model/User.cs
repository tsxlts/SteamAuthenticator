using Newtonsoft.Json;
using SteamKit;

namespace Steam_Authenticator.Model
{
    public class User : JsonStreamSerializer<User>
    {
        public string Account { get; set; }

        public string SteamId { get; set; }

        public string NickName { get; set; }

        public string RefreshToken { get; set; }

        public string Avatar { get; set; }

        public BuffUser BuffUser { get; set; }
    }

    public class BuffUser
    {
        public string BuffCookies { get; set; }

        public string UserId { get; set; }

        public string SteamId { get; set; }

        public string Avatar { get; set; }

        public string Nickname { get; set; }

        public BuffUserSetting Setting { get; set; } = new BuffUserSetting
        {
            AutoAcceptGiveOffer = false
        };
    }

    public class BuffUserSetting
    {
        /// <summary>
        /// 是否自动接收索取报价
        /// 发货报价
        /// </summary>
        public bool AutoAcceptGiveOffer { get; set; }
    }
}

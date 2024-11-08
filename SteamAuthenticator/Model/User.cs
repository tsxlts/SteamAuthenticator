﻿using Newtonsoft.Json;

namespace Steam_Authenticator.Model
{
    public class User : JsonStreamSerializer<User>
    {
        public string Account { get; set; }

        public string SteamId { get; set; }

        public string NickName { get; set; }

        public string RefreshToken { get; set; }

        public string Avatar { get; set; }

        public UserSetting Setting { get; set; } = new UserSetting();

        public BuffUser BuffUser { get; set; }
    }

    public class UserSetting
    {
        /// <summary>
        /// 是否自动检测确认信息
        /// </summary>
        [JsonProperty("periodic_checking_confirmation")]
        public bool PeriodicCheckingConfirmation { get; set; }

        /// <summary>
        /// 是否自动确认报价
        /// </summary>
        [JsonProperty("auto_confirm_trade")]
        public bool AutoConfirmTrade { get; set; }

        /// <summary>
        /// 是否自动确认市场上架
        /// </summary>
        [JsonProperty("auto_confirm_market")]
        public bool AutoConfirmMarket { get; set; }

        /// <summary>
        /// 是否自动接收赠送报价
        /// 别人送给我的
        /// </summary>
        [JsonProperty("auto_accept_receive_offer")]
        public bool AutoAcceptReceiveOffer { get; set; }

        /// <summary>
        /// 是否自动接收索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer")]
        public bool AutoAcceptGiveOffer { get; set; }
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

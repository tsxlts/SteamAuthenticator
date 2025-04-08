using Newtonsoft.Json;

namespace Steam_Authenticator.Model
{
    public class User : JsonStreamSerializer
    {
        public string Account { get; set; }

        public string SteamId { get; set; }

        public string NickName { get; set; }

        public string RefreshToken { get; set; }

        public string Avatar { get; set; }

        public UserSetting Setting { get; set; } = new UserSetting();
    }

    public class UserSetting
    {
        /// <summary>
        /// 是否自动检测确认信息
        /// </summary>
        [JsonProperty("periodic_checking_confirmation")]
        public bool PeriodicCheckingConfirmation { get; set; }

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

        #region 全部报价
        /// <summary>
        /// 是否自动接受 全部 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer")]
        public bool AutoAcceptGiveOffer { get; set; }

        /// <summary>
        /// 是否自动确认 全部 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade")]
        public bool AutoConfirmTrade { get; set; }
        #endregion

        #region BUFF报价
        /// <summary>
        /// 是否自动接受 BUFF 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_buff")]
        public bool AutoAcceptGiveOffer_Buff { get; set; }

        /// <summary>
        /// 是否自动确认 BUFF 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade_buff")]
        public bool AutoConfirmTrade_Buff { get; set; }
        #endregion

        #region ECO报价
        /// <summary>
        /// 是否自动接受 ECO 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_eco")]
        public bool AutoAcceptGiveOffer_Eco { get; set; }

        /// <summary>
        /// 是否自动确认 ECO 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade_eco")]
        public bool AutoConfirmTrade_Eco { get; set; }
        #endregion

        #region 悠悠报价
        /// <summary>
        /// 是否自动接受 ECO 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_youpin")]
        public bool AutoAcceptGiveOffer_YouPin { get; set; }

        /// <summary>
        /// 是否自动确认 ECO 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade_youpin")]
        public bool AutoConfirmTrade_YouPin { get; set; }
        #endregion

        #region 其他报价
        /// <summary>
        /// 是否自动接受 其他 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_other")]
        public bool AutoAcceptGiveOffer_Other { get; set; }

        /// <summary>
        /// 是否自动确认 其他 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade_other")]
        public bool AutoConfirmTrade_Other { get; set; }
        #endregion

        #region 自定义报价
        /// <summary>
        /// 是否自动接受 自定义 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_cstom")]
        public bool AutoAcceptGiveOffer_Custom { get; set; }

        /// <summary>
        /// 是否自动确认 自定义 报价
        /// </summary>
        [JsonProperty("auto_confirm_trade_cstom")]
        public bool AutoConfirmTrade_Custom { get; set; }
        #endregion

        /// <summary>
        /// 自动接受索取报价 自定义 规则
        /// </summary>
        public AcceptOfferRuleSetting AutoAcceptGiveOfferRule { get; set; } = new AcceptOfferRuleSetting();

        /// <summary>
        /// 是否自动确认报价
        /// </summary>
        /// <returns></returns>
        public bool AutoConfirmOffer()
        {
            return AutoConfirmTrade
                || AutoConfirmTrade_Buff
                || AutoConfirmTrade_Eco
                || AutoConfirmTrade_YouPin
                || AutoConfirmTrade_Other
                || AutoConfirmTrade_Custom;
        }

        /// <summary>
        /// 是否自动接受索取报价 别人索取我的
        /// </summary>
        /// <returns></returns>
        public bool AutoAcceptGive()
        {
            return AutoAcceptGiveOffer
                || AutoAcceptGiveOffer_Buff
                || AutoAcceptGiveOffer_Eco
                || AutoAcceptGiveOffer_YouPin
                || AutoAcceptGiveOffer_Other
                || AutoAcceptGiveOffer_Custom;
        }

        /// <summary>
        /// 是否自动接受赠送报价 别人赠送给我的
        /// </summary>
        /// <returns></returns>
        public bool AutoAcceptReceive()
        {
            return AutoAcceptReceiveOffer;
        }
    }

    public class BuffUser : JsonStreamSerializer
    {
        public string BuffCookies { get; set; }

        public string UserId { get; set; }

        public string SteamId { get; set; }

        public string Avatar { get; set; }

        public string Nickname { get; set; }
    }

    public class EcoUser : JsonStreamSerializer
    {
        public string ClientId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpireTime { get; set; }

        public string UserId { get; set; }

        public List<string> SteamIds { get; set; } = new List<string>();

        public string Avatar { get; set; }

        public string Nickname { get; set; }
    }

    public class YouPinUser : JsonStreamSerializer
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public string SteamId { get; set; }

        public string Avatar { get; set; }

        public string Nickname { get; set; }
    }
}

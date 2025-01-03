using Newtonsoft.Json;
using Steam_Authenticator.Model.ECO;

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
        /// 是否自动接受 全部 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer")]
        public bool AutoAcceptGiveOffer { get; set; }

        /// <summary>
        /// 是否自动接受 BUFF 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_buff")]
        public bool AutoAcceptGiveOffer_Buff { get; set; }

        /// <summary>
        /// 是否自动接受 其他 索取报价
        /// 别人索取我的
        /// </summary>
        [JsonProperty("auto_accept_give_offer_other")]
        public bool AutoAcceptGiveOffer_Other { get; set; }

        /// <summary>
        /// 是否自动接受 自定义 索取报价
        /// 别人索取我的
        /// </summary>
        public bool AutoAcceptGiveOffer_Custom { get; set; }

        /// <summary>
        /// 自动接受索取报价 自定义 规则
        /// </summary>
        public AcceptOfferRuleSetting AutoAcceptGiveOfferRule { get; set; } = new AcceptOfferRuleSetting();
    }

    public class BuffUser : JsonStreamSerializer<BuffUser>
    {
        public string BuffCookies { get; set; }

        public string UserId { get; set; }

        public string SteamId { get; set; }

        public string Avatar { get; set; }

        public string Nickname { get; set; }
    }

    public class EcoUser : JsonStreamSerializer<EcoUser>
    {
        public string ClientId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpireTime { get; set; }

        public string UserId { get; set; }

        public List<SteamUser> SteamUsers { get; set; } = new List<SteamUser>();

        public string Avatar { get; set; }

        public string Nickname { get; set; }

        public List<AutoBuyGoods> BuyGoods { get; set; } = new List<AutoBuyGoods>();

        public class SteamUser
        {
            public string SteamId { get; set; }

            public string NickName { get; set; }
        }

        public class AutoBuyGoods
        {
            private (DateTime Time, decimal Price)? currentPrice;

            public string GameId { get; set; }

            public string HashName { get; set; }

            public string Name { get; set; }

            public decimal MaxPrice { get; set; }

            public int BuySize { get; set; } = 1;

            public int Interval { get; set; } = 500;

            public string SteamId { get; set; }

            public PayType PayType { get; set; }

            public string NotifyAddress { get; set; }

            public bool Enabled { get; set; }

            public List<TimeRange> RunTimes { get; set; } = new List<TimeRange>();

            public bool IsRunTime(DateTime currentTime)
            {
                if (!(RunTimes?.Any() ?? false))
                {
                    return true;
                }
                if (RunTimes.Any(c => c.Start == c.End))
                {
                    return true;
                }

                var currentTimeOnly = new TimeOnly(currentTime.Hour, currentTime.Minute, currentTime.Second);
                foreach (var item in RunTimes)
                {
                    if (item.Start < item.End)
                    {
                        if (item.Start <= currentTimeOnly && currentTimeOnly <= item.End)
                        {
                            return true;
                        }

                        continue;
                    }

                    if (!(item.Start <= currentTimeOnly && currentTimeOnly <= item.End))
                    {
                        return true;
                    }
                }

                return false;
            }

            public (DateTime Time, decimal Price)? GetCurrentPrice()
            {
                return currentPrice;
            }

            public void SetCurrentPrice(DateTime time, decimal price)
            {
                currentPrice = (time, price);
            }
        }

        public class TimeRange
        {
            public TimeOnly Start { get; set; }

            public TimeOnly End { get; set; }
        }
    }
}

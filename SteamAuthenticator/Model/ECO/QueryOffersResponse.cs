namespace Steam_Authenticator.Model.ECO
{
    public class QueryOffersResponse
    {
        public string CurrentSteamId { get; set; }

        public string OrderNum { get; set; }

        public string OfferId { get; set; }

        public string GoodsName { get; set; }

        /// <summary>
        /// 发送报价=1,
        /// 接受报价=2,
        /// 令牌确认=3
        /// </summary>
        public int State { get; set; }
    }
}

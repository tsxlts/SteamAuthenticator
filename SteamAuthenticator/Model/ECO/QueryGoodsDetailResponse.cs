
namespace Steam_Authenticator.Model.ECO
{
    public class QueryGoodsDetailResponse
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 游戏Id
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// HashName
        /// </summary>
        public string HashName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 最低售价
        /// </summary>
        public decimal BottomPrice { get; set; }
    }
}

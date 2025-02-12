
namespace Steam_Authenticator.Model.ECO
{
    public class QuerySellGoodsResponse
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNum { get; set; }

        /// <summary>
        /// 商品售价
        /// </summary>
        public decimal SellingPrice { get; set; }
    }
}

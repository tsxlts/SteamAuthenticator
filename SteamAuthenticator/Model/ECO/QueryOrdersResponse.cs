
namespace Steam_Authenticator.Model.ECO
{
    public class QueryOrdersResponse
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>		
        public string GoodsName { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>		
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 资产Id
        /// </summary>
        public string AssetId { get; set; }

        public OrderState State { get; set; }

        public OrderDetailState DetailOrderState { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
    }

}

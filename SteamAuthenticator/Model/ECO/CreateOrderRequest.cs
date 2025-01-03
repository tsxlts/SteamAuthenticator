namespace Steam_Authenticator.Model.ECO
{
    public class CreateOrderRequest
    {
        /// <summary>
        /// 商品
        /// </summary>
        public List<GoodsInfo> Goods { get; set; }

        /// <summary>
        /// 是否批量购买
        /// </summary>
        public bool IsBatchBuy { get; set; }

        public string SteamID { get; set; }

        public OrderType OrderType { get; set; } = OrderType.购买;

        public PayType PayType { get; set; } = PayType.余额;

        public PayScene PayScene { get; set; } = PayScene.H5;

        public OrderFrom CreateFrom { get; set; } = OrderFrom.Unknown;
    }

    /// <summary>
    /// 商品
    /// </summary>
    public class GoodsInfo
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsNum { get; set; }

        /// <summary>
        /// 当前商品价格
        /// </summary>
        public decimal? Price { get; set; }
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 购买
        /// </summary>
        购买 = 1,

        /// <summary>
        /// 赠送
        /// </summary>
        赠送 = 2,

        /// <summary>
        /// 求购
        /// </summary>
        求购 = 3,

        /// <summary>
        /// 还价
        /// </summary>
        还价 = 4,

        /// <summary>
        /// 租赁
        /// </summary>
        租赁 = 5,

        /// <summary>
        /// 预售
        /// </summary>
        预售 = 6,

        /// <summary>
        /// 转卖
        /// </summary>
        转卖 = 7,

        /// <summary>
        /// 预售转卖
        /// </summary>
        预售转卖 = 8,

        /// <summary>
        /// 炉主参与合炉，从市场购买材料给自己收货账号
        /// </summary>
        合炉购买 = 9,

        /// <summary>
        /// 用户参与合炉，从市场购买材料赠送给炉主收货账号
        /// </summary>
        合炉赠送 = 10
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 待付款
        /// </summary>
        待付款 = 7,

        /// <summary>
        /// 等待发货
        /// 未发送报价
        /// </summary>
        等待发货 = 1,

        /// <summary>
        /// 发货中
        /// 已发送报价等待令牌确认
        /// </summary>
        发货中 = 8,

        /// <summary>
        /// 等待对方确认报价
        /// 卖家发送报价后等待买家确认收货
        /// 买家发送报价后等待卖家接收报价
        /// </summary>
        等待对方确认 = 2,

        /// <summary>
        /// 交易取消
        /// </summary>
        交易取消 = 3,

        /// <summary>
        ///交易成功 
        /// </summary>
        交易成功 = 6,

        /// <summary>
        /// 交易暂挂
        /// </summary>
        /// 
        交易暂挂 = 9,

        /// <summary>
        /// 待支付尾款
        /// </summary>
        待支付尾款 = 20
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderDetailState
    {
        /// <summary>
        /// 交易取消
        /// </summary>
        交易取消 = 3,

        /// <summary>
        ///交易成功 
        /// </summary>
        /// 
        交易成功 = 6,

        /// <summary>
        /// 待付款
        /// </summary>
        待付款 = 7,

        /// <summary>
        /// 卖家发货中
        /// </summary>
        发货中 = 8,

        /// <summary>
        /// 待支付尾款
        /// </summary>
        /// 
        待支付尾款 = 9,

        /// <summary>
        /// 交易异常
        /// </summary>
        交易异常 = 10,

        /// <summary>
        /// 转卖成功
        /// </summary>
        转卖成功 = 11,
    }

    /// <summary>
    /// 支付方式枚举
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 余额支付
        /// </summary>
        余额 = -1,

        None = 0,

        /// <summary>
        /// 支付宝支付
        /// </summary>
        支付宝 = 1,

        /// <summary>
        /// 花呗支付
        /// </summary>
        花呗 = 2,

        /// <summary>
        /// 花呗分期支付
        /// </summary>
        花呗分期 = 3,

        /// <summary>
        /// 微信支付
        /// </summary>
        微信 = 4,

        /// <summary>
        /// 银行卡支付
        /// </summary>
        银行卡 = 5,

        /// <summary>
        /// 云闪付
        /// </summary>
        云闪付 = 6,
    }

    /// <summary>
    /// 支付场景
    /// </summary>
    public enum PayScene
    {
        /// <summary>
        /// PC扫码
        /// </summary>
        扫码 = 1,

        /// <summary>
        /// PC跳转
        /// </summary>
        PC = 2,

        /// <summary>
        /// H5
        /// </summary>
        H5 = 3,

        /// <summary>
        /// App
        /// </summary>
        App = 4
    }

    /// <summary>
    /// 页面动作类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 跳转
        /// </summary>
        Redirect = 1,

        /// <summary>
        /// 二维码
        /// </summary>
        QrCode = 2,

        /// <summary>
        /// Post提交
        /// </summary>
        Post = 3,

        /// <summary>
        /// SDK
        /// </summary>
        SDK = 4
    }

    /// <summary>
    /// 订单来源
    /// </summary>
    public enum OrderFrom
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL = -1,

        /// <summary>
        /// 未知来源
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 来源PC端
        /// </summary>
        PC = 1,

        /// <summary>
        /// 来源安卓端
        /// </summary>
        Android = 2,

        /// <summary>
        /// 来源苹果端
        /// </summary>
        IOS = 3,

        /// <summary>
        /// 自营供应
        /// </summary>
        WaxpexSupply = 4,

        /// <summary>
        /// Market自营供应
        /// </summary>
        MarketSupply = 5
    }
}

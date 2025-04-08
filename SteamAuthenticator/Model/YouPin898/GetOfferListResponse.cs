
namespace Steam_Authenticator.Model.YouPin898
{
    public class GetOfferListResponse
    {
        public List<OrderInfo> orderInfoList { get; set; } = new List<OrderInfo>();

        public List<MenuCodeInfo> menuCodeInfoList { get; set; } = new List<MenuCodeInfo>();
    }

    public class OrderInfo
    {
        public Templateinfo templateInfo { get; set; }
        public string orderNo { get; set; }
        public string orderType { get; set; }
        public string commodityQuantity { get; set; }
        public string offerId { get; set; }
        public string offerType { get; set; }
        public string orderStatus { get; set; }
        public string orderSubStatus { get; set; }
        public string offerReceiveUserId { get; set; }
        public string offerReceiveUserSteamRegTime { get; set; }
        public string showReceiveSteamInfo { get; set; }
        public string orderUpdateTime { get; set; }
        public Buttonlist[] buttonList { get; set; }
        public string menuCode { get; set; }
        public string offerCountdownPreDesc { get; set; }
        public string offerOperateSuccessDesc { get; set; }
        public string offerCountdown { get; set; }
        public string currentTime { get; set; }
        public string offerSurplusCountdown { get; set; }
        public string offerEndCountdownDesc { get; set; }
        public string orderTypeIcon { get; set; }
        public bool leaseOrderBuyerFlag { get; set; }
    }

    public class Templateinfo
    {
        public string templateId { get; set; }
        public string templateName { get; set; }
        public string templateHashName { get; set; }
        public string typeName { get; set; }
        public string rarityName { get; set; }
        public string rarityColor { get; set; }
        public string exteriorName { get; set; }
        public string exteriorNameAbbr { get; set; }
        public string exteriorColor { get; set; }
        public string qualityName { get; set; }
        public string qualityColor { get; set; }
        public string iconUrl { get; set; }
    }

    public class Buttonlist
    {
        public string name { get; set; }
        public string type { get; set; }
        public object popupTip { get; set; }
        public object reconfirmTip { get; set; }
        public string fill { get; set; }
        public object color { get; set; }
        public string nameColor { get; set; }
        public string borderColor { get; set; }
        public bool hideAfterSuccess { get; set; }
        public bool hideButton { get; set; }
    }

    public class MenuCodeInfo
    {
        public string code { get; set; }
        public int quantity { get; set; }
        public bool allFlag { get; set; }
    }
}

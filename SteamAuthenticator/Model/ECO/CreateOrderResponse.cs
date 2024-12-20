
namespace Steam_Authenticator.Model.ECO
{
    public class CreateOrderResponse
    {
        public List<string> OrderNum { get; set; } = new List<string>();

        public string PayId { get; set; }

        public string OrderId { get; set; }

        public ActionType ActionType { get; set; }

        public string Action { get; set; }

        public object ActionParam { get; set; }

        public int RefreshTime { get; set; }
    }
}

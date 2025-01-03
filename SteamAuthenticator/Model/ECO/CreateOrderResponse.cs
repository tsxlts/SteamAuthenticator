
namespace Steam_Authenticator.Model.ECO
{
    public class CreateOrderResponse : PayOrdersResponse
    {
        public List<string> OrderNum { get; set; } = new List<string>();
    }
}

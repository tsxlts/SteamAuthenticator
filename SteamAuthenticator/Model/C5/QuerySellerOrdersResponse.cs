
namespace Steam_Authenticator.Model.C5
{
    public class QuerySellerOrdersResponse
    {
        public int total { get; set; }
        public int pages { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public List<C5Order> list { get; set; } = new List<C5Order>();
    }

    public class C5Order
    {

        public string orderId { get; set; }

        public string productId { get; set; }

        public string appId { get; set; }

        public string itemId { get; set; }

        public string name { get; set; }

        public string marketHashName { get; set; }

        public string imageUrl { get; set; }

        public decimal price { get; set; }

        public string status { get; set; }

        public string statusName { get; set; }
    }
}

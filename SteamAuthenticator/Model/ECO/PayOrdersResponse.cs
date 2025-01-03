
using Newtonsoft.Json;

namespace Steam_Authenticator.Model.ECO
{
    public class PayOrdersResponse
    {
        public string PayId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActionType ActionType { get; set; }

        public string Action { get; set; }

        public object ActionParam { get; set; }

        public int RefreshTime { get; set; }
    }
}

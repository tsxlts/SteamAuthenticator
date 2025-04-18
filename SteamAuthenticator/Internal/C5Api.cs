using System.Net.Http.Json;
using Steam_Authenticator.Model.C5;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Internal
{
    internal static class C5Api
    {
        public const string Api = "https://openapi.c5game.com";

        public static async Task<IWebResponse<C5Response<UserInfoResponse>>> QueryUserInfo(string appkey, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<C5Response<UserInfoResponse>>($"{Api}/merchant/account/v1/steamInfo?" +
                $"app-key={appkey}",
                headers: InitHeaders(appkey),
                cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<C5Response<List<string>>>> CheckOffers(string appkey, List<string> offers, CancellationToken cancellationToken = default)
        {
            var content = JsonContent.Create(offers);

            var response = await SteamApi.PostAsync<C5Response<List<string>>>($"{Api}/merchant/offer/v1/exists?" +
                $"app-key={appkey}",
                content,
                headers: InitHeaders(appkey),
                cancellationToken: cancellationToken);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="status">
        /// 不传:全部订单,
        /// 0:待付款,
        /// 1:待发货,
        /// 2;发货中,
        /// 3:是待收货,
        /// 10:已完成,
        /// 11:已取消</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IWebResponse<C5Response<QuerySellerOrdersResponse>>> QuerySellerOrders(string appkey, int status = 1, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<C5Response<QuerySellerOrdersResponse>>($"{Api}/merchant/order/v1/list?" +
                $"app-key={appkey}" +
                $"&status={status}" +
                $"&page=1" +
                $"&limit=200",
               headers: InitHeaders(appkey),
               cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<C5Response<object>>> Deliver(string appkey, List<string> orders, CancellationToken cancellationToken = default)
        {
            var content = JsonContent.Create(orders);

            var response = await SteamApi.PostAsync<C5Response<object>>($"{Api}/merchant/order/v1/deliver?" +
                $"app-key={appkey}",
                content,
                headers: InitHeaders(appkey),
                cancellationToken: cancellationToken);
            return response;
        }

        public static IDictionary<string, string> InitHeaders(string appkey)
        {
            IDictionary<string, string> headers = new Dictionary<string, string>
            {
                {"app-key",appkey },
            };

            return headers;
        }
    }
}

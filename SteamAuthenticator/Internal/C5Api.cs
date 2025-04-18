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

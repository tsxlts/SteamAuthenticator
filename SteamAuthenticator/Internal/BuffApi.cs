
using Steam_Authenticator.Model.BUFF;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Internal
{
    internal static class BuffApi
    {
        public const string Api = "https://buff.163.com";

        public static async Task<IWebResponse<BuffResponse<BuffUserInfoResponse>>> QueryUserInfo(CookieCollection cookies, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<BuffResponse<BuffUserInfoResponse>>($"{Api}/account/api/user/info", headers: InitHeaders(cookies), currentCookies: cookies, cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<BuffResponse<List<SteamTradeResponse>>>> QuerySteamTrade(CookieCollection cookies, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<BuffResponse<List<SteamTradeResponse>>>($"{Api}/api/market/steam_trade", headers: InitHeaders(cookies), currentCookies: cookies, cancellationToken: cancellationToken);
            return response;
        }

        public static IDictionary<string, string> InitHeaders(CookieCollection cookies)
        {
            IDictionary<string, string> headers = new Dictionary<string, string>
            {
                {"X-CSRFToken",cookies?["csrf_token"]?.Value },
                {"Referer","https://buff.163.com/" },
                {"Origin","https://buff.163.com/" },
                {"Accept","*/*" }
            };

            return headers;
        }
    }
}

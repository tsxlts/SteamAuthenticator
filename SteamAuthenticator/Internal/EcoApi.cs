using Newtonsoft.Json.Linq;
using Steam_Authenticator.Model.ECO;
using SteamKit;
using SteamKit.Model;
using System.Net.Http.Json;

namespace Steam_Authenticator.Internal
{
    internal static class EcoApi
    {
        public const string Api = "https://api.ecosteam.cn";

        public static async Task<IWebResponse<EcoResponse<Model.ECO.RefreshTokenResponse>>> RefreshTokenAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.PostAsync<EcoResponse<Model.ECO.RefreshTokenResponse>>($"{Api}/Api/Login/RefreshToken", JsonContent.Create(new
            {
                RefreshTokenType = 1,
                ClientType = 2,
                ClientId = clientId,
                RefreshToken = refreshToken,
            }), cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<EcoResponse<QueryUserResponse>> QueryUserAsync(string loginToken, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<EcoResponse<QueryUserResponse>>($"{Api}/Api/Customer/GetUserInfo", headers: InitHeaders(null, loginToken), cancellationToken: cancellationToken);
            return response.Body;
        }

        public static async Task<EcoResponse<List<QuerySteamUserResponse>>> QuerySteamUserAsync(string loginToken, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<EcoResponse<List<QuerySteamUserResponse>>>($"{Api}/Api/SteamUser/QuerySteamAccountList", headers: InitHeaders(null, loginToken), cancellationToken: cancellationToken);
            return response.Body;
        }

        public static async Task<EcoResponse<QueryUserResponse>> QueryUserAsync(EcoClient client, CancellationToken cancellationToken = default)
        {
            var response = await GetAsync<QueryUserResponse>(client, $"{Api}/Api/Customer/GetUserInfo", cancellationToken);
            return response;
        }

        public static async Task<EcoResponse<List<QuerySteamUserResponse>>> QuerySteamUserAsync(EcoClient client, CancellationToken cancellationToken = default)
        {
            var response = await GetAsync<List<QuerySteamUserResponse>>(client, $"{Api}/Api/SteamUser/QuerySteamAccountList", cancellationToken);
            return response;
        }

        public static async Task<EcoResponse<PagesModel<QuerySellGoodsResponse>>> QuerySellGoodsAsync(EcoClient client, string gameId, string hashName, decimal maxPrice, int count, CancellationToken cancellationToken = default)
        {
            var response = await PostAsync<PagesModel<QuerySellGoodsResponse>>(client, $"{Api}/Api/SteamGoods/SellGoodsQuery", new
            {
                PageIndex = 1,
                PageSize = count,
                StartPice = 0,
                EndPice = maxPrice,
                GameId = gameId,
                HashName = hashName
            }, cancellationToken);
            return response;
        }

        public static async Task<EcoResponse<CreateOrderResponse>> CreateOrdersAsync(EcoClient client, string steamId, PayType payType, IEnumerable<GoodsInfo> goods, CancellationToken cancellationToken = default)
        {
            var response = await PostAsync<CreateOrderResponse>(client, $"{Api}/Api/Order/CreateOrdersNew", new CreateOrderRequest
            {
                SteamID = steamId,

                Goods = goods.ToList(),

                PayType = payType,
                PayScene = PayScene.H5,
                OrderType = OrderType.购买,

                IsBatchBuy = true,
            }, cancellationToken);
            return response;
        }

        public static async Task<EcoResponse<List<QueryOffersResponse>>> QueryOffers(EcoClient client, string gameId, CancellationToken cancellationToken = default)
        {
            var response = await GetAsync<List<QueryOffersResponse>>(client, $"{Api}/Api/OneClickOffers/OneClickOffersList?gameId={gameId}", cancellationToken);
            return response;
        }

        private static async Task<EcoResponse<TResponse>> GetAsync<TResponse>(EcoClient client, string url, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.GetAsync<EcoResponse<JToken>>(url, headers: InitHeaders(null, client.Token), cancellationToken: cancellationToken);
            if (response.Body?.StatusCode == "4001" || response.Body?.StatusData?.ResultCode == "4001")
            {
                var refreshTokenResponse = await client.RefreshTokenAsync(false, cancellationToken);
                if (!refreshTokenResponse)
                {
                    return ToResponse<TResponse>(response);
                }

                response = await SteamApi.GetAsync<EcoResponse<JToken>>(url, headers: InitHeaders(null, client.Token), cancellationToken: cancellationToken);
            }

            return ToResponse<TResponse>(response);
        }

        private static async Task<EcoResponse<TResponse>> PostAsync<TResponse>(EcoClient client, string url, object @params, CancellationToken cancellationToken = default)
        {
            var response = await SteamApi.PostAsync<EcoResponse<JToken>>(url, JsonContent.Create(@params), headers: InitHeaders(null, client.Token), cancellationToken: cancellationToken);
            if (response.Body?.StatusCode == "4001" || response.Body?.StatusData?.ResultCode == "4001")
            {
                var refreshTokenResponse = await client.RefreshTokenAsync(false, cancellationToken);
                if (!refreshTokenResponse)
                {
                    return ToResponse<TResponse>(response);
                }

                response = await SteamApi.PostAsync<EcoResponse<JToken>>(url, JsonContent.Create(@params), headers: InitHeaders(null, client.Token), cancellationToken: cancellationToken);
            }

            return ToResponse<TResponse>(response);
        }

        private static IDictionary<string, string> InitHeaders(IDictionary<string, string> headers = null, string loginToken = null)
        {
            IDictionary<string, string> result = new Dictionary<string, string>
            {
                {"Authorization",$"Bearer {loginToken}" }
            };
            if (headers?.Any() ?? false)
            {
                foreach (var key in headers)
                {
                    result.Add(key);
                }
            }
            return result;
        }

        private static EcoResponse<TResponse> ToResponse<TResponse>(IWebResponse<EcoResponse<JToken>> response)
        {
            if (response.Body == null)
            {
                return null;
            }
            if (response.Body.StatusData == null)
            {
                return new EcoResponse<TResponse>
                {
                    StatusCode = response.Body.StatusCode,
                    StatusMsg = response.Body.StatusMsg,
                    StatusData = null
                };
            }
            if (response.Body.StatusData.ResultData == null)
            {
                return new EcoResponse<TResponse>
                {
                    StatusCode = response.Body.StatusCode,
                    StatusMsg = response.Body.StatusMsg,
                    StatusData = new Statusdata<TResponse>
                    {
                        ResultCode = response.Body.StatusData.ResultCode,
                        ResultMsg = response.Body.StatusData.ResultMsg,
                        ResultData = default
                    }
                };
            }

            if (response.Body.StatusData.ResultData.Type == JTokenType.String)
            {
                if (typeof(TResponse) == typeof(string))
                {
                    return new EcoResponse<TResponse>
                    {
                        StatusCode = response.Body.StatusCode,
                        StatusMsg = response.Body.StatusMsg,
                        StatusData = new Statusdata<TResponse>
                        {
                            ResultCode = response.Body.StatusData.ResultCode,
                            ResultMsg = response.Body.StatusData.ResultMsg,
                            ResultData = response.Body.StatusData.ResultData.Value<TResponse>()
                        }
                    };
                }

                return new EcoResponse<TResponse>
                {
                    StatusCode = response.Body.StatusCode,
                    StatusMsg = response.Body.StatusMsg,
                    StatusData = new Statusdata<TResponse>
                    {
                        ResultCode = response.Body.StatusData.ResultCode,
                        ResultMsg = response.Body.StatusData.ResultMsg,
                        ResultData = default
                    }
                };
            }

            return new EcoResponse<TResponse>
            {
                StatusCode = response.Body.StatusCode,
                StatusMsg = response.Body.StatusMsg,
                StatusData = new Statusdata<TResponse>
                {
                    ResultCode = response.Body.StatusData.ResultCode,
                    ResultMsg = response.Body.StatusData.ResultMsg,
                    ResultData = response.Body.StatusData.ResultData.ToObject<TResponse>()
                }
            };
        }
    }
}

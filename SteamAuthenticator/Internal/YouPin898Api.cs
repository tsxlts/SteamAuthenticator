using Newtonsoft.Json;
using Steam_Authenticator.Model.YouPin898;
using SteamKit;
using SteamKit.Model;
using System.Net.Http.Json;

namespace Steam_Authenticator.Internal
{
    internal static class YouPin898Api
    {
        public const string Api = "https://api.youpin898.com";
        public const string AppType = "7";
        public const string AppVersion = "5.30.1";

        public readonly static string DeviceId;
        public readonly static string UK;
        public readonly static string RequestTag;

        static YouPin898Api()
        {
            DeviceId = Helper.GetMachineGuid();
            if (string.IsNullOrEmpty(DeviceId))
            {
                DeviceId = Helper.GetMachineUniqueId();
            }

            UK = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            RequestTag = Guid.NewGuid().ToString();
        }

        public static async Task<IWebResponse<YouPin898Response<SendSmsCodeResponse>>> SendSmsCode(string sessionId, string area, string phone, CancellationToken cancellationToken)
        {
            var content = JsonContent.Create(new
            {
                Sessionid = sessionId,
                Area = area,
                Mobile = phone,
                Code = ""
            });
            var response = await SteamApi.PostAsync<YouPin898Response<SendSmsCodeResponse>>($"{Api}/api/user/Auth/SendSignInSmsCode",
                content,
                headers: InitDefaultHeaders(), cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<YouPin898Response<SmsCodeLoginResponse>>> SmsCodeLogin(string sessionId, string area, string phone, string smsCode, CancellationToken cancellationToken)
        {
            string url = $"{Api}/api/user/Auth/SmsUpSignIn";
            if (!string.IsNullOrEmpty(smsCode))
            {
                url = $"{Api}/api/user/Auth/SmsSignIn";
            }

            var content = JsonContent.Create(new
            {
                SessionId = sessionId,
                Area = area,
                Mobile = phone,
                Code = smsCode,
            });
            var response = await SteamApi.PostAsync<YouPin898Response<SmsCodeLoginResponse>>(url,
                content,
                headers: InitDefaultHeaders(), cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<YouPin898Response<SmsUpSignInConfigResponse>>> GetSmsUpSignInConfig(CancellationToken cancellationToken)
        {
            var response = await SteamApi.GetAsync<YouPin898Response<SmsUpSignInConfigResponse>>($"{Api}/api/user/Auth/GetSmsUpSignInConfig",
                headers: InitDefaultHeaders(), cancellationToken: cancellationToken);
            return response;
        }

        public static async Task<IWebResponse<YouPin898Response<GetUserInfoResponse>>> GetUserInfo(string token, CancellationToken cancellationToken)
        {
            var response = await SteamApi.GetAsync<YouPin898Response<GetUserInfoResponse>>($"{Api}/api/user/Account/getUserInfo",
                headers: InitDefaultHeaders(token: token), cancellationToken: cancellationToken);
            return response;
        }

        private static IDictionary<string, string> InitDefaultHeaders(IDictionary<string, string> headers = null, string token = "")
        {
            var deviceInfo = new
            {
                deviceId = DeviceId,
                deviceType = "Xiaomi 15 Ultra",
                hasSteamApp = "1",
                requestTag = RequestTag,
                systemName = "Android",
                systemVersion = "11"
            };
            IDictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"uk",UK },
                {"AppType",AppType },
                {"App-Version",AppVersion },
                {"DeviceType","0" },
                {"Deviceid",DeviceId },
                {"DeviceToken",DeviceId },
                {"requestTag",RequestTag },
                {"platform","android" },
                {"User-Agent","okhttp/3.14.9" },
                {"Authorization", $"Bearer {token}" },
                {"Device-Info", JsonConvert.SerializeObject(deviceInfo) }
            };

            if (headers?.Any() ?? false)
            {
                foreach (var item in headers)
                {
                    if (result.ContainsKey(item.Key))
                    {
                        result[item.Key] = item.Value;
                        continue;
                    }

                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
    }
}

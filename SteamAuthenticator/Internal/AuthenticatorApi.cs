
using SteamKit;
using System.Net.Http.Json;
using static Steam_Authenticator.Factory.HttpClientFactory;

namespace Steam_Authenticator.Internal
{
    internal class AuthenticatorApi
    {
        public const string Api = "http://api.vdaima.cn";

        /// <summary>
        /// 提交需求
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        public static async Task<Result<MsgResult>> SubmitRequirements(string subject, string body, string contactInfo)
        {
            string url = $"{Api}/steam/Api/SteamAuthenticator/SubmitRequirements";
            var data = new
            {
                Project = "SteamAuthenticator",
                Subject = subject,
                Body = body,
                ContactInfo = contactInfo,
            };

            return await Post<MsgResult>(url, data);
        }

        /// <summary>
        /// 反馈BUG
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task<Result<MsgResult>> SubmitBug(Version version, string body)
        {
            string url = $"{Api}/steam/Api/SteamAuthenticator/SubmitBug";
            var data = new
            {
                Project = "SteamAuthenticator",
                Version = version.ToString(),
                Body = body
            };

            return await Post<MsgResult>(url, data);
        }

        public static async Task<Result<MsgResult>> Report(Version version, string machineId, IEnumerable<string> steamIds, IEnumerable<string> buffIds, IEnumerable<string> ecoIds, IEnumerable<string> youpinIds)
        {
            string url = $"{Api}/steam/Api/SteamAuthenticator/Report";
            var data = new
            {
                Project = "SteamAuthenticator",
                Version = version.ToString(),
                MachineId = machineId,
                SteamIds = steamIds,
                BuffIds = buffIds,
                EcoIds = ecoIds,
                YouPinIds = youpinIds
            };

            return await Post<MsgResult>(url, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<Result<T>> Post<T>(string url, object data)
        {
            var response = await SteamApi.PostAsync<Result<T>>(url, JsonContent.Create(data), proxy: NoProxy.Instance);
            return response.Body;
        }
    }

    public class Result<T>
    {
        public string ResultCode { get; set; }

        public string ResultMsg { get; set; }

        public T ResultData { get; set; }

        public bool IsSuccess()
        {
            return ResultCode == "0";
        }
    }

    public class MsgResult
    {
        public string Msg { get; set; }
    }
}

using SteamKit.Factory;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Steam_Authenticator.Factory
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public HttpClient GetHttpClient(Uri uri, IWebProxy proxy = null)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.All,
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = (HttpRequestMessage request, X509Certificate2 certificate2, X509Chain chain, SslPolicyErrors errors) => true
            };

            if (proxy != null)
            {
                httpClientHandler.Proxy = proxy;
                httpClientHandler.UseProxy = true;
            }
            else
            {
                var systemProxyInfo = typeof(HttpClient).Assembly.GetType("System.Net.Http.SystemProxyInfo");
                var getSystemProxy = systemProxyInfo.GetMethod("ConstructSystemProxy");
                var sysProxy = getSystemProxy.Invoke(null, null) as IWebProxy;

                httpClientHandler.Proxy = sysProxy;
                httpClientHandler.UseProxy = sysProxy != null;
            }

            return new HttpClient(httpClientHandler);
        }

        /// <summary>
        /// 请求完成时调用
        /// </summary>
        /// <param name="client"></param>
        public void Complete(HttpClient client)
        {
            client.Dispose();
        }
    }
}

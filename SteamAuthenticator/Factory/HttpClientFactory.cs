using System.Net;
using SteamKit.Factory;

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
            SocketsHttpHandler httpClientHandler = new SocketsHttpHandler
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.All,
                AllowAutoRedirect = false,
            };

            if (proxy != null)
            {
                httpClientHandler.Proxy = proxy;
                httpClientHandler.UseProxy = !(proxy is NoProxy);
            }
            else
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.PooledConnectionLifetime = TimeSpan.FromMinutes(2);
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

        public class NoProxy : IWebProxy
        {
            public static NoProxy Instance = new NoProxy();

            private NoProxy()
            {
            }

            public ICredentials Credentials { get; set; }

            public Uri GetProxy(Uri destination)
            {
                throw new NotImplementedException();
            }

            public bool IsBypassed(Uri host)
            {
                throw new NotImplementedException();
            }
        }
    }
}

using SteamKit.Factory;
using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace Steam_Authenticator.Factory
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        private ConcurrentDictionary<string, IHttpClientContext> Clients;

        public HttpClientFactory()
        {
            Clients = new ConcurrentDictionary<string, IHttpClientContext>();
        }

        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public IHttpClientContext GetHttpClientContext(Uri uri, bool useCookies, bool allowAutoRedirect, IWebProxy? proxy = null)
        {
            var domain = uri.Host;
            var port = uri.Port;
            var clientKey = new StringBuilder();
            clientKey.AppendLine($"[Server]{domain}:{port}");
            if (proxy != null)
            {
                clientKey.AppendLine($"[Proxy]{(proxy as WebProxy)?.Address?.ToString() ?? "Custom"}");
            }
            else
            {
                clientKey.AppendLine($"[Proxy]System");
            }
            clientKey.AppendLine($"[useCookies]{useCookies}");
            clientKey.AppendLine($"[allowAutoRedirect]{allowAutoRedirect}");

            var context = Clients.GetOrAdd(clientKey.ToString(), key =>
            {
                SocketsHttpHandler httpClientHandler = new SocketsHttpHandler
                {
                    UseCookies = useCookies,
                    AutomaticDecompression = DecompressionMethods.All,
                    AllowAutoRedirect = allowAutoRedirect,
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

                var client = new HttpClient(httpClientHandler);
                return new DefaultHttpClientContext(client, false);
            });

            return context;
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

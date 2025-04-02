using CefSharp;
using CefSharp.WinForms;

namespace Steam_Authenticator.Forms
{
    public partial class Browser : Form
    {
        private readonly ChromiumWebBrowser browser;
        private readonly IDictionary<string, IEnumerable<SteamKit.Cookie>> cookies;
        private Task initializing;

        public Browser()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser()
            {
                Dock = DockStyle.Fill
            };
            cookies = new Dictionary<string, IEnumerable<SteamKit.Cookie>>(StringComparer.OrdinalIgnoreCase);

            WebPanel.Controls.Clear();
            WebPanel.Controls.Add(browser);
            initializing = Task.CompletedTask;
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            string loadingUrl = Path.Combine("file:///", AppContext.BaseDirectory, "html", "loading.html");
            initializing = browser.LoadUrlAsync(loadingUrl);
        }

        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
        }

        public void LoadHtlm(string html)
        {
            browser.LoadHtml(html);
        }

        public void SetCookies(string url, params SteamKit.Cookie[] cookies)
        {
            this.cookies.Remove(url);
            this.cookies.TryAdd(url, cookies);
        }

        public async Task<LoadUrlAsyncResponse> LoadUrl(Uri url)
        {
            await initializing;
            await SetCookies();

            return await browser.LoadUrlAsync(url.ToString());
        }

        private async Task SetCookies()
        {
            await browser.GetCookieManager().DeleteCookiesAsync();
            if (cookies?.Any() ?? false)
            {
                foreach (var item in cookies)
                {
                    foreach (var cookie in item.Value)
                    {
                        await browser.GetCookieManager().SetCookieAsync(item.Key, new Cookie
                        {
                            Domain = cookie.Domain,
                            Path = "/",
                            Name = cookie.Name,
                            Value = cookie.Value
                        });
                    }
                }
            }
        }
    }
}

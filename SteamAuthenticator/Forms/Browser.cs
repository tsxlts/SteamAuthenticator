using CefSharp;
using CefSharp.WinForms;

namespace Steam_Authenticator.Forms
{
    public partial class Browser : Form
    {
        private readonly ChromiumWebBrowser browser;

        public Browser()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser()
            {
                Dock = DockStyle.Fill
            };
            WebPanel.Controls.Clear();
            WebPanel.Controls.Add(browser);
        }

        private void Browser_Load(object sender, EventArgs e)
        {
        }

        public void LoadHtlm(string html)
        {
            browser.LoadHtml(html);
        }

        public async Task SetCookie(string url, params SteamKit.Cookie[] cookies)
        {
            await browser.LoadUrlAsync(url);
            if (cookies?.Any() ?? false)
            {
                foreach (var cookie in cookies)
                {
                    await browser.GetCookieManager().SetCookieAsync(url, new Cookie
                    {
                        Name = cookie.Name,
                        Value = cookie.Value
                    });
                }
            }
        }

        public async Task<LoadUrlAsyncResponse> LoadUrl(Uri url)
        {
            var result = await browser.LoadUrlAsync(url.ToString());
            return result;
        }

        public async Task<LoadUrlAsyncResponse> LoadUrl(Uri url, params SteamKit.Cookie[] cookies)
        {
            string loadingUrl = Path.Combine("file:///", AppContext.BaseDirectory, "html", "loading.html");
            await browser.LoadUrlAsync(loadingUrl);
            if (cookies?.Any() ?? false)
            {
                string main = $"{url.Scheme}://{url.Host}";
                foreach (var cookie in cookies)
                {
                    await browser.GetCookieManager().SetCookieAsync(main, new Cookie
                    {
                        Domain = cookie.Domain,
                        Path = "/",
                        Name = cookie.Name,
                        Value = cookie.Value
                    });
                }
            }

            return await browser.LoadUrlAsync(url.ToString());
        }
    }
}

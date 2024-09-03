using CefSharp;
using CefSharp.WinForms;

namespace Steam_Authenticator.Forms
{
    public partial class Browser : Form
    {
        private ChromiumWebBrowser browser;

        public Browser()
        {
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser()
            {
                Dock = DockStyle.Fill
            };
            WebPanel.Controls.Clear();
            WebPanel.Controls.Add(browser);
        }

        public void LoadHtlm(string html)
        {
            browser.LoadHtml(html);
        }

        public async Task<LoadUrlAsyncResponse> LoadUrl(string url)
        {
            return await browser.LoadUrlAsync(url);
        }
    }
}

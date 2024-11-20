using System.Net;

namespace Steam_Authenticator.Forms
{
    public partial class ProxySetting : Form
    {
        public ProxySetting()
        {
            InitializeComponent();
        }

        private void ProxySetting_Load(object sender, EventArgs e)
        {
            Appsetting.Instance.AppSetting.Entry.Proxy = Appsetting.Instance.AppSetting.Entry.Proxy ?? new HostProxy();
            Appsetting.Instance.AppSetting.Entry.Domain = Appsetting.Instance.AppSetting.Entry.Domain ?? new Domain();

            useCustomerProxy.Checked = Appsetting.Instance.AppSetting.Entry.UseCustomerProxy;
            proxyAddressBox.Text = Appsetting.Instance.AppSetting.Entry.Proxy.Address;
            proxyHostBox.Text = Appsetting.Instance.AppSetting.Entry.Proxy.Host;
            proxyPortBox.Value = Appsetting.Instance.AppSetting.Entry.Proxy.Port;

            useCustomerDomain.Checked = Appsetting.Instance.AppSetting.Entry.UseCustomerDomain;
            communityBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamCommunity;
            apiBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamApi;
            storeBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamPowered;
            loginBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamLogin;
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                saveBtn.Enabled = false;

                if (!string.IsNullOrWhiteSpace(proxyAddressBox.Text) || !string.IsNullOrWhiteSpace(proxyHostBox.Text))
                {
                    IWebProxy webProxy;
                    if (!string.IsNullOrWhiteSpace(proxyAddressBox.Text))
                    {
                        webProxy = new WebProxy(new Uri(proxyAddressBox.Text));
                    }
                    else
                    {
                        webProxy = new WebProxy(proxyHostBox.Text, (int)proxyPortBox.Value);
                    }

                    var testProxy = await SteamKit.SteamApi.GetAsync("https://www.baidu.com", proxy: webProxy);
                }

                Appsetting.Instance.AppSetting.Entry.UseCustomerProxy = useCustomerProxy.Checked;
                Appsetting.Instance.AppSetting.Entry.Proxy.Address = proxyAddressBox.Text;
                Appsetting.Instance.AppSetting.Entry.Proxy.Host = proxyHostBox.Text;
                Appsetting.Instance.AppSetting.Entry.Proxy.Port = (int)proxyPortBox.Value;

                Appsetting.Instance.AppSetting.Entry.UseCustomerDomain = useCustomerDomain.Checked;
                Appsetting.Instance.AppSetting.Entry.Domain.SteamCommunity = communityBox.Text;
                Appsetting.Instance.AppSetting.Entry.Domain.SteamApi = apiBox.Text;
                Appsetting.Instance.AppSetting.Entry.Domain.SteamPowered = storeBox.Text;
                Appsetting.Instance.AppSetting.Entry.Domain.SteamLogin = loginBox.Text;

                Appsetting.Instance.AppSetting.Save();

                MessageBox.Show("已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"你设置的代理似乎不可用{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                saveBtn.Enabled = Enabled;
            }
        }
    }
}

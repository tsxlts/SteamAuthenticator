
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
            communityBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamCommunity;
            apiBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamApi;
            storeBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamPowered;
            loginBox.Text = Appsetting.Instance.AppSetting.Entry.Domain.SteamLogin;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Appsetting.Instance.AppSetting.Entry.Domain.SteamCommunity = communityBox.Text;
            Appsetting.Instance.AppSetting.Entry.Domain.SteamApi = apiBox.Text;
            Appsetting.Instance.AppSetting.Entry.Domain.SteamPowered = storeBox.Text;
            Appsetting.Instance.AppSetting.Entry.Domain.SteamLogin = loginBox.Text;

            Appsetting.Instance.AppSetting.Save();

            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

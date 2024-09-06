using SteamKit.WebClient;

namespace Steam_Authenticator.Forms
{
    public partial class ImportAuthenticator : Form
    {

        private readonly SteamCommunityClient webClient;

        public ImportAuthenticator(SteamCommunityClient webClient)
        {
            InitializeComponent();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SharedSecret) && string.IsNullOrWhiteSpace(IdentitySecret))
            {
                MessageBox.Show("登录秘钥和是否秘钥至少有一项不能为空");

                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelBtm_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string RevocationCode => RevocationCodeBox.Text;
        public string SharedSecret => SharedSecretBox.Text;
        public string IdentitySecret => IdentitySecretBox.Text;
    }
}

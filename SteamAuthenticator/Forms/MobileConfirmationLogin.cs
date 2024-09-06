
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.WebClient;

namespace Steam_Authenticator.Forms
{
    public partial class MobileConfirmationLogin : Form
    {
        private readonly SteamCommunityClient webClient;
        private readonly ulong clientId;
        private readonly int version;

        public MobileConfirmationLogin(SteamCommunityClient webClient, ulong clientId, int varsion)
        {
            InitializeComponent();

            this.webClient = webClient;
            this.clientId = clientId;
            this.version = varsion;
        }

        private async void acceptBtn_Click(object sender, EventArgs e)
        {
            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
            {
                MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var signature = GuardCodeGenerator.GenerateSignature(version, clientId, ulong.Parse(webClient.SteamId), guard.SharedSecret);
            var result = await SteamAuthentication.UpdateAuthSessionWithMobileConfirmationAsync(webClient.WebApiToken,
            webClient.SteamId,
            (long)clientId,
            version,
            signature: Convert.ToBase64String(signature),
            confirm: true
            );

            Close();
        }

        private async void declineBtn_Click(object sender, EventArgs e)
        {
            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
            {
                MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var signature = GuardCodeGenerator.GenerateSignature(version, clientId, ulong.Parse(webClient.SteamId), guard.SharedSecret);
            var result = await SteamAuthentication.UpdateAuthSessionWithMobileConfirmationAsync(webClient.WebApiToken,
            webClient.SteamId,
            (long)clientId,
            version,
            signature: Convert.ToBase64String(signature),
            confirm: false
            );

            Close();
        }
    }
}

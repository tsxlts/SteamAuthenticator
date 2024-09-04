
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.WebClient;
using System.Text;
using static Steam_Authenticator.Internal.Utils;

namespace Steam_Authenticator.Forms
{
    public partial class ConfirmationsPopup : Form
    {
        private readonly SteamCommunityClient webClient;
        private readonly List<SteamKit.Model.Confirmation> confirmations;

        public ConfirmationsPopup(SteamCommunityClient webClient, IEnumerable<SteamKit.Model.Confirmation> confirmations)
        {
            InitializeComponent();

            this.webClient = webClient;
            this.confirmations = confirmations.ToList();
        }

        private void ConfirmationsPopup_Load(object sender, EventArgs e)
        {
            int tradeCount = confirmations.Count(c => c.ConfType == SteamEnum.ConfirmationType.Trade);
            int marketCount = confirmations.Count(c => c.ConfType == SteamEnum.ConfirmationType.MarketListing);

            StringBuilder stringBuilder = new StringBuilder($"你有");
            if (tradeCount > 0)
            {
                stringBuilder.AppendLine($"{tradeCount}个报价");
            }
            if (marketCount > 0)
            {
                stringBuilder.AppendLine($"{marketCount}个市场上架");
            }
            stringBuilder.AppendLine("待确认");

            userLabel.Text = webClient.Account;
            tipsLabel.Text = stringBuilder.ToString();
        }

        private async void accept_decline_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            try
            {
                button.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard == null)
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool accept = false;
                    if (button.Tag?.ToString() == "accept")
                    {
                        accept = true;
                        msgLabel.Text = "正在确认...";
                    }
                    else if (button.Tag?.ToString() == "decline")
                    {
                        accept = false;
                        msgLabel.Text = "正在拒绝...";
                    }

                    bool success = await HandleConfirmation(webClient, guard, confirmations, accept, cts.Token);
                    if (success)
                    {
                        confirmations.Clear();
                        Close();
                    }
                    else
                    {
                        msgLabel.Text = "处理失败，请重新操作";

                        MessageBox.Show("操作失败,请重新操作");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button.Enabled = true;
            }
        }

        private void detailBtn_Click(object sender, EventArgs e)
        {
            Confirmations confirmation = new Confirmations(webClient);
            confirmation.ShowDialog();
        }
    }
}

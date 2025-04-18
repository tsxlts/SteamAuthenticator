
using System.Text;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.WebClient;
using static Steam_Authenticator.Internal.Utils;

namespace Steam_Authenticator.Forms
{
    public partial class ConfirmationsPopup : Form
    {
        private readonly UserClient client;
        private readonly SteamCommunityClient webClient;

        private readonly SemaphoreSlim locker;
        private readonly List<SteamKit.Model.Confirmation> confirmations;

        public ConfirmationsPopup(UserClient client, IEnumerable<SteamKit.Model.Confirmation> confirmations)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.client = client;
            this.webClient = client.Client;

            this.locker = new SemaphoreSlim(1, 1);
            this.confirmations = confirmations.ToList();
        }

        private void ConfirmationsPopup_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void ConfirmationsPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private async void accept_decline_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            try
            {
                button.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(client.GetAccount());
                if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                {
                    MessageBox.Show($"用户[{client.GetAccount()}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        Hide();
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
            Confirmations confirmation = new Confirmations(this, client)
            {
                Width = 600,
                Height = 400
            };
            confirmation.ShowDialog();
        }

        private void Init()
        {
            try
            {
                int tradeCount = confirmations.Count(c => c.ConfType == SteamEnum.ConfirmationType.Trade);
                int marketCount = confirmations.Count(c => c.ConfType == SteamEnum.ConfirmationType.MarketListing);
                var otherConfirms = confirmations.Where(c => !new[]
                {
                    SteamEnum.ConfirmationType.Trade, SteamEnum.ConfirmationType.MarketListing
                }.Contains(c.ConfType)).ToList();

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"你有");
                if (tradeCount > 0)
                {
                    stringBuilder.AppendLine($"{tradeCount}个报价");
                }
                if (marketCount > 0)
                {
                    stringBuilder.AppendLine($"{marketCount}个市场上架");
                }
                if (otherConfirms.Count > 0)
                {
                    stringBuilder.AppendLine($"{otherConfirms.Count}个{otherConfirms.First().Headline}等其他事项");
                }
                stringBuilder.AppendLine("待确认");

                userLabel.Text = client.GetAccount();
                tipsLabel.Text = stringBuilder.ToString();
            }
            catch
            {

            }
        }

        public void SetConfirmations(IEnumerable<SteamKit.Model.Confirmation> confirmations)
        {
            if (!locker.Wait(0))
            {
                return;
            }

            try
            {
                this.confirmations.Clear();
                this.confirmations.AddRange(confirmations);
                Init();
            }
            catch
            {

            }
            finally
            {
                locker.Release();
            }
        }
    }
}

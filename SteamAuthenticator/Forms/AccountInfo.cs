
using Steam_Authenticator.Internal;
using SteamKit;
using System.Diagnostics;
using static Steam_Authenticator.Internal.Configuration;

namespace Steam_Authenticator.Forms
{
    public partial class AccountInfo : Form
    {
        private readonly UserClient client;

        public AccountInfo(UserClient userClient)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            client = userClient;
        }

        private async void AccountInfo_Load(object sender, EventArgs e)
        {
            steamNameBox.Text = client.User.NickName;
            steamIdBox.Text = client.User.SteamId;

            await Init();
        }

        private void copySteamIdBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(steamIdBox.Text);
        }

        private void copyTradeLinkBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(tradeLinkBox.Text);
        }

        private void copyApikeyBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(apikeyBox.Text);
        }

        private void tradeStatusBtn_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(HelpWhyCantITrade) { UseShellExecute = true });
        }

        private async Task Init()
        {
            List<Task> tasks = new List<Task>();

            var task1 = SteamApi.GetTradeLinkAsync(client.User.SteamId, client.Client.WebCookie).ContinueWith(tradeLink =>
            {
                string result = tradeLink.Result.Body;
                if (!string.IsNullOrWhiteSpace(result))
                {
                    tradeLinkBox.Text = result;
                }
            });
            tasks.Add(task1);

            var task2 = SteamApi.GetApiKeyAsync(client.Client.WebCookie).ContinueWith(apikey =>
            {
                string result = apikey.Result.Body;
                if (!string.IsNullOrWhiteSpace(result))
                {
                    apikeyBox.Text = result;
                }
            });
            tasks.Add(task2);

            var task3 = TradableStatusCheck();
            tasks.Add(task3);

            await Task.WhenAll(tasks);
        }

        private async Task TradableStatusCheck()
        {
            try
            {
                tradeStatusBtn.Hide();

                if (!client.Client.LoggedIn)
                {
                    return;
                }

                var tradable = await SteamApi.TradableCheckAsync(client.Client.WebCookie);
                var tradableBody = tradable.Body;
                if (tradableBody == null)
                {
                    tradeStatusBox.ForeColor = Color.OrangeRed;
                    tradeStatusBox.Text = $"检测失败 ({(int)tradable.HttpStatusCode})";
                    return;
                }

                if (!tradableBody.Tradable)
                {
                    tradeStatusBox.ForeColor = Color.Red;
                    tradeStatusBox.Text = $"无法交易 ({tradableBody.Reason})";
                    tradeStatusBtn.Show();
                    return;
                }

                if (!string.IsNullOrWhiteSpace(DefaultTradeLinkPartner) && !string.IsNullOrWhiteSpace(DefaultTradeLinkToken))
                {
                    var tradeHoldDurations = await SteamApi.QueryTradeHoldDurationsAsync(null,
                    client.Client.WebApiToken,
                    Extension.GetSteamId(DefaultTradeLinkPartner), DefaultTradeLinkToken);
                    var myEscrow = tradeHoldDurations.Body?.MyEscrow;
                    if (myEscrow?.EscrowEndDurationSeconds > 0)
                    {
                        tradeStatusBox.ForeColor = Color.OrangeRed;
                        tradeStatusBox.Text = $"出货交易暂挂";
                        return;
                    }
                }

                tradeStatusBox.ForeColor = Color.Green;
                tradeStatusBox.Text = "可交易";
            }
            catch
            {

            }
        }
    }
}

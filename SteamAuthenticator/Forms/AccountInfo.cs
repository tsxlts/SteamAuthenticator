
using Steam_Authenticator.Internal;
using SteamKit;
using SteamKit.WebClient;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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
            await Init();
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
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
            try
            {
                refreshBtn.Enabled = false;

                steamNameBox.Text = client.User.NickName;
                steamIdBox.Text = client.User.SteamId;

                loginStatusBox.Text = client.Client.LoggedIn ? "已登录" : "未登录";
                loginStatusBox.ForeColor = client.Client.LoggedIn ? Color.Green : Color.Red;

                guardStatusBox.Text = "";
                tradeLinkBox.Text = "";
                apikeyBox.Text = "";
                tradeStatusBox.Text = "";
                tradePermissionBox.Text = "";
                bansStatusBox.Text = "";

                tradeStatusBtn.Hide();

                guardStatusLoading.Show();
                tradeLinkLoading.Show();
                apikeyLoading.Show();
                tradeStatusLoading.Show();
                tradePermissionLoading.Show();
                bansStatusLoading.Show();

                if (!client.Client.LoggedIn)
                {
                    return;
                }

                List<Task> tasks = new List<Task>();

                var task1 = SteamAuthenticator.QueryAuthenticatorStatusAsync(client.Client.WebApiToken, client.User.SteamId).ContinueWith(status =>
                {
                    guardStatusLoading.Hide();
                    guardStatusBox.Text = "**********";

                    var result = status.Result.Body;
                    if (result == null)
                    {
                        return;
                    }

                    switch (result.GuardScheme)
                    {
                        case SteamEnum.SteamGuardScheme.None:
                            guardStatusBox.Text = "未绑定令牌验证器";
                            break;
                        case SteamEnum.SteamGuardScheme.Email:
                            guardStatusBox.Text = "邮箱验证器";
                            break;
                        case SteamEnum.SteamGuardScheme.Device:
                            guardStatusBox.Text = $"手机验证器 ({result.DeviceId})";
                            break;
                    }
                });
                tasks.Add(task1);

                var task2 = SteamApi.GetTradeLinkAsync(client.User.SteamId, client.Client.WebCookie).ContinueWith(tradeLink =>
                {
                    tradeLinkLoading.Hide();

                    string result = tradeLink.Result.Body;
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return;
                    }

                    tradeLinkBox.Text = result;
                });
                tasks.Add(task2);

                var task3 = SteamApi.GetApiKeyAsync(client.Client.WebCookie).ContinueWith(apikey =>
                {
                    apikeyLoading.Hide();

                    string result = apikey.Result.Body;
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return;
                    }

                    apikeyBox.Text = result;
                });
                tasks.Add(task3);

                var task4 = TradableStatusCheck().ContinueWith(task =>
                {
                    tradeStatusLoading.Hide();
                });
                tasks.Add(task4);

                tradePermissionBox.Text = "**********";
                if (!string.IsNullOrWhiteSpace(DefaultPartnerTradeLink))
                {
                    var task5 = SteamApi.QueryTradePermissionsAsync(client.Client.WebCookie, DefaultPartnerTradeLink).ContinueWith(tradePermissions =>
                    {
                        tradePermissionLoading.Hide();

                        var result = tradePermissions.Result.Body;
                        if (!(result?.Any() ?? false))
                        {
                            return;
                        }

                        List<string> permissions = new List<string>();
                        StringBuilder builder;
                        string permission;
                        foreach (var item in result)
                        {
                            builder = new StringBuilder(item.Name);

                            bool g = item.AllowedToTradeItems();
                            bool r = item.AllowedToRecieveItems();

                            permission = (g, r) switch
                            {
                                var p when p.g && p.r => "全部",
                                var p when p.g && !p.r => "仅可发送报价",
                                var p when !p.g && p.r => "仅可接收报价",
                                _ => "无法进行报价交易"
                            };
                            builder.Append($" ({permission})");

                            permissions.Add(builder.ToString());
                        }

                        tradePermissionBox.Text = string.Join($"；{Environment.NewLine}", permissions);
                    });
                    tasks.Add(task5);
                }

                var task6 = SteamApi.QueryPlayerBansAsync(null, client.Client.WebApiToken, [client.User.SteamId]).ContinueWith(playersBans =>
                {
                    bansStatusLoading.Hide();
                    bansStatusBox.Text = "**********";

                    var result = playersBans.Result.Body?.Players?.FirstOrDefault();
                    if (result == null)
                    {
                        return;
                    }

                    List<string> bans = new List<string>();
                    if (result.CommunityBanned)
                    {
                        bans.Add("社区封禁");
                    }
                    if (result.VACBanned)
                    {
                        bans.Add("VAC封禁");
                    }
                    if (result.NumberOfGameBans > 0)
                    {
                        bans.Add("游戏封禁");
                    }

                    bansStatusBox.ForeColor = bans.Any() ? Color.Red : Color.Green;
                    bansStatusBox.Text = bans.Any() ? string.Join("；", bans) : "未封禁";
                });
                tasks.Add(task6);

                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                guardStatusLoading.Hide();
                tradeLinkLoading.Hide();
                apikeyLoading.Hide();
                tradeStatusLoading.Hide();
                tradePermissionLoading.Hide();
                bansStatusLoading.Hide();

                refreshBtn.Enabled = true;
            }
        }

        private async Task TradableStatusCheck()
        {
            try
            {
                SteamHelpClient helpClient = new SteamHelpClient();
                await helpClient.LoginAsync(client.Client.RefreshToken);
                var tradable = await SteamApi.TradableCheckAsync(helpClient.WebCookie);
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

                var partnerTradeLink = DefaultPartnerTradeLink ?? "";
                var partnerMatch = Regex.Match(partnerTradeLink, @"[&?]partner=(?<partner>\d+)([&]*?)");
                if (partnerMatch.Success)
                {
                    var partner = partnerMatch.Groups["partner"].Value;
                    var token = string.Empty;

                    var tokenMatch = Regex.Match(DefaultPartnerTradeLink, @"[&?]token=(?<token>.*)([&]*?)");
                    if (tokenMatch.Success)
                    {
                        token = tokenMatch.Groups["token"].Value;
                    }

                    var tradeHoldDurations = await SteamApi.QueryTradeHoldDurationsAsync(null,
                    client.Client.WebApiToken,
                    Extension.GetSteamId(partner),
                    token);
                    var myEscrow = tradeHoldDurations.Body?.MyEscrow;
                    if (myEscrow?.EscrowEndDurationSeconds > 0)
                    {
                        var timespan = TimeSpan.FromSeconds(myEscrow.EscrowEndDurationSeconds);
                        string endDuration = "";
                        if (timespan.Days > 0)
                        {
                            endDuration = $"{endDuration}{timespan.Days}天";
                        }
                        if (timespan.Hours > 0)
                        {
                            endDuration = $"{endDuration}{timespan.Hours}小时";
                        }

                        tradeStatusBox.ForeColor = Color.OrangeRed;
                        tradeStatusBox.Text = $"出货交易暂挂 {endDuration}";
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

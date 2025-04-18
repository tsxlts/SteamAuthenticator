using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text;
using Steam_Authenticator.Controls;
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;
using static Steam_Authenticator.Internal.Utils;

namespace Steam_Authenticator.Forms
{
    public partial class Offers : Form
    {
        private readonly Form mainForm;
        private readonly UserClient client;
        private readonly SteamCommunityClient webClient;
        private bool refreshing = false;
        private IEnumerable<Offer> thisOffers = new List<Offer>();

        public Offers(Form mainForm, UserClient client)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.client = client;
            this.webClient = client.Client;

            Width = this.mainForm.Width;
            Height = this.mainForm.Height;
        }

        private async void Offers_Load(object sender, EventArgs e)
        {
            Location = this.mainForm.Location;
            mainForm.Hide();

            if (!webClient.LoggedIn)
            {
                return;
            }

            Text = $"交易报价 [{client.GetAccount()}]";

            receivedOffer.Checked = true;
            sentOffer.Checked = true;
            receivedOffer.Visible = false;
            sentOffer.Visible = false;

            await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            offersPanel.AutoScroll = true;
        }

        private void Offers_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Location = this.Location;
            mainForm.Show();
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        }

        private async void acceptAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                acceptAllBtn.Enabled = false;

                if (!webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录Steam帐号");
                    return;
                }
                if (thisOffers == null || !thisOffers.Any(c => !c.IsOurOffer))
                {
                    return;
                }

                if (MessageBox.Show("你确定要接受所有报价吗？", "接受报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                await HandleOffer(webClient, thisOffers.Where(c => !c.IsOurOffer), true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);

                await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                acceptAllBtn.Enabled = true;
                acceptAllBtn.Focus();
            }
        }

        private async void declineAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                declineAllBtn.Enabled = false;

                if (!webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录Steam帐号");
                    return;
                }
                if (thisOffers == null || !thisOffers.Any(c => !c.IsOurOffer))
                {
                    return;
                }

                if (MessageBox.Show("你确定要拒绝所有报价吗？", "拒绝报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                await HandleOffer(webClient, thisOffers.Where(c => !c.IsOurOffer), false, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);

                await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                declineAllBtn.Enabled = true;
                declineAllBtn.Focus();
            }
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            var button = (OfferButton)sender;
            try
            {
                button.Enabled = false;
                var offer = button.Offer;

                await HandleOffer(webClient, new[] { offer }, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);

                await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
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

        private async void btnDecline_Click(object sender, EventArgs e)
        {
            var button = (OfferButton)sender;
            try
            {
                button.Enabled = false;
                var offer = button.Offer;

                await HandleOffer(webClient, new[] { offer }, false, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);

                await RefreshOffers(new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
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

        private async void btnDetail_Click(object sender, EventArgs e)
        {
            Uri offerUrl = null;
            try
            {
                var setting = Appsetting.Instance.AppSetting.Entry;

                var button = (OfferButton)sender;
                var offer = button.Offer;

                Browser browser = new Browser()
                {
                    Text = "交易报价",
                    Width = 600,
                    Height = 400
                };
                browser.Show();

                if (!string.IsNullOrWhiteSpace(setting.Domain.SteamCommunity))
                {
                    offerUrl = new Uri($"{setting.Domain.SteamCommunity}/tradeoffer/{offer.TradeOfferId}/");
                }
                else
                {
                    offerUrl = new Uri($"{SteamBulider.DefaultSteamCommunity}/tradeoffer/{offer.TradeOfferId}/");
                }

                browser.SetCookies($"{offerUrl.Scheme}://{offerUrl.Host}", webClient.WebCookie.ToArray());
                await browser.LoadUrl(offerUrl);
            }
            catch (Exception ex)
            {
                Process.Start(new ProcessStartInfo(offerUrl.ToString()) { UseShellExecute = true });

                MessageBox.Show(ex.Message);
            }
        }

        private async Task RefreshOffers(CancellationToken cancellationToken)
        {
            if (refreshing)
            {
                return;
            }

            try
            {
                refreshing = true;

                refreshBtn.Text = "正在刷新";
                refreshBtn.Enabled = false;

                var queryOffers = await webClient.TradeOffer.QueryOffersAsync(sentOffer: true, receivedOffer: true, onlyActive: true,
                    cancellationToken: cancellationToken);

                var descriptions = queryOffers?.Descriptions ?? new List<BaseDescription>();
                var offers = new List<Offer>();

                if (sentOffer.Checked)
                {
                    offers.AddRange(queryOffers?.TradeOffersSent?.OrderBy(c => c.TimeCreated)?.ToList() ?? new List<Offer>());
                }
                if (receivedOffer.Checked)
                {
                    offers.AddRange(queryOffers?.TradeOffersReceived?.OrderBy(c => c.TimeCreated)?.ToList() ?? new List<Offer>());
                }
                thisOffers = offers;

                offersPanel.Controls.Clear();

                if (offers.Count == 0)
                {
                    Label errorLabel = new Label()
                    {
                        Text = "没有任何报价信息",
                        AutoSize = false,
                        ForeColor = Color.FromArgb(255, 140, 0),
                        TextAlign = ContentAlignment.TopCenter,
                        Dock = DockStyle.Fill,
                        Padding = new Padding(0, 20, 0, 0)
                    };
                    offersPanel.Controls.Add(errorLabel);
                    return;
                }

                IEnumerable<PlayerSummaries> playerSummaries = new List<PlayerSummaries>();

                try
                {
                    var queryPlayerSummaries = await webClient.User.QueryPlayerSummariesAsync(offers.Select(c => Extension.GetSteamId(c.AccountIdOther.ToString())), cancellationToken);
                    if (queryPlayerSummaries.Players?.Any() ?? false)
                    {
                        playerSummaries = queryPlayerSummaries.Players;
                    }
                }
                catch
                {

                }

                IEnumerable<BaseDescription> giveDescription;
                IEnumerable<BaseDescription> receiveDescription;
                StringBuilder nameBuilder;
                StringBuilder assetBuilder;
                StringBuilder statusBuilder;
                PlayerSummaries player;

                foreach (var offer in offers)
                {
                    giveDescription = descriptions.Where(c => offer.ItemsToGive?.Any(a => a.ClassId == c.ClassId && a.InstanceId == c.InstanceId) ?? false);
                    receiveDescription = descriptions.Where(c => offer.ItemsToReceive?.Any(a => a.ClassId == c.ClassId && a.InstanceId == c.InstanceId) ?? false);
                    player = playerSummaries.FirstOrDefault(c => c.SteamId == Extension.GetSteamId(offer.AccountIdOther.ToString()));

                    Panel panel = new Panel() { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding { Bottom = 10 } };
                    panel.Paint += (s, e) =>
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(panel.ClientRectangle, Color.FromArgb(255, 228, 181), Color.FromArgb(255, 255, 224), 90F))
                        {
                            e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                        }
                    };

                    if (!string.IsNullOrEmpty(player.AvatarMedium))
                    {
                        PictureBox pictureBox = new PictureBox() { Width = 60, Height = 60, Location = new Point(20, 20), SizeMode = PictureBoxSizeMode.Zoom };
                        pictureBox.LoadAsync(player.AvatarMedium);
                        panel.Controls.Add(pictureBox);
                    }

                    nameBuilder = new StringBuilder();
                    nameBuilder.AppendLine($"{new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(offer.TimeCreated).ToLocalTime():yyyy/MM/dd HH:mm:ss}" +
                        $"\n{(offer.IsOurOffer ? $"你向 {player?.SteamName ?? offer.AccountIdOther.ToString()} 发起报价" : $"你收到来自 {player?.SteamName ?? offer.AccountIdOther.ToString()} 的报价")}");
                    if (player != null)
                    {
                        nameBuilder.AppendLine($"{player.SteamName} {DateTime.UnixEpoch.AddSeconds(player.TimeCreated).ToLocalTime():yyyy 年 MM 月 dd 日 HH 时 mm 分} 加入Steam");
                    }

                    assetBuilder = new StringBuilder();
                    Color assetColor = Color.Green;
                    if (giveDescription?.Any() ?? false)
                    {
                        assetColor = Color.Red;
                        if (offer.ItemsToGive.Count > 1)
                        {
                            assetBuilder.AppendLine($"您将送出 {giveDescription.First().MarketName} 等多件物品");
                        }
                        else
                        {
                            assetBuilder.AppendLine($"您将送出 {giveDescription.First().MarketName}");
                        }
                    }
                    if (receiveDescription?.Any() ?? false)
                    {
                        if (offer.ItemsToReceive.Count > 1)
                        {
                            assetBuilder.AppendLine($"您将收到 {receiveDescription.First().MarketName} 等多件物品");
                        }
                        else
                        {
                            assetBuilder.AppendLine($"您将收到 {receiveDescription.First().MarketName} ");
                        }
                    }

                    statusBuilder = new StringBuilder();
                    Color statusColor = Color.FromArgb(0, 0, 238);
                    if (offer.IsOurOffer)
                    {
                        switch (offer.TradeOfferState)
                        {
                            case TradeOfferState.NeedsConfirmation:
                                statusBuilder.AppendLine($"等待你 {(offer.ConfirmationMethod == TradeOfferConfirmationMethod.Email ? "邮箱令牌确认" : "手机令牌确认")}");
                                statusColor = Color.FromArgb(238, 0, 238);
                                break;

                            default:
                                statusBuilder.AppendLine($"等待对方 接受报价");
                                break;
                        }
                    }
                    else
                    {
                        switch (offer.ConfirmationMethod)
                        {
                            case TradeOfferConfirmationMethod.Email:
                                statusBuilder.AppendLine("等待你 邮箱令牌确认");
                                statusColor = Color.FromArgb(238, 0, 238);
                                break;
                            case TradeOfferConfirmationMethod.MobileApp:
                                statusBuilder.AppendLine("等待你 手机令牌确认");
                                statusColor = Color.FromArgb(238, 0, 238);
                                break;

                            case TradeOfferConfirmationMethod.Invalid:
                            default:
                                statusBuilder.AppendLine($"等待你 接受报价");
                                break;
                        }
                    }

                    int y = 20;
                    Label nameLabel = new Label()
                    {
                        Text = $"{nameBuilder}",
                        AutoSize = true,
                        ForeColor = Color.DeepSkyBlue,// offer.IsOurOffer ? sentOffer.ForeColor : receivedOffer.ForeColor,
                        Location = new Point(90, y),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(nameLabel);

                    y = y + nameLabel.Height + 10;
                    Label assetLabel = new Label()
                    {
                        Text = $"{assetBuilder}",
                        AutoSize = true,
                        ForeColor = assetColor,
                        Location = new Point(90, y),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(assetLabel);

                    y = y + assetLabel.Height + 10;
                    Label summaryLabel = new Label()
                    {
                        Location = new Point(90, y),
                        AutoSize = false,
                        Height = 0
                    };
                    if (!string.IsNullOrWhiteSpace(offer.Message))
                    {
                        summaryLabel = new Label()
                        {
                            Text = $"{offer.Message}",
                            AutoSize = true,
                            ForeColor = Color.FromArgb(128, 128, 128),
                            Location = new Point(90, y),
                            BackColor = Color.Transparent
                        };
                        panel.Controls.Add(summaryLabel);
                    }

                    y = y + summaryLabel.Height + 10;
                    Label statusLabel = new Label()
                    {
                        Text = $"{statusBuilder}",
                        AutoSize = true,
                        ForeColor = statusColor,
                        Location = new Point(90, y),
                        BackColor = Color.Transparent
                    };
                    panel.Controls.Add(statusLabel);

                    y = y + statusLabel.Height + 10;
                    OfferButton acceptButton = new OfferButton()
                    {
                        Text = "接受",
                        Location = new Point(90, y),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0 },
                        BackColor = Color.FromArgb(102, 153, 255),
                        ForeColor = Color.Snow,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowOnly,
                        Offer = offer,
                        Visible = !offer.IsOurOffer
                    };
                    acceptButton.Click += btnAccept_Click;
                    panel.Controls.Add(acceptButton);

                    OfferButton cancelButton = new OfferButton()
                    {
                        Text = "拒绝",
                        Location = new Point(180, y),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0 },
                        BackColor = Color.FromArgb(102, 153, 255),
                        ForeColor = Color.Snow,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowOnly,
                        Offer = offer,
                        Visible = !offer.IsOurOffer
                    };
                    cancelButton.Click += btnDecline_Click;
                    panel.Controls.Add(cancelButton);

                    OfferButton detailButton = new OfferButton()
                    {
                        Text = "查看",
                        Location = new Point(270, y),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0 },
                        BackColor = Color.FromArgb(102, 153, 255),
                        ForeColor = Color.Snow,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowOnly,
                        Offer = offer,
                        Visible = !offer.IsOurOffer
                    };
                    detailButton.Click += btnDetail_Click;
                    panel.Controls.Add(detailButton);

                    offersPanel.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                refreshing = false;

                refreshBtn.Text = "刷新报价";
                refreshBtn.Enabled = true;
                refreshBtn.Focus();
            }
        }

        private async void offerRole_CheckedChanged(object sender, EventArgs e)
        {
            await RefreshOffers(default);
        }
    }
}

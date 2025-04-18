
using System.Text;
using Steam_Authenticator.Factory;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class TransferAssets : Form
    {
        private bool running = false;
        private CancellationTokenSource runningToken;

        private UserClient receiver;
        private List<UserClient> deliverers = new List<UserClient>();

        public TransferAssets()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private void TransferAssets_Load(object sender, EventArgs e)
        {
            receiverSendOffer.Checked = true;
            cs2Game.Checked = true;
            allAssets.Checked = true;
            autoConfirm.Checked = true;
        }

        private void TransferAssets_FormClosing(object sender, FormClosingEventArgs e)
        {
            runningToken?.Cancel();
        }

        private void autoAccept_CheckedChanged(object sender, EventArgs e)
        {
            if (autoAccept.Checked)
            {
                return;
            }

            autoConfirm.Checked = false;
        }

        private void autoConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (!autoConfirm.Checked)
            {
                return;
            }

            autoAccept.Checked = true;
        }

        private void selectReceiverBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (running)
            {
                return;
            }

            List<SelectOption> selectOptions = new List<SelectOption>();

            var users = Appsetting.Instance.Clients.ToArray();
            foreach (var user in users)
            {
                selectOptions.Add(new SelectOption
                {
                    Value = user.User.SteamId,
                    Text = $"{user.User.Account} ({user.User.NickName}) [{user.User.SteamId}]",
                    Checked = receiver?.User?.SteamId == user.User.SteamId
                });
            }

            var options = new Options("选择库存转入方", $"请选择 库存转入方Steam帐号 {Environment.NewLine}如果没有你想要的帐号, 请确认是否已登录你想要的帐号")
            {
                Width = this.Width - 20,
                Height = this.Height - 20,
                ItemSize = new Size(145, 20),
                Multiselect = false,
                Datas = selectOptions.OrderBy(c => c.Text).ToList(),
            };
            if (options.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var selected = options.Selected.FirstOrDefault();
            if (selected == null)
            {
                return;
            }

            receiver = Appsetting.Instance.Clients.FirstOrDefault(c => c.User.SteamId == selected.Value);
            if (receiver == null)
            {
                return;
            }

            receiverBox.Text = $"{receiver.User.Account} ({receiver.User.NickName}) [{receiver.User.SteamId}]";
        }

        private void selectDelivererBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (running)
            {
                return;
            }

            List<SelectOption> selectOptions = new List<SelectOption>();

            var users = Appsetting.Instance.Clients.ToArray();
            foreach (var user in users)
            {
                selectOptions.Add(new SelectOption
                {
                    Value = user.User.SteamId,
                    Text = $"{user.User.Account} ({user.User.NickName}) [{user.User.SteamId}]",
                    Checked = deliverers.Any(c => c.User.SteamId == user.User.SteamId)
                });
            }

            var options = new Options("选择库存转出方", $"请选择 库存转出方Steam帐号 {Environment.NewLine}如果没有你想要的帐号, 请确认是否已登录你想要的帐号")
            {
                Width = this.Width - 20,
                Height = this.Height - 20,
                ItemSize = new Size(145, 20),
                Multiselect = true,
                Datas = selectOptions.OrderBy(c => c.Text).ToList(),
            };
            if (options.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            deliverers.Clear();
            var selecteds = options.Selected;

            UserClient deliverer;
            foreach (var selected in selecteds)
            {
                deliverer = Appsetting.Instance.Clients.FirstOrDefault(c => c.User.SteamId == selected.Value);
                if (deliverer == null)
                {
                    return;
                }

                deliverers.Add(deliverer);
            }

            ReloadDeliverers();
        }

        private async void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                running = true;
                okBtn.Enabled = false;
                selectReceiverBtn.Enabled = false;
                selectDelivererBtn.Enabled = false;

                runningToken = new CancellationTokenSource();

                if (receiver == null)
                {
                    MessageBox.Show("请选择库存转入方Steam帐号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!receiver.Client.LoggedIn)
                {
                    MessageBox.Show("库存转入方Steam帐号未登录, 请先登录库存转入方Steam帐号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var gameId = "730";
                var contextId = "2";

                var taskHandlers = new List<Task<string>>();
                TaskFactory taskFactory = new TaskFactory();
                Task<string> taskHandler;
                Panel panel;

                int index = 0;
                int size = 10;
                StringBuilder stringBuilder = new StringBuilder();
                do
                {
                    if (runningToken.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        var group = deliverers.Skip(index * size).Take(size).ToArray();
                        if (group.Length == 0)
                        {
                            break;
                        }

                        taskHandlers.Clear();

                        foreach (var item in group)
                        {
                            if (runningToken.IsCancellationRequested)
                            {
                                break;
                            }

                            if (item.User.SteamId == receiver.User.SteamId)
                            {
                                continue;
                            }

                            taskHandler = taskFactory.StartNew((obj) =>
                            {
                                Label msgLabel = null;

                                try
                                {
                                    var deliverer = obj as UserClient;
                                    panel = deliverersPanel.Controls.Find(deliverer.User.SteamId, false)?.FirstOrDefault() as Panel;
                                    if (panel == null)
                                    {
                                        return string.Empty;
                                    }

                                    msgLabel = panel.Controls.Find("msg", false)?.FirstOrDefault() as Label;
                                    if (msgLabel == null)
                                    {
                                        return string.Empty;
                                    }

                                    if (!deliverer.Client.LoggedIn)
                                    {
                                        msgLabel.Text = "登录信息已失效, 请登录后重新操作";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }

                                    msgLabel.Text = "准备就绪...";

                                    msgLabel.Text = "正在查询库存...";
                                    var inventoryResponse = SteamApi.QueryPartnerInventoryAsync(receiver.Client.SessionId, partnerSteamId: deliverer.User.SteamId,
                                        partnerToken: "",
                                        appId: gameId,
                                        contextId: contextId,
                                        receiver.Client.WebCookie).GetAwaiter().GetResult();
                                    if (inventoryResponse.Body == null || !inventoryResponse.Body.Success)
                                    {
                                        msgLabel.Text = "检测到可能未公开库存, 正在公开库存...";
                                        var setPrivacySettingResponse = SteamApi.SetAccountPrivacySettingAsync(deliverer.Client.SessionId, deliverer.Client.SteamId, new AccountPrivacy
                                        {
                                            PrivacySettings = new AccountPrivacySettings
                                            {
                                                PrivacyProfile = SteamEnum.CommunityVisibilityState.公开,
                                                PrivacyInventory = SteamEnum.CommunityVisibilityState.公开,
                                                PrivacyFriendsList = SteamEnum.CommunityVisibilityState.私密,
                                                PrivacyInventoryGifts = SteamEnum.CommunityVisibilityState.私密,
                                                PrivacyOwnedGames = SteamEnum.CommunityVisibilityState.私密,
                                                PrivacyPlaytime = SteamEnum.CommunityVisibilityState.私密,
                                            },
                                            CommentPermission = SteamEnum.CommentPermission.私密,
                                        }, deliverer.Client.WebCookie).GetAwaiter().GetResult();
                                        if (setPrivacySettingResponse.Body?.Privacy?.PrivacySettings?.PrivacyInventory != SteamEnum.CommunityVisibilityState.公开)
                                        {
                                            msgLabel.Text = $"公开库存失败, 需要你手动公开库存";
                                            msgLabel.ForeColor = Color.Red;
                                            return string.Empty;
                                        }

                                        msgLabel.Text = "正在重新查询库存...";
                                        inventoryResponse = SteamApi.QueryPartnerInventoryAsync(receiver.Client.SessionId, partnerSteamId: deliverer.User.SteamId,
                                           partnerToken: "",
                                           appId: gameId,
                                           contextId: contextId,
                                           receiver.Client.WebCookie).GetAwaiter().GetResult();
                                        if (inventoryResponse.Body == null || !inventoryResponse.Body.Success)
                                        {
                                            msgLabel.Text = $"查询库存失败, 请确认是否已正常公开库存, {inventoryResponse.HttpStatusCode}";
                                            msgLabel.ForeColor = Color.Red;
                                            return string.Empty;
                                        }
                                    }

                                    if (!(inventoryResponse.Body.Inventories?.Any() ?? false))
                                    {
                                        msgLabel.Text = $"未查询到可交易的库存, 跳过处理";
                                        return string.Empty;
                                    }

                                    msgLabel.Text = "正在获取交易链接...";
                                    var tradeLinkResponse = SteamApi.GetTradeLinkAsync(deliverer.Client.SteamId, deliverer.Client.WebCookie).GetAwaiter().GetResult();
                                    if (string.IsNullOrWhiteSpace(tradeLinkResponse.Body))
                                    {
                                        msgLabel.Text = $"未获取到交易链接, 跳过处理, {tradeLinkResponse.HttpStatusCode}";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }
                                    Utils.CheckTradeLink(tradeLinkResponse.Body, out var partner, out var token);

                                    msgLabel.Text = "开始发送报价...";
                                    var currentAssets = new List<SendOfferAsset>();
                                    var partnerAssets = inventoryResponse.Body.Inventories.Select(c => new SendOfferAsset
                                    {
                                        AssetId = $"{c.AssetId}",
                                        AppId = gameId,
                                        ContextId = contextId,
                                        Amount = c.Amount,
                                    }).ToList();
                                    var sendOfferResponse = SteamApi.SendOfferAsync(receiver.Client.SessionId,
                                        deliverer.User.SteamId,
                                        receiverPartner: partner,
                                        receiverToken: token,
                                        currentAssets: currentAssets,
                                        receiverAssets: partnerAssets,
                                        postscript: $"转移库存到 {receiver.User.NickName}",
                                        receiver.Client.WebCookie).GetAwaiter().GetResult();
                                    if (string.IsNullOrWhiteSpace(sendOfferResponse.Body?.TradeOfferId))
                                    {
                                        msgLabel.Text = $"{sendOfferResponse.Body?.Error ?? $"发送报价失败, HttpStatusCode: {sendOfferResponse.HttpStatusCode}"}";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }
                                    msgLabel.Text = $"发送报价成功, 报价Id: {sendOfferResponse.Body.TradeOfferId}, 等待接受报价...";

                                    if (!autoAccept.Checked)
                                    {
                                        return string.Empty;
                                    }

                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 开始接受报价...";
                                    var accesptOfferResponse = SteamApi.AcceptOfferAsync(deliverer.Client.SessionId, sendOfferResponse.Body.TradeOfferId, deliverer.Client.WebCookie).GetAwaiter().GetResult();
                                    if (accesptOfferResponse.HttpStatusCode != System.Net.HttpStatusCode.OK || accesptOfferResponse.Body == null)
                                    {
                                        msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, {accesptOfferResponse.Body?.Error ?? $"接受报价失败, HttpStatusCode: {sendOfferResponse.HttpStatusCode}"}";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }
                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 接受报价成功, 等待{(accesptOfferResponse.Body.NeedMobileConfirmation ? "手机" : "邮箱")}令牌确认...";

                                    if (!accesptOfferResponse.Body.NeedMobileConfirmation)
                                    {
                                        return string.Empty;
                                    }
                                    if (!autoConfirm.Checked)
                                    {
                                        return string.Empty;
                                    }

                                    var guard = Appsetting.Instance.Manifest.GetGuard(deliverer.GetAccount());
                                    if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                                    {
                                        msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 未提供令牌信息, 需要你手动令牌确认...";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }

                                    int retry = 0;
                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 开始查询令牌确认信息...";
                                    Confirmation confirmation;
                                    do
                                    {
                                        var queryConfirmResponse = SteamApi.QueryConfirmationsAsync(deliverer.Client.SteamId,
                                            deviceId: guard.DeviceId,
                                            identitySecret: guard.IdentitySecret,
                                            deliverer.Client.WebCookie).GetAwaiter().GetResult();
                                        confirmation = queryConfirmResponse.Body?.Confirmations?.FirstOrDefault(c => $"{c.CreatorId}" == sendOfferResponse.Body.TradeOfferId);
                                        if (confirmation == null)
                                        {
                                            if ((++retry) < 3)
                                            {
                                                Thread.Sleep(1000);
                                                continue;
                                            }

                                            msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 查询令牌确认信息失败, 等待确认库存是否转移成功";
                                            msgLabel.ForeColor = Color.Red;
                                            goto CheckResult;
                                            return string.Empty;
                                        }

                                        break;
                                    } while (true);

                                    retry = 0;
                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId} [{confirmation.Id}], 开始令牌确认...";
                                    do
                                    {
                                        var confirmOfferResponse = SteamApi.AllowConfirmationAsync(deliverer.Client.SteamId,
                                            confirmationId: confirmation.Id,
                                            confirmationKey: confirmation.Key,
                                            deviceId: guard.DeviceId,
                                            identitySecret: guard.IdentitySecret,
                                            deliverer.Client.WebCookie).GetAwaiter().GetResult();
                                        if (confirmOfferResponse.HttpStatusCode != System.Net.HttpStatusCode.OK || !confirmOfferResponse.Body)
                                        {
                                            if ((++retry) < 3)
                                            {
                                                Thread.Sleep(1000);
                                                continue;
                                            }

                                            msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId} [{confirmation.Id}], 令牌确认失败, 等待确认库存是否转移成功";
                                            msgLabel.ForeColor = Color.Red;
                                            goto CheckResult;
                                            return string.Empty;
                                        }

                                        msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 库存转移成功";
                                        msgLabel.ForeColor = Color.Green;
                                        return string.Empty;
                                    } while (true);

                                CheckResult:
                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 正在确认库存转移结果...";
                                    inventoryResponse = SteamApi.QueryPartnerInventoryAsync(receiver.Client.SessionId, partnerSteamId: deliverer.User.SteamId,
                                        partnerToken: "",
                                        appId: gameId,
                                        contextId: contextId,
                                        receiver.Client.WebCookie).GetAwaiter().GetResult();
                                    if (inventoryResponse.Body == null || !inventoryResponse.Body.Success)
                                    {
                                        msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 确认库存转移结果失败, 前往库存页面查看库存是否转移成功";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }

                                    if (inventoryResponse.Body.Inventories?.Any(c => partnerAssets.Any(p => p.AssetId == $"{c.AssetId}")) ?? false)
                                    {
                                        msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 需要你手动处理报价";
                                        msgLabel.ForeColor = Color.Red;
                                        return string.Empty;
                                    }

                                    msgLabel.Text = $"报价Id: {sendOfferResponse.Body.TradeOfferId}, 库存转移成功";
                                    msgLabel.ForeColor = Color.Green;
                                    return string.Empty;
                                }
                                catch (Exception ex)
                                {
                                    AppLogger.Instance.Error(ex);

                                    msgLabel.Text = $"处理异常, {ex.Message}";

                                    return string.Empty;
                                }
                            }, item);
                            taskHandlers.Add(taskHandler);
                        }

                        await Task.WhenAll(taskHandlers);
                    }
                    catch
                    {
                    }

                    index++;
                } while (true);

                MessageBox.Show("操作完成, 请查看处理进度是否有异常, 前往库存页面查看库存是否转移成功", "转移库存", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作异常, 请前往库存页面查看库存是否转移成功" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "转移库存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                running = false;
                okBtn.Enabled = true;
                selectReceiverBtn.Enabled = true;
                selectDelivererBtn.Enabled = true;
            }
        }

        private void ReloadDeliverers()
        {
            deliverersPanel.Controls.Clear();
            int lineWith = deliverersPanel.Width - 25;
            Color[] colors = [Color.PaleTurquoise, Color.Wheat];
            int y = 0;
            int index = 0;
            foreach (var deliverer in deliverers)
            {
                var line = new Panel
                {
                    Name = deliverer.User.SteamId,
                    Location = new Point(5, y),
                    Width = lineWith,
                    Height = 23,
                    BackColor = colors[(index++) % colors.Length]
                };

                var userLabel = new Label
                {
                    Name = "user",
                    Text = $"{deliverer.User.Account} ({deliverer.User.NickName}) [{deliverer.User.SteamId}]",
                    AutoSize = false,
                    AutoEllipsis = true,
                    Size = new Size(250, 16),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(5, 3),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                };
                line.Controls.Add(userLabel);

                var deleteBtn = new Label
                {
                    Name = deliverer.User.SteamId,
                    Text = $"删除",
                    AutoSize = false,
                    Size = new Size(40, 16),
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(line.Width - 45, 3),
                    Cursor = Cursors.Hand,
                    ForeColor = Color.Gray,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                };
                deleteBtn.Click += (object sender, EventArgs e) =>
                {
                    if (running)
                    {
                        return;
                    }

                    deliverers.Remove(deliverer);
                    ReloadDeliverers();
                };
                line.Controls.Add(deleteBtn);

                var msgLabel = new Label
                {
                    Name = "msg",
                    Text = $"",
                    AutoSize = false,
                    AutoEllipsis = true,
                    Size = new Size(lineWith - userLabel.Width - deleteBtn.Width - 20, 16),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Color.Blue,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                };
                msgLabel.Location = new Point(deleteBtn.Location.X - msgLabel.Width - 5, 3);
                line.Controls.Add(msgLabel);

                deliverersPanel.Controls.Add(line);

                y = y + 25;
            }
        }
    }
}

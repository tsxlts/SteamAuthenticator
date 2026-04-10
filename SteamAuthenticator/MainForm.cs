using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Factory;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Handler;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly Version currentVersion;
        private readonly TaskFactory taskFactory;

        private readonly ConcurrentDictionary<string, ConfirmationsPopup> confirmationsPopupDialogs;

        private readonly System.Threading.Timer refreshMsgTimer;
        private readonly TimeSpan refreshMsgTimerMinPeriod = TimeSpan.FromSeconds(10);

        private readonly System.Threading.Timer refreshClientInfoTimer;
        private readonly TimeSpan refreshClientInfoTimerMinPeriod = TimeSpan.FromSeconds(60);

        private readonly System.Threading.Timer refreshUserTimer;
        private readonly TimeSpan refreshUserTimerMinPeriod = TimeSpan.FromSeconds(60);

        private readonly System.Threading.Timer checkVersionTimer;
        private readonly System.Threading.Timer reportTimer;

        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);

        private readonly ContextMenuStrip mainNotifyMenuStrip;
        private readonly ContextMenuStrip userContextMenuStrip;

        private bool showBalloonTip = true;
        private UserClient currentClient = null;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            Appsetting.Instance.AppSetting.WithChanged((s, e) =>
            {
                usersPanel.RefreshClients();
            });
            Appsetting.Instance.Manifest.WithChanged((s, e) =>
            {
                usersPanel.RefreshClients();
            });

            var match = Regex.Match(Application.ProductVersion, @"^[\d.]+");
            currentVersion = new Version(match.Value);
            versionLabel.Text = $"v{currentVersion}";

            taskFactory = new TaskFactory();

            confirmationsPopupDialogs = new ConcurrentDictionary<string, ConfirmationsPopup>();

            refreshMsgTimer = new System.Threading.Timer(RefreshMsg, null, -1, -1);
            refreshClientInfoTimer = new System.Threading.Timer(RefreshClientInfo, null, -1, -1);
            refreshUserTimer = new System.Threading.Timer(RefreshUser, null, -1, -1);
            checkVersionTimer = new System.Threading.Timer((obj) =>
            {
                try
                {
                    CheckVersion().GetAwaiter().GetResult();
                }
                catch
                {

                }
            }, null, -1, -1);
            reportTimer = new System.Threading.Timer((obj) =>
            {
                try
                {
                    Report().GetAwaiter().GetResult();
                }
                catch
                {

                }
            }, null, -1, -1);

            var usersPanelContextMenuStrip = new ContextMenuStrip();
            usersPanelContextMenuStrip.Items.Add("刷新").Click += (send, e) =>
            {
                usersPanel.Reset();
            };
            usersPanelContextMenuStrip.Items.Add("添加帐号").Click += addUserBtn_Click;
            usersPanel.ContextMenuStrip = usersPanelContextMenuStrip;

            mainNotifyMenuStrip = new ContextMenuStrip();
            mainNotifyMenuStrip.Items.Add("打开").Click += (sender, e) =>
            {
                this.ShowForm();
            };
            mainNotifyMenuStrip.Items.Add("退出").Click += (sender, e) =>
            {
                this.mainNotifyIcon.Visible = false;
                this.Close();
                this.Dispose();
                Environment.Exit(Environment.ExitCode);
            };

            userContextMenuStrip = new ContextMenuStrip();
            userContextMenuStrip.Items.Add("切换").Click += setCurrentClientMenuItem_Click;
            userContextMenuStrip.Items.Add("设置").Click += settingMenuItem_Click;
            userContextMenuStrip.Items.Add("帐号信息").Click += accountInfoMenuItem_Click;
            userContextMenuStrip.Items.Add("Steam库存").Click += inventoryMenuItem_Click;
            userContextMenuStrip.Items.Add("复制Cookie").Click += copyCookieMenuItem_Click;
            userContextMenuStrip.Items.Add("复制AccessToken").Click += copyAccessTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("复制RefreshToken").Click += copyRefreshTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("重新登录").Click += reloginMenuItem_Click;
            userContextMenuStrip.Items.Add("退出登录").Click += logoutMenuItem_Click;
            userContextMenuStrip.Items.Add("移除帐号").Click += removeUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            mainNotifyIcon.ContextMenuStrip = mainNotifyMenuStrip;

            IEnumerable<IUserPanelHandler> userPanelHandlers = new List<IUserPanelHandler>
            {
                new BUFFUserPanelHandler(buffUsersPanel),
                new ECOUserPanelHandler(ecoUsersPanel),
                new YouPinUserPanelHandler(youpinUsersPanel),
                new C5UserPanelHandler(c5UsersPanel),
            };

            var loadUsers = new List<Task> { LoadUsers() };
            loadUsers.AddRange(userPanelHandlers.Select(c => c.LoadUsersAsync()));

            await CheckNetwork();

            await Task.WhenAll(loadUsers);

            await Task.Run(() =>
            {
                try
                {
                    var setup = new FileInfo(Appsetting.Instance.SetupApplication);
                    var install = new FileInfo(Appsetting.Instance.Install);
                    if (install.Exists)
                    {
                        if (setup.Directory.Exists)
                        {
                            Directory.Delete(setup.DirectoryName, true);
                        }

                        install.Directory.MoveTo(setup.DirectoryName);
                    }
                }
                catch
                {

                }
            });

            checkVersionTimer.Change(TimeSpan.Zero, TimeSpan.FromHours(3));
            reportTimer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(30));

            await ShowTips().ConfigureAwait(false);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();

                this.mainNotifyIcon.Visible = true;
                if (showBalloonTip)
                {
                    showBalloonTip = false;
                    this.mainNotifyIcon.ShowBalloonTip(3000, "提示", "应用程序以缩小到托盘，双击图标可打开窗口", ToolTipIcon.Info);
                }
                return;
            }

            Dispose();
        }

        private void mainNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowForm();
        }

        private void RefreshMsg(object _)
        {
            try
            {
                List<Task> tasks = new List<Task>();

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    tasks.Add(QueryWalletDetails(tokenSource.Token));

                    Task.WaitAll(tasks.ToArray());
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshMsgTimer(TimeSpan.FromSeconds(2), refreshMsgTimerMinPeriod);
            }
        }

        private void RefreshClientInfo(object _)
        {
            var setting = Appsetting.Instance.AppSetting.Entry;

            try
            {
                List<Task> tasks = new List<Task>();

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    tasks.Add(QueryOffers(tokenSource.Token));
                    tasks.Add(QueryConfirmations(tokenSource.Token));

                    Task.WaitAll(tasks.ToArray());
                }
            }
            catch
            {

            }
            finally
            {
                TimeSpan dueTime = TimeSpan.FromSeconds(Math.Max(1, setting.AutoRefreshInternal));
                TimeSpan period = TimeSpan.FromSeconds(Math.Max(refreshClientInfoTimerMinPeriod.TotalSeconds, setting.AutoRefreshInternal * 2));
                ResetRefreshClientInfoTimer(dueTime, period);
            }
        }

        private async Task QueryWalletDetails(CancellationToken cancellationToken)
        {
            if (currentClient == null)
            {
                return;
            }

            try
            {
                var webClient = currentClient.Client;

                var walletDetails = await webClient.User.QueryWalletDetailsAsync(cancellationToken);

                if (!(walletDetails?.HasWallet ?? false))
                {
                    Balance.Text = "---";
                    DelayedBalance.Text = "---";
                    return;
                }

                Balance.Text = $"{walletDetails.FormattedBalance}";
                DelayedBalance.Text = $"{walletDetails.FormattedDelayedBalance}";
            }
            catch
            {
            }
        }

        private async Task QueryOffers(CancellationToken cancellationToken)
        {
            try
            {
                var setting = Appsetting.Instance.AppSetting.Entry;
                if (!setting.PeriodicCheckingConfirmation)
                {
                    return;
                }

                List<Task> tasks = new List<Task>();
                var checkClients = Appsetting.Instance.Clients.Where(c => c.User.Setting.PeriodicCheckingConfirmation).ToList();
                var buffClients = Appsetting.Instance.BuffClients;
                var ecoClients = Appsetting.Instance.EcoClients;
                var youpinClients = Appsetting.Instance.YouPinClients;
                var c5Clients = Appsetting.Instance.C5Clients;
                foreach (var itemClient in checkClients)
                {
                    if (itemClient == null)
                    {
                        continue;
                    }

                    var task = taskFactory.StartNew((obj) =>
                    {
                        var client = obj as UserClient;
                        var webClient = client.Client;
                        var user = client.User;
                        var buffClient = buffClients.FirstOrDefault(c => c.User.SteamId == user.SteamId);
                        var ecoClient = ecoClients.FirstOrDefault(c => c.User.SteamIds?.Contains(user.SteamId) ?? false);
                        var youpinClient = youpinClients.FirstOrDefault(c => c.User.SteamId == user.SteamId);
                        var c5Client = c5Clients.FirstOrDefault(c => c.User.SteamIds?.Contains(user.SteamId) ?? false);

                        bool acceptAll = user.Setting.AutoAcceptGiveOffer;
                        bool accpetBuff = acceptAll || user.Setting.AutoAcceptGiveOffer_Buff;
                        bool accectEco = acceptAll || user.Setting.AutoAcceptGiveOffer_Eco;
                        bool accectYouPin = acceptAll || user.Setting.AutoAcceptGiveOffer_YouPin;
                        bool accectC5 = acceptAll || user.Setting.AutoAcceptGiveOffer_C5;
                        bool accpetOther = acceptAll || user.Setting.AutoAcceptGiveOffer_Other;
                        bool acceptCustom = acceptAll || user.Setting.AutoAcceptGiveOffer_Custom;

                        bool confirmAll = user.Setting.AutoConfirmTrade;
                        bool confirmBuff = confirmAll || user.Setting.AutoConfirmTrade_Buff;
                        bool confirmEco = confirmAll || user.Setting.AutoConfirmTrade_Eco;
                        bool confirmYouPin = confirmAll || user.Setting.AutoConfirmTrade_YouPin;
                        bool confirmC5 = confirmAll || user.Setting.AutoConfirmTrade_C5;
                        bool confirmOther = confirmAll || user.Setting.AutoConfirmTrade_Other;
                        bool confirmCustom = confirmAll || user.Setting.AutoConfirmTrade_Custom;

                        List<Offer> receivedOffers = new List<Offer>();
                        List<Offer> sentOffer = new List<Offer>();
                        List<Offer> autoConfirmOffers = new List<Offer>();

                        int? buffOfferCount = null;
                        int? ecoOfferCount = null;
                        int? youpinOfferCount = null;
                        int? c5OfferCount = null;
                        try
                        {
                            if (!webClient.LoggedIn)
                            {
                                return;
                            }

                            var queryOffers = webClient.TradeOffer.QueryOffersAsync(sentOffer: true, receivedOffer: true, onlyActive: true, cancellationToken: cancellationToken).Result;
                            var descriptions = queryOffers?.Descriptions ?? new List<BaseDescription>();
                            receivedOffers = queryOffers?.TradeOffersReceived ?? new List<Offer>();
                            sentOffer = queryOffers?.TradeOffersSent ?? new List<Offer>();

                            var receiveOffers = receivedOffers.Where(c => !(c.ItemsToGive?.Any() ?? false));

                            var giveOffers = new List<Offer>();
                            var reveiceGiveOffers = receivedOffers.Where(c => c.ItemsToGive?.Any() ?? false);
                            var sentGiveOffers = sentOffer.Where(c => c.ItemsToGive?.Any() ?? false);
                            giveOffers.AddRange(reveiceGiveOffers);
                            giveOffers.AddRange(sentGiveOffers);

                            AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###查询 Steam 报价###" +
                                $"{Environment.NewLine}发货报价: [{string.Join(",", giveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}收货报价: [{string.Join(",", receiveOffers.Select(c => c.TradeOfferId))}]");

                            if (receiveOffers.Any() && user.Setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }

                            var customGiveOffers = new List<Offer>();
                            var buffGiveOffers = new List<Offer>();
                            var ecoGiveOffers = new List<Offer>();
                            var youpinGiveOffers = new List<Offer>();
                            var c5GiveOffers = new List<Offer>();
                            var otherGiveOffers = giveOffers.ToList();

                            #region 自定义报价
                            if (giveOffers.Any())
                            {
                                var rules = (new[]
                                {
                                    user.Setting.AutoAcceptGiveOfferRule.OfferMessage,
                                    user.Setting.AutoAcceptGiveOfferRule.AssetName
                                }).Where(c => c.Enabled);

                                foreach (var offer in giveOffers)
                                {
                                    foreach (var rule in rules)
                                    {
                                        if (customGiveOffers.Any(c => c.TradeOfferId == offer.TradeOfferId))
                                        {
                                            break;
                                        }

                                        switch (rule.Type)
                                        {
                                            case AcceptOfferRuleSetting.RuleType.报价消息:
                                                string message = offer.Message ?? "";
                                                if (rule.Check(message))
                                                {
                                                    customGiveOffers.Add(offer);
                                                }
                                                break;

                                            case AcceptOfferRuleSetting.RuleType.饰品名称:
                                                var itemDescriptions = descriptions.Where(c => offer.ItemsToGive.Any(a => c.ClassId == a.ClassId && c.InstanceId == a.InstanceId)).ToList();
                                                if (itemDescriptions.Count > 0 && itemDescriptions.All(d => rule.Check(d.MarketName) || rule.Check(d.MarketHashName)))
                                                {
                                                    customGiveOffers.Add(offer);
                                                }
                                                break;
                                        }
                                    }
                                }

                                otherGiveOffers.RemoveAll(o => customGiveOffers.Any(c => c.TradeOfferId == o.TradeOfferId));
                            }
                            #endregion

                            #region BUFF 报价
                            buffOfferCount = 0;
                            if (giveOffers.Any() && buffClient != null)
                            {
                                buffOfferCount = null;
                                var buffOffer = buffClient.QuerySteamTrade().Result;
                                if (buffOffer.Body?.IsSuccess ?? false)
                                {
                                    var buffOfferIds = buffOffer.Body.data?.Select(c => c.tradeofferid)?.ToList() ?? new List<string>();
                                    buffGiveOffers = giveOffers.Where(c => buffOfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();

                                    otherGiveOffers.RemoveAll(c => buffOfferIds.Contains(c.TradeOfferId));

                                    buffOfferCount = receivedOffers.Count(c => buffOfferIds.Any(offerId => c.TradeOfferId == offerId));
                                }
                                else
                                {
                                    otherGiveOffers = new List<Offer>();
                                }

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###查询 BUFF 报价###" +
                                    $"{Environment.NewLine}BUFF响应: httpStatusCode:{buffOffer.HttpStatusCode}, code:{buffOffer.Body?.code}, msg:{buffOffer.Body?.msg}, error:{buffOffer.Body?.error}" +
                                    $"{Environment.NewLine}订单报价信息: [{string.Join(",", buffOffer.Body?.data?.Select(c => $"{c.id}#{c.tradeofferid}") ?? new List<string>())}]" +
                                    $"{Environment.NewLine}发货报价: [{string.Join(",", buffGiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            #region ECO 报价
                            ecoOfferCount = 0;
                            if (giveOffers.Any() && ecoClient != null)
                            {
                                ecoOfferCount = null;

                                var gameIds = descriptions.Select(c => c.AppId).Distinct();
                                var queryOfferTasks = gameIds.Select(gameId => ecoClient.QueryOffers(gameId));
                                var queryEcoOffers = Task.WhenAll(queryOfferTasks).ContinueWith(offerTasks =>
                                {
                                    var results = offerTasks.Result;
                                    var errorResult = results.FirstOrDefault(c => !(c?.IsSuccess ?? false));
                                    if (errorResult != null)
                                    {
                                        return errorResult;
                                    }

                                    List<Model.ECO.QueryOffersResponse> list = new List<Model.ECO.QueryOffersResponse>();
                                    foreach (var item in results)
                                    {
                                        list.AddRange(item.StatusData.ResultData);
                                    }

                                    var successResult = results.First();
                                    successResult.StatusData.ResultData = list;
                                    return successResult;
                                });

                                var ecoOffer = queryEcoOffers.Result;
                                if (ecoOffer?.IsSuccess ?? false)
                                {
                                    var ecoOfferIds = ecoOffer.StatusData?.ResultData
                                    ?.Where(c => c.CurrentSteamId == user.SteamId && !string.IsNullOrWhiteSpace(c.OfferId))
                                    ?.Select(c => c.OfferId)?.ToList() ?? new List<string>();
                                    ecoGiveOffers = giveOffers.Where(c => ecoOfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();

                                    otherGiveOffers.RemoveAll(c => ecoOfferIds.Contains(c.TradeOfferId));

                                    ecoOfferCount = receivedOffers.Count(c => ecoOfferIds.Any(offerId => c.TradeOfferId == offerId));
                                }
                                else
                                {
                                    otherGiveOffers = new List<Offer>();
                                }

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###查询 ECO 报价###" +
                                    $"{Environment.NewLine}ECO响应: code:{ecoOffer?.StatusData?.ResultCode}, msg:{ecoOffer?.StatusData?.ResultMsg}" +
                                    $"{Environment.NewLine}订单报价信息: [{string.Join(",", ecoOffer?.StatusData?.ResultData?.Select(c => $"{c.OrderNum}#{c.OfferId}") ?? new List<string>())}]" +
                                    $"{Environment.NewLine}发货报价: [{string.Join(",", ecoGiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            #region 悠悠报价
                            youpinOfferCount = 0;
                            if (giveOffers.Any() && youpinClient != null)
                            {
                                youpinOfferCount = null;
                                var youpinOffer = youpinClient.GetOfferList().Result;
                                if (youpinOffer.Body?.IsSuccess() ?? false)
                                {
                                    var youpinOfferIds = youpinOffer.Body.GetData()?.orderInfoList?.Select(c => c.offerId)?.ToList() ?? new List<string>();
                                    youpinGiveOffers = giveOffers.Where(c => youpinOfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();

                                    otherGiveOffers.RemoveAll(c => youpinOfferIds.Contains(c.TradeOfferId));

                                    youpinOfferCount = receivedOffers.Count(c => youpinOfferIds.Any(offerId => c.TradeOfferId == offerId));
                                }
                                else
                                {
                                    otherGiveOffers = new List<Offer>();
                                }

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###查询 悠悠 报价###" +
                                   $"{Environment.NewLine}悠悠响应: httpStatusCode:{youpinOffer.HttpStatusCode}, code:{youpinOffer.Body?.GetCode()}, msg:{youpinOffer.Body?.GetMsg()}" +
                                   $"{Environment.NewLine}订单报价信息: [{string.Join(",", youpinOffer.Body?.GetData()?.orderInfoList?.Select(c => $"{c.orderNo}#{c.offerId}") ?? new List<string>())}]" +
                                   $"{Environment.NewLine}发货报价: [{string.Join(",", youpinGiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            #region C5报价
                            c5OfferCount = 0;
                            if (giveOffers.Any() && c5Client != null)
                            {
                                c5OfferCount = null;
                                var c5Offer = c5Client.CheckOffers(giveOffers.Select(c => c.TradeOfferId)).Result;
                                if (c5Offer.Body?.success ?? false)
                                {
                                    var c5OfferIds = c5Offer.Body.data ?? new List<string>();
                                    c5GiveOffers = giveOffers.Where(c => c5OfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();

                                    otherGiveOffers.RemoveAll(c => c5OfferIds.Contains(c.TradeOfferId));

                                    c5OfferCount = receivedOffers.Count(c => c5OfferIds.Any(offerId => c.TradeOfferId == offerId));
                                }
                                else
                                {
                                    otherGiveOffers = new List<Offer>();
                                }

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###查询 C5Game 报价###" +
                                   $"{Environment.NewLine}C5响应: httpStatusCode:{c5Offer.HttpStatusCode}, code:{c5Offer.Body?.errorCode}, msg:{c5Offer.Body?.errorMsg}" +
                                   $"{Environment.NewLine}订单报价信息: [{string.Join(",", c5Offer.Body?.data?.Select(c => $"{c}") ?? new List<string>())}]" +
                                   $"{Environment.NewLine}发货报价: [{string.Join(",", c5GiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            List<Offer> autoAcceptOffers = new List<Offer>();
                            if (acceptAll)
                            {
                                var acceptItemOffers = giveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && acceptCustom)
                            {
                                var acceptItemOffers = customGiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accpetBuff)
                            {
                                var acceptItemOffers = buffGiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accectEco)
                            {
                                var acceptItemOffers = ecoGiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accectYouPin)
                            {
                                var acceptItemOffers = youpinGiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accectC5)
                            {
                                var acceptItemOffers = c5GiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accpetOther)
                            {
                                var acceptItemOffers = otherGiveOffers.ToList();
                                autoAcceptOffers.AddRange(acceptItemOffers);
                            }
                            autoAcceptOffers = autoAcceptOffers.GroupBy(c => c.TradeOfferId).Select(c => c.First()).ToList();

                            var acceptOffes = autoAcceptOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                            HandleOffer(webClient, acceptOffes, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();

                            if (confirmAll)
                            {
                                autoConfirmOffers.AddRange(giveOffers);
                            }
                            if (!confirmAll && confirmCustom)
                            {
                                autoConfirmOffers.AddRange(customGiveOffers);
                            }
                            if (!confirmAll && confirmBuff)
                            {
                                autoConfirmOffers.AddRange(buffGiveOffers);
                            }
                            if (!confirmAll && confirmEco)
                            {
                                autoConfirmOffers.AddRange(ecoGiveOffers);
                            }
                            if (!confirmAll && confirmYouPin)
                            {
                                autoConfirmOffers.AddRange(youpinGiveOffers);
                            }
                            if (!confirmAll && confirmC5)
                            {
                                autoConfirmOffers.AddRange(c5GiveOffers);
                            }
                            if (!confirmAll && confirmOther)
                            {
                                autoConfirmOffers.AddRange(otherGiveOffers);
                            }

                            autoConfirmOffers = autoConfirmOffers.GroupBy(c => c.TradeOfferId).Select(c => c.First()).ToList();

                            AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###发货报价结果###" +
                                $"{Environment.NewLine}自定义发货报价: [{string.Join(",", customGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}BUFF发货报价: [{string.Join(",", buffGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}ECO发货报价: [{string.Join(",", ecoGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}悠悠发货报价: [{string.Join(",", youpinGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}C5发货报价: [{string.Join(",", c5GiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}其他发货报价: [{string.Join(",", otherGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}自动接受报价: [{string.Join(",", autoAcceptOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}自动确认报价: [{string.Join(",", autoConfirmOffers.Select(c => c.TradeOfferId))}]");
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        catch (AggregateException)
                        {
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Instance.Error(ex);
                            AppLogger.Instance.Debug("queryOffer", client.User.SteamId, $"###查询报价信息失败###" +
                                $"{Environment.NewLine}异常信息: {ex.Message}");
                        }
                        finally
                        {
                            client.SetAutoConfirmOffers(autoConfirmOffers);
                            client.SetReceivedOffers(receivedOffers);

                            usersPanel.SetOffer(client, receivedOffers.Count);
                            buffUsersPanel.SetOffer(buffClient, buffOfferCount);
                            ecoUsersPanel.SetOffer(ecoClient, ecoOfferCount);
                            youpinUsersPanel.SetOffer(youpinClient, youpinOfferCount);
                            c5UsersPanel.SetOffer(c5Client, c5OfferCount);

                            if (user.SteamId == currentClient?.User.SteamId)
                            {
                                OfferCountLabel.Text = $"{receivedOffers.Count}";
                            }
                        }
                    }, itemClient);
                    tasks.Add(task);
                }

                var notCheckClients = Appsetting.Instance.Clients.Where(c => !c.User.Setting.PeriodicCheckingConfirmation).ToList();
                tasks.Add(Task.Run(() =>
                {
                    foreach (var client in notCheckClients)
                    {
                        if (client == null)
                        {
                            continue;
                        }

                        if (client.User.SteamId == currentClient?.User.SteamId)
                        {
                            OfferCountLabel.Text = $"---";
                        }

                        usersPanel.SetOffer(client, null);

                        var buffClinet = buffClients.FirstOrDefault(c => c.User.SteamId == client.User.SteamId);
                        if (buffClinet != null)
                        {
                            buffUsersPanel.SetOffer(buffClinet, null);
                        }

                        var ecoClient = ecoClients.FirstOrDefault(c => c.User.SteamIds?.Contains(client.User.SteamId) ?? false);
                        if (ecoClient != null)
                        {
                            ecoUsersPanel.SetOffer(ecoClient, null);
                        }
                    }
                }));

                await Task.WhenAll(tasks);
            }
            catch
            {

            }
        }

        private async Task QueryConfirmations(CancellationToken cancellationToken)
        {
            var setting = Appsetting.Instance.AppSetting.Entry;
            if (!setting.PeriodicCheckingConfirmation)
            {
                return;
            }

            List<Task> tasks = new List<Task>();
            var checkClients = Appsetting.Instance.Clients.Where(c => c.User.Setting.PeriodicCheckingConfirmation).ToList();
            foreach (var itemClient in checkClients)
            {
                if (itemClient == null)
                {
                    continue;
                }

                var task = taskFactory.StartNew((obj) =>
                {
                    var client = obj as UserClient;
                    int? confirmationCount = null;
                    try
                    {
                        var webClient = client.Client;
                        var user = client.User;

                        if (!webClient.LoggedIn)
                        {
                            return;
                        }

                        var confirmationPopup = confirmationsPopupDialogs.GetOrAdd(client.User.SteamId, new ConfirmationsPopup(client, [])
                        {
                            StartPosition = FormStartPosition.CenterScreen
                        });

                        var steamNotifications = SteamApi.QuerySteamNotificationsAsync(webClient.WebApiToken, includeHidden: false,
                            includeConfirmation: true,
                            includePinned: false,
                            includeRead: false,
                            countOnly: false,
                            language: webClient.Language).GetAwaiter().GetResult();

                        var steamNotificationsBody = steamNotifications.Body;
                        confirmationCount = steamNotificationsBody?.ConfirmationCount;

                        AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###查询待确认信息###" +
                            $"{Environment.NewLine}Steam响应: httpStatusCode:{steamNotifications.HttpStatusCode}" +
                            $"{Environment.NewLine}待确认事项数量: {confirmationCount}");

                        if (steamNotificationsBody != null && confirmationCount == 0)
                        {
                            this.Invoke(() =>
                            {
                                confirmationPopup.SetConfirmations([]);
                                confirmationPopup.Hide();
                            });
                            return;
                        }

                        Guard guard = Appsetting.Instance.Manifest.GetGuard(client.GetAccount());
                        if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                        {
                            return;
                        }

                        var queryConfirmations = webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret, cancellationToken).Result;
                        if (!(queryConfirmations?.Success ?? false))
                        {
                            AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###查询待确认信息失败###" +
                                $"{Environment.NewLine}Steam响应: {JsonConvert.SerializeObject(queryConfirmations)}");
                            return;
                        }

                        var confirmations = queryConfirmations.Confirmations ?? new List<Confirmation>();
                        var autoConfirm = new List<Confirmation>();
                        var waitConfirm = new List<Confirmation>();

                        confirmationCount = confirmations.Count;

                        foreach (var conf in confirmations.Where(c => c.ConfType != ConfirmationType.Trade))
                        {
                            if ((conf.ConfType == ConfirmationType.MarketListing && user.Setting.AutoConfirmMarket))
                            {
                                autoConfirm.Add(conf);
                                continue;
                            }

                            waitConfirm.Add(conf);
                        }

                        var autoConfirmOffers = client.AutoConfirmOffers;
                        foreach (var conf in confirmations.Where(c => c.ConfType == ConfirmationType.Trade))
                        {
                            if (autoConfirmOffers.Any(o => o.TradeOfferId == $"{conf.CreatorId}"))
                            {
                                autoConfirm.Add(conf);
                                continue;
                            }

                            waitConfirm.Add(conf);
                        }

                        if (autoConfirm.Any())
                        {
                            try
                            {
                                using (var maxcts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                                {
                                    using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, maxcts.Token))
                                    {
                                        bool accept = HandleConfirmation(webClient, guard, autoConfirm, true, cts.Token).Result;
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }

                        if (waitConfirm.Any() && setting.ConfirmationAutoPopup)
                        {
                            this.Invoke(() =>
                            {
                                confirmationPopup.SetConfirmations(waitConfirm);
                                confirmationPopup.Show();
                            });
                        }

                        if (!waitConfirm.Any())
                        {
                            this.Invoke(() =>
                            {
                                confirmationPopup.SetConfirmations([]);
                                confirmationPopup.Hide();
                            });
                        }

                        AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###查询待确认信息结果###" +
                            $"{Environment.NewLine}已开启自动确认数据: [{string.Join(",", autoConfirm.Select(c => $"{c.ConfTypeName}#{c.CreatorId}#{c.Id}"))}]" +
                            $"{Environment.NewLine}未开启自动确认数据: [{string.Join(",", waitConfirm.Select(c => $"{c.ConfTypeName}#{c.CreatorId}#{c.Id}"))}]");
                    }
                    catch (OperationCanceledException)
                    {

                    }
                    catch (AggregateException)
                    {
                    }
                    catch (Exception ex)
                    {
                        AppLogger.Instance.Error(ex);
                        AppLogger.Instance.Debug("queryConfirmation", client.User.SteamId, $"###查询待确认信息失败###" +
                            $"{Environment.NewLine}异常信息: {ex.Message}");
                    }
                    finally
                    {
                        usersPanel.SetConfirmation(client, confirmationCount);

                        if (client.User.SteamId == currentClient?.User.SteamId)
                        {
                            ConfirmationCountLable.Text = $"{confirmationCount ?? 0}";
                        }
                    }
                }, itemClient);
                tasks.Add(task);
            }

            var notCheckClients = Appsetting.Instance.Clients.Where(c => !c.User.Setting.PeriodicCheckingConfirmation).ToList();
            tasks.Add(Task.Run(() =>
            {
                foreach (var client in notCheckClients)
                {
                    if (client == null)
                    {
                        continue;
                    }

                    if (client.User.SteamId == currentClient?.User.SteamId)
                    {
                        ConfirmationCountLable.Text = $"---";
                    }

                    usersPanel.SetConfirmation(client, null);
                }
            }));

            await Task.WhenAll(tasks);
        }

        private void RefreshUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = usersPanel.ItemPanels;
                    foreach (SteamUserPanel userPanel in controlCollection)
                    {
                        if (!userPanel.HasItem)
                        {
                            continue;
                        }

                        var userClient = userPanel.Client;
                        userClient.RefreshClientAsync(tokenSource.Token).GetAwaiter().GetResult();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshUserTimer(refreshUserTimerMinPeriod, refreshUserTimerMinPeriod * 1.5);
            }
        }

        private void ResetRefreshMsgTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshMsgTimer.Change(dueTime, period);
        }

        private void ResetRefreshClientInfoTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshClientInfoTimer.Change(dueTime, period);
        }

        private void ResetRefreshUserTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshUserTimer.Change(dueTime, period);
        }

        private async Task<bool> CheckVersion()
        {
            if (!checkVersionLocker.Wait(0))
            {
                return true;
            }

            try
            {
                var result = await SteamApi.GetAsync<JObject>(ProjectInfo.LatestRelease);
                var resultObj = result.Body;
                if (!(resultObj?.TryGetValue("tag_name", out var tag_name) ?? false))
                {
                    return false;
                }

                var match = Regex.Match(tag_name.Value<string>(), @"[\d.]+");
                var newVersion = new Version(match.Value);

                if (currentVersion < newVersion)
                {
                    var assets = resultObj.Value<JArray>("assets");
                    string name = assets.FirstOrDefault()?.Value<string>("name");
                    string updateUrl = assets.FirstOrDefault()?.Value<string>("browser_download_url");
                    string body = resultObj.Value<string>("body");
                    DateTime published = resultObj.Value<DateTime>("published_at");
                    if (!string.IsNullOrWhiteSpace(updateUrl))
                    {
                        this.Invoke(() =>
                        {
                            ApplicationUpgrade applicationUpgrade = new ApplicationUpgrade(currentVersion, newVersion, published, body, updateUrl, updateUrl, name);
                            DialogResult updateDialog = applicationUpgrade.ShowDialog();
                        });
                    }
                    return true;
                }
            }
            catch
            {

            }
            finally
            {
                checkVersionLocker.Release();
            }

            return false;
        }

        private async Task Report()
        {
            try
            {
                string machineId = Helper.GetMachineGuid();
                if (string.IsNullOrEmpty(machineId))
                {
                    machineId = Helper.GetMachineUniqueId();
                }
                var steamIds = Appsetting.Instance.Clients.Select(c => c.User.SteamId);
                var buffIds = Appsetting.Instance.BuffClients.Select(c => c.User.UserId);
                var ecoIds = Appsetting.Instance.EcoClients.Select(c => c.User.UserId);
                var youpinIds = Appsetting.Instance.YouPinClients.Select(c => c.User.UserId);
                var c5Ids = Appsetting.Instance.C5Clients.Select(c => c.User.UserId);
                var response = await AuthenticatorApi.Report(version: currentVersion, machineId, steamIds, buffIds, ecoIds, youpinIds, c5Ids);
            }
            catch
            {
            }
        }

        internal void ShowForm()
        {
            if (this.Visible)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }

                this.Activate();
                return;
            }

            this.CenterToScreen();
            this.Show();
            this.Activate();
        }

        private Task ShowTips()
        {
            try
            {
                if (Appsetting.Instance.AppSetting.Entry.TipVersion >= Appsetting.Instance.LastTipVersion)
                {
                    return Task.CompletedTask;
                }

                new About(currentVersion).ShowDialog();

                Appsetting.Instance.AppSetting.Entry.TipVersion = Appsetting.Instance.LastTipVersion;
                Appsetting.Instance.AppSetting.Save();
            }
            catch
            {
            }

            return Task.CompletedTask;
        }

        private async Task<bool> CheckNetwork()
        {
            try
            {
                await SteamApi.GetSteamIdAsync(new CookieCollection());
            }
            catch (HttpRequestException ex)
            {
                this.Invoke(() =>
                {
                    MessageBox.Show($"访问Steam网络失败" +
                    $"{Environment.NewLine}" +
                    $"请确保你已经开启加速器, 并将加速器设置为“路由模式”" +
                    $"{Environment.NewLine}" +
                    $"如果你已经设置了代理, 请确保代理服务器可用" +
                    $"{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"错误信息：" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                return false;
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Error(ex);
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            refreshUserTimer.Dispose();
            refreshClientInfoTimer.Dispose();
            foreach (var client in Appsetting.Instance.Clients)
            {
                client.Client.Dispose();
            }
        }
    }
}


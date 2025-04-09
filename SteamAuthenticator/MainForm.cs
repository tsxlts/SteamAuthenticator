using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Handler;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;
using System.Text.RegularExpressions;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly Version currentVersion;
        private readonly TaskFactory taskFactory;

        private readonly System.Threading.Timer refreshMsgTimer;
        private readonly TimeSpan refreshMsgTimerMinPeriod = TimeSpan.FromSeconds(10);

        private readonly System.Threading.Timer refreshClientInfoTimer;
        private readonly TimeSpan refreshClientInfoTimerMinPeriod = TimeSpan.FromSeconds(60);

        private readonly System.Threading.Timer refreshUserTimer;
        private readonly System.Threading.Timer checkVersionTimer;

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

            var report = Report();

            IEnumerable<IUserPanelHandler> userPanelHandlers = new List<IUserPanelHandler>
            {
                new BUFFUserPanelHandler(buffUsersPanel),
                new ECOUserPanelHandler(ecoUsersPanel),
                new YouPinUserPanelHandler(youpinUsersPanel),
            };

            var loadUsers = new List<Task> { LoadUsers() };
            loadUsers.AddRange(userPanelHandlers.Select(c => c.LoadUsersAsync()));

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
                    tasks.Add(QueryAuthSessionsForAccount(tokenSource.Token));
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

        private async Task QueryAuthSessionsForAccount(CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();

            var userClients = Appsetting.Instance.Clients;
            foreach (var itemClient in userClients)
            {
                if (itemClient == null)
                {
                    continue;
                }
                if (!itemClient.LoginConfirmLocker.Wait(0))
                {
                    continue;
                }

                var task = taskFactory.StartNew((obj) =>
                {
                    var userClient = obj as UserClient;
                    try
                    {
                        var webClient = userClient.Client;

                        Guard guard = Appsetting.Instance.Manifest.GetGuard(userClient.GetAccount());
                        if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
                        {
                            return;
                        }

                        var queryAuthSessions = SteamAuthentication.QueryAuthSessionsForAccountAsync(webClient.WebApiToken, cancellationToken).GetAwaiter().GetResult();
                        var clients = queryAuthSessions.Body?.ClientIds;
                        if (clients?.Count > 0)
                        {
                            var querySession = SteamAuthentication.QueryAuthSessionInfoAsync(webClient.WebApiToken, clients[0], cancellationToken).GetAwaiter().GetResult();
                            var sessionInfo = querySession.Body;
                            if (sessionInfo == null)
                            {
                                return;
                            }

                            this.Invoke(() =>
                            {
                                string clientType = sessionInfo.PlatformType switch
                                {
                                    var platform when platform == AuthTokenPlatformType.SteamClient => "SteamClient",
                                    var platform when platform == AuthTokenPlatformType.MobileApp => "Steam App",
                                    var platform when platform == AuthTokenPlatformType.WebBrowser => "网页浏览器",
                                    _ => "未知设备"
                                };
                                var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                                MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(userClient, (ulong)clients[0], sessionInfo.Version);
                                mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{userClient.GetAccount()} 有新的登录请求";
                                mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                                mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP 地址：{sessionInfo.IP}";
                                mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("，", regions)}";

                                mobileConfirmationLogin.ShowDialog();
                            });
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        userClient?.LoginConfirmLocker.Release();
                    }
                }, itemClient);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
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
                var youpinClinets = Appsetting.Instance.YouPinClients;
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
                        var buffClinet = buffClients.FirstOrDefault(c => c.User.SteamId == user.SteamId);
                        var ecoClient = ecoClients.FirstOrDefault(c => c.User.SteamIds?.Contains(user.SteamId) ?? false);
                        var youpinClient = youpinClinets.FirstOrDefault(c => c.User.SteamId == user.SteamId);

                        bool acceptAll = user.Setting.AutoAcceptGiveOffer;
                        bool accpetBuff = acceptAll || user.Setting.AutoAcceptGiveOffer_Buff;
                        bool accectEco = acceptAll || user.Setting.AutoAcceptGiveOffer_Eco;
                        bool accectYouPin = acceptAll || user.Setting.AutoAcceptGiveOffer_YouPin;
                        bool accpetOther = acceptAll || user.Setting.AutoAcceptGiveOffer_Other;
                        bool acceptCustom = acceptAll || user.Setting.AutoAcceptGiveOffer_Custom;

                        bool confirmAll = user.Setting.AutoConfirmTrade;
                        bool confirmBuff = confirmAll || user.Setting.AutoConfirmTrade_Buff;
                        bool confirmEco = confirmAll || user.Setting.AutoConfirmTrade_Eco;
                        bool confirmYouPin = confirmAll || user.Setting.AutoConfirmTrade_YouPin;
                        bool confirmOther = confirmAll || user.Setting.AutoConfirmTrade_Other;
                        bool confirmCustom = confirmAll || user.Setting.AutoConfirmTrade_Custom;

                        List<Offer> receivedOffers = new List<Offer>();
                        List<Offer> sentOffer = new List<Offer>();
                        List<Offer> autoConfirmOffers = new List<Offer>();

                        int? buffOfferCount = null;
                        int? ecoOfferCount = null;
                        int? youpinOfferCount = null;
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
                            if (receiveOffers.Any() && user.Setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }

                            var reveiceGiveOffers = receivedOffers.Where(c => c.ItemsToGive?.Any() ?? false);
                            var sentGiveOffers = sentOffer.Where(c => c.ItemsToGive?.Any() ?? false);

                            var giveOffers = new List<Offer>();
                            giveOffers.AddRange(reveiceGiveOffers);
                            giveOffers.AddRange(sentGiveOffers);

                            var customGiveOffers = new List<Offer>();
                            var buffGiveOffers = new List<Offer>();
                            var ecoGiveOffers = new List<Offer>();
                            var youpinGiveOffers = new List<Offer>();
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
                            if (giveOffers.Any() && buffClinet != null)
                            {
                                buffOfferCount = null;
                                var buffOffer = buffClinet.QuerySteamTrade().Result;
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
                            }
                            #endregion

                            List<Offer> acceptOffers = new List<Offer>();
                            if (acceptAll)
                            {
                                var acceptItemOffers = giveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && acceptCustom)
                            {
                                var acceptItemOffers = customGiveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accpetBuff)
                            {
                                var acceptItemOffers = buffGiveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accectEco)
                            {
                                var acceptItemOffers = ecoGiveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accectYouPin)
                            {
                                var acceptItemOffers = youpinGiveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }
                            if (!acceptAll && accpetOther)
                            {
                                var acceptItemOffers = otherGiveOffers.Where(c => !c.IsOurOffer && c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                acceptOffers.AddRange(acceptItemOffers);
                            }

                            acceptOffers = acceptOffers.GroupBy(c => c.TradeOfferId).Select(c => c.First()).ToList();
                            HandleOffer(webClient, acceptOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();

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
                            if (!confirmAll && confirmOther)
                            {
                                autoConfirmOffers.AddRange(otherGiveOffers);
                            }

                            autoConfirmOffers = autoConfirmOffers.GroupBy(c => c.TradeOfferId).Select(c => c.First()).ToList();
                        }
                        catch
                        {
                        }
                        finally
                        {
                            client.SetAutoConfirmOffers(autoConfirmOffers);
                            client.SetReceivedOffers(receivedOffers);

                            usersPanel.SetOffer(client, receivedOffers.Count);
                            buffUsersPanel.SetOffer(buffClinet, buffOfferCount);
                            ecoUsersPanel.SetOffer(ecoClient, ecoOfferCount);
                            youpinUsersPanel.SetOffer(youpinClient, youpinOfferCount);

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

                if (!itemClient.ConfirmationPopupLocker.Wait(0))
                {
                    return;
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

                        var steamNotifications = SteamApi.QuerySteamNotificationsAsync(webClient.WebApiToken, includeHidden: false,
                            includeConfirmation: true,
                            includePinned: false,
                            includeRead: false,
                            countOnly: false,
                            language: webClient.Language).GetAwaiter().GetResult();
                        var steamNotificationsBody = steamNotifications.Body;

                        confirmationCount = steamNotificationsBody?.ConfirmationCount;
                        if (steamNotificationsBody != null && confirmationCount == 0)
                        {
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
                                ConfirmationsPopup confirmationPopup = new ConfirmationsPopup(client, waitConfirm);
                                confirmationPopup.ShowDialog();
                            });
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        client.ConfirmationPopupLocker.Release();

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

                        var userClinet = userPanel.Client;
                        var client = userClinet.Client;
                        var user = userClinet.User;

                        var palyerSummaries = SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId }, cancellationToken: tokenSource.Token).GetAwaiter().GetResult();
                        if (palyerSummaries.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            userClinet.LogoutAsync().GetAwaiter().GetResult();
                        }

                        bool reloadCurrent = false;
                        if (client.LoggedIn)
                        {
                            userPanel.SetItemName(userPanel.ItemDisplayName, Color.Green);
                        }
                        else
                        {
                            userPanel.SetItemName(userPanel.ItemDisplayName, Color.Red);

                            reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                        }

                        var player = palyerSummaries.Body?.Players?.FirstOrDefault();
                        if (player != null)
                        {
                            if (player.SteamName != user.NickName || player.AvatarFull != user.Avatar)
                            {
                                user.NickName = player.SteamName;
                                user.Avatar = player.AvatarFull;
                                Appsetting.Instance.Manifest.SaveSteamUser(client.SteamId, user);

                                userPanel.SetItemIcon(user.Avatar);

                                reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                            }
                        }

                        if (reloadCurrent)
                        {
                            SetCurrentClient(userClinet, true);
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshUserTimer(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
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
                var response = await AuthenticatorApi.Report(version: currentVersion, machineId);
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


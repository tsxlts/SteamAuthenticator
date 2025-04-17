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
            usersPanelContextMenuStrip.Items.Add("ˢ��").Click += (send, e) =>
            {
                usersPanel.Reset();
            };
            usersPanelContextMenuStrip.Items.Add("����ʺ�").Click += addUserBtn_Click;
            usersPanel.ContextMenuStrip = usersPanelContextMenuStrip;

            mainNotifyMenuStrip = new ContextMenuStrip();
            mainNotifyMenuStrip.Items.Add("��").Click += (sender, e) =>
            {
                this.ShowForm();
            };
            mainNotifyMenuStrip.Items.Add("�˳�").Click += (sender, e) =>
            {
                this.mainNotifyIcon.Visible = false;
                this.Close();
                this.Dispose();
                Environment.Exit(Environment.ExitCode);
            };

            userContextMenuStrip = new ContextMenuStrip();
            userContextMenuStrip.Items.Add("�л�").Click += setCurrentClientMenuItem_Click;
            userContextMenuStrip.Items.Add("����").Click += settingMenuItem_Click;
            userContextMenuStrip.Items.Add("�ʺ���Ϣ").Click += accountInfoMenuItem_Click;
            userContextMenuStrip.Items.Add("Steam���").Click += inventoryMenuItem_Click;
            userContextMenuStrip.Items.Add("����Cookie").Click += copyCookieMenuItem_Click;
            userContextMenuStrip.Items.Add("����AccessToken").Click += copyAccessTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("����RefreshToken").Click += copyRefreshTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("���µ�¼").Click += reloginMenuItem_Click;
            userContextMenuStrip.Items.Add("�˳���¼").Click += logoutMenuItem_Click;
            userContextMenuStrip.Items.Add("�Ƴ��ʺ�").Click += removeUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            mainNotifyIcon.ContextMenuStrip = mainNotifyMenuStrip;

            IEnumerable<IUserPanelHandler> userPanelHandlers = new List<IUserPanelHandler>
            {
                new BUFFUserPanelHandler(buffUsersPanel),
                new ECOUserPanelHandler(ecoUsersPanel),
                new YouPinUserPanelHandler(youpinUsersPanel),
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
                    this.mainNotifyIcon.ShowBalloonTip(3000, "��ʾ", "Ӧ�ó�������С�����̣�˫��ͼ��ɴ򿪴���", ToolTipIcon.Info);
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
                                    var platform when platform == AuthTokenPlatformType.WebBrowser => "��ҳ�����",
                                    _ => "δ֪�豸"
                                };
                                var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                                MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(userClient, (ulong)clients[0], sessionInfo.Version);
                                mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{userClient.GetAccount()} ���µĵ�¼����";
                                mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                                mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP ��ַ��{sessionInfo.IP}";
                                mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("��", regions)}";

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
                        var buffClient = buffClients.FirstOrDefault(c => c.User.SteamId == user.SteamId);
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

                            var giveOffers = new List<Offer>();
                            var reveiceGiveOffers = receivedOffers.Where(c => c.ItemsToGive?.Any() ?? false);
                            var sentGiveOffers = sentOffer.Where(c => c.ItemsToGive?.Any() ?? false);
                            giveOffers.AddRange(reveiceGiveOffers);
                            giveOffers.AddRange(sentGiveOffers);

                            AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###��ѯ Steam ����###" +
                                $"{Environment.NewLine}��������: [{string.Join(",", giveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}�ջ�����: [{string.Join(",", receiveOffers.Select(c => c.TradeOfferId))}]");

                            if (receiveOffers.Any() && user.Setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }

                            var customGiveOffers = new List<Offer>();
                            var buffGiveOffers = new List<Offer>();
                            var ecoGiveOffers = new List<Offer>();
                            var youpinGiveOffers = new List<Offer>();
                            var otherGiveOffers = giveOffers.ToList();

                            #region �Զ��屨��
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
                                            case AcceptOfferRuleSetting.RuleType.������Ϣ:
                                                string message = offer.Message ?? "";
                                                if (rule.Check(message))
                                                {
                                                    customGiveOffers.Add(offer);
                                                }
                                                break;

                                            case AcceptOfferRuleSetting.RuleType.��Ʒ����:
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

                            #region BUFF ����
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

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###��ѯ BUFF ����###" +
                                    $"{Environment.NewLine}BUFF��Ӧ: httpStatusCode:{buffOffer.HttpStatusCode}, code:{buffOffer.Body?.code}, msg:{buffOffer.Body?.msg}, error:{buffOffer.Body?.error}" +
                                    $"{Environment.NewLine}����������Ϣ: [{string.Join(",", buffOffer.Body?.data?.Select(c => $"{c.id}#{c.tradeofferid}") ?? new List<string>())}]" +
                                    $"{Environment.NewLine}��������: [{string.Join(",", buffGiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            #region ECO ����
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

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###��ѯ ECO ����###" +
                                    $"{Environment.NewLine}ECO��Ӧ: code:{ecoOffer?.StatusData?.ResultCode}, msg:{ecoOffer?.StatusData?.ResultMsg}" +
                                    $"{Environment.NewLine}����������Ϣ: [{string.Join(",", ecoOffer?.StatusData?.ResultData?.Select(c => $"{c.OrderNum}#{c.OfferId}") ?? new List<string>())}]" +
                                    $"{Environment.NewLine}��������: [{string.Join(",", ecoGiveOffers.Select(c => c.TradeOfferId))}]");
                            }
                            #endregion

                            #region ���Ʊ���
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

                                AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###��ѯ ���� ����###" +
                                   $"{Environment.NewLine}������Ӧ: httpStatusCode:{youpinOffer.HttpStatusCode}, code:{youpinOffer.Body?.GetCode()}, msg:{youpinOffer.Body?.GetMsg()}" +
                                   $"{Environment.NewLine}����������Ϣ: [{string.Join(",", youpinOffer.Body?.GetData()?.orderInfoList?.Select(c => $"{c.orderNo}#{c.offerId}") ?? new List<string>())}]" +
                                   $"{Environment.NewLine}��������: [{string.Join(",", youpinGiveOffers.Select(c => c.TradeOfferId))}]");
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
                            if (!confirmAll && confirmOther)
                            {
                                autoConfirmOffers.AddRange(otherGiveOffers);
                            }

                            autoConfirmOffers = autoConfirmOffers.GroupBy(c => c.TradeOfferId).Select(c => c.First()).ToList();

                            AppLogger.Instance.Debug("queryOffer", user.SteamId, $"###�������۽��###" +
                                $"{Environment.NewLine}�Զ��巢������: [{string.Join(",", customGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}BUFF��������: [{string.Join(",", buffGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}ECO��������: [{string.Join(",", ecoGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}���Ʒ�������: [{string.Join(",", youpinGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}������������: [{string.Join(",", otherGiveOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}�Զ����ܱ���: [{string.Join(",", autoAcceptOffers.Select(c => c.TradeOfferId))}]" +
                                $"{Environment.NewLine}�Զ�ȷ�ϱ���: [{string.Join(",", autoConfirmOffers.Select(c => c.TradeOfferId))}]");
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
                            AppLogger.Instance.Debug("queryOffer", client.User.SteamId, $"###��ѯ������Ϣʧ��###" +
                                $"{Environment.NewLine}�쳣��Ϣ: {ex.Message}");
                        }
                        finally
                        {
                            client.SetAutoConfirmOffers(autoConfirmOffers);
                            client.SetReceivedOffers(receivedOffers);

                            usersPanel.SetOffer(client, receivedOffers.Count);
                            buffUsersPanel.SetOffer(buffClient, buffOfferCount);
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

                        AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###��ѯ��ȷ����Ϣ###" +
                            $"{Environment.NewLine}Steam��Ӧ: httpStatusCode:{steamNotifications.HttpStatusCode}" +
                            $"{Environment.NewLine}��ȷ����������: {confirmationCount}");

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
                            AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###��ѯ��ȷ����Ϣʧ��###" +
                                $"{Environment.NewLine}Steam��Ӧ: {JsonConvert.SerializeObject(queryConfirmations)}");
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

                        AppLogger.Instance.Debug("queryConfirmation", user.SteamId, $"###��ѯ��ȷ����Ϣ���###" +
                            $"{Environment.NewLine}�ѿ����Զ�ȷ������: [{string.Join(",", autoConfirm.Select(c => $"{c.ConfTypeName}#{c.CreatorId}#{c.Id}"))}]" +
                            $"{Environment.NewLine}δ�����Զ�ȷ������: [{string.Join(",", waitConfirm.Select(c => $"{c.ConfTypeName}#{c.CreatorId}#{c.Id}"))}]");
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
                        AppLogger.Instance.Debug("queryConfirmation", client.User.SteamId, $"###��ѯ��ȷ����Ϣʧ��###" +
                            $"{Environment.NewLine}�쳣��Ϣ: {ex.Message}");
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
                var response = await AuthenticatorApi.Report(version: currentVersion, machineId, steamIds, buffIds, ecoIds, youpinIds);
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
                    MessageBox.Show($"����Steam����ʧ��" +
                    $"{Environment.NewLine}" +
                    $"��ȷ�����Ѿ�����������, ��������������Ϊ��·��ģʽ��" +
                    $"{Environment.NewLine}" +
                    $"������Ѿ������˴���, ��ȷ���������������" +
                    $"{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"������Ϣ��" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


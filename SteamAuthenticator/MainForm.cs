using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
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
        private readonly System.Threading.Timer refreshMsgTimer;
        private readonly System.Threading.Timer refreshClientInfoTimer;
        private readonly System.Threading.Timer refreshUserTimer;
        private readonly System.Threading.Timer refreshBuffUserTimer;
        private readonly System.Threading.Timer refreshEcoUserTimer;
        private readonly TimeSpan refreshMsgTimerMinPeriod = TimeSpan.FromSeconds(10);
        private readonly TimeSpan refreshClientInfoTimerMinPeriod = TimeSpan.FromSeconds(60);
        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);
        private readonly ContextMenuStrip mainNotifyMenuStrip;
        private readonly ContextMenuStrip userContextMenuStrip;
        private readonly ContextMenuStrip buffUserContextMenuStrip;
        private readonly ContextMenuStrip ecoUserContextMenuStrip;

        private bool showBalloonTip = true;
        private string initialDirectory = null;
        private UserClient currentClient = null;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            var match = Regex.Match(Application.ProductVersion, @"^[\d.]+");
            currentVersion = new Version(match.Value);
            versionLabel.Text = $"v{currentVersion}";

            refreshMsgTimer = new System.Threading.Timer(RefreshMsg, null, -1, -1);
            refreshClientInfoTimer = new System.Threading.Timer(RefreshClientInfo, null, -1, -1);
            refreshUserTimer = new System.Threading.Timer(RefreshUser, null, -1, -1);
            refreshBuffUserTimer = new System.Threading.Timer(RefreshBuffUser, null, -1, -1);
            refreshEcoUserTimer = new System.Threading.Timer(RefreshEcoUser, null, -1, -1);

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
            userContextMenuStrip.Items.Add("����Cookie").Click += copyCookieMenuItem_Click;
            userContextMenuStrip.Items.Add("����AccessToken").Click += copyAccessTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("����RefreshToken").Click += copyRefreshTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("���µ�¼").Click += reloginMenuItem_Click;
            userContextMenuStrip.Items.Add("�˳���¼").Click += logoutMenuItem_Click;
            userContextMenuStrip.Items.Add("�Ƴ��ʺ�").Click += removeUserMenuItem_Click;

            buffUserContextMenuStrip = new ContextMenuStrip();
            buffUserContextMenuStrip.Items.Add("���µ�¼").Click += buffReloginMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("�˳���¼").Click += buffLogoutMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("�Ƴ��ʺ�").Click += removeBuffUserMenuItem_Click;

            ecoUserContextMenuStrip = new ContextMenuStrip();
            ecoUserContextMenuStrip.Items.Add("���µ�¼").Click += ecoReloginMenuItem_Click;
            ecoUserContextMenuStrip.Items.Add("�˳���¼").Click += ecoLogoutMenuItem_Click;
            ecoUserContextMenuStrip.Items.Add("�Ƴ��ʺ�").Click += removeEcoUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            mainNotifyIcon.ContextMenuStrip = mainNotifyMenuStrip;

            await Task.WhenAll(LoadUsers(), LoadBuffUsers(), LoadEcoUsers());

            var user = Appsetting.Instance.Clients?.FirstOrDefault(c => c.User.SteamId == Appsetting.Instance.AppSetting.Entry.CurrentUser);
            user = user ?? Appsetting.Instance.Clients?.FirstOrDefault();
            if (user != null)
            {
                SetCurrentClient(user);
            }

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

            await CheckVersion();
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
            if (currentClient == null || !currentClient.LoginConfirmLocker.Wait(0))
            {
                return;
            }
            try
            {
                var webClient = currentClient.Client;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(currentClient.GetAccount());
                if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
                {
                    return;
                }

                var queryAuthSessions = await SteamAuthentication.QueryAuthSessionsForAccountAsync(webClient.WebApiToken, cancellationToken);
                var clients = queryAuthSessions.Body?.ClientIds;
                if (clients?.Count > 0)
                {
                    var querySession = await SteamAuthentication.QueryAuthSessionInfoAsync(webClient.WebApiToken, clients[0], cancellationToken);
                    var sessionInfo = querySession.Body;
                    if (sessionInfo == null)
                    {
                        return;
                    }

                    string clientType = sessionInfo.PlatformType switch
                    {
                        var platform when platform == AuthTokenPlatformType.SteamClient => "SteamClient",
                        var platform when platform == AuthTokenPlatformType.MobileApp => "Steam App",
                        var platform when platform == AuthTokenPlatformType.WebBrowser => "��ҳ�����",
                        _ => "δ֪�豸"
                    };
                    var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                    MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(currentClient, (ulong)clients[0], sessionInfo.Version);
                    mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{currentClient.GetAccount()} ���µĵ�¼����";
                    mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                    mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP ��ַ��{sessionInfo.IP}";
                    mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("��", regions)}";

                    mobileConfirmationLogin.ShowDialog();
                }
            }
            catch
            {
            }
            finally
            {
                currentClient?.LoginConfirmLocker.Release();
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
                foreach (var client in checkClients)
                {
                    if (client == null)
                    {
                        continue;
                    }

                    var task = Task.Run(() =>
                    {
                        var webClient = client.Client;
                        var user = client.User;
                        var buffClinet = buffClients.FirstOrDefault(c => c.User.SteamId == user.SteamId);

                        bool acceptAll = user.Setting.AutoAcceptGiveOffer;
                        bool accpetBuff = acceptAll || user.Setting.AutoAcceptGiveOffer_Buff;
                        bool accpetOther = acceptAll || user.Setting.AutoAcceptGiveOffer_Other;

                        bool confirmAll = user.Setting.AutoConfirmTrade;
                        bool confirmBuff = confirmAll || user.Setting.AutoConfirmTrade_Buff;
                        bool confirmOther = confirmAll || user.Setting.AutoConfirmTrade_Other;

                        List<Offer> tradeOffers = new List<Offer>();
                        List<Offer> buffGiveOffers = null;
                        List<Offer> otherGiveOffers = null;

                        List<Offer> autoConfirmOffers = new List<Offer>();

                        try
                        {
                            if (!webClient.LoggedIn)
                            {
                                return;
                            }

                            var queryOffers = webClient.TradeOffer.QueryOffersAsync(sentOffer: false, receivedOffer: true, onlyActive: true, cancellationToken: cancellationToken).Result;
                            var descriptions = queryOffers?.Descriptions ?? new List<BaseDescription>();
                            tradeOffers = queryOffers?.TradeOffersReceived ?? new List<Offer>();

                            var receiveOffers = tradeOffers.Where(c => !(c.ItemsToGive?.Any() ?? false));
                            var giveOffers = tradeOffers.Where(c => c.ItemsToGive?.Any() ?? false);

                            if (receiveOffers.Any() && user.Setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }

                            if (user.Setting.AutoAcceptGiveOffer_Custom)
                            {
                                var rules = new[]
                                {
                                    user.Setting.AutoAcceptGiveOfferRule.OfferMessage,
                                    user.Setting.AutoAcceptGiveOfferRule.AssetName
                                }.Where(c => c.Enabled);

                                List<Offer> customOffers = new List<Offer>();
                                foreach (var offer in giveOffers)
                                {
                                    foreach (var rule in rules)
                                    {
                                        if (customOffers.Any(c => c.TradeOfferId == offer.TradeOfferId))
                                        {
                                            break;
                                        }

                                        switch (rule.Type)
                                        {
                                            case AcceptOfferRuleSetting.RuleType.������Ϣ:
                                                string message = offer.Message ?? "";
                                                if (rule.Check(message))
                                                {
                                                    customOffers.Add(offer);
                                                }
                                                break;

                                            case AcceptOfferRuleSetting.RuleType.��Ʒ����:
                                                var itemDescriptions = descriptions.Where(c => offer.ItemsToGive.Any(a => c.ClassId == a.ClassId && c.InstanceId == a.InstanceId)).ToList();
                                                if (itemDescriptions.Count > 0 && itemDescriptions.All(d => rule.Check(d.MarketName) || rule.Check(d.MarketHashName)))
                                                {
                                                    customOffers.Add(offer);
                                                }
                                                break;
                                        }
                                    }
                                }

                                if (customOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid))
                                {
                                    var acceptOffers = customOffers.Where(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                    HandleOffer(webClient, acceptOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                                }

                                //AutoConfirmOffer(client, customOffers, cancellationToken);
                                client.SetAutoConfirmOffers(customOffers);
                                return;
                            }

                            buffGiveOffers = new List<Offer>();
                            otherGiveOffers = giveOffers.ToList();

                            if (giveOffers.Any() && buffClinet != null)
                            {
                                var buffOffer = buffClinet.QuerySteamTrade().Result;
                                if (buffOffer.Body?.IsSuccess ?? false)
                                {
                                    var buffOfferIds = buffOffer.Body.data?.Select(c => c.tradeofferid)?.ToList() ?? new List<string>();
                                    buffGiveOffers = giveOffers.Where(c => buffOfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();

                                    otherGiveOffers.RemoveAll(c => buffOfferIds.Contains(c.TradeOfferId));
                                }
                                else
                                {
                                    otherGiveOffers = new List<Offer>();
                                }
                            }


                            if (acceptAll)
                            {
                                var acceptOffers = giveOffers.Where(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                HandleOffer(webClient, acceptOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();

                                if (confirmAll)
                                {
                                    autoConfirmOffers.AddRange(giveOffers);
                                }
                            }
                            if (!acceptAll && accpetBuff)
                            {
                                var acceptOffers = buffGiveOffers.Where(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                HandleOffer(webClient, acceptOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();

                                if (confirmBuff)
                                {
                                    autoConfirmOffers.AddRange(buffGiveOffers);
                                }
                            }
                            if (!acceptAll && accpetOther)
                            {
                                var acceptOffers = otherGiveOffers.Where(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid).ToList();
                                HandleOffer(webClient, acceptOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();

                                if (confirmOther)
                                {
                                    autoConfirmOffers.AddRange(otherGiveOffers);
                                }
                            }

                        }
                        catch
                        {
                        }
                        finally
                        {
                            client.SetAutoConfirmOffers(autoConfirmOffers);

                            if (user.SteamId == currentClient?.User.SteamId)
                            {
                                OfferCountLabel.Text = $"{tradeOffers.Count}";
                                OfferCountLabel.Tag = tradeOffers;
                            }

                            usersPanel.SetOffer(client, tradeOffers.Count);

                            if (buffClinet != null)
                            {
                                buffUsersPanel.SetOffer(buffClinet, buffGiveOffers?.Count);
                            }
                        }
                    });
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
                            OfferCountLabel.Tag = new List<Offer>();
                        }

                        usersPanel.SetOffer(client, null);

                        var buffClinet = buffClients.FirstOrDefault(c => c.User.SteamId == client.User.SteamId);
                        if (buffClinet != null)
                        {
                            buffUsersPanel.SetOffer(buffClinet, null);
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
            foreach (var client in checkClients)
            {
                if (client == null)
                {
                    continue;
                }

                var task = Task.Run(() =>
                {
                    if (!client.ConfirmationPopupLocker.Wait(0))
                    {
                        return;
                    }

                    try
                    {
                        var webClient = client.Client;
                        var user = client.User;

                        if (!webClient.LoggedIn)
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
                        if (client.User.SteamId == currentClient?.User.SteamId)
                        {
                            ConfirmationCountLable.Text = $"{confirmations.Count}";
                        }

                        usersPanel.SetConfirmation(client, confirmations.Count);

                        List<Confirmation> autoConfirm = new List<Confirmation>();
                        List<Confirmation> waitConfirm = new List<Confirmation>();

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
                            ConfirmationsPopup confirmationPopup = new ConfirmationsPopup(client, waitConfirm);
                            confirmationPopup.ShowDialog();
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        client.ConfirmationPopupLocker.Release();
                    }
                });
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

                if (walletDetails?.HasWallet ?? false)
                {
                    Balance.Text = $"{walletDetails.FormattedBalance}";

                    DelayedBalance.Text = "��0.00";
                    if (!string.IsNullOrWhiteSpace(walletDetails.FormattedDelayedBalance))
                    {
                        DelayedBalance.Text = $"{walletDetails.FormattedDelayedBalance}";
                    }
                }
            }
            catch
            {
            }
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

        private void ResetRefreshBuffUserTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshBuffUserTimer.Change(dueTime, period);
        }

        private void ResetRefreshEcoUserTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshEcoUserTimer.Change(dueTime, period);
        }

        private async Task<bool> CheckVersion()
        {
            if (!checkVersionLocker.Wait(0))
            {
                return true;
            }

            try
            {
                var result = await SteamApi.GetAsync<JObject>("https://api.github.com/repos/tsxlts/SteamAuthenticator/releases/latest");
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
                        ApplicationUpgrade applicationUpgrade = new ApplicationUpgrade(currentVersion, newVersion, published, body, updateUrl, updateUrl, name);
                        DialogResult updateDialog = applicationUpgrade.ShowDialog();
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

            this.Show();
            this.Activate();
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


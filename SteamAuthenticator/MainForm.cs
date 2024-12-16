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
        private readonly TimeSpan refreshMsgTimerMinPeriod = TimeSpan.FromSeconds(10);
        private readonly TimeSpan refreshClientInfoTimerMinPeriod = TimeSpan.FromSeconds(20);
        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);
        private readonly ContextMenuStrip mainNotifyMenuStrip;
        private readonly ContextMenuStrip userContextMenuStrip;
        private readonly ContextMenuStrip buffUserContextMenuStrip;

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
            userContextMenuStrip.Items.Add("复制Cookie").Click += copyCookieMenuItem_Click;
            userContextMenuStrip.Items.Add("复制AccessToken").Click += copyAccessTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("复制RefreshToken").Click += copyRefreshTokenMenuItem_Click;
            userContextMenuStrip.Items.Add("重新登录").Click += reloginMenuItem_Click;
            userContextMenuStrip.Items.Add("退出登录").Click += logoutMenuItem_Click;
            userContextMenuStrip.Items.Add("移除帐号").Click += removeUserMenuItem_Click;

            buffUserContextMenuStrip = new ContextMenuStrip();
            //buffUserContextMenuStrip.Items.Add("设置").Click += buffSettingMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("重新登录").Click += buffReloginMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("退出登录").Click += buffLogoutMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("移除帐号").Click += removeBuffUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            mainNotifyIcon.ContextMenuStrip = mainNotifyMenuStrip;

            await Task.WhenAll(LoadUsers(), LoadBuffUsers());

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

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
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
                        var platform when platform == AuthTokenPlatformType.WebBrowser => "网页浏览器",
                        _ => "未知设备"
                    };
                    var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                    MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(currentClient, (ulong)clients[0], sessionInfo.Version);
                    mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{currentClient.GetAccount()} 有新的登录请求";
                    mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                    mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP 地址：{sessionInfo.IP}";
                    mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("，", regions)}";

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
                        int offerCount = 0;
                        int buffOfferCount = 0;
                        try
                        {
                            if (!webClient.LoggedIn)
                            {
                                return;
                            }

                            var queryOffers = webClient.TradeOffer.QueryOffersAsync(sentOffer: false, receivedOffer: true, onlyActive: true, cancellationToken: cancellationToken).Result;
                            var descriptions = queryOffers?.Descriptions ?? new List<BaseDescription>();
                            var offers = queryOffers?.TradeOffersReceived ?? new List<Offer>();
                            offerCount = offers.Count;

                            if (client.User.SteamId == currentClient?.User.SteamId)
                            {
                                OfferCountLabel.Text = $"{offerCount}";
                                OfferCountLabel.Tag = offers;
                            }

                            var receiveOffers = offers.Where(c => !(c.ItemsToGive?.Any() ?? false)).ToList();
                            var giveOffers = offers.Where(c => c.ItemsToGive?.Any() ?? false).ToList();

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
                                            case AcceptOfferRuleSetting.RuleType.报价消息:
                                                string message = offer.Message ?? "";
                                                if (rule.Check(message))
                                                {
                                                    customOffers.Add(offer);
                                                }
                                                break;

                                            case AcceptOfferRuleSetting.RuleType.饰品名称:
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
                                    HandleOffer(webClient, customOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                                }

                                return;
                            }

                            bool acceptAll = user.Setting.AutoAcceptGiveOffer;
                            bool accpetBuff = acceptAll || user.Setting.AutoAcceptGiveOffer_Buff;
                            bool accpetOther = acceptAll || user.Setting.AutoAcceptGiveOffer_Other;

                            if (!acceptAll && giveOffers.Any() && buffClinet != null)
                            {
                                var buffOffer = buffClinet.QuerySteamTrade().Result;
                                if (!(buffOffer.Body?.IsSuccess ?? false))
                                {
                                    return;
                                }

                                var buffOfferIds = buffOffer.Body.data?.Select(c => c.tradeofferid)?.ToList() ?? new List<string>();
                                var buffOffers = giveOffers.Where(c => buffOfferIds.Any(offerId => c.TradeOfferId == offerId)).ToList();
                                buffOfferCount = buffOffers.Count;

                                giveOffers.RemoveAll(c => buffOffers.Any(b => b.TradeOfferId == c.TradeOfferId));

                                if (buffOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid) && accpetBuff)
                                {
                                    HandleOffer(webClient, buffOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                                }
                            }

                            if (giveOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid) && accpetOther)
                            {
                                HandleOffer(webClient, giveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            UserPanel userPanel = usersPanel.Controls.Find(webClient.SteamId ?? "--", false).FirstOrDefault() as UserPanel;
                            if (userPanel != null)
                            {
                                userPanel.SetOffer(offerCount);
                            }

                            if (buffClinet != null)
                            {
                                BuffUserPanel buffUserPanel = buffUsersPanel.Controls.Find(buffClinet.User.UserId, false).FirstOrDefault() as BuffUserPanel;
                                if (buffUserPanel != null)
                                {
                                    buffUserPanel.SetOffer(buffOfferCount);
                                }
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

                        UserPanel userPanel = usersPanel.Controls.Find(client.User.SteamId ?? "--", false).FirstOrDefault() as UserPanel;
                        if (userPanel != null)
                        {
                            userPanel.SetOffer(null);
                        }

                        var buffClinet = buffClients.FirstOrDefault(c => c.User.SteamId == client.User.SteamId);
                        if (buffClinet != null)
                        {
                            BuffUserPanel buffUserPanel = buffUsersPanel.Controls.Find(buffClinet.User.UserId, false).FirstOrDefault() as BuffUserPanel;
                            if (buffUserPanel != null)
                            {
                                buffUserPanel.SetOffer(null);
                            }
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

                        UserPanel userPanel = usersPanel.Controls.Find(webClient.SteamId, false).FirstOrDefault() as UserPanel;
                        if (userPanel != null)
                        {
                            userPanel.SetConfirmation(confirmations.Count);
                        }

                        List<Confirmation> autoConfirm = new List<Confirmation>();
                        List<Confirmation> waitConfirm = new List<Confirmation>();

                        foreach (var conf in confirmations)
                        {
                            if ((conf.ConfType == ConfirmationType.MarketListing && user.Setting.AutoConfirmMarket) ||
                              (conf.ConfType == ConfirmationType.Trade && user.Setting.AutoConfirmTrade))
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

                    UserPanel userPanel = usersPanel.Controls.Find(client.User.SteamId, false).FirstOrDefault() as UserPanel;
                    if (userPanel != null)
                    {
                        userPanel.SetConfirmation(null);
                    }
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

                    DelayedBalance.Text = "￥0.00";
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
                    var controlCollection = usersPanel.Controls.Cast<UserPanel>().ToArray();
                    foreach (UserPanel userPanel in controlCollection)
                    {
                        if (string.IsNullOrWhiteSpace(userPanel.UserName))
                        {
                            continue;
                        }

                        var userClinet = userPanel.UserClient;
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
                            userPanel.SetUserName(userPanel.UserName, Color.Green);
                        }
                        else
                        {
                            userPanel.SetUserName(userPanel.UserName, Color.Red);

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

                                userPanel.SetUserAvatar(user.Avatar);

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


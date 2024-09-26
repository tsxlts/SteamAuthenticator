
using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly Version currentVersion;
        private readonly System.Threading.Timer timer;
        private readonly System.Threading.Timer refreshUserTimer;
        private readonly TimeSpan timerMinPeriod = TimeSpan.FromSeconds(20);
        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);
        private readonly ContextMenuStrip userContextMenuStrip;
        private readonly ContextMenuStrip buffUserContextMenuStrip;

        private string initialDirectory = null;
        private UserClient currentClient = null;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            var match = Regex.Match(Application.ProductVersion, @"[\d.]+");
            currentVersion = new Version(match.Value);
            versionLabel.Text = $"v{currentVersion}";

            timer = new System.Threading.Timer(RefreshClientMsg, null, -1, -1);
            refreshUserTimer = new System.Threading.Timer(RefreshUser, null, -1, -1);

            userContextMenuStrip = new ContextMenuStrip();
            userContextMenuStrip.Items.Add("切换").Click += setCurrentClientMenuItem_Click;
            userContextMenuStrip.Items.Add("重新登录").Click += loginMenuItem_Click;
            userContextMenuStrip.Items.Add("退出登录").Click += removeUserMenuItem_Click;

            buffUserContextMenuStrip = new ContextMenuStrip();
            buffUserContextMenuStrip.Items.Add("设置").Click += buffSettingMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("重新登录").Click += buffLoginMenuItem_Click;
            buffUserContextMenuStrip.Items.Add("退出登录").Click += removeBuffUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await LoadUsers();
            await LoadBuffUsers();

            var user = Appsetting.Instance.Clients?.FirstOrDefault(c => c.User.SteamId == Appsetting.Instance.AppSetting.Entry.CurrentUser);
            user = user ?? Appsetting.Instance.Clients?.FirstOrDefault();
            if (user != null)
            {
                SetCurrentClient(user);
            }

            await CheckVersion();
        }

        private void RefreshClientMsg(object _)
        {
            var setting = Appsetting.Instance.AppSetting.Entry;

            try
            {
                List<Task> tasks = new List<Task>();

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    tasks.Add(QueryAuthSessionsForAccount(tokenSource.Token));

                    tasks.Add(QueryOffers(tokenSource.Token));
                    tasks.Add(QueryConfirmations(tokenSource.Token));

                    tasks.Add(QueryWalletDetails(tokenSource.Token));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch
            {

            }
            finally
            {
                TimeSpan dueTime = TimeSpan.FromSeconds(Math.Max(1, setting.AutoRefreshInternal));
                TimeSpan period = TimeSpan.FromSeconds(Math.Max(timerMinPeriod.TotalSeconds, setting.AutoRefreshInternal * 2));
                ResetTimer(dueTime, period);
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

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
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

                    MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(webClient, (ulong)clients[0], sessionInfo.Version);
                    mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{webClient.SteamId} 有新的登录请求";
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
                var clients = setting.CheckAllConfirmation ? Appsetting.Instance.Clients : new List<UserClient> { currentClient };
                foreach (var client in clients)
                {
                    if (client == null)
                    {
                        continue;
                    }

                    var task = Task.Run(() =>
                    {
                        var webClient = client.Client;
                        var buffClinet = client.BuffClient;
                        int offerCount = 0;
                        int buffOfferCount = 0;
                        try
                        {
                            var queryOffers = webClient.TradeOffer.QueryOffersAsync(sentOffer: false, receivedOffer: true, onlyActive: true, cancellationToken: cancellationToken).Result;
                            var offers = queryOffers?.TradeOffersReceived ?? new List<Offer>();
                            offerCount = offers.Count;

                            if (client.User.SteamId == currentClient?.User.SteamId)
                            {
                                OfferCountLabel.Text = $"{offerCount}";
                                OfferCountLabel.Tag = offers;
                            }

                            var receiveOffers = offers.Where(c => !(c.ItemsToGive?.Any() ?? false)).ToList();
                            var giveOffers = offers.Where(c => c.ItemsToGive?.Any() ?? false).ToList();

                            if (receiveOffers.Any() && setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }

                            if (giveOffers.Any() && buffClinet != null)
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

                                if (buffOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid) && buffClinet.User.Setting.AutoAcceptGiveOffer)
                                {
                                    HandleOffer(webClient, buffOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                                }
                            }

                            if (giveOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid) && setting.AutoAcceptGiveOffer)
                            {
                                HandleOffer(webClient, giveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            UserPanel userPanel = usersPanel.Controls.Find(webClient.SteamId, false).FirstOrDefault() as UserPanel;
                            if (userPanel != null)
                            {
                                Label offerLabel = userPanel.Controls.Find("offer", false).FirstOrDefault() as Label;
                                if (offerLabel != null)
                                {
                                    offerLabel.Text = $"{offerCount}";
                                }
                            }

                            if (buffClinet != null)
                            {
                                BuffUserPanel buffUserPanel = buffUsersPanel.Controls.Find(buffClinet.User.UserId, false).FirstOrDefault() as BuffUserPanel;
                                if (buffUserPanel != null)
                                {
                                    Label offerLabel = buffUserPanel.Controls.Find("offer", false).FirstOrDefault() as Label;
                                    if (offerLabel != null)
                                    {
                                        offerLabel.Text = $"{buffOfferCount}";
                                    }
                                }
                            }
                        }
                    });
                    tasks.Add(task);
                }

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
            var clients = setting.CheckAllConfirmation ? Appsetting.Instance.Clients : new List<UserClient> { currentClient };
            foreach (var client in clients)
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

                        Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                        if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                        {
                            return;
                        }

                        var queryConfirmations = webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret, cancellationToken).Result;
                        if (!queryConfirmations.Success)
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
                            Label confirmationLabel = userPanel.Controls.Find("confirmation", false).FirstOrDefault() as Label;
                            if (confirmationLabel != null)
                            {
                                confirmationLabel.Text = $"{confirmations.Count}";
                            }
                        }

                        List<Confirmation> autoConfirm = new List<Confirmation>();
                        List<Confirmation> waitConfirm = new List<Confirmation>();

                        foreach (var conf in confirmations)
                        {
                            if ((conf.ConfType == ConfirmationType.MarketListing && setting.AutoConfirmMarket) ||
                              (conf.ConfType == ConfirmationType.Trade && setting.AutoConfirmTrade))
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
                                bool accept = HandleConfirmation(webClient, guard, autoConfirm, true, cancellationToken).Result;
                            }
                            catch
                            {

                            }
                        }

                        if (waitConfirm.Any() && setting.ConfirmationAutoPopup)
                        {
                            ConfirmationsPopup confirmationPopup = new ConfirmationsPopup(webClient, waitConfirm);
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
                        //var nameLabel = userPanel.Controls.Cast<Control>().FirstOrDefault(c => c.Name == "username") as Label;
                        var nameLabel = userPanel.Controls.Find("username", false)?.FirstOrDefault() as Label;
                        if (nameLabel == null)
                        {
                            continue;
                        }

                        var client = userPanel.UserClient.Client;
                        var user = userPanel.UserClient.User;

                        var palyerSummaries = SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId }, cancellationToken: tokenSource.Token).GetAwaiter().GetResult();
                        if (palyerSummaries.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            client.LogoutAsync().GetAwaiter().GetResult();
                        }

                        bool reloadCurrent = false;
                        if (client.LoggedIn)
                        {
                            nameLabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            nameLabel.ForeColor = Color.Red;

                            reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                        }

                        var player = palyerSummaries.Body?.Players?.FirstOrDefault();
                        if (player != null)
                        {
                            if (player.SteamName != user.NickName || player.AvatarFull != user.Avatar)
                            {
                                user.NickName = player.SteamName;
                                user.Avatar = player.AvatarFull;
                                Appsetting.Instance.Manifest.AddUser(client.SteamId, user);

                                PictureBox pictureBox = userPanel.Controls.Find("useravatar", false)?.FirstOrDefault() as PictureBox;
                                pictureBox?.LoadAsync(user.Avatar);

                                reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                            }
                        }

                        if (reloadCurrent)
                        {
                            SetCurrentClient(userPanel.UserClient, true);
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

        private void ResetTimer(TimeSpan dueTime, TimeSpan period)
        {
            timer.Change(dueTime, period);
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
                    string updateUrl = assets.FirstOrDefault()?.Value<string>("browser_download_url");

                    if (!string.IsNullOrWhiteSpace(updateUrl))
                    {
                        DialogResult updateDialog = MessageBox.Show($"有最新版本可用（{tag_name}），是否立即更新", "版本更新", MessageBoxButtons.YesNo);
                        if (updateDialog == DialogResult.Yes)
                        {
                            Process.Start("explorer.exe", updateUrl);
                        }
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
    }
}


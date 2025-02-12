using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.WebClient;
using System.Text;
using System.Web;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm
    {
        private void UsersPanel_SizeChanged(object sender, EventArgs e)
        {
            usersPanel.Reset();
        }

        private async void addUserBtn_Click(object sender, EventArgs e)
        {
            await Login(false, null);
        }

        private async void btnUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Control control = sender as Control;
            SteamUserPanel panel = control.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            await SwitchUser(userClient);
        }

        private void copyCookieMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in userClient.Client.WebCookie)
            {
                stringBuilder.Append($"{item.Name}={HttpUtility.UrlEncode(item.Value)}; ");
            }
            if (stringBuilder.Length < 1)
            {
                return;
            }

            Clipboard.SetText(stringBuilder.ToString());
        }

        private void copyAccessTokenMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;
            string accessToken = userClient.Client.AccessToken;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return;
            }

            Clipboard.SetText(accessToken);
        }

        private void copyRefreshTokenMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;
            string refreshToken = userClient.Client.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            Clipboard.SetText(refreshToken);
        }

        private async void setCurrentClientMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            await SwitchUser(userClient);
        }

        private void settingMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            var userSetting = new Forms.UserSetting(userClient.User);
            userSetting.ShowDialog();
        }

        private async void reloginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            if (await userClient.LoginAsync())
            {
                return;
            }

            await Login(false, userClient.GetAccount());
        }

        private async void logoutMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            await userClient.LogoutAsync();

            ResetRefreshUserTimer(TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(10));
        }

        private void removeUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            userClient.Client.Dispose();

            Appsetting.Instance.Manifest.RemoveSteamUser(userClient.User.SteamId, out var entry);
            Appsetting.Instance.Clients.Remove(userClient);

            usersPanel.RemoveClient(userClient);

            if (userClient.User.SteamId == currentClient?.User?.SteamId)
            {
                SetCurrentClient(Appsetting.Instance.Clients.FirstOrDefault() ?? userClient, true);
            }
        }

        private void offersNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            SteamUserPanel panel = control.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            Offers offersForm = new Offers(this, userClient);
            offersForm.ShowDialog();
        }

        private void confirmationNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            SteamUserPanel panel = control.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;
            var webClient = userClient.Client;

            if (webClient == null || !webClient.LoggedIn)
            {
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(userClient.GetAccount());
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{userClient.GetAccount()} 未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmations = new Confirmations(this, userClient);
            confirmations.ShowDialog();
        }

        private async Task LoadUsers()
        {
            try
            {
                usersPanel.ClearItems();
                var accounts = Appsetting.Instance.Manifest.GetSteamUsers().ToList();

                foreach (string account in accounts)
                {
                    User user = Appsetting.Instance.Manifest.GetSteamUser(account);
                    UserClient client = new UserClient(user, new SteamCommunityClient());

                    AddUserPanel(client);

                    Appsetting.Instance.Clients.Add(client);
                }

                {

                    SteamUserPanel panel = usersPanel.AddItemPanel(false, UserClient.None);

                    panel.ItemIcon.Image = Properties.Resources.add;
                    panel.ItemIcon.Click += addUserBtn_Click;

                    panel.ItemName.Text = $"添加帐号";
                    panel.ItemName.ForeColor = Color.FromArgb(244, 164, 96);
                    panel.ItemName.Click += addUserBtn_Click;

                    panel.Icons.Hide();
                    panel.Offer.Hide();
                    panel.Confirmation.Hide();
                }

                var tasks = Appsetting.Instance.Clients.Select(c => c.LoginAsync());
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetRefreshMsgTimer(TimeSpan.Zero, refreshMsgTimerMinPeriod);
                ResetRefreshClientInfoTimer(TimeSpan.Zero, refreshClientInfoTimerMinPeriod);
                ResetRefreshUserTimer(TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }
        }

        private async Task<UserClient> Login(bool relogin, string account)
        {
            if (relogin)
            {
                if (MessageBox.Show($"帐号 {account} 已掉线，是否请重新登录", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return null;
                }
            }

            Login login = new Login(account);
            if (login.ShowDialog() == DialogResult.OK && (login.Client?.LoggedIn ?? false))
            {
                var userClient = await SaveUser(login.Client);
                var user = userClient.User;

                if (Appsetting.Instance.Clients.Count == 1)
                {
                    SetCurrentClient(Appsetting.Instance.Clients[0]);
                }

                AddUserPanel(userClient);

                return userClient;
            }

            return null;
        }

        private async Task<UserClient> SaveUser(SteamCommunityClient client)
        {
            if (client?.LoggedIn ?? false)
            {
                var localUser = Appsetting.Instance.Manifest.GetSteamUser(client.SteamId);

                client.SetLanguage(Language.Schinese);

                var players = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId });
                var player = players.Body?.Players?.FirstOrDefault();

                var user = new User
                {
                    Account = !string.IsNullOrWhiteSpace(client.Account) ? client.Account : localUser?.Account,

                    SteamId = client.SteamId,
                    RefreshToken = client.RefreshToken,

                    NickName = player?.SteamName ?? localUser?.NickName ?? client.SteamId,
                    Avatar = player?.AvatarFull ?? localUser?.Avatar ?? "",

                    Setting = localUser?.Setting ?? new Model.UserSetting()
                };

                UserClient userClient = new UserClient(user, client);

                Appsetting.Instance.Manifest.SaveSteamUser(client.SteamId, user);

                Appsetting.Instance.Clients.RemoveAll(c => c.User.SteamId == user.SteamId);
                Appsetting.Instance.Clients.Add(userClient);
                return userClient;

            }
            return null;
        }

        private async Task SwitchUser(UserClient userClient)
        {
            try
            {
                if (!userClient.Client.LoggedIn)
                {
                    if (!await userClient.LoginAsync())
                    {
                        userClient = await Login(true, userClient.GetAccount()) ?? userClient;
                    }
                }

                SetCurrentClient(userClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"切换用户失败{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetCurrentClient(UserClient userClient, bool reload = false)
        {
            if (!reload)
            {
                if (userClient?.User?.SteamId == currentClient?.User?.SteamId && userClient.Client.LoggedIn == currentClient?.Client.LoggedIn)
                {
                    return;
                }
            }

            Text = $"Steam验证器";

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            SteamId.Text = "---";
            Balance.Text = "￥0.00";
            DelayedBalance.Text = "￥0.00";

            OfferCountLabel.Text = "---";
            ConfirmationCountLable.Text = "---";

            if (userClient?.Client?.LoggedIn ?? false)
            {
                Text = $"Steam验证器 {userClient.GetAccount()}[{userClient.User.NickName}]";

                UserImg.Image = Properties.Resources.userimg;
                if (!string.IsNullOrWhiteSpace(userClient.User.Avatar))
                {
                    UserImg.LoadAsync(userClient.User.Avatar);
                }
                UserName.ForeColor = Color.Green;
                UserName.Text = $"{userClient.GetAccount()} [{userClient.User.NickName}]";
                SteamId.Text = $"{userClient.User.SteamId}";
                Balance.Text = "￥0.00";
                DelayedBalance.Text = "￥0.00";
            }

            currentClient = userClient;
            Appsetting.Instance.AppSetting.Entry.CurrentUser = currentClient.User.SteamId;
            Appsetting.Instance.AppSetting.Save();
        }

        private SteamUserPanel AddUserPanel(UserClient userClient)
        {
            userClient.Client.SetLanguage(Language.Schinese);
            SteamUserPanel panel = usersPanel.AddItemPanel(true, userClient);

            panel.ItemIcon.MouseClick += btnUser_Click;
            panel.ItemIcon.ContextMenuStrip = userContextMenuStrip;

            panel.ItemName.MouseClick += btnUser_Click;
            panel.ItemName.ContextMenuStrip = userContextMenuStrip;

            panel.Offer.Click += offersNumberBtn_Click;

            panel.Confirmation.Click += confirmationNumberBtn_Click;

            panel.Client
                .WithStartLogin(() => panel.ItemName.ForeColor = Color.FromArgb(128, 128, 128))
                .WithEndLogin(loggined =>
                {
                    panel.ItemName.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }
    }
}

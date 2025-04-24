using System.Text;
using System.Web;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.WebClient;
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
            await Login(null);
        }

        private void btnUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Control control = sender as Control;
            SteamUserPanel panel = control.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            SetCurrentClient(userClient);
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

            Utils.CopyText(stringBuilder.ToString());
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

            Utils.CopyText(accessToken);
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

            Utils.CopyText(refreshToken);
        }

        private void setCurrentClientMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            SetCurrentClient(userClient);
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

        private void accountInfoMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            var accountInfo = new AccountInfo(userClient)
            {
                Width = 800,
                Height = 500
            };
            accountInfo.ShowDialog();
        }

        private void inventoryMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;
            if (!userClient.Client.LoggedIn)
            {
                return;
            }

            var inventory = new Inventory(userClient);
            inventory.ShowDialog();
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

            await Login(userClient.GetAccount());
        }

        private async void logoutMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            SteamUserPanel panel = menuStrip.SourceControl.Parent as SteamUserPanel;
            UserClient userClient = panel.Client;

            await userClient.LogoutAsync();

            ResetRefreshUserTimer(TimeSpan.Zero, refreshUserTimerMinPeriod);
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
                SetCurrentClient(Appsetting.Instance.Clients.FirstOrDefault());
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
                    UserClient client = new UserClient(user, new SteamCommunityClient(), false);
                    Appsetting.Instance.Clients.Add(client);

                    AddUserPanel(client);

                    if (client.User.SteamId == Appsetting.Instance.AppSetting.Entry.CurrentUser)
                    {
                        SetCurrentClient(client);
                    }
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
                ResetRefreshUserTimer(TimeSpan.Zero, refreshUserTimerMinPeriod);
            }
        }

        private async Task<UserClient> Login(string defaultAccount)
        {
            Login login = new Login(defaultAccount);
            if (login.ShowDialog() != DialogResult.OK || !(login.Client?.LoggedIn ?? false))
            {
                return null;
            }

            var client = login.Client;
            client.SetLanguage(Language.Schinese);

            var account = await client.GetAccountNameAsync();

            var players = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId });
            var player = players.Body?.Players?.FirstOrDefault();

            var localUser = Appsetting.Instance.Manifest.GetSteamUser(client.SteamId);

            var user = new User
            {
                Account = !string.IsNullOrWhiteSpace(account) ? account : localUser?.Account,

                SteamId = client.SteamId,
                RefreshToken = client.RefreshToken,

                NickName = player?.SteamName ?? localUser?.NickName ?? client.SteamId,
                Avatar = player?.AvatarFull ?? localUser?.Avatar ?? "",

                Setting = localUser?.Setting ?? new Model.UserSetting()
            };
            UserClient userClient = new UserClient(user, client, true);

            Appsetting.Instance.Manifest.SaveSteamUser(client.SteamId, user);
            Appsetting.Instance.Clients.RemoveAll(c => c.User.SteamId == user.SteamId);
            Appsetting.Instance.Clients.Add(userClient);

            AddUserPanel(userClient);

            if (Appsetting.Instance.Clients.Count == 1 || currentClient.User.SteamId == userClient.User.SteamId)
            {
                SetCurrentClient(userClient);
            }
            return userClient;
        }

        private void SetCurrentClient(UserClient userClient)
        {
            try
            {
                if (userClient?.User?.SteamId == currentClient?.User?.SteamId && userClient.Client.LoggedIn == currentClient?.Client.LoggedIn)
                {
                    return;
                }

                usersPanel.SetChecked(userClient, true);

                UserName.Text = userClient?.User == null ? "---" : $"{userClient.GetAccount()} [{userClient.User.NickName}]";
                SteamId.Text = userClient?.User == null ? "---" : $"{userClient.User.SteamId}";

                UserImg.Image = Properties.Resources.userimg;
                if (!string.IsNullOrWhiteSpace(userClient?.User?.Avatar))
                {
                    UserImg.LoadAsync(userClient.User.Avatar);
                }

                Balance.Text = "---";
                DelayedBalance.Text = "---";
                OfferCountLabel.Text = "---";
                ConfirmationCountLable.Text = "---";

                currentClient = userClient;
                Appsetting.Instance.AppSetting.Entry.CurrentUser = currentClient?.User?.SteamId;
                Appsetting.Instance.AppSetting.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"切换用户失败{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SteamUserPanel AddUserPanel(UserClient userClient)
        {
            userClient.Client.SetLanguage(Language.Schinese);
            SteamUserPanel panel = usersPanel.AddItemPanel(true, userClient);

            userClient.ClientRefreshed += (sender, e) =>
            {
                if (!e.Changed)
                {
                    return;
                }

                var client = e.Client as UserClient;

                usersPanel.SetItemName(client, client.GetAccount(), client.LoggedIn ? Color.Green : Color.Red);
                usersPanel.SetItemIcon(client, client.User.Avatar);

                if (client.User.SteamId == currentClient?.User?.SteamId)
                {
                    SetCurrentClient(client);
                }
            };

            panel.ItemIcon.MouseClick += btnUser_Click;
            panel.ItemIcon.ContextMenuStrip = userContextMenuStrip;

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

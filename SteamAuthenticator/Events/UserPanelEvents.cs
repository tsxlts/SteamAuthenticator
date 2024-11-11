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
            ResetUserPanel();
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
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await SwitchUser(userClient);
        }

        private void copyCookieMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

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

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;
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

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;
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

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await SwitchUser(userClient);
        }

        private void settingMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            var userSetting = new Forms.UserSetting(userClient.User);
            userSetting.ShowDialog();
        }

        private async void loginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            if (await userClient.LoginAsync())
            {
                return;
            }

            await Login(false, userClient.User.Account);
        }

        private async void removeUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await userClient.Client.LogoutAsync();
            userClient.Client.Dispose();

            Appsetting.Instance.Manifest.RemoveUser(userClient.User.SteamId, out var entry);
            Appsetting.Instance.Clients.Remove(userClient);
            usersPanel.Controls.Remove(panel);
            ResetUserPanel();

            if (userClient.User.SteamId == currentClient?.User?.SteamId)
            {
                SetCurrentClient(Appsetting.Instance.Clients.FirstOrDefault() ?? userClient, true);
            }
        }

        private void offersNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            Offers offersForm = new Offers(this, userClient.Client);
            offersForm.ShowDialog();
        }

        private void confirmationNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;
            var webClient = userClient.Client;

            if (webClient == null || !webClient.LoggedIn)
            {
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{webClient.Account} 未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmations = new Confirmations(this, webClient);
            confirmations.ShowDialog();
        }

        private async Task LoadUsers()
        {
            try
            {
                usersPanel.Controls.Clear();

                int startX = GetUserControlStartPointX(out int cells);

                Appsetting.Instance.Clients.RemoveAll(c => !c.Client.LoggedIn);

                IEnumerable<string> accounts = Appsetting.Instance.Manifest.GetUsers();
                int index = 0;
                foreach (string account in accounts)
                {
                    User user = Appsetting.Instance.Manifest.GetUser(account);
                    BuffClient buffClient = null;
                    if (user.BuffUser != null)
                    {
                        buffClient = new BuffClient(user.BuffUser);
                    }

                    UserPanel panel = CreateUserPanel(startX, cells, index, new UserClient(user, new SteamCommunityClient(), buffClient));
                    usersPanel.Controls.Add(panel);

                    index++;

                    Appsetting.Instance.Clients.Add(panel.UserClient);
                }

                {
                    UserPanel panel = new UserPanel()
                    {
                        Size = new Size(80, 116),
                        Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                        UserClient = UserClient.None
                    };

                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = 80,
                        Height = 80,
                        Location = new Point(0, 0),
                        Cursor = Cursors.Hand,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    pictureBox.Image = Properties.Resources.add;
                    pictureBox.Click += addUserBtn_Click;
                    panel.Controls.Add(pictureBox);

                    Label nameLabel = new Label()
                    {
                        Text = $"添加帐号",
                        AutoSize = false,
                        AutoEllipsis = true,
                        Cursor = Cursors.Hand,
                        Size = new Size(80, 18),
                        TextAlign = ContentAlignment.TopCenter,
                        ForeColor = Color.FromArgb(244, 164, 96),
                        Location = new Point(0, 80)
                    };
                    nameLabel.Click += addUserBtn_Click;
                    panel.Controls.Add(nameLabel);
                    usersPanel.Controls.Add(panel);
                }

                var tasks = Appsetting.Instance.Clients.Select(c => c.LoginAsync());
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetTimer(TimeSpan.Zero, timerMinPeriod);
                ResetRefreshUserTimer(TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }
        }

        private async Task<UserClient> Login(bool relogin, string account)
        {
            if (relogin)
            {
                MessageBox.Show($"帐号 {account} 已掉线，请重新登录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                var controlCollection = usersPanel.Controls.Cast<UserPanel>().ToList();
                var index = controlCollection.FindIndex(c => c.UserClient.User.SteamId == user.SteamId);

                if (index < 0)
                {
                    index = controlCollection.Count - 1;
                }
                else
                {
                    controlCollection.RemoveAt(index);
                }

                int startX = GetUserControlStartPointX(out int cells);
                UserPanel panel = CreateUserPanel(startX, cells, index, userClient);
                controlCollection.Insert(index, panel);

                usersPanel.Controls.Clear();
                usersPanel.Controls.AddRange(controlCollection.ToArray());
                ResetUserPanel();

                return panel.UserClient;
            }

            return null;
        }

        private async Task<UserClient> SaveUser(SteamCommunityClient client)
        {
            if (client?.LoggedIn ?? false)
            {
                client.SetLanguage(Language.Schinese);

                var players = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId });
                var player = players.Body?.Players?.FirstOrDefault();

                var user = new User
                {
                    Account = client.Account,
                    SteamId = client.SteamId,
                    RefreshToken = client.RefreshToken,
                    NickName = player?.SteamName ?? client.SteamId,
                    Avatar = player?.AvatarFull ?? "",
                    BuffUser = null
                };

                UserClient userClient = new UserClient(user, client, null);

                Appsetting.Instance.Manifest.AddUser(client.SteamId, user);

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
                        userClient = await Login(true, userClient.User.Account);
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
                Text = $"Steam验证器 {userClient.User.Account}[{userClient.User.NickName}]";

                UserImg.Image = Properties.Resources.userimg;
                if (!string.IsNullOrWhiteSpace(userClient.User.Avatar))
                {
                    UserImg.LoadAsync(userClient.User.Avatar);
                }
                UserName.ForeColor = Color.Green;
                UserName.Text = $"{userClient.User.Account} [{userClient.User.NickName}]";
                SteamId.Text = $"{userClient.User.SteamId}";
                Balance.Text = "￥0.00";
                DelayedBalance.Text = "￥0.00";
            }

            currentClient = userClient;
            Appsetting.Instance.AppSetting.Entry.CurrentUser = currentClient.User.SteamId;
            Appsetting.Instance.AppSetting.Save();
        }

        private UserPanel CreateUserPanel(int startX, int cells, int index, UserClient userClient)
        {
            userClient.Client.SetLanguage(Language.Schinese);
            UserPanel panel = new UserPanel()
            {
                Name = userClient.User.SteamId,
                Size = new Size(80, 116),
                Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                UserClient = userClient
            };

            PictureBox pictureBox = new PictureBox()
            {
                Name = "useravatar",
                Width = 80,
                Height = 80,
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.Zoom,
                InitialImage = Properties.Resources.loading,
            };
            string avatar = userClient.User.Avatar;
            pictureBox.Image = Properties.Resources.userimg;
            if (!string.IsNullOrEmpty(avatar))
            {
                pictureBox.LoadAsync(avatar);
            }
            pictureBox.MouseClick += btnUser_Click;
            pictureBox.ContextMenuStrip = userContextMenuStrip;
            panel.Controls.Add(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{userClient.User.Account} [{userClient.User.NickName}]",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, 80)
            };
            nameLabel.MouseClick += btnUser_Click;
            nameLabel.ContextMenuStrip = userContextMenuStrip;
            panel.Controls.Add(nameLabel);

            Label offerLabel = new Label()
            {
                Name = "offer",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopRight,
                ForeColor = Color.FromArgb(255, 128, 0),
                Location = new Point(0, 98)
            };
            offerLabel.Click += offersNumberBtn_Click;
            panel.Controls.Add(offerLabel);

            Label confirmationLabel = new Label()
            {
                Name = "confirmation",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(0, 128, 255),
                Location = new Point(42, 98)
            };
            confirmationLabel.Click += confirmationNumberBtn_Click;
            panel.Controls.Add(confirmationLabel);

            panel.UserClient
                .WithStartLogin(() => nameLabel.ForeColor = Color.FromArgb(128, 128, 128))
                .WithEndLogin(loggined =>
                {
                    nameLabel.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private void ResetUserPanel()
        {
            try
            {
                var controlCollection = usersPanel.Controls.Cast<Control>().ToArray();

                int x = GetUserControlStartPointX(out int cells);

                int index = 0;
                foreach (Control control in controlCollection)
                {
                    control.Location = new Point(x * (index % cells) + 10, 126 * (index / cells) + 10);
                    index++;
                }

                usersPanel.Controls.Clear();
                usersPanel.Controls.AddRange(controlCollection);
            }
            catch
            {

            }
        }

        private int GetUserControlStartPointX(out int cells)
        {
            cells = (usersPanel.Size.Width - 30) / 80;
            int size = (usersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            if (size < 85)
            {
                cells = cells - 1;
                size = (usersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            }
            return size;
        }
    }
}

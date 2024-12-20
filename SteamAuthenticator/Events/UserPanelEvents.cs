﻿using Steam_Authenticator.Controls;
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
            panel.RefreshIcon();
        }

        private async void reloginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

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

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await userClient.LogoutAsync();

            ResetRefreshUserTimer(TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(10));
        }

        private void removeUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            userClient.Client.Dispose();

            Appsetting.Instance.Manifest.RemoveSteamUser(userClient.User.SteamId, out var entry);
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

            Offers offersForm = new Offers(this, userClient);
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
                usersPanel.Controls.Clear();

                int startX = GetUserControlStartPointX(out int cells);

                Appsetting.Instance.Clients.RemoveAll(c => !c.Client.LoggedIn);

                var accounts = Appsetting.Instance.Manifest.GetSteamUsers().ToList();
                int index = 0;
                foreach (string account in accounts)
                {
                    User user = Appsetting.Instance.Manifest.GetSteamUser(account);

                    UserPanel panel = CreateUserPanel(startX, cells, index, new UserClient(user, new SteamCommunityClient()));
                    usersPanel.Controls.Add(panel);

                    index++;

                    Appsetting.Instance.Clients.Add(panel.UserClient);
                }

                {
                    UserPanel panel = new UserPanel(false)
                    {
                        Size = new Size(80, 136),
                        Location = new Point(startX * (index % cells) + 10, 146 * (index / cells) + 10),
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
                    panel.SetUserAvatarBox(pictureBox);

                    IconLabel iconLabel = new IconLabel()
                    {
                        Name = "icons",
                        Size = new Size(80, 20),
                        IconSize = new Size(16, 16),
                        Location = new Point(0, 80),
                    };
                    panel.SetIconsBox(iconLabel);

                    Label nameLabel = new Label()
                    {
                        Text = $"添加帐号",
                        AutoSize = false,
                        AutoEllipsis = true,
                        Cursor = Cursors.Hand,
                        Size = new Size(80, 18),
                        TextAlign = ContentAlignment.TopCenter,
                        ForeColor = Color.FromArgb(244, 164, 96),
                        Location = new Point(0, iconLabel.Location.Y + iconLabel.Height)
                    };
                    nameLabel.Click += addUserBtn_Click;
                    panel.SetUserNameBox(nameLabel);
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
                var localUser = Appsetting.Instance.Manifest.GetSteamUser(client.SteamId);

                client.SetLanguage(Language.Schinese);

                var players = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId });
                var player = players.Body?.Players?.FirstOrDefault();

                var user = new User
                {
                    Account = !string.IsNullOrWhiteSpace(client.Account) ? client.Account : localUser.Account,

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

        private UserPanel CreateUserPanel(int startX, int cells, int index, UserClient userClient)
        {
            userClient.Client.SetLanguage(Language.Schinese);
            UserPanel panel = new UserPanel(true)
            {
                Name = userClient.User.SteamId,
                Size = new Size(80, 136),
                Location = new Point(startX * (index % cells) + 10, 146 * (index / cells) + 10),
                UserClient = userClient
            };
            var auto_deliver = new CustomIcon(Properties.Resources.auto_deliver_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (!userClient.User.Setting.PeriodicCheckingConfirmation)
                    {
                        return CustomIcon.ConvertToPurple(icon);
                    }

                    if (userClient.User.Setting.AutoAcceptGiveOffer)
                    {
                        return icon;
                    }
                    if (userClient.User.Setting.AutoAcceptGiveOffer_Buff)
                    {
                        return icon;
                    }
                    if (userClient.User.Setting.AutoAcceptGiveOffer_Other)
                    {
                        return icon;
                    }
                    if (userClient.User.Setting.AutoAcceptGiveOffer_Custom)
                    {
                        return icon;
                    }

                    return CustomIcon.ConvertToGrayscale(icon);
                }
            });
            var auto_confirm = new CustomIcon(Properties.Resources.auto_confirm_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (!userClient.User.Setting.PeriodicCheckingConfirmation)
                    {
                        return CustomIcon.ConvertToPurple(icon);
                    }

                    if (userClient.User.Setting.AutoConfirmTrade)
                    {
                        return icon;
                    }
                    if (userClient.User.Setting.AutoConfirmMarket)
                    {
                        return icon;
                    }

                    return CustomIcon.ConvertToGrayscale(icon);
                }
            });
            var auto_accept = new CustomIcon(Properties.Resources.auto_accept_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (!userClient.User.Setting.PeriodicCheckingConfirmation)
                    {
                        return CustomIcon.ConvertToPurple(icon);
                    }

                    if (userClient.User.Setting.AutoAcceptReceiveOffer)
                    {
                        return icon;
                    }

                    return CustomIcon.ConvertToGrayscale(icon);
                }
            });
            var icons = new CustomIcon[] { auto_deliver, auto_confirm, auto_accept };

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
            panel.SetUserAvatarBox(pictureBox);

            IconLabel iconLabel = new IconLabel(icons)
            {
                Name = "icons",
                Size = new Size(80, 20),
                IconSize = new Size(16, 16),
                Location = new Point(0, 80),
            };
            panel.SetIconsBox(iconLabel);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{userClient.GetAccount()} [{userClient.User.NickName}]",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, iconLabel.Location.Y + iconLabel.Height)
            };
            nameLabel.MouseClick += btnUser_Click;
            nameLabel.ContextMenuStrip = userContextMenuStrip;
            panel.SetUserNameBox(nameLabel);

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
                Location = new Point(0, nameLabel.Location.Y + nameLabel.Height)
            };
            offerLabel.Click += offersNumberBtn_Click;
            panel.SetOfferBox(offerLabel);

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
                Location = new Point(42, nameLabel.Location.Y + nameLabel.Height)
            };
            confirmationLabel.Click += confirmationNumberBtn_Click;
            panel.SetConfirmationBox(confirmationLabel);

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
                    control.Location = new Point(x * (index % cells) + 10, 146 * (index / cells) + 10);
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

using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;
using System.Web;

namespace Steam_Authenticator
{
    public partial class MainForm
    {
        private void buffUserPanel_SizeChanged(object sender, EventArgs e)
        {
            ResetBuffUserPanel();
        }

        private async void btnBuffUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Control control = sender as Control;
            BuffUserPanel panel = control.Parent as BuffUserPanel;
            BuffClient buffClient = panel.Client;

            if (buffClient.LoggedIn)
            {
                return;
            }

            await buffClient.RefreshAsync(true);
        }

        private void addBuffUserBtn_Click(object sender, EventArgs e)
        {
            BuffLogin("请扫码登录 BUFF 帐号");
        }

        private void buffOffersNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            BuffUserPanel panel = control.Parent as BuffUserPanel;
            BuffClient buffClient = panel.Client;
        }

        private void buffSettingMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            BuffUserPanel panel = menuStrip.SourceControl.Parent as BuffUserPanel;
            BuffClient client = panel.Client;

            BuffSetting buffSetting = new BuffSetting(client.User.Setting);
            if (buffSetting.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var setting = buffSetting.Setting;
            client.User.Setting = setting;

            Appsetting.Instance.Manifest.SaveBuffUser(client.User.UserId, client.User);
        }

        private async void buffLoginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            BuffUserPanel panel = menuStrip.SourceControl.Parent as BuffUserPanel;
            BuffClient client = panel.Client;

            await client.RefreshAsync(true);

            if (client.LoggedIn)
            {
                return;
            }

            BuffLogin($"登录信息已失效{Environment.NewLine}请重新扫码登录 BUFF 帐号");
        }

        private void removeBuffUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            BuffUserPanel panel = menuStrip.SourceControl.Parent as BuffUserPanel;
            BuffClient client = panel.Client;

            Appsetting.Instance.Manifest.RemoveBuffUser(client.User.UserId, out var entry);

            ResetBuffUserPanel();
        }

        private async Task LoadBuffUsers()
        {
            try
            {
                buffUsersPanel.Controls.Clear();

                int startX = GetBuffUserControlStartPointX(out int cells);

                Appsetting.Instance.BuffClients.RemoveAll(c => !c.LoggedIn);

                IEnumerable<string> accounts = Appsetting.Instance.Manifest.GetBuffUser();
                int index = 0;
                foreach (string account in accounts)
                {
                    BuffUser user = Appsetting.Instance.Manifest.GetBuffUser(account);
                    BuffClient client = new BuffClient(user);

                    BuffUserPanel panel = CreateUserPanel(startX, cells, index, client);
                    buffUsersPanel.Controls.Add(panel);

                    index++;
                }

                {
                    BuffUserPanel panel = new BuffUserPanel()
                    {
                        Size = new Size(80, 116),
                        Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                        Client = BuffClient.None
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
                    pictureBox.Click += addBuffUserBtn_Click;
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
                    nameLabel.Click += addBuffUserBtn_Click;
                    panel.Controls.Add(nameLabel);
                    buffUsersPanel.Controls.Add(panel);
                }

                var tasks = Appsetting.Instance.BuffClients.Select(c => c.RefreshAsync(true));
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetRefreshBuffUserTimer(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(10));
            }
        }

        private BuffClient BuffLogin(string tips)
        {
            var buffAuth = new BuffAuth(tips);
            if (buffAuth.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            var buffUser = new BuffUser
            {
                UserId = buffAuth.Result.Body.data.id,
                SteamId = buffAuth.Result.Body.data.steamid,
                Nickname = buffAuth.Result.Body.data.nickname,
                Avatar = buffAuth.Result.Body.data.avatar,
                BuffCookies = string.Join("; ", buffAuth.Result.Cookies.Select(cookie => $"{cookie.Name}={HttpUtility.UrlEncode(cookie.Value)}"))
            };
            var buffClient = new BuffClient(buffUser)
            {
                LoggedIn = true
            };

            Appsetting.Instance.Manifest.SaveBuffUser(buffClient.User.UserId, buffClient.User);

            var controlCollection = buffUsersPanel.Controls.Cast<BuffUserPanel>().ToList();
            var index = controlCollection.FindIndex(c => c.Client.User.UserId == buffUser.UserId);

            if (index < 0)
            {
                index = controlCollection.Count - 1;
            }
            else
            {
                controlCollection.RemoveAt(index);
            }

            int startX = GetBuffUserControlStartPointX(out int cells);
            BuffUserPanel panel = CreateUserPanel(startX, cells, index, buffClient);
            controlCollection.Insert(index, panel);

            buffUsersPanel.Controls.Clear();
            buffUsersPanel.Controls.AddRange(controlCollection.ToArray());
            ResetBuffUserPanel();

            return buffClient;
        }

        private BuffUserPanel CreateUserPanel(int startX, int cells, int index, BuffClient buffClient)
        {
            BuffUserPanel panel = new BuffUserPanel()
            {
                Name = buffClient.User.UserId,
                Size = new Size(80, 116),
                Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                Client = buffClient
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
            string avatar = buffClient.User.Avatar;
            pictureBox.Image = Properties.Resources.userimg;
            if (!string.IsNullOrEmpty(avatar))
            {
                pictureBox.LoadAsync(avatar);
            }
            pictureBox.MouseClick += btnBuffUser_Click;
            pictureBox.ContextMenuStrip = buffUserContextMenuStrip;
            panel.Controls.Add(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{buffClient.User.Nickname}",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = buffClient.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, 80)
            };
            nameLabel.MouseClick += btnBuffUser_Click;
            nameLabel.ContextMenuStrip = buffUserContextMenuStrip;
            panel.Controls.Add(nameLabel);

            Label offerLabel = new Label()
            {
                Name = "offer",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Default,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.FromArgb(255, 128, 0),
                Location = new Point(0, 98)
            };
            offerLabel.Click += buffOffersNumberBtn_Click;
            panel.Controls.Add(offerLabel);

            panel.Client
                .WithStartLogin((relogin) =>
                {
                    if (!relogin)
                    {
                        return;
                    }

                    nameLabel.ForeColor = Color.FromArgb(128, 128, 128);
                })
                .WithEndLogin((relogin, loggined) =>
                {
                    nameLabel.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private void ResetBuffUserPanel()
        {
            try
            {
                var controlCollection = buffUsersPanel.Controls.Cast<BuffUserPanel>().ToArray();

                int x = GetBuffUserControlStartPointX(out int cells);

                int index = 0;
                foreach (Control control in controlCollection)
                {
                    control.Location = new Point(x * (index % cells) + 10, 126 * (index / cells) + 10);
                    index++;
                }

                buffUsersPanel.Controls.Clear();
                buffUsersPanel.Controls.AddRange(controlCollection);
            }
            catch
            {

            }
        }

        private int GetBuffUserControlStartPointX(out int cells)
        {
            cells = (buffUsersPanel.Size.Width - 30) / 80;
            int size = (buffUsersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            if (size < 85)
            {
                cells = cells - 1;
                size = (buffUsersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            }
            return size;
        }

        private void RefreshBuffUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = buffUsersPanel.Controls.Cast<BuffUserPanel>().ToArray();
                    foreach (BuffUserPanel userPanel in controlCollection)
                    {
                        //var nameLabel = userPanel.Controls.Cast<Control>().FirstOrDefault(c => c.Name == "username") as Label;
                        var nameLabel = userPanel.Controls.Find("username", false)?.FirstOrDefault() as Label;
                        if (nameLabel == null)
                        {
                            continue;
                        }

                        var buffClient = userPanel.Client;
                        var user = buffClient.User;

                        buffClient.RefreshAsync(false, tokenSource.Token).GetAwaiter().GetResult();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshBuffUserTimer(TimeSpan.FromSeconds(10 * 60), TimeSpan.FromSeconds(10 * 60));
            }
        }
    }
}

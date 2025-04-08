using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;

namespace Steam_Authenticator
{
    public partial class MainForm
    {
        private const double RefreshYouPinUserInterval = 1 * 60;

        private void youpinUsersPanel_SizeChanged(object sender, EventArgs e)
        {
            youpinUsersPanel.Reset();
        }

        private async void youpinUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var control = sender as Control;
            var panel = control.Parent as YouPinUserPanel;
            var client = panel.Client;

            if (client.LoggedIn)
            {
                return;
            }

            await client.RefreshAsync(true);
        }

        private void addYouPinUserBtn_Click(object sender, EventArgs e)
        {
            YouPinLogin($"请登录 悠悠有品 帐号");
        }

        private async void youpinReloginMenuItem_Click(object sender, EventArgs e)
        {
            (YouPinUserPanel panel, YouPinClient client) = GetClient(sender as ToolStripMenuItem);

            await client.RefreshAsync(true);

            if (client.LoggedIn)
            {
                return;
            }

            YouPinLogin($"登录信息已失效{Environment.NewLine}请重新登录 悠悠有品 帐号");
        }

        private async void youpinLogoutMenuItem_Click(object sender, EventArgs e)
        {
            (YouPinUserPanel panel, YouPinClient client) = GetClient(sender as ToolStripMenuItem);

            await client.LogoutAsync();

            ResetRefreshYouPinUserTimer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(RefreshBuffUserInterval));
        }

        private void removeYouPinUserMenuItem_Click(object sender, EventArgs e)
        {
            (YouPinUserPanel panel, YouPinClient client) = GetClient(sender as ToolStripMenuItem);

            Appsetting.Instance.Manifest.RemoveYouPinUser(client.User.UserId, out var entry);
            Appsetting.Instance.YouPinClients.Remove(client);

            youpinUsersPanel.RemoveClient(client);
        }

        private async Task LoadYouPinUsers()
        {
            try
            {
                youpinUsersPanel.ClearItems();

                var accounts = Appsetting.Instance.Manifest.GetYouPinUser().ToList();

                foreach (string account in accounts)
                {
                    YouPinUser user = Appsetting.Instance.Manifest.GetYouPinUser(account);
                    YouPinClient client = new YouPinClient(user);
                    Appsetting.Instance.YouPinClients.Add(client);

                    AddUserPanel(client);
                }

                {
                    YouPinUserPanel panel = youpinUsersPanel.AddItemPanel(false, YouPinClient.None);

                    panel.ItemIcon.Image = Properties.Resources.add;
                    panel.ItemIcon.Click += addYouPinUserBtn_Click;

                    panel.ItemName.Text = $"添加帐号";
                    panel.ItemName.ForeColor = Color.FromArgb(244, 164, 96);
                    panel.ItemName.Click += addYouPinUserBtn_Click;

                    panel.Offer.Hide();
                }

                var tasks = Appsetting.Instance.YouPinClients.Select(c => c.RefreshAsync(true));
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetRefreshYouPinUserTimer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(RefreshYouPinUserInterval));
            }
        }

        private YouPinUserPanel AddUserPanel(YouPinClient client)
        {
            YouPinUserPanel panel = youpinUsersPanel.AddItemPanel(true, client);

            panel.ItemIcon.MouseClick += youpinUser_Click;
            panel.ItemIcon.ContextMenuStrip = youpinUserContextMenuStrip;

            panel.Client
                .WithStartLogin((relogin) =>
                {
                    if (!relogin)
                    {
                        return;
                    }

                    panel.ItemName.ForeColor = Color.FromArgb(128, 128, 128);
                })
                .WithEndLogin((relogin, loggined) =>
                {
                    panel.ItemName.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private YouPinClient YouPinLogin(string tips)
        {
            var auth = new YouPinLogin(tips);
            if (auth.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            var user = new YouPinUser
            {
                UserId = auth.Result.UserId,
                Nickname = auth.Result.NickName,
                Avatar = auth.Result.Avatar,
                SteamId = auth.Result.SteamId,
                Token = auth.Token
            };
            var client = new YouPinClient(user)
            {
                LoggedIn = true
            };

            Appsetting.Instance.Manifest.SaveYouPinUser(client.User.UserId, client.User);
            Appsetting.Instance.YouPinClients.RemoveAll(c => c.User.UserId == user.UserId);
            Appsetting.Instance.YouPinClients.Add(client);

            AddUserPanel(client);

            return client;
        }

        private (YouPinUserPanel Panel, YouPinClient Client) GetClient(ToolStripMenuItem menuItem)
        {
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            YouPinUserPanel panel = menuStrip.SourceControl.Parent as YouPinUserPanel;
            YouPinClient client = panel.Client;

            return (panel, client);
        }

        private void RefreshYouPinUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = youpinUsersPanel.ItemPanels;
                    foreach (var userPanel in controlCollection)
                    {
                        if (!userPanel.HasItem)
                        {
                            continue;
                        }

                        var clinet = userPanel.Client;
                        var user = clinet.User;

                        clinet.RefreshAsync(false, tokenSource.Token).GetAwaiter().GetResult();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshYouPinUserTimer(TimeSpan.FromSeconds(RefreshEcoUserInterval), TimeSpan.FromSeconds(RefreshEcoUserInterval * 1.5d));
            }
        }
    }
}

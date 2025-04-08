using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;
using System.Web;

namespace Steam_Authenticator
{
    public partial class MainForm
    {
        private const double RefreshBuffUserInterval = 5 * 60;

        private void buffUserPanel_SizeChanged(object sender, EventArgs e)
        {
            buffUsersPanel.Reset();
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
            BuffLogin($"请使用 BUFF App 扫码{Environment.NewLine}登录 BUFF 帐号");
        }

        private async void buffReloginMenuItem_Click(object sender, EventArgs e)
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

        private async void buffLogoutMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            BuffUserPanel panel = menuStrip.SourceControl.Parent as BuffUserPanel;
            BuffClient client = panel.Client;

            await client.LogoutAsync();

            ResetRefreshBuffUserTimer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(RefreshBuffUserInterval));
        }

        private void removeBuffUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            BuffUserPanel panel = menuStrip.SourceControl.Parent as BuffUserPanel;
            BuffClient client = panel.Client;

            Appsetting.Instance.Manifest.RemoveBuffUser(client.User.UserId, out var entry);
            Appsetting.Instance.BuffClients.Remove(client);

            buffUsersPanel.RemoveClient(client);
        }

        private async Task LoadBuffUsers()
        {
            try
            {
                buffUsersPanel.ClearItems();

                var accounts = Appsetting.Instance.Manifest.GetBuffUser().ToList();

                foreach (string account in accounts)
                {
                    BuffUser user = Appsetting.Instance.Manifest.GetBuffUser(account);
                    BuffClient client = new BuffClient(user);
                    Appsetting.Instance.BuffClients.Add(client);

                    AddUserPanel(client);
                }

                {
                    BuffUserPanel panel = buffUsersPanel.AddItemPanel(false, BuffClient.None);

                    panel.ItemIcon.Image = Properties.Resources.add;
                    panel.ItemIcon.Click += addBuffUserBtn_Click;

                    panel.ItemName.Text = $"添加帐号";
                    panel.ItemName.ForeColor = Color.FromArgb(244, 164, 96);
                    panel.ItemName.Click += addBuffUserBtn_Click;

                    panel.Offer.Hide();
                }

                var tasks = Appsetting.Instance.BuffClients.Select(c => c.RefreshAsync(true));
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetRefreshBuffUserTimer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(RefreshBuffUserInterval));
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
            Appsetting.Instance.BuffClients.RemoveAll(c => c.User.UserId == buffUser.UserId);
            Appsetting.Instance.BuffClients.Add(buffClient);

            AddUserPanel(buffClient);

            return buffClient;
        }

        private BuffUserPanel AddUserPanel(BuffClient buffClient)
        {
            BuffUserPanel panel = buffUsersPanel.AddItemPanel(true, buffClient);

            panel.ItemIcon.MouseClick += btnBuffUser_Click;
            panel.ItemIcon.ContextMenuStrip = buffUserContextMenuStrip;

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

        private void RefreshBuffUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = buffUsersPanel.ItemPanels;
                    foreach (BuffUserPanel userPanel in controlCollection)
                    {
                        if (!userPanel.HasItem)
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
                ResetRefreshBuffUserTimer(TimeSpan.FromSeconds(RefreshBuffUserInterval), TimeSpan.FromSeconds(RefreshBuffUserInterval * 1.5d));
            }
        }
    }
}

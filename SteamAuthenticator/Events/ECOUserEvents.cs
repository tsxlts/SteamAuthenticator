using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;

namespace Steam_Authenticator
{
    public partial class MainForm
    {
        private const double RefreshEcoUserInterval = 1 * 60;

        private async void ecoBuffUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Control control = sender as Control;
            EcoUserPanel panel = control.Parent as EcoUserPanel;
            EcoClient client = panel.Client;

            if (client.LoggedIn)
            {
                return;
            }

            await client.RefreshTokenAsync(true);
        }

        private async void addEcoUserBtn_Click(object sender, EventArgs e)
        {
            await EcoLogin("请扫码登录 ECO 帐号");
        }

        private async void ecoReloginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            EcoUserPanel panel = menuStrip.SourceControl.Parent as EcoUserPanel;
            EcoClient client = panel.Client;

            await client.RefreshTokenAsync(true);

            if (client.LoggedIn)
            {
                return;
            }

            await EcoLogin($"登录信息已失效{Environment.NewLine}请重新扫码登录 ECO 帐号");
        }

        private async void ecoLogoutMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            EcoUserPanel panel = menuStrip.SourceControl.Parent as EcoUserPanel;
            EcoClient client = panel.Client;

            await client.LogoutAsync();

            ResetRefreshEcoUserTimer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(RefreshEcoUserInterval));
        }

        private void removeEcoUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            EcoUserPanel panel = menuStrip.SourceControl.Parent as EcoUserPanel;
            EcoClient client = panel.Client;

            Appsetting.Instance.Manifest.RemoveEcoUser(client.User.UserId, out var entry);
            Appsetting.Instance.EcoClients.Remove(client);

            ecoUsersPanel.RemoveClient(client);
        }

        private async Task LoadEcoUsers()
        {
            try
            {
                ecoUsersPanel.ClearItems();

                var accounts = Appsetting.Instance.Manifest.GetEcoUser().ToList();

                foreach (string account in accounts)
                {
                    EcoUser user = Appsetting.Instance.Manifest.GetEcoUser(account);
                    EcoClient client = new EcoClient(user);

                    AddUserPanel(client);

                    Appsetting.Instance.EcoClients.Add(client);
                }

                {
                    EcoUserPanel panel = ecoUsersPanel.AddItemPanel(false, EcoClient.None);

                    panel.ItemIcon.Image = Properties.Resources.add;
                    panel.ItemIcon.Click += addEcoUserBtn_Click;

                    panel.ItemName.Text = $"添加帐号";
                    panel.ItemName.ForeColor = Color.FromArgb(244, 164, 96);
                    panel.ItemName.Click += addEcoUserBtn_Click;

                    panel.Offer.Hide();
                }

                var tasks = Appsetting.Instance.EcoClients.Select(c => c.RefreshTokenAsync(true));
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetRefreshEcoUserTimer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(RefreshEcoUserInterval));
            }
        }

        private async Task<EcoClient> EcoLogin(string tips)
        {
            var auth = new EcoAuth(tips);
            if (auth.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            var authResponse = auth.Result;

            var steamUserResponse = await EcoApi.QuerySteamUserAsync(authResponse.Token);
            var steamUserData = steamUserResponse?.StatusData?.ResultData;

            var user = new EcoUser
            {
                UserId = authResponse.UserId,
                Nickname = authResponse.UserName,
                Avatar = authResponse.Avatar ?? "",
                SteamIds = steamUserData?.Select(c => c.SteamId).ToList() ?? new List<string>(),
                ClientId = authResponse.ClientId,

                RefreshToken = authResponse.RefreshToken,
                RefreshTokenExpireTime = authResponse.RefreshTokenExpireDate
            };
            var client = new EcoClient(user)
            {
                Token = authResponse.Token
            };

            Appsetting.Instance.Manifest.SaveEcoUser(client.User.UserId, client.User);
            Appsetting.Instance.EcoClients.RemoveAll(c => c.User.UserId == user.UserId);
            Appsetting.Instance.EcoClients.Add(client);

            AddUserPanel(client);

            return client;
        }

        private EcoUserPanel AddUserPanel(EcoClient client)
        {
            EcoUserPanel panel = ecoUsersPanel.AddItemPanel(true, client);

            panel.ItemIcon.MouseClick += ecoBuffUser_Click;
            panel.ItemIcon.ContextMenuStrip = ecoUserContextMenuStrip;

            panel.ItemName.MouseClick += ecoBuffUser_Click;
            panel.ItemName.ContextMenuStrip = ecoUserContextMenuStrip;

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

        private void RefreshEcoUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = ecoUsersPanel.ItemPanels;
                    foreach (EcoUserPanel userPanel in controlCollection)
                    {
                        if (!userPanel.HasItem)
                        {
                            continue;
                        }

                        var buffClient = userPanel.Client;
                        var user = buffClient.User;

                        buffClient.RefreshClientAsync(tokenSource.Token).GetAwaiter().GetResult();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ResetRefreshEcoUserTimer(TimeSpan.FromSeconds(RefreshEcoUserInterval), TimeSpan.FromSeconds(RefreshEcoUserInterval * 1.5d));
            }
        }
    }
}

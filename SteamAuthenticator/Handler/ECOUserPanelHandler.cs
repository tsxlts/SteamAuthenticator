using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Handler
{
    internal class ECOUserPanelHandler : UserPanelHandler<EcoUserPanel, EcoClient>
    {
        public ECOUserPanelHandler(ItemCollectionPanel<EcoUserPanel, EcoClient> itemCollection) : base(itemCollection)
        {
        }

        protected override async Task<List<EcoUserPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default)
        {
            List<EcoUserPanel> panels = new List<EcoUserPanel>();

            var accounts = Appsetting.Instance.Manifest.GetEcoUser().ToList();
            foreach (string account in accounts)
            {
                EcoUser user = Appsetting.Instance.Manifest.GetEcoUser(account);
                EcoClient client = new EcoClient(user);
                Appsetting.Instance.EcoClients.Add(client);

                panels.Add(AddUserPanel(client));
            }

            {
                EcoUserPanel panel = UsersPanel.AddItemPanel(false, EcoClient.None);

                panel.ItemIcon.Image = Properties.Resources.add;
                panel.ItemIcon.Click += async (sender, e) =>
                {
                    await AddUser();
                };

                panel.ItemName.Text = $"添加帐号";
                panel.ItemName.ForeColor = Color.FromArgb(244, 164, 96);
                panel.ItemName.Click += async (sender, e) =>
                {
                    await AddUser();
                };

                panel.Offer.Hide();
            }

            var tasks = Appsetting.Instance.EcoClients.Select(c => c.RefreshTokenAsync(true));
            await Task.WhenAll(tasks);

            return panels;
        }

        protected override async Task<EcoUserPanel> AddUserInternal()
        {
            return await EcoLogin($"请使用 ECO App 扫码{Environment.NewLine}登录 ECO 帐号");
        }

        protected override Task RemoveUserInternal(EcoUserPanel panel, EcoClient client)
        {
            Appsetting.Instance.Manifest.RemoveEcoUser(client.User.UserId, out var entry);
            Appsetting.Instance.EcoClients.Remove(client);
            return Task.CompletedTask;
        }

        protected override async Task LogoutInternal(EcoUserPanel panel, EcoClient client)
        {
            await client.LogoutAsync();
        }

        protected override async Task ReloginInternal(EcoUserPanel panel, EcoClient client)
        {
            await client.RefreshTokenAsync(true);
            if (client.LoggedIn)
            {
                return;
            }

            await EcoLogin($"登录信息已失效{Environment.NewLine}请重新扫码登录 ECO 帐号");
        }

        protected override async Task RefreshUserInternal()
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                var controlCollection = UsersPanel.ItemPanels;
                foreach (EcoUserPanel userPanel in controlCollection)
                {
                    if (!userPanel.HasItem)
                    {
                        continue;
                    }

                    var buffClient = userPanel.Client;
                    var user = buffClient.User;

                    await buffClient.RefreshClientAsync(tokenSource.Token);
                }
            }
        }

        private async Task<EcoUserPanel> EcoLogin(string tips)
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

            return AddUserPanel(client);
        }

        private EcoUserPanel AddUserPanel(EcoClient client)
        {
            EcoUserPanel panel = UsersPanel.AddItemPanel(true, client);

            panel.ItemIcon.ContextMenuStrip = UserMenu;

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
    }
}

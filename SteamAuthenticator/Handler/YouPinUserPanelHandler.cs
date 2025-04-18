using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Handler
{
    internal class YouPinUserPanelHandler : UserPanelHandler<YouPinUserPanel, YouPinClient>
    {
        public YouPinUserPanelHandler(ItemCollectionPanel<YouPinUserPanel, YouPinClient> itemCollection) : base(itemCollection)
        {
        }

        protected override async Task<List<YouPinUserPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default)
        {
            List<YouPinUserPanel> panels = new List<YouPinUserPanel>();

            var accounts = Appsetting.Instance.Manifest.GetYouPinUser().ToList();
            foreach (string account in accounts)
            {
                YouPinUser user = Appsetting.Instance.Manifest.GetYouPinUser(account);
                YouPinClient client = new YouPinClient(user, false);
                Appsetting.Instance.YouPinClients.Add(client);

                panels.Add(AddUserPanel(client));
            }

            {
                YouPinUserPanel panel = UsersPanel.AddItemPanel(false, YouPinClient.None);

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

            var tasks = Appsetting.Instance.YouPinClients.Select(c => c.LoginAsync());
            await Task.WhenAll(tasks);

            return panels;
        }

        protected override Task<YouPinUserPanel> AddUserInternal()
        {
            return Task.FromResult(YouPinLogin($"请登录 悠悠有品 帐号"));
        }

        protected override Task RemoveUserInternal(YouPinUserPanel panel, YouPinClient client)
        {
            Appsetting.Instance.Manifest.RemoveYouPinUser(client.User.UserId, out var entry);
            Appsetting.Instance.YouPinClients.Remove(client);
            return Task.CompletedTask;
        }

        protected override async Task ReloginInternal(YouPinUserPanel panel, YouPinClient client)
        {
            await client.LoginAsync();
            if (client.LoggedIn)
            {
                return;
            }

            YouPinLogin($"登录信息已失效{Environment.NewLine}请重新登录 悠悠有品 帐号");
        }

        private YouPinUserPanel AddUserPanel(YouPinClient client)
        {
            YouPinUserPanel panel = UsersPanel.AddItemPanel(true, client);

            panel.ItemIcon.ContextMenuStrip = UserMenu;

            panel.Client
                .WithStartLogin(() =>
                {
                    panel.ItemName.ForeColor = Color.FromArgb(128, 128, 128);
                })
                .WithEndLogin((loggined) =>
                {
                    panel.ItemName.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private YouPinUserPanel YouPinLogin(string tips)
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
            var client = new YouPinClient(user, true)
            {
            };

            Appsetting.Instance.Manifest.SaveYouPinUser(client.User.UserId, client.User);
            Appsetting.Instance.YouPinClients.RemoveAll(c => c.User.UserId == user.UserId);
            Appsetting.Instance.YouPinClients.Add(client);

            return AddUserPanel(client);
        }
    }
}

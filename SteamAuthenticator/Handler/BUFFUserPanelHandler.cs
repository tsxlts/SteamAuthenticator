using System.Web;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Handler
{
    internal class BUFFUserPanelHandler : UserPanelHandler<BuffUserPanel, BuffClient>
    {
        public BUFFUserPanelHandler(ItemCollectionPanel<BuffUserPanel, BuffClient> itemCollection) : base(itemCollection)
        {
        }

        protected override async Task<List<BuffUserPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default)
        {
            List<BuffUserPanel> panels = new List<BuffUserPanel>();

            var accounts = Appsetting.Instance.Manifest.GetBuffUser().ToList();
            foreach (string account in accounts)
            {
                BuffUser user = Appsetting.Instance.Manifest.GetBuffUser(account);
                BuffClient client = new BuffClient(user, false);
                Appsetting.Instance.BuffClients.Add(client);

                panels.Add(AddUserPanel(client));
            }

            {
                BuffUserPanel panel = UsersPanel.AddItemPanel(false, BuffClient.None);

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

            var tasks = Appsetting.Instance.BuffClients.Select(c => c.LoginAsync());
            await Task.WhenAll(tasks);

            return panels;
        }

        protected override Task<BuffUserPanel> AddUserInternal()
        {
            return Task.FromResult(Login($"请使用 BUFF App 扫码{Environment.NewLine}登录 BUFF 帐号"));
        }

        protected override Task RemoveUserInternal(BuffUserPanel panel, BuffClient client)
        {
            Appsetting.Instance.Manifest.RemoveBuffUser(client.User.UserId, out var entry);
            Appsetting.Instance.BuffClients.Remove(client);
            return Task.CompletedTask;
        }

        protected override async Task ReloginInternal(BuffUserPanel panel, BuffClient client)
        {
            await client.LoginAsync();
            if (client.LoggedIn)
            {
                return;
            }

            Login($"登录信息已失效{Environment.NewLine}请重新扫码登录 BUFF 帐号");
        }

        private BuffUserPanel Login(string tips)
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
            var buffClient = new BuffClient(buffUser, true)
            {
            };

            Appsetting.Instance.Manifest.SaveBuffUser(buffClient.User.UserId, buffClient.User);
            Appsetting.Instance.BuffClients.RemoveAll(c => c.User.UserId == buffUser.UserId);
            Appsetting.Instance.BuffClients.Add(buffClient);

            return AddUserPanel(buffClient);
        }

        private BuffUserPanel AddUserPanel(BuffClient buffClient)
        {
            BuffUserPanel panel = UsersPanel.AddItemPanel(true, buffClient);

            panel.ItemIcon.ContextMenuStrip = UserMenu;
            panel.ItemIcon.MouseClick += user_MouseClick;

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

        private async void user_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var control = sender as Control;
            var userPanel = control.Parent as BuffUserPanel;
            var userClient = userPanel.Client;
            if (userClient.LoggedIn)
            {
                return;
            }

            await ReloginInternal(userPanel, userClient);
        }
    }
}

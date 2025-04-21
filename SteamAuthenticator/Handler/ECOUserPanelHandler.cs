using Steam_Authenticator.Controls;
using Steam_Authenticator.Factory;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.ECO;

namespace Steam_Authenticator.Handler
{
    internal class ECOUserPanelHandler : UserPanelHandler<EcoUserPanel, EcoClient>
    {
        private readonly System.Threading.Timer autoDeliverTimer;
        private readonly TimeSpan autoDeliverTimerMinPeriod = TimeSpan.FromSeconds(10);

        public ECOUserPanelHandler(ItemCollectionPanel<EcoUserPanel, EcoClient> itemCollection) : base(itemCollection)
        {
            autoDeliverTimer = new System.Threading.Timer(AutoDeliver, null, -1, -1);
        }

        protected override async Task<List<EcoUserPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default)
        {
            try
            {
                List<EcoUserPanel> panels = new List<EcoUserPanel>();

                var accounts = Appsetting.Instance.Manifest.GetEcoUser().ToList();
                foreach (string account in accounts)
                {
                    EcoUser user = Appsetting.Instance.Manifest.GetEcoUser(account);
                    EcoClient client = new EcoClient(user, false);
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

                var tasks = Appsetting.Instance.EcoClients.Select(c => c.LoginAsync());
                await Task.WhenAll(tasks);

                return panels;
            }
            finally
            {
                autoDeliverTimer.Change(TimeSpan.Zero, autoDeliverTimerMinPeriod * 1.5);
            }
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

        protected override async Task ReloginInternal(EcoUserPanel panel, EcoClient client)
        {
            await client.LoginAsync();
            if (client.LoggedIn)
            {
                return;
            }

            await EcoLogin($"登录信息已失效{Environment.NewLine}请重新扫码登录 ECO 帐号");
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
            var client = new EcoClient(user, true)
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

            var userMenu = new ContextMenuStrip();
            userMenu.Items.AddRange(UserMenu.Items.OfType<ToolStripItem>().ToArray());
            userMenu.Items.Add("设置").Click += (sender, e) =>
            {
                var client = GetClient(sender as ToolStripMenuItem);
                var setting = new ECOSetting(client.Client.User);
                setting.ShowDialog();
            };

            panel.ItemIcon.ContextMenuStrip = userMenu;

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

        private void AutoDeliver(object _)
        {
            try
            {
                var clients = Appsetting.Instance.EcoClients.Where(c => c.User.Setting?.AutoSendOffer ?? false).ToList();

                TaskFactory taskFactory = new TaskFactory();
                List<Task> tasks = new List<Task>();
                foreach (var item in clients)
                {
                    if (!item.LoggedIn)
                    {
                        continue;
                    }

                    var task = taskFactory.StartNew((obj) =>
                    {
                        var client = obj as EcoClient;
                        try
                        {
                            var queryOrders = client.QueryOffers("730").GetAwaiter().GetResult();
                            var orders = queryOrders?.StatusData?.ResultData?.Where(c => c.State == 1)?.ToList() ?? new List<QueryOffersResponse>();
                            AppLogger.Instance.Debug(client.User.UserId, "ECO-queryOrder", $"###查询 ECO 订单###" +
                                $"{Environment.NewLine}ECO响应: code:{queryOrders?.StatusData?.ResultCode}, msg:{queryOrders?.StatusData?.ResultMsg}" +
                                $"{Environment.NewLine}发货订单信息: [{string.Join(",", orders.Select(c => $"{c.OrderNum}") ?? new List<string>())}]");

                            if (!orders.Any())
                            {
                                return;
                            }

                            var deliver = client.ResolveOffers(orders.Select(c => c.OrderNum)).GetAwaiter().GetResult();
                            AppLogger.Instance.Debug(client.User.UserId, "ECO-deliver", $"###ECO 订单发货###" +
                                $"{Environment.NewLine}ECO响应: code:{deliver?.StatusData?.ResultCode}, msg:{deliver?.StatusData?.ResultMsg}");
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Instance.Debug(client.User.UserId, "ECO-deliver", $"###ECO 订单发货异常###" +
                                $"{Environment.NewLine}{ex.Message}");

                            AppLogger.Instance.Error(ex);
                        }
                    }, item);
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch
            {

            }
            finally
            {
                autoDeliverTimer?.Change(autoDeliverTimerMinPeriod, autoDeliverTimerMinPeriod * 3);
            }
        }
    }
}

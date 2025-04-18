using Steam_Authenticator.Controls;
using Steam_Authenticator.Factory;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Handler
{
    internal class C5UserPanelHandler : UserPanelHandler<C5UserPanel, C5Client>
    {
        private readonly System.Threading.Timer autoDeliverTimer;
        private readonly TimeSpan autoDeliverTimerMinPeriod = TimeSpan.FromSeconds(10);

        public C5UserPanelHandler(ItemCollectionPanel<C5UserPanel, C5Client> itemCollection) : base(itemCollection)
        {
            autoDeliverTimer = new System.Threading.Timer(AutoDeliver, null, -1, -1);
        }

        protected override async Task<List<C5UserPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default)
        {
            try
            {
                List<C5UserPanel> panels = new List<C5UserPanel>();

                var accounts = Appsetting.Instance.Manifest.GetC5User().ToList();
                foreach (string account in accounts)
                {
                    C5User user = Appsetting.Instance.Manifest.GetC5User(account);
                    C5Client client = new C5Client(user, false);
                    Appsetting.Instance.C5Clients.Add(client);

                    panels.Add(AddUserPanel(client));
                }

                {
                    C5UserPanel panel = UsersPanel.AddItemPanel(false, C5Client.None);

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

                var tasks = Appsetting.Instance.C5Clients.Select(c => c.LoginAsync());
                await Task.WhenAll(tasks);

                return panels;
            }
            finally
            {
                autoDeliverTimer.Change(TimeSpan.Zero, autoDeliverTimerMinPeriod * 1.5);
            }
        }

        protected override Task<C5UserPanel> AddUserInternal()
        {
            return Login($"请设置 C5Game 开放平台AppKey");
        }

        protected override Task RemoveUserInternal(C5UserPanel panel, C5Client client)
        {
            Appsetting.Instance.Manifest.RemoveC5User(client.User.UserId, out var entry);
            Appsetting.Instance.C5Clients.Remove(client);
            return Task.CompletedTask;
        }

        protected override async Task ReloginInternal(C5UserPanel panel, C5Client client)
        {
            await client.LoginAsync();
            if (client.LoggedIn)
            {
                return;
            }

            await Login($"你的AppKey已失效{Environment.NewLine}请重新设置 C5Game 开放平台AppKey");
        }

        private C5UserPanel AddUserPanel(C5Client client)
        {
            C5UserPanel panel = UsersPanel.AddItemPanel(true, client);

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

        private async Task<C5UserPanel> Login(string tips)
        {
            var auth = new RichTextInput("设置C5Game帐号", tips, true, "请输入AppKey")
            {
                Width = 300,
                Height = 200
            };
            if (auth.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            var appkey = auth.InputValue;
            var userResponse = await C5Api.QueryUserInfo(appkey);
            if (userResponse.Body?.errorCode != 0 || !userResponse.Body.success || userResponse.Body.data == null)
            {
                MessageBox.Show($"获取用户信息失败" +
                    $"{Environment.NewLine}错误代码：{userResponse.Body?.errorCode}" +
                    $"{Environment.NewLine}错误信息：{userResponse.Body?.errorMsg}",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }

            var userInfo = userResponse.Body.data;

            var user = new C5User
            {
                UserId = userInfo.uid,
                Nickname = userInfo.nickname,
                Avatar = userInfo.avatar,
                SteamIds = userInfo.steamList.Select(c => c.steamId)?.ToList() ?? new List<string>(),
                AppKey = appkey
            };
            var client = new C5Client(user, true)
            {
            };

            Appsetting.Instance.Manifest.SaveC5User(client.User.UserId, client.User);
            Appsetting.Instance.C5Clients.RemoveAll(c => c.User.UserId == user.UserId);
            Appsetting.Instance.C5Clients.Add(client);

            return AddUserPanel(client);
        }

        private void AutoDeliver(object _)
        {
            try
            {
                var clients = Appsetting.Instance.C5Clients.ToList();

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
                        var client = obj as C5Client;
                        try
                        {
                            var queryOrders = client.QuerySellerOrders(1).GetAwaiter().GetResult();
                            AppLogger.Instance.Debug(client.User.UserId, "C5Game-queryOrder", $"###查询 C5Game 订单###" +
                                $"{Environment.NewLine}C5响应: httpStatusCode:{queryOrders.HttpStatusCode}, code:{queryOrders.Body?.errorCode}, msg:{queryOrders.Body?.errorMsg}" +
                                $"{Environment.NewLine}发货订单信息: [{string.Join(",", queryOrders.Body?.data?.list?.Select(c => $"{c.orderId}#{c.statusName}") ?? new List<string>())}]");

                            var orders = queryOrders.Body?.data?.list ?? new List<Model.C5.C5Order>();
                            if (!orders.Any())
                            {
                                return;
                            }

                            var deliver = client.Deliver(orders.Select(c => c.orderId)).GetAwaiter().GetResult();
                            AppLogger.Instance.Debug(client.User.UserId, "C5Game-deliver", $"###C5Game 订单发货###" +
                                $"{Environment.NewLine}C5响应: httpStatusCode:{deliver.HttpStatusCode}, code:{deliver.Body?.errorCode}, msg:{deliver.Body?.errorMsg}");
                        }
                        catch (Exception ex)
                        {
                            AppLogger.Instance.Debug(client.User.UserId, "C5Game-deliver", $"###C5Game 订单发货异常###" +
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

using SkiaSharp;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.ECO;
using System.Diagnostics;
using System.Text;
using Vdaima.Utils.Email;

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

        private void ecoAutoBuySettingMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            EcoUserPanel panel = menuStrip.SourceControl.Parent as EcoUserPanel;
            EcoClient client = panel.Client;

            EcoAutoBuySetting ecoAutoBuySetting = new EcoAutoBuySetting(client);
            ecoAutoBuySetting.ShowDialog();
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

                AutoBuy(null);
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

            var localUser = Appsetting.Instance.Manifest.GetEcoUser(authResponse.UserId);

            var steamUserResponse = await EcoApi.QuerySteamUserAsync(authResponse.Token);
            var steamUserData = steamUserResponse?.StatusData?.ResultData;

            var user = new EcoUser
            {
                UserId = authResponse.UserId,
                Nickname = authResponse.UserName,
                Avatar = authResponse.Avatar ?? "",
                SteamUsers = steamUserData?.Select(c => new EcoUser.SteamUser
                {
                    SteamId = c.SteamId,
                    NickName = c.NickName
                }).ToList() ?? new List<EcoUser.SteamUser>(),
                ClientId = authResponse.ClientId,

                RefreshToken = authResponse.RefreshToken,
                RefreshTokenExpireTime = authResponse.RefreshTokenExpireDate,

                BuyGoods = localUser.BuyGoods ?? new List<EcoUser.AutoBuyGoods>()
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

        private Task AutoBuy(object _)
        {
            return Task.Run(() =>
            {
                var emailSetting = new EmailSetting
                {
                    SenderName = "天山雪莲",
                    Sender = "test@vdaima.cn",
                    Password = "test.123",
                    Server = "smtp.vdaima.cn",
                    Port = 465,
                    UseSsl = true,
                };
                EmailSender emailSender = new EmailSender(emailSetting);

                TaskFactory taskFactory = new TaskFactory();
                Stopwatch stopwatch;
                while (true)
                {
                    long interval = 500;
                    stopwatch = Stopwatch.StartNew();
                    try
                    {
                        List<Task> tasks = new List<Task>();

                        var clients = Appsetting.Instance.EcoClients;
                        foreach (var item in clients)
                        {
                            foreach (var itemGoods in item.User.BuyGoods.Where(c => c.Enabled))
                            {
                                var task = taskFactory.StartNew((obj) =>
                                {
                                    try
                                    {
                                        var @params = ((EcoClient Client, EcoUser.AutoBuyGoods Goods))obj;
                                        var client = @params.Client;
                                        var goods = @params.Goods;

                                        var goodsDetailResponse = EcoApi.QueryGoodsDetailAsync(goods.GameId, goods.HashName).GetAwaiter().GetResult();
                                        var goodsDetail = goodsDetailResponse?.StatusData?.ResultData;
                                        if (goodsDetail == null || goodsDetail.BottomPrice > goods.MaxPrice)
                                        {
                                            return;
                                        }

                                        decimal ecoPrice = goodsDetail.BottomPrice;

                                        AutoBuy(client, gameId: goods.GameId, hashName: goods.HashName,
                                          maxPrice: goods.MaxPrice, queryCount: 20,
                                          buySize: goods.BuySize,
                                          steamId: goods.SteamId,
                                          payType: goods.PayType).ContinueWith(results =>
                                          {
                                              var createOrders = results.Result;
                                              List<string> orders = new List<string>();
                                              List<string> payUrls = new List<string>();
                                              var goodsCount = createOrders.GoodsCount;
                                              StringBuilder msgBuilder = new StringBuilder();

                                              foreach (var orderResponse in createOrders.OrderResponses)
                                              {
                                                  var orderCount = orderResponse.StatusData?.ResultData?.OrderNum?.Count;

                                                  var msg = orderResponse.StatusMsg;
                                                  if (!string.IsNullOrWhiteSpace(orderResponse.StatusData?.ResultMsg))
                                                  {
                                                      msg = orderResponse.StatusData?.ResultMsg;
                                                  }

                                                  string payurl = orderResponse.StatusData?.ResultData?.Action;
                                                  if (payurl?.StartsWith("data:image/png;base64,") ?? false)
                                                  {
                                                      payurl = payurl.Replace("data:image/png;base64,", "");
                                                      var buffer = Convert.FromBase64String(payurl);
                                                      using (var skiaImage = SKBitmap.Decode(buffer))
                                                      {
                                                          var skiaReader = new ZXing.SkiaSharp.BarcodeReader();
                                                          var skiaResult = skiaReader.Decode(skiaImage);
                                                          payurl = skiaResult?.Text;
                                                      }
                                                  }

                                                  orders.AddRange(orderResponse.StatusData?.ResultData?.OrderNum ?? new List<string>());
                                                  payUrls.Add(payurl);
                                                  msgBuilder.AppendLine($"[{msg}]");
                                              }

                                              if (string.IsNullOrWhiteSpace(goods.NotifyAddress))
                                              {
                                                  return;
                                              }

                                              StringBuilder payBuilder = new StringBuilder();
                                              foreach (var item in payUrls)
                                              {
                                                  payBuilder.Append($"<p>支付地址：{item}</p>");
                                              }

                                              emailSender.SendEmailAsync(goods.NotifyAddress,
                                                  $"市场价格通知",
                                                  $"<div>" +
                                                  $"<p style='font-weight: bold;'>ECO市场价格通知</p>" +
                                                  $"</div>" +
                                                  $"<div>" +
                                                  $"<p>饰品名称：{goods.HashName}</p>" +
                                                  $"<p>监控价格：{goods.MaxPrice}</p>" +
                                                  $"<p>市场价格：{ecoPrice}</p>" +
                                                  $"<p>商品数量：{goodsCount}</p>" +
                                                  $"<p>下单数量：{orders.Count}</p>" +
                                                  $"<p>下单返回：{msgBuilder}</p>" +
                                                  $"</div>" +
                                                  $"<div>{payBuilder}</div>",
                                                  MimeKit.Text.TextFormat.Html);
                                          });
                                    }
                                    catch
                                    {

                                    }
                                }, (item, itemGoods));

                                tasks.Add(task);
                            };
                        }

                        Task.WaitAll(tasks.ToArray());
                    }
                    catch
                    {

                    }
                    finally
                    {
                        interval = interval - stopwatch.ElapsedMilliseconds;

                        if (interval > 0)
                        {
                            Thread.Sleep((int)interval);
                        }
                    }
                }
            });
        }

        private async Task<(IEnumerable<EcoResponse<CreateOrderResponse>> OrderResponses, int GoodsCount)> AutoBuy(EcoClient client, string gameId, string hashName, decimal maxPrice, int queryCount, int buySize, string steamId, PayType payType)
        {
            try
            {
                var goodsResponse = await EcoApi.QuerySellGoodsAsync(client, gameId: gameId, hashName: hashName, maxPrice: maxPrice, count: queryCount);
                var goods = goodsResponse?.StatusData?.ResultData?.PageResult;
                if (!(goods?.Any() ?? false))
                {
                    return (new[]{new EcoResponse<CreateOrderResponse>
                    {
                        StatusCode = goodsResponse?.StatusCode ?? "9999",
                        StatusMsg = goodsResponse?.StatusMsg,
                        StatusData = new Statusdata<CreateOrderResponse>
                        {
                            ResultCode = goodsResponse?.StatusData?.ResultCode ?? "9999",
                            ResultMsg = goodsResponse?.StatusData?.ResultMsg,
                            ResultData = null
                        }
                    } }, 0);
                }

                List<Task<EcoResponse<CreateOrderResponse>>> tasks = new List<Task<EcoResponse<CreateOrderResponse>>>();

                do
                {
                    var items = goods.Take(buySize).ToList();
                    var buyGoods = new List<GoodsInfo>();
                    foreach (var item in items)
                    {
                        buyGoods.Add(new GoodsInfo { GoodsNum = item.GoodsNum, Price = item.SellingPrice });
                        goods.RemoveAll(c => c.GoodsNum == item.GoodsNum);
                    }

                    var creameOrdersItem = EcoApi.CreateOrdersAsync(client,
                        steamId,
                        payType,
                        buyGoods);

                    tasks.Add(creameOrdersItem);

                } while (goods.Any());

                var result = await Task.WhenAll(tasks);
                return (result, goods.Count);
            }
            catch (Exception ex)
            {
                return (new[]{new EcoResponse<CreateOrderResponse>
                {
                    StatusCode = "-1",
                    StatusMsg = ex.Message,
                    StatusData = null
                } }, 0);
            }
        }
    }
}

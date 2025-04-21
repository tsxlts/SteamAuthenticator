using System.Diagnostics;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Model.Steam;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class Inventory : Form
    {
        private readonly UserClient client;
        private readonly IDictionary<string, SelfInventoryResponse> appInventory;
        private readonly ContextMenuStrip inventoryPanelContextMenuStrip;
        private readonly int pageSize = 24;

        public Inventory(UserClient client)
        {
            MaximizeBox = MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeComponent();

            this.client = client;
            appInventory = new Dictionary<string, SelfInventoryResponse>();

            inventoryPanelContextMenuStrip = new ContextMenuStrip();
            inventoryPanelContextMenuStrip.Items.Add("刷新库存").Click += refreshInventory_Click;
            inventoryPanelContextMenuStrip.Items.Add("新建交易报价").Click += createOffer_Click;
        }

        private async void Inventory_Load(object sender, EventArgs e)
        {
            await Init();
        }

        private async void reloadBtn_Click(object sender, EventArgs e)
        {
            await Init();
        }

        private async void refreshInventory_Click(object sender, EventArgs e)
        {
            var page = inventoryPages.TabPages[inventoryPages.SelectedIndex].Controls.Find("inventoryPanel", false)?.FirstOrDefault() as InventoryCollectionPanel;
            if (page == null)
            {
                return;
            }

            var context = page.Context;
            appInventory.Remove(context.AppId);

            await LoadAsset(1, pageSize);
        }

        private async void createOffer_Click(object sender, EventArgs e)
        {
            RichTextInput richTextInput = new RichTextInput("新建交易报价", "请输入对方交易链接", true, "对方交易链接不能为空");
            richTextInput.ShowDialog();

            string tradeLink = richTextInput.InputValue;
            if (string.IsNullOrWhiteSpace(tradeLink))
            {
                return;
            }

            Browser browser = new Browser()
            {
                Size = new Size(500, 800)
            };
            browser.Show();

            try
            {
                var uri = new Uri(tradeLink);
                browser.SetCookies($"{uri.Scheme}://{uri.Host}", client.Client.WebCookie.ToArray());
                await browser.LoadUrl(uri);
            }
            catch
            {
                browser.Close();
                browser.Dispose();
            }
        }

        private async void inventoryPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inventoryPages.SelectedIndex < 0)
            {
                return;
            }

            ResetPage();
            await LoadAsset(1, pageSize);
        }

        private async void preBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int.TryParse(currentPageBox.Text, out int pageIndex);
            await LoadAsset(pageIndex - 1, pageSize);
        }

        private async void netBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int.TryParse(currentPageBox.Text, out int pageIndex);
            await LoadAsset(pageIndex + 1, pageSize);
        }

        private void inventory_Click(object sender, EventArgs e)
        {
            try
            {
                var inventoryPanel = (sender as Control)?.Parent as InventoryPanel;
                if (inventoryPanel?.Client?.Description == null)
                {
                    return;
                }

                string url = $"{SteamBulider.DefaultSteamCommunity}" +
                    $"/market/listings" +
                    $"/{inventoryPanel.Client.Description.AppId}" +
                    $"/{Uri.EscapeDataString(inventoryPanel.Client.Description.MarketHashName)}";

                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {

            }
        }

        private async Task Init()
        {
            try
            {
                reloadBtn.Enabled = false;

                await LoadContexts();
                await LoadAsset(1, pageSize);
            }
            finally
            {
                reloadBtn.Enabled = true;
            }
        }

        private async Task LoadContexts(CancellationToken cancellationToken = default)
        {
            try
            {
                using (new Loading(defaultInventory))
                {
                    SteamId.Text = client.Client.SteamId;
                    UserName.Text = $"{client.GetAccount()} （{client.User.NickName}）";

                    var contextsResponse = await SteamApi.GetAppInventoryContextsAsync(client.Client.SteamId, client.Client.WebCookie);
                    var contexts = contextsResponse.Body;
                    inventoryPages.TabPages.Clear();
                    if (!contexts.Any())
                    {
                        var tab = new TabPage
                        {
                            Text = "Steam",
                        };
                        var inventoryPanel = new InventoryCollectionPanel(new AppInventoryContextsResponse { AppId = "753", Name = "Steam" })
                        {
                            Name = "inventoryPanel",
                            TabIndex = 100,
                            AutoScroll = true,
                            Dock = DockStyle.Fill,
                            Location = new Point(3, 3),
                            BackColor = Color.White,
                            ContextMenuStrip = inventoryPanelContextMenuStrip
                        };
                        inventoryPanel.SizeChanged += (sender, args) =>
                        {
                            inventoryPanel.Reset();
                        };

                        tab.Controls.Add(inventoryPanel);
                        inventoryPages.TabPages.Add(tab);
                        return;
                    }

                    inventoryPages.TabPages.Clear();
                    foreach (var item in contexts)
                    {
                        var tab = new TabPage
                        {
                            Text = item.Name,
                        };
                        var inventoryPanel = new InventoryCollectionPanel(item)
                        {
                            Name = "inventoryPanel",
                            TabIndex = 100,
                            AutoScroll = true,
                            Dock = DockStyle.Fill,
                            Location = new Point(3, 3),
                            BackColor = Color.White,
                            ContextMenuStrip = inventoryPanelContextMenuStrip
                        };
                        inventoryPanel.SizeChanged += (sender, args) =>
                        {
                            inventoryPanel.Reset();
                        };

                        tab.Controls.Add(inventoryPanel);
                        inventoryPages.TabPages.Add(tab);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载库存失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAsset(int pageInex, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                prePageBtn.Enabled = false;
                nextPageBtn.Enabled = false;

                var page = inventoryPages.TabPages[inventoryPages.SelectedIndex].Controls.Find("inventoryPanel", false)?.FirstOrDefault() as InventoryCollectionPanel;
                if (page == null)
                {
                    ResetPage();
                    return;
                }

                page.ClearItems();
                var context = page.Context;

                using (new Loading(page))
                {
                    if (!appInventory.TryGetValue(context.AppId, out var inventoryResponse))
                    {
                        inventoryResponse = await client.Client.Inventory.QueryInventoryAsync(context.AppId, context.Contexts.FirstOrDefault()?.Id ?? "2", cancellationToken);
                        if (!inventoryResponse.Success)
                        {
                            return;
                        }
                        appInventory.TryAdd(context.AppId, inventoryResponse);
                    }

                    int inventoryTotal = inventoryResponse.Inventories.Count;
                    int pageTotal = (int)Math.Ceiling(inventoryTotal * 1.0m / pageSize);
                    pageInex = Math.Max(1, pageInex);
                    pageInex = Math.Min(pageInex, pageTotal);

                    var inventories = inventoryResponse.Inventories.Skip((pageInex - 1) * pageSize).Take(pageSize);
                    var descriptions = inventoryResponse.Descriptions;

                    List<SteamInventory> inventoryContext = new List<SteamInventory>();
                    foreach (var item in inventories)
                    {
                        InventoryDescription description = descriptions.FirstOrDefault(c => c.ClassId == item.ClassId && c.InstanceId == item.InstanceId);
                        inventoryContext.Add(new SteamInventory(item, description));
                    }
                    var panels = page.AddItemPanels(true, inventoryContext);
                    panels.ForEach(c =>
                    {
                        c.ItemIcon.Click += inventory_Click;
                    });

                    inventoryTotalBox.Text = $"{inventoryTotal}";
                    pageTotalBox.Text = $"{pageTotal}";
                    currentPageBox.Text = $"{pageInex}";

                    prePageBtn.Enabled = pageInex > 1;
                    nextPageBtn.Enabled = pageInex < pageTotal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载库存失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetPage()
        {
            inventoryTotalBox.Text = "0";
            pageTotalBox.Text = "1";
            currentPageBox.Text = "1";

            prePageBtn.Enabled = nextPageBtn.Enabled = true;
        }
    }
}

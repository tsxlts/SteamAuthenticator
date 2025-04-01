using Steam_Authenticator.Controls;
using Steam_Authenticator.Model.Steam;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class Inventory : Form
    {
        private readonly UserClient client;
        private List<AppInventoryContextsResponse> contexts = new List<AppInventoryContextsResponse>();

        public Inventory(UserClient client)
        {
            InitializeComponent();

            this.client = client;
        }

        private async void Inventory_Load(object sender, EventArgs e)
        {
            inventoryPages.SelectedIndexChanged += InventoryPages_SelectedIndexChanged;

            await LoadContexts();
        }

        private async void InventoryPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadAsset();
        }

        private async Task LoadContexts(CancellationToken cancellationToken = default)
        {
            try
            {
                SteamId.Text = client.Client.SteamId;
                UserName.Text = client.GetAccount();

                var contextsResponse = await SteamApi.GetAppInventoryContextsAsync(client.Client.SteamId, client.Client.WebCookie);
                var contexts = contextsResponse.Body;
                if (!contexts.Any())
                {
                    return;
                }

                this.contexts = contexts;
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
                    };
                    inventoryPanel.SizeChanged += (sender, args) =>
                    {
                        inventoryPanel.Reset();
                    };

                    tab.Controls.Add(inventoryPanel);
                    inventoryPages.TabPages.Add(tab);
                }

                await LoadAsset(cancellationToken);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task LoadAsset(CancellationToken cancellationToken = default)
        {
            try
            {
                var page = inventoryPages.TabPages[inventoryPages.SelectedIndex].Controls.Find("inventoryPanel", false)?.FirstOrDefault() as InventoryCollectionPanel;
                var context = page.Context;

                var inventoryResponse = await client.Client.Inventory.QueryInventoryAsync(context.AppId, context.Contexts.FirstOrDefault()?.Id ?? "2", cancellationToken);
                var inventories = inventoryResponse.Inventories;
                var descriptions = inventoryResponse.Descriptions;

                foreach (var item in inventories)
                {
                    InventoryDescription description = descriptions.FirstOrDefault(c => c.ClassId == item.ClassId && c.InstanceId == item.InstanceId);

                    var panel = page.AddItemPanel(true, new SteamInventory(item, description));
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}

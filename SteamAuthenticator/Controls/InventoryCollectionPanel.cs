using Steam_Authenticator.Model.Steam;
using SteamKit.Model;

namespace Steam_Authenticator.Controls
{
    internal class InventoryCollectionPanel : ItemCollectionPanel<InventoryPanel, SteamInventory>
    {
        private readonly AppInventoryContextsResponse context;

        public InventoryCollectionPanel(AppInventoryContextsResponse context)
        {
            this.context = context;
        }

        protected override void RefreshItems()
        {
            foreach (var client in ItemPanels)
            {
                if (!client.HasItem)
                {
                    continue;
                }

                client.SetItemName(GetAssetName(client.Client), Color.Gray);
                client.SetItemIcon(GetIcon(client.Client));
            }
        }

        protected override InventoryPanel CreateUserPanel(bool hasItem, SteamInventory client)
        {
            int index = ItemPanels.Count;
            int y = ItemSize.Height + 10;

            InventoryPanel panel = new InventoryPanel(hasItem)
            {
                Size = ItemSize,
                Location = new Point(ItemStartX * (index % ItemCells) + 10, y * (index / ItemCells) + 10),
                Client = client
            };

            PictureBox pictureBox = new PictureBox()
            {
                Name = "assetIcon",
                Width = ItemSize.Width - 10,
                Height = ItemSize.Width - 10,
                Location = new Point(5, 5),
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.Zoom,
                InitialImage = Properties.Resources.loading,
            };
            string iconurl = GetIcon(client);
            pictureBox.Image = Properties.Resources.loading;
            if (!string.IsNullOrEmpty(iconurl))
            {
                pictureBox.LoadAsync(iconurl);
            }
            panel.SetItemIconBox(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "assetName",
                Text = GetAssetName(client),
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Location = new Point(0, 80)
            };
            panel.SetItemNameBox(nameLabel);

            Label exteriorLabel = new Label()
            {
                Name = "exterior",
                Text = GetExteriorName(client),
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Location = new Point(0, 98)
            };
            panel.SetExteriorBox(exteriorLabel);

            return panel;
        }

        protected override Size ItemSize => new Size(80, 116);

        public AppInventoryContextsResponse Context => context;

        private string GetAssetName(SteamInventory asset)
        {
            return asset.Description?.Name;
        }

        private string GetExteriorName(SteamInventory asset)
        {
            return asset.Description?.Tags?.FirstOrDefault(c => c.Category == "Exterior")?.Name;
        }

        private string GetIcon(SteamInventory asset)
        {
            return $"https://community.fastly.steamstatic.com/economy/image/{asset.Description?.IconUrl}";
        }
    }
}

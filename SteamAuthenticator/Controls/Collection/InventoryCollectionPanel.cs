using Steam_Authenticator.Model.Steam;
using SteamKit;
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
                BackColor = Color.FromArgb(128, 128, 128, 128),
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

            var tradableTime = GetTradableTime(client, out var color);
            Label tradableTimeLabel = new Label()
            {
                Name = "tradableTime",
                Text = tradableTime,
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 16),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = color,
                BackColor = Color.FromArgb(200, 128, 128, 128),
                Location = new Point(0, pictureBox.Height - 16)
            };
            pictureBox.Controls.Add(tradableTimeLabel);

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

        private string GetTradableTime(SteamInventory asset, out Color color)
        {
            foreach (var item in asset.Description.OwnerDescriptions ?? new List<AssetDescription>())
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    continue;
                }

                var time = Extension.GetAssetTradableExpires(item.Value);
                if (!time.HasValue)
                {
                    continue;
                }

                color = Color.Red;
                var diff = time.Value - DateTime.Now;
                if (diff.TotalDays > 1)
                {
                    return $"{diff.Days}天{diff.Hours}小时";
                }

                if (diff.TotalHours > 1)
                {
                    return $"{diff.Hours}小时{diff.Minutes}分";
                }

                if (diff.TotalMinutes > 1)
                {
                    return $"{diff.Minutes}分";
                }

                return $"1分";
            }

            if (!asset.Description.Tradable)
            {
                color = Color.Red;
                return "不可交易";
            }

            color = Color.Green;
            return "可交易";
        }
    }
}

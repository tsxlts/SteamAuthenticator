namespace Steam_Authenticator.Controls
{
    internal class EcoUserCollectionPanel : ItemCollectionPanel<EcoUserPanel, EcoClient>
    {
        public EcoUserPanel SetOffer(EcoClient client, int? offerCount)
        {
            if (client == null)
            {
                return null;
            }

            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.SetOffer(offerCount);
            return panel;
        }

        protected override Size ItemSize => new Size(80, 116);

        protected override EcoUserPanel CreateUserPanel(bool hasUser, EcoClient client)
        {
            int index = ItemPanels.Count;
            int y = ItemSize.Height + 10;

            EcoUserPanel panel = new EcoUserPanel(hasUser)
            {
                Size = ItemSize,
                Location = new Point(ItemStartX * (index % ItemCells) + 10, y * (index / ItemCells) + 10),
                Client = client
            };

            PictureBox pictureBox = new PictureBox()
            {
                Name = "useravatar",
                Width = 80,
                Height = 80,
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.Zoom,
                InitialImage = Properties.Resources.loading,
            };
            string avatar = client.User.Avatar;
            pictureBox.Image = Properties.Resources.userimg;
            if (!string.IsNullOrEmpty(avatar))
            {
                if (!avatar.StartsWith("http"))
                {
                    avatar = $"https:{avatar}";
                }
                pictureBox.LoadAsync(avatar);
            }
            panel.SetItemIconBox(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{client.User.Nickname}",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, 80)
            };
            panel.SetItemNameBox(nameLabel);

            Label offerLabel = new Label()
            {
                Name = "offer",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Default,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.FromArgb(255, 128, 0),
                Location = new Point(0, 98)
            };
            panel.SetOfferBox(offerLabel);

            return panel;
        }

        protected override void RefreshItems()
        {
            foreach (var client in ItemPanels)
            {
                if (!client.HasItem)
                {
                    continue;
                }

                client.SetItemIcon(client.Client.User.Avatar);
                client.SetItemName(client.Client.User.Nickname, client.Client.LoggedIn ? Color.Green : Color.Red);
            }
        }
    }
}

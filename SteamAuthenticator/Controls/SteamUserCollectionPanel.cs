
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Controls
{
    internal class SteamUserCollectionPanel : ItemCollectionPanel<SteamUserPanel, UserClient>
    {
        public SteamUserPanel SetOffer(UserClient client, int? offerCount)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.SetOffer(offerCount);
            return panel;
        }

        public SteamUserPanel SetConfirmation(UserClient client, int? confirmationCount)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.SetConfirmation(confirmationCount);
            return panel;
        }

        public SteamUserPanel RefreshIcon(UserClient client)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.RefreshIcon();
            return panel;
        }

        public void RefreshClients()
        {
            foreach (var client in ItemPanels)
            {
                client.RefreshIcon();
            }
        }

        protected override Size ItemSize => new Size(80, 136);

        protected override SteamUserPanel CreateUserPanel(bool hasUser, UserClient client)
        {
            int index = ItemPanels.Count;
            int y = ItemSize.Height + 10;

            SteamUserPanel panel = new SteamUserPanel(hasUser)
            {
                Size = ItemSize,
                Location = new Point(ItemStartX * (index % ItemCells) + 10, y * (index / ItemCells) + 10),
                Client = client
            };

            var guard = new CustomIcon(Properties.Resources.steam32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    Guard guard = Appsetting.Instance.Manifest.GetGuard(client.GetAccount());
                    if (guard != null)
                    {
                        return icon.Source;
                    }

                    return icon.ConvertToGrayscale();
                }
            });
            var auto_deliver = new CustomIcon(Properties.Resources.auto_deliver_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (client.User.Setting.PeriodicCheckingConfirmation && client.User.Setting.AutoAcceptGive())
                    {
                        return icon.Source;
                    }

                    return icon.ConvertToGrayscale();
                }
            });
            var auto_confirm = new CustomIcon(Properties.Resources.auto_confirm_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (client.User.Setting.PeriodicCheckingConfirmation && client.User.Setting.AutoConfirmOffer())
                    {
                        return icon.Source;
                    }

                    return icon.ConvertToGrayscale();
                }
            });
            var auto_accept = new CustomIcon(Properties.Resources.auto_accept_32, new CustomIcon.Options
            {
                Convert = (icon) =>
                {
                    if (client.User.Setting.PeriodicCheckingConfirmation && client.User.Setting.AutoAcceptReceive())
                    {
                        return icon.Source;
                    }

                    return icon.ConvertToGrayscale();
                }
            });
            var icons = new CustomIcon[] { guard, auto_deliver, auto_confirm, auto_accept };

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
                pictureBox.LoadAsync(avatar);
            }
            panel.SetItemIconBox(pictureBox);

            IconLabel iconLabel = new IconLabel(icons)
            {
                Name = "icons",
                Size = new Size(80, 20),
                IconSize = new Size(16, 16),
                Location = new Point(0, 80),
            };
            panel.SetIconsBox(iconLabel);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{client.GetAccount()} [{client.User.NickName}]",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = client.Client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, iconLabel.Location.Y + iconLabel.Height)
            };
            panel.SetItemNameBox(nameLabel);

            Label offerLabel = new Label()
            {
                Name = "offer",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopRight,
                ForeColor = Color.FromArgb(255, 128, 0),
                Location = new Point(0, nameLabel.Location.Y + nameLabel.Height)
            };
            panel.SetOfferBox(offerLabel);

            Label confirmationLabel = new Label()
            {
                Name = "confirmation",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(0, 128, 255),
                Location = new Point(42, nameLabel.Location.Y + nameLabel.Height)
            };
            panel.SetConfirmationBox(confirmationLabel);

            return panel;
        }
    }
}

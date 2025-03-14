
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

        protected override SteamUserPanel[] Sort(SteamUserPanel[] itemPanels)
        {
            var sortResults = itemPanels.OrderBy(c => c.HasItem ? 1 : 999).ThenBy(c =>
            {
                Guard guard = Appsetting.Instance.Manifest.GetGuard(c.Client.GetAccount());
                return guard != null ? 1 : 9;
            }).ThenBy(c => c.Client.User.Setting.AutoAcceptGive() ? 1 : 9)
            .ThenBy(c => c.Client.User.Setting.AutoConfirmOffer() ? 1 : 9)
            .ThenBy(c => c.Client.User.Setting.AutoAcceptReceive() ? 1 : 9)
            .ThenBy(c => c.Client.GetAccount());
            return base.Sort(sortResults.ToArray());
        }

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
                Width = ItemSize.Width - 10,
                Height = ItemSize.Width - 10,
                Location = new Point(5, 5),
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
                Size = new Size(75, 20),
                IconSize = new Size(15, 15),
                Location = new Point(5, 80),
            };
            panel.SetIconsBox(iconLabel);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{client.GetAccount()}",
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

        protected override void RefreshItems()
        {
            foreach (var client in ItemPanels)
            {
                if (!client.HasItem)
                {
                    continue;
                }

                client.SetItemIcon(client.Client.User.Avatar);
                client.SetItemName(client.Client.GetAccount(), client.Client.Client.LoggedIn ? Color.Green : Color.Red);
            }
        }
    }
}

namespace Steam_Authenticator.Controls
{
    internal abstract class ItemCollectionPanel<TItemPanel, TClient> : Panel where TItemPanel : ClientItemPanel<TClient> where TClient : IClient
    {
        protected int ItemStartX;
        protected int ItemCells;

        public ItemCollectionPanel()
        {
            this.SizeChanged += (object sender, EventArgs e) =>
            {
                ItemStartX = GetUserPanelStartPointX(out ItemCells);
            };
        }

        public List<TItemPanel> AddItemPanels(bool hasItem, IEnumerable<TClient> clients)
        {
            List<TItemPanel> itemPanels = new List<TItemPanel>();
            foreach (var client in clients)
            {
                TItemPanel userPanel = AddItem(hasItem, client);

                itemPanels.Add(userPanel);
            }

            this.Controls.Clear();
            this.Controls.AddRange(ItemPanels.ToArray());
            Reset();
            return itemPanels;
        }

        public TItemPanel AddItemPanel(bool hasItem, TClient client)
        {
            TItemPanel userPanel = AddItem(hasItem, client);

            this.Controls.Clear();
            this.Controls.AddRange(ItemPanels.ToArray());
            Reset();
            return userPanel;
        }

        public TItemPanel SetItemIcon(TClient client, string icon)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.SetItemIcon(icon);
            return panel;
        }

        public TItemPanel SetItemName(TClient client, string name, Color color)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            panel?.SetItemName(name, color);
            return panel;
        }

        public TItemPanel RemoveClient(TClient client)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            ItemPanels.Remove(panel);
            this.Controls.Remove(panel);
            Reset();
            return panel;
        }

        public TItemPanel SetChecked(TClient client, bool isChecked)
        {
            var panel = ItemPanels.Find(c => c.Client?.Key?.Equals(client.Key) ?? false);
            foreach (var item in ItemPanels)
            {
                item.SetChecked(false);
            }

            panel?.SetChecked(isChecked);
            return panel;
        }

        public void ClearItems()
        {
            ItemPanels.Clear();
            this.Controls.Clear();
        }

        public virtual void Reset()
        {
            try
            {
                int height = ItemSize.Height;

                var controlCollection = Sort(this.Controls.Cast<TItemPanel>().ToArray());

                int index = 0;
                foreach (Panel control in controlCollection)
                {
                    control.Location = new Point(ItemStartX * (index % ItemCells) + 10, (height + 10) * (index / ItemCells) + 10);
                    index++;
                }

                this.Controls.Clear();
                this.Controls.AddRange(controlCollection);

                RefreshItems();
            }
            catch
            {

            }
        }

        public int GetUserPanelStartPointX(out int cells)
        {
            int width = ItemSize.Width;

            cells = (this.Size.Width - 30) / width;
            int size = (this.Size.Width - 30 - cells * width) / (cells - 1) + width;
            if (size < width + 5)
            {
                cells = cells - 1;
                size = (this.Size.Width - 30 - cells * width) / (cells - 1) + width;
            }
            return size;
        }

        public readonly List<TItemPanel> ItemPanels = new List<TItemPanel>();

        protected virtual TItemPanel[] Sort(TItemPanel[] itemPanels)
        {
            return itemPanels;
        }

        protected abstract Size ItemSize { get; }

        protected abstract TItemPanel CreateUserPanel(bool hasItem, TClient client);

        protected abstract void RefreshItems();

        private TItemPanel AddItem(bool hasItem, TClient client)
        {
            int index = ItemPanels.FindIndex(c => c.Client?.Key == client?.Key);
            if (index < 0)
            {
                index = ItemPanels.Count(c => c.HasItem);
            }
            else
            {
                ItemPanels.RemoveAt(index);
            }

            TItemPanel panel = CreateUserPanel(hasItem, client);
            if (!hasItem)
            {
                panel.BgColor = Color.Transparent;
            }

            ItemPanels.Insert(index, panel);

            return panel;
        }
    }
}

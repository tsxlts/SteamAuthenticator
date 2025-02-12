
namespace Steam_Authenticator.Controls
{
    internal class ItemPanel : Panel
    {
        public readonly bool HasItem;

        public PictureBox ItemIcon;
        public Label ItemName;

        public ItemPanel(bool hasItem)
        {
            this.HasItem = hasItem;
        }

        public ItemPanel SetItemIconBox(PictureBox icon)
        {
            this.ItemIcon = icon;
            this.Controls.Add(this.ItemIcon);
            return this;
        }

        public ItemPanel SetItemNameBox(Label name)
        {
            this.ItemName = name;
            this.Controls.Add(this.ItemName);
            return this;
        }

        public ItemPanel SetItemIcon(string icon)
        {
            ItemIcon?.LoadAsync(icon);
            return this;
        }

        public ItemPanel SetItemName(string name, Color color)
        {
            if (!HasItem)
            {
                return this;
            }

            ItemName.Text = name;
            ItemName.ForeColor = color;
            return this;
        }

        public string ItemDisplayName => HasItem ? ItemName?.Text : null;
    }
}

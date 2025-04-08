
using Steam_Authenticator.Model.Steam;

namespace Steam_Authenticator.Controls
{
    internal class InventoryPanel : ClientItemPanel<SteamInventory>
    {
        private Label Exterior;

        public InventoryPanel(bool hasUser) : base(hasUser)
        {

        }

        public ItemPanel SetExteriorBox(Label name)
        {
            this.Exterior = name;
            this.Controls.Add(this.Exterior);
            return this;
        }

        public ItemPanel SetExteriorName(string name, Color color)
        {
            if (!HasItem)
            {
                return this;
            }

            Exterior.Text = name;
            Exterior.ForeColor = color;
            return this;
        }
    }
}

using SteamKit.Model;

namespace Steam_Authenticator.Model.Steam
{
    public class SteamInventory : Client
    {
        private readonly Inventory inventory;
        private readonly InventoryDescription description;

        public SteamInventory(Inventory inventory, InventoryDescription description)
        {
            this.inventory = inventory;
            this.description = description;
        }

        public string Key => inventory.AssetId.ToString();

        public Inventory Asset => inventory;

        public InventoryDescription Description => description;
    }
}

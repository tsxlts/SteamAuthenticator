using SteamKit.Model;

namespace Steam_Authenticator.Model.Steam
{
    public class SteamInventory : IClient
    {
        private readonly Inventory inventory;
        private readonly SelfInventoryDescription description;

        public SteamInventory(Inventory inventory, SelfInventoryDescription description)
        {
            this.inventory = inventory;
            this.description = description;
        }

        public string Key => inventory.AssetId.ToString();

        public Inventory Asset => inventory;

        public SelfInventoryDescription Description => description;
    }
}

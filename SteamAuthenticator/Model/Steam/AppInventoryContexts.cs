namespace Steam_Authenticator.Model.Steam
{
    public class AppInventoryContexts
    {
        private readonly SteamInventory inventory;

        public AppInventoryContexts(SteamInventory inventory)
        {
            this.inventory = inventory;
        }
    }
}

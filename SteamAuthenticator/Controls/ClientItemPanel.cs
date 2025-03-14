namespace Steam_Authenticator.Controls
{
    internal abstract class ClientItemPanel<TClient> : ItemPanel where TClient : Client
    {

        public ClientItemPanel(bool hasItem) : base(hasItem)
        {
        }

        public TClient Client { get; set; }

        public void SetChecked(bool isChecked)
        {
            if (!HasItem)
            {
                return;
            }

            BorderColor = isChecked ? Color.OrangeRed : Color.LightGray;
        }
    }
}

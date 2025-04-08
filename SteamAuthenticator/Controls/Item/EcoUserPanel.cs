namespace Steam_Authenticator.Controls
{
    internal class EcoUserPanel : ClientItemPanel<EcoClient>
    {
        public Label Offer;

        public EcoUserPanel(bool hasUser) : base(hasUser)
        {

        }

        public EcoUserPanel SetOfferBox(Label offer)
        {
            this.Offer = offer;
            this.Controls.Add(this.Offer);
            return this;
        }

        public EcoUserPanel SetOffer(int? offerCount)
        {
            if (!HasItem)
            {
                return this;
            }

            Offer.Text = offerCount.HasValue ? $"{offerCount}" : "---";
            return this;
        }
    }
}

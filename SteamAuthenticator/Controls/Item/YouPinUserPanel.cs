namespace Steam_Authenticator.Controls
{
    internal class YouPinUserPanel : ClientItemPanel<YouPinClient>
    {
        public Label Offer;

        public YouPinUserPanel(bool hasUser) : base(hasUser)
        {

        }

        public YouPinUserPanel SetOfferBox(Label offer)
        {
            this.Offer = offer;
            this.Controls.Add(this.Offer);
            return this;
        }

        public YouPinUserPanel SetOffer(int? offerCount)
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

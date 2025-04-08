namespace Steam_Authenticator.Controls
{
    internal class BuffUserPanel : ClientItemPanel<BuffClient>
    {
        public Label Offer;

        public BuffUserPanel(bool hasUser) : base(hasUser)
        {

        }

        public BuffUserPanel SetOfferBox(Label offer)
        {
            this.Offer = offer;
            this.Controls.Add(this.Offer);
            return this;
        }

        public BuffUserPanel SetOffer(int? offerCount)
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


namespace Steam_Authenticator.Controls
{
    internal class C5UserPanel : ClientItemPanel<C5Client>
    {
        public Label Offer;

        public C5UserPanel(bool hasUser) : base(hasUser)
        {

        }

        public C5UserPanel SetOfferBox(Label offer)
        {
            this.Offer = offer;
            this.Controls.Add(this.Offer);
            return this;
        }

        public C5UserPanel SetOffer(int? offerCount)
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

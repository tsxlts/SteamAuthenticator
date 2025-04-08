namespace Steam_Authenticator.Controls
{
    internal class SteamUserPanel : ClientItemPanel<UserClient>
    {
        public Label Offer;
        public Label Confirmation;
        public IconLabel Icons;

        public SteamUserPanel(bool hasUser) : base(hasUser)
        {

        }

        public SteamUserPanel SetOfferBox(Label offer)
        {
            this.Offer = offer;
            this.Controls.Add(this.Offer);
            return this;
        }

        public SteamUserPanel SetConfirmationBox(Label confirmation)
        {
            this.Confirmation = confirmation;
            this.Controls.Add(this.Confirmation);
            return this;
        }

        public SteamUserPanel SetIconsBox(IconLabel icons)
        {
            this.Icons = icons;
            this.Controls.Add(this.Icons);
            return this;
        }

        public SteamUserPanel SetOffer(int? offerCount)
        {
            if (!HasItem)
            {
                return this;
            }

            Offer.Text = offerCount.HasValue ? $"{offerCount}" : "---";
            return this;
        }

        public SteamUserPanel SetConfirmation(int? confirmationCount)
        {
            if (!HasItem)
            {
                return this;
            }

            Confirmation.Text = confirmationCount.HasValue ? $"{confirmationCount}" : "---";
            return this;
        }

        public SteamUserPanel RefreshIcon()
        {
            this.Icons?.Refresh();
            return this;
        }
    }
}

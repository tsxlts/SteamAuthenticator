namespace Steam_Authenticator.Controls
{
    internal class UserPanel : Panel
    {
        private readonly bool hasUser;

        private PictureBox useravatar;
        private Label username;
        private Label offer;
        private Label confirmation;
        private IconLabel icons;

        public UserPanel(bool hasUser)
        {
            this.hasUser = hasUser;
        }

        public UserClient UserClient { get; set; }

        public UserPanel SetUserAvatarBox(PictureBox useravatar)
        {
            this.useravatar = useravatar;
            this.Controls.Add(this.useravatar);
            return this;
        }

        public UserPanel SetUserNameBox(Label username)
        {
            this.username = username;
            this.Controls.Add(this.username);
            return this;
        }

        public UserPanel SetOfferBox(Label offer)
        {
            this.offer = offer;
            this.Controls.Add(this.offer);
            return this;
        }

        public UserPanel SetConfirmationBox(Label confirmation)
        {
            this.confirmation = confirmation;
            this.Controls.Add(this.confirmation);
            return this;
        }

        public UserPanel SetIconsBox(IconLabel icons)
        {
            this.icons = icons;
            this.Controls.Add(this.icons);
            return this;
        }

        public UserPanel SetUserAvatar(string avatar)
        {
            useravatar?.LoadAsync(avatar);
            return this;
        }

        public UserPanel SetUserName(string userName, Color color)
        {
            if (!hasUser)
            {
                return this;
            }

            username.Text = userName;
            username.ForeColor = color;
            return this;
        }

        public UserPanel SetOffer(int? offerCount)
        {
            if (!hasUser)
            {
                return this;
            }

            offer.Text = offerCount.HasValue ? $"{offerCount}" : "---";
            return this;
        }

        public UserPanel SetConfirmation(int? confirmationCount)
        {
            if (!hasUser)
            {
                return this;
            }

            confirmation.Text = confirmationCount.HasValue ? $"{confirmationCount}" : "---";
            return this;
        }

        public UserPanel RefreshIcon()
        {
            this.icons?.Refresh();
            return this;
        }

        public string UserName => hasUser ? username?.Text : null;
    }
}

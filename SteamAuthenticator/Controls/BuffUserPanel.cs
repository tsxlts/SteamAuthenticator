namespace Steam_Authenticator.Controls
{
    internal class BuffUserPanel : Panel
    {
        private readonly bool hasUser;

        private PictureBox useravatar;
        private Label username;
        private Label offer;

        public BuffUserPanel(bool hasUser)
        {
            this.hasUser = hasUser;
        }

        public BuffClient Client { get; set; }


        public BuffUserPanel SetUserAvatarBox(PictureBox useravatar)
        {
            this.useravatar = useravatar;
            this.Controls.Add(this.useravatar);
            return this;
        }

        public BuffUserPanel SetUserNameBox(Label username)
        {
            this.username = username;
            this.Controls.Add(this.username);
            return this;
        }

        public BuffUserPanel SetOfferBox(Label offer)
        {
            this.offer = offer;
            this.Controls.Add(this.offer);
            return this;
        }

        public BuffUserPanel SetUserAvatar(string avatar)
        {
            useravatar?.LoadAsync(avatar);
            return this;
        }

        public BuffUserPanel SetUserName(string userName, Color color)
        {
            if (!hasUser)
            {
                return this;
            }

            username.Text = userName;
            username.ForeColor = color;
            return this;
        }

        public BuffUserPanel SetOffer(int? offerCount)
        {
            if (!hasUser)
            {
                return this;
            }

            offer.Text = offerCount.HasValue ? $"{offerCount}" : "---";
            return this;
        }

        public string UserName => hasUser ? username?.Text : null;
    }
}

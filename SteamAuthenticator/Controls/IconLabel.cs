namespace Steam_Authenticator.Controls
{
    internal class IconLabel : Label
    {
        private readonly CustomIcon[] icons = new CustomIcon[0];

        public IconLabel()
        {
            Size = new Size(32, 32);
        }

        public IconLabel(params CustomIcon[] icons) : this()
        {
            this.icons = icons ?? new CustomIcon[0];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int x = 0;
            int y = this.Height - IconSize.Height;
            foreach (var icon in icons)
            {
                if (x + IconSize.Width + 3 > this.Width)
                {
                    x = 0;
                    y = y - IconSize.Height;
                }

                e.Graphics.DrawIcon(icon.Icon, new Rectangle(x, y, IconSize.Width, IconSize.Height));

                x = x + 3 + IconSize.Width;
            }
        }

        public Size IconSize { get; init; } = new Size(16, 16);

        public new bool AutoSize => false;
    }
}

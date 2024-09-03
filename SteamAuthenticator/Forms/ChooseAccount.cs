
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class ChooseAccount : Form
    {
        public ChooseAccount()
        {
            InitializeComponent();
        }

        public User User { get; private set; }

        private void ChooseAccount_Load(object sender, EventArgs e)
        {
            IEnumerable<string> accounts = Appsetting.Instance.Manifest.GetUsers();

            int index = 0;
            if (accounts.Any())
            {
                var uses = accounts.Reverse().Take(9);
                foreach (string account in uses)
                {
                    User user = Appsetting.Instance.Manifest.GetUser(account);

                    Panel panel = new Panel()
                    {
                        Size = new Size(80, 110),
                        Location = new Point(100 * (index % 5), 130 * (index / 5)),
                        Tag = user
                    };

                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = 80,
                        Height = 80,
                        Location = new Point(0, 0),
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };
                    string avatar = user.Avatar;
                    pictureBox.Image = Properties.Resources.userimg;
                    if (!string.IsNullOrEmpty(avatar))
                    {
                        pictureBox.LoadAsync(avatar);
                    }
                    panel.Controls.Add(pictureBox);

                    Label nameLabel = new Label()
                    {
                        Text = $"{user.NickName}",
                        AutoSize = false,
                        Size = new Size(80, 30),
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.FromArgb(128, 128, 128),
                        Location = new Point(0, 80)
                    };
                    panel.Controls.Add(nameLabel);

                    pictureBox.Cursor = Cursors.Hand;
                    nameLabel.Cursor = Cursors.Hand;
                    pictureBox.Click += btnUser_Click;
                    nameLabel.Click += btnUser_Click;

                    UsersPanel.Controls.Add(panel);

                    index++;
                }
            }

            {
                Panel panel = new Panel()
                {
                    Size = new Size(80, 110),
                    Location = new Point(100 * (index % 5), 130 * (index / 5)),
                    Tag = null
                };

                PictureBox pictureBox = new PictureBox() { Width = 80, Height = 80, Location = new Point(0, 0), SizeMode = PictureBoxSizeMode.Zoom };
                pictureBox.Image = Properties.Resources.add;
                panel.Controls.Add(pictureBox);

                Label nameLabel = new Label()
                {
                    Text = $"添加帐号",
                    AutoSize = false,
                    Size = new Size(80, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(244, 164, 96),
                    Location = new Point(0, 80)
                };
                panel.Controls.Add(nameLabel);

                pictureBox.Cursor = Cursors.Hand;
                nameLabel.Cursor = Cursors.Hand;
                pictureBox.Click += btnUser_Click;
                nameLabel.Click += btnUser_Click;

                UsersPanel.Controls.Add(panel);
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Panel panel = control.Parent as Panel;
            User user = panel?.Tag as User;
            User = user;
            DialogResult = DialogResult.OK;
        }
    }
}

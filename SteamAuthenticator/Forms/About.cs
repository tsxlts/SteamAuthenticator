using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Forms
{
    public partial class About : Form
    {
        private readonly Version currentVersion;

        public About(Version currentVersion)
        {
            InitializeComponent();

            this.currentVersion = currentVersion;
        }

        private void About_Load(object sender, EventArgs e)
        {
            projectBox.Text = ProjectInfo.Url;
            versionBox.Text = currentVersion.ToString();

            qqBox.Text = "2570096963";
            wechatBox.Text = "**********";
        }

        private void copyProjectBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(projectBox.Text);
        }

        private void copyQQBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(qqBox.Text);
        }

        private void copyWeChatBtn_Click(object sender, EventArgs e)
        {
            Utils.CopyText(wechatBox.Text);
        }
    }
}

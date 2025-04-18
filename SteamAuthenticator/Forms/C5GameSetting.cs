using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class C5GameSetting : Form
    {
        private readonly C5User user;

        public C5GameSetting(C5User user)
        {
            InitializeComponent();

            this.Text = $"C5GAME设置 {user.Nickname}";
            this.user = user;
        }

        private void C5GameSetting_Load(object sender, EventArgs e)
        {
            autoSendOffer.Checked = user.Setting.AutoSendOffer;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            user.Setting.AutoSendOffer = autoSendOffer.Checked;

            Appsetting.Instance.Manifest.SaveC5User(user.UserId, user);

            DialogResult = DialogResult.OK;
        }
    }
}

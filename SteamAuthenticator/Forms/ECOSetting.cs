using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class ECOSetting : Form
    {
        private readonly EcoUser user;

        public ECOSetting(EcoUser user)
        {
            InitializeComponent();

            this.Text = $"ECO设置 {user.Nickname}";
            this.user = user;
        }

        private void ECOSetting_Load(object sender, EventArgs e)
        {
            autoSendOffer.Checked = user.Setting.AutoSendOffer;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            user.Setting.AutoSendOffer = autoSendOffer.Checked;

            Appsetting.Instance.Manifest.SaveEcoUser(user.UserId, user);

            DialogResult = DialogResult.OK;
        }
    }
}

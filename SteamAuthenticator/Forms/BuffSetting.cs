using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class BuffSetting : Form
    {
        public BuffSetting(BuffUserSetting setting)
        {
            InitializeComponent();

            Setting = setting;
        }

        private void BuffSetting_Load(object sender, EventArgs e)
        {
            autoAcceptGiveOffer.Checked = Setting.AutoAcceptGiveOffer;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Setting.AutoAcceptGiveOffer = autoAcceptGiveOffer.Checked;

            DialogResult = DialogResult.OK;
        }

        public BuffUserSetting Setting { get; private set; }
    }
}

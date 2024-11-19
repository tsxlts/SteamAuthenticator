using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class UserSetting : Form
    {
        private readonly User user;
        private bool fullyLoaded = false;
        private bool autoAcceptGiveOffer_All_Click = false;

        public UserSetting(User user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void UserSetting_Load(object sender, EventArgs e)
        {
            periodicChecking.Checked = user.Setting.PeriodicCheckingConfirmation;
            autoConfirmTrade.Checked = user.Setting.AutoConfirmTrade;
            autoConfirmMarket.Checked = user.Setting.AutoConfirmMarket;
            autoAcceptReceiveOffer.Checked = user.Setting.AutoAcceptReceiveOffer;
            autoAcceptGiveOffer.Checked = user.Setting.AutoAcceptGiveOffer;
            autoAcceptGiveOffer_Buff.Checked = user.Setting.AutoAcceptGiveOffer_Buff;
            autoAcceptGiveOffer_Other.Checked = user.Setting.AutoAcceptGiveOffer_Other;

            SetControlsEnabledState(periodicChecking.Checked);

            fullyLoaded = true;
        }

        private void autoConfirmTrade_CheckedChanged(object sender, EventArgs e)
        {
            if (autoConfirmTrade.Checked)
            {
                ShowWarning(autoConfirmTrade);
            }
        }

        private void autoConfirmMarket_CheckedChanged(object sender, EventArgs e)
        {
            if (autoConfirmMarket.Checked)
            {
                ShowWarning(autoConfirmMarket);
            }
        }

        private void periodicChecking_CheckedChanged(object sender, EventArgs e)
        {
            SetControlsEnabledState(periodicChecking.Checked);
        }

        private void autoAcceptGiveOffer_All_CheckedChanged(object sender, EventArgs e)
        {
            autoAcceptGiveOffer_All_Click = true;

            autoAcceptGiveOffer_Buff.Checked = autoAcceptGiveOffer_Other.Checked = autoAcceptGiveOffer.Checked;

            autoAcceptGiveOffer_All_Click = false;
        }

        private void autoAcceptGiveOffer_CheckedChanged(object sender, EventArgs e)
        {
            if (autoAcceptGiveOffer_All_Click)
            {
                return;
            }

            CheckBox checkBox = sender as CheckBox;
            if (!checkBox.Checked)
            {
                autoAcceptGiveOffer.Checked = false;
                return;
            }

            autoAcceptGiveOffer.Checked = autoAcceptGiveOffer_Buff.Checked && autoAcceptGiveOffer_Other.Checked;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            user.Setting.PeriodicCheckingConfirmation = periodicChecking.Checked;
            user.Setting.AutoConfirmTrade = autoConfirmTrade.Checked;
            user.Setting.AutoConfirmMarket = autoConfirmMarket.Checked;
            user.Setting.AutoAcceptReceiveOffer = autoAcceptReceiveOffer.Checked;
            user.Setting.AutoAcceptGiveOffer = autoAcceptGiveOffer.Checked;
            user.Setting.AutoAcceptGiveOffer_Buff = autoAcceptGiveOffer_Buff.Checked;
            user.Setting.AutoAcceptGiveOffer_Other = autoAcceptGiveOffer_Other.Checked;

            Appsetting.Instance.Manifest.AddUser(user.SteamId, user);

            MessageBox.Show("已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }

        private void SetControlsEnabledState(bool enabled)
        {
            autoConfirmMarket.Enabled = autoConfirmTrade.Enabled
                = autoAcceptReceiveOffer.Enabled
                = autoAcceptGiveOffer.Enabled = autoAcceptGiveOffer_Buff.Enabled = autoAcceptGiveOffer_Other.Enabled
                = enabled;
        }

        private void ShowWarning(CheckBox affectedBox)
        {
            if (!fullyLoaded)
            {
                return;
            }

            var result = MessageBox.Show($"警告：" +
                $"{Environment.NewLine}" +
                $"启用此功能将严重降低您的物品的安全性！" +
                $"{Environment.NewLine}" +
                $"使用此选项的风险由您自行承担。" +
                $"{Environment.NewLine}" +
                $"您确定要继续吗？", "警告!", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                affectedBox.Checked = false;
            }
        }
    }
}

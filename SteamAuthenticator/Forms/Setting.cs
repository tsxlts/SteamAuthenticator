

namespace Steam_Authenticator.Forms
{
    public partial class Setting : Form
    {
        private bool fullyLoaded = false;

        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            periodicChecking.Checked = Appsetting.Instance.AppSetting.Entry.PeriodicCheckingConfirmation;
            autoRefreshInternal.Value = Appsetting.Instance.AppSetting.Entry.AutoRefreshInternal;
            checkAll.Checked = Appsetting.Instance.AppSetting.Entry.CheckAllConfirmation;
            confirmationAutoPopup.Checked = Appsetting.Instance.AppSetting.Entry.ConfirmationAutoPopup;
            autoConfirmTrade.Checked = Appsetting.Instance.AppSetting.Entry.AutoConfirmTrade;
            autoConfirmMarket.Checked = Appsetting.Instance.AppSetting.Entry.AutoConfirmMarket;
            autoAcceptReceiveOffer.Checked = Appsetting.Instance.AppSetting.Entry.AutoAcceptReceiveOffer;
            autoAcceptGiveOffer.Checked = Appsetting.Instance.AppSetting.Entry.AutoAcceptGiveOffer;

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

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Appsetting.Instance.AppSetting.Entry.PeriodicCheckingConfirmation = periodicChecking.Checked;
            Appsetting.Instance.AppSetting.Entry.AutoRefreshInternal = (int)autoRefreshInternal.Value;
            Appsetting.Instance.AppSetting.Entry.CheckAllConfirmation = checkAll.Checked;
            Appsetting.Instance.AppSetting.Entry.ConfirmationAutoPopup = confirmationAutoPopup.Checked;
            Appsetting.Instance.AppSetting.Entry.AutoConfirmTrade = autoConfirmTrade.Checked;
            Appsetting.Instance.AppSetting.Entry.AutoConfirmMarket = autoConfirmMarket.Checked;
            Appsetting.Instance.AppSetting.Entry.AutoAcceptReceiveOffer = autoAcceptReceiveOffer.Checked;
            Appsetting.Instance.AppSetting.Entry.AutoAcceptGiveOffer = autoAcceptGiveOffer.Checked;

            Appsetting.Instance.AppSetting.Save();

            Close();
        }

        private void SetControlsEnabledState(bool enabled)
        {
            checkAll.Enabled = confirmationAutoPopup.Enabled = autoConfirmMarket.Enabled = autoConfirmTrade.Enabled = autoAcceptReceiveOffer.Enabled = autoAcceptGiveOffer.Enabled = enabled;
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

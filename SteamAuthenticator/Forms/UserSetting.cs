using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class UserSetting : Form
    {
        private readonly User user;
        private bool fullyLoaded = false;

        public readonly List<CheckBox> autoAcceptGiveOfferBoxs = new List<CheckBox>();
        public readonly List<CheckBox> autoConfirmOfferBoxs = new List<CheckBox>();

        public UserSetting(User user)
        {
            InitializeComponent();

            this.Text = $"用户设置 {user.Account}";
            this.user = user;

            autoAcceptGiveOfferBoxs = new List<CheckBox>
            {
                autoAcceptGiveOffer,
                autoAcceptGiveOffer_Buff,
                autoAcceptGiveOffer_Eco,
                autoAcceptGiveOffer_YouPin,
                autoAcceptGiveOffer_C5,
                autoAcceptGiveOffer_Other,
                autoAcceptGiveOffer_Custom
            };
            autoConfirmOfferBoxs = new List<CheckBox>
            {
                autoConfirmTrade,
                autoConfirmTrade_Buff,
                autoConfirmTrade_Eco,
                autoConfirmTrade_YouPin,
                autoConfirmTrade_C5,
                autoConfirmTrade_Other,
                autoConfirmTrade_Custom
            };
        }

        private void UserSetting_Load(object sender, EventArgs e)
        {
            periodicChecking.Checked = user.Setting.PeriodicCheckingConfirmation;

            autoConfirmMarket.Checked = user.Setting.AutoConfirmMarket;

            autoAcceptGiveOffer.Checked = user.Setting.AutoAcceptGiveOffer;
            autoConfirmTrade.Checked = user.Setting.AutoConfirmTrade;

            autoAcceptGiveOffer_Buff.Checked = user.Setting.AutoAcceptGiveOffer_Buff;
            autoConfirmTrade_Buff.Checked = user.Setting.AutoConfirmTrade_Buff;

            autoAcceptGiveOffer_Eco.Checked = user.Setting.AutoAcceptGiveOffer_Eco;
            autoConfirmTrade_Eco.Checked = user.Setting.AutoConfirmTrade_Eco;

            autoAcceptGiveOffer_YouPin.Checked = user.Setting.AutoAcceptGiveOffer_YouPin;
            autoConfirmTrade_YouPin.Checked = user.Setting.AutoConfirmTrade_YouPin;

            autoAcceptGiveOffer_C5.Checked = user.Setting.AutoAcceptGiveOffer_C5;
            autoConfirmTrade_C5.Checked = user.Setting.AutoConfirmTrade_C5;

            autoAcceptGiveOffer_Other.Checked = user.Setting.AutoAcceptGiveOffer_Other;
            autoConfirmTrade_Other.Checked = user.Setting.AutoConfirmTrade_Other;

            autoAcceptGiveOffer_Custom.Checked = user.Setting.AutoAcceptGiveOffer_Custom;
            autoConfirmTrade_Custom.Checked = user.Setting.AutoConfirmTrade_Custom;

            autoAcceptReceiveOffer.Checked = user.Setting.AutoAcceptReceiveOffer;

            SetControlsEnabledState(periodicChecking.Checked);

            fullyLoaded = true;
        }

        private void autoConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is CheckBox checkBox) || !checkBox.Checked)
            {
                return;
            }

            ShowWarning(checkBox);
        }

        private void periodicChecking_CheckedChanged(object sender, EventArgs e)
        {
            SetControlsEnabledState(periodicChecking.Checked);
        }

        private void autoAcceptGiveOffer_All_CheckedChanged(object sender, EventArgs e)
        {
            if (!fullyLoaded)
            {
                return;
            }

            foreach (var item in autoAcceptGiveOfferBoxs)
            {
                if (item == autoAcceptGiveOffer)
                {
                    continue;
                }

                item.Enabled = !autoAcceptGiveOffer.Checked;
            }
        }

        private void autoConfirmTrade_CheckedChanged(object sender, EventArgs e)
        {
            if (!fullyLoaded)
            {
                return;
            }

            foreach (var item in autoConfirmOfferBoxs)
            {
                if (item == autoConfirmTrade)
                {
                    continue;
                }

                item.Enabled = !autoConfirmTrade.Checked;
            }
        }

        private void setAcceptGiveOfferRoleBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AcceptOfferRule acceptOfferRule = new AcceptOfferRule(user);
            acceptOfferRule.ShowDialog();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            user.Setting.PeriodicCheckingConfirmation = periodicChecking.Checked;
            user.Setting.AutoConfirmMarket = autoConfirmMarket.Checked;

            user.Setting.AutoAcceptGiveOffer = autoAcceptGiveOffer.Checked;
            user.Setting.AutoConfirmTrade = autoConfirmTrade.Checked;

            user.Setting.AutoAcceptGiveOffer_Buff = autoAcceptGiveOffer_Buff.Checked;
            user.Setting.AutoConfirmTrade_Buff = autoConfirmTrade_Buff.Checked;

            user.Setting.AutoAcceptGiveOffer_Eco = autoAcceptGiveOffer_Eco.Checked;
            user.Setting.AutoConfirmTrade_Eco = autoConfirmTrade_Eco.Checked;

            user.Setting.AutoAcceptGiveOffer_YouPin = autoAcceptGiveOffer_YouPin.Checked;
            user.Setting.AutoConfirmTrade_YouPin = autoConfirmTrade_YouPin.Checked;

            user.Setting.AutoAcceptGiveOffer_C5 = autoAcceptGiveOffer_C5.Checked;
            user.Setting.AutoConfirmTrade_C5 = autoConfirmTrade_C5.Checked;

            user.Setting.AutoAcceptGiveOffer_Other = autoAcceptGiveOffer_Other.Checked;
            user.Setting.AutoConfirmTrade_Other = autoConfirmTrade_Other.Checked;

            user.Setting.AutoAcceptGiveOffer_Custom = autoAcceptGiveOffer_Custom.Checked;
            user.Setting.AutoConfirmTrade_Custom = autoConfirmTrade_Custom.Checked;

            user.Setting.AutoAcceptReceiveOffer = autoAcceptReceiveOffer.Checked;

            Appsetting.Instance.Manifest.SaveSteamUser(user.SteamId, user);

            DialogResult = DialogResult.OK;
        }

        private void SetControlsEnabledState(bool enabled)
        {
            autoConfirmMarket.Enabled = autoAcceptReceiveOffer.Enabled = enabled;
            autoAcceptGiveOfferBoxs.ForEach(x => x.Enabled = enabled);
            autoConfirmOfferBoxs.ForEach(x => x.Enabled = enabled);

            if (!enabled)
            {
                return;
            }

            if (autoAcceptGiveOffer.Checked)
            {
                foreach (var item in autoAcceptGiveOfferBoxs)
                {
                    if (item == autoAcceptGiveOffer)
                    {
                        continue;
                    }

                    item.Enabled = !autoAcceptGiveOffer.Checked;
                }
            }
            if (autoConfirmTrade.Checked)
            {
                foreach (var item in autoConfirmOfferBoxs)
                {
                    if (item == autoConfirmTrade)
                    {
                        continue;
                    }

                    item.Enabled = !autoConfirmTrade.Checked;
                }
            }
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

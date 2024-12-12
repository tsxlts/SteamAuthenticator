using Steam_Authenticator.Model;
using System.Data;

namespace Steam_Authenticator.Forms
{
    public partial class AcceptOfferRule : Form
    {
        private readonly User user;

        public AcceptOfferRule(User user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void AcceptOfferRule_Load(object sender, EventArgs e)
        {
            offerMessage.Checked = user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Enabled;
            offerMessageBox.Text = user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Value;
            switch (user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition)
            {
                case AcceptOfferRuleSetting.ConditionType.等于:
                    offerMessageEquals.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.不等于:
                    offerMessageNotEquals.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.包含:
                    offerMessageContains.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.不包含:
                    offerMessageNotContains.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.正则匹配:
                    offerMessageRegex.Checked = true;
                    break;
            }

            assetName.Checked = user.Setting.AutoAcceptGiveOfferRule.AssetName.Enabled;
            assetNameBox.Text = user.Setting.AutoAcceptGiveOfferRule.AssetName.Value;
            switch (user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition)
            {
                case AcceptOfferRuleSetting.ConditionType.等于:
                    assetNameEquals.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.不等于:
                    assetNameNotEquals.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.包含:
                    assetNameContains.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.不包含:
                    assetNameNotContains.Checked = true;
                    break;
                case AcceptOfferRuleSetting.ConditionType.正则匹配:
                    assetNameRegex.Checked = true;
                    break;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (offerMessage.Checked)
            {
                var vaules = offerMessageBox.Text.Split(Environment.NewLine).Select(c => c.Trim()).Where(c => !string.IsNullOrWhiteSpace(c));
                if (!vaules.Any())
                {
                    MessageBox.Show("选中交易附言后, 交易附言匹配内容不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    offerMessageBox.Focus();
                    return;
                }
            }
            if (assetName.Checked)
            {
                var vaules = assetNameBox.Text.Split(Environment.NewLine).Select(c => c.Trim()).Where(c => !string.IsNullOrWhiteSpace(c));
                if (!vaules.Any())
                {
                    MessageBox.Show("选中物品名称后, 物品名称匹配内容不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    assetNameBox.Focus();
                    return;
                }
            }

            user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Enabled = offerMessage.Checked;
            user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Value = offerMessageBox.Text;
            if (offerMessageEquals.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition = AcceptOfferRuleSetting.ConditionType.等于;
            }
            if (offerMessageNotEquals.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition = AcceptOfferRuleSetting.ConditionType.不等于;
            }
            if (offerMessageContains.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition = AcceptOfferRuleSetting.ConditionType.包含;
            }
            if (offerMessageNotContains.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition = AcceptOfferRuleSetting.ConditionType.不包含;
            }
            if (offerMessageRegex.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.OfferMessage.Condition = AcceptOfferRuleSetting.ConditionType.正则匹配;
            }

            user.Setting.AutoAcceptGiveOfferRule.AssetName.Enabled = assetName.Checked;
            user.Setting.AutoAcceptGiveOfferRule.AssetName.Value = assetNameBox.Text;
            if (assetNameEquals.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition = AcceptOfferRuleSetting.ConditionType.等于;
            }
            if (assetNameNotEquals.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition = AcceptOfferRuleSetting.ConditionType.不等于;
            }
            if (assetNameContains.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition = AcceptOfferRuleSetting.ConditionType.包含;
            }
            if (assetNameNotContains.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition = AcceptOfferRuleSetting.ConditionType.不包含;
            }
            if (assetNameRegex.Checked)
            {
                user.Setting.AutoAcceptGiveOfferRule.AssetName.Condition = AcceptOfferRuleSetting.ConditionType.正则匹配;
            }

            Appsetting.Instance.Manifest.SaveSteamUser(user.SteamId, user);

            DialogResult = DialogResult.OK;
        }
    }
}

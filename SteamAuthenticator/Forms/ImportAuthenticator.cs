﻿namespace Steam_Authenticator.Forms
{
    public partial class ImportAuthenticator : Form
    {

        public ImportAuthenticator(UserClient client)
        {
            InitializeComponent();

            AccountNameBox.Enabled = false;
            AccountNameBox.Text = client.GetAccount();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SharedSecret) && string.IsNullOrWhiteSpace(IdentitySecret))
            {
                MessageBox.Show("登录秘钥和身份秘钥至少有一项不能为空");

                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelBtm_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string AccountName => AccountNameBox.Text;
        public string RevocationCode => RevocationCodeBox.Text;
        public string SharedSecret => SharedSecretBox.Text;
        public string IdentitySecret => IdentitySecretBox.Text;
    }
}

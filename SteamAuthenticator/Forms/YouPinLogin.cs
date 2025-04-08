using Steam_Authenticator.Internal;
using Steam_Authenticator.Model.YouPin898;

namespace Steam_Authenticator.Forms
{
    public partial class YouPinLogin : Form
    {
        private readonly string SessionId;

        public YouPinLogin(string tips)
        {
            InitializeComponent();

            msg.Text = tips;

            SessionId = Guid.NewGuid().ToString();
        }

        private async void sendSmsCodeBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                sendSmsCodeBtn.Enabled = false;

                string area = "86";
                string phone = phoneBox.Text;
                if (string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show($"请输入手机号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sendSmsCode = await YouPin898Api.SendSmsCode(SessionId, area, phone, default);
                if (sendSmsCode.Body == null)
                {
                    MessageBox.Show($"发送验证码失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sendSmsCodeBody = sendSmsCode.Body;

                if (sendSmsCodeBody.IsSuccess())
                {
                    return;
                }

                if (sendSmsCodeBody.GetCode() == 5050)
                {
                    var smsConfig = await YouPin898Api.GetSmsUpSignInConfig(default);
                    if (smsConfig.Body == null)
                    {
                        MessageBox.Show($"发送验证码失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var smsConfigBody = smsConfig.Body;
                    if (!smsConfigBody.IsSuccess())
                    {
                        MessageBox.Show($"发送验证码失败" +
                            $"{Environment.NewLine}" +
                            $"{smsConfigBody.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    var smsConfigData = smsConfigBody.GetData();

                    MessageBox.Show($"该手机号需要手动发送短信进行验证" +
                           $"{Environment.NewLine}" +
                           $"请编辑发送短信 {smsConfigData.SmsUpContent} 到号码 {smsConfigData.SmsUpNumber} 完成登录" +
                           $"{Environment.NewLine}" +
                           $"发送完成后直接点击 登录 即可", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show($"发送验证码失败" +
                    $"{Environment.NewLine}" +
                    $"{sendSmsCodeBody.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送验证码失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sendSmsCodeBtn.Enabled = true;
            }
        }

        private async void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                okBtn.Enabled = false;

                string area = "86";
                string phone = phoneBox.Text;
                string code = codeBox.Text;
                if (string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show($"请输入手机号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var smsLogin = await YouPin898Api.SmsCodeLogin(SessionId, area, phone, code, default);
                if (smsLogin.Body == null)
                {
                    MessageBox.Show($"登录失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var smsLoginBody = smsLogin.Body;
                var smsLoginData = smsLoginBody.GetData();
                if (!smsLoginBody.IsSuccess() || string.IsNullOrWhiteSpace(smsLoginData.Token))
                {
                    MessageBox.Show($"登录失败" +
                        $"{Environment.NewLine}" +
                        $"{smsLoginBody.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Token = smsLoginData.Token;

                var userInfo = await YouPin898Api.GetUserInfo(Token, default);
                var userData = userInfo.Body.GetData();
                if (userData == null)
                {
                    MessageBox.Show($"获取用户信息失败" +
                        $"{Environment.NewLine}" +
                        $"{userInfo.Body?.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Result = userData;

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登录失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                okBtn.Enabled = true;
            }
        }

        public GetUserInfoResponse Result { get; private set; }

        public string Token { get; private set; }
    }
}

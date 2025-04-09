using Steam_Authenticator.Internal;
using Steam_Authenticator.Model.YouPin898;

namespace Steam_Authenticator.Forms
{
    public partial class YouPinLogin : Form
    {
        private readonly string SessionId;
        private PcSendSmsCodeResponse pcSendSmsCodeResponse;

        public YouPinLogin(string tips)
        {
            InitializeComponent();

            msg.Text = tips;
            appLogin.Checked = true;

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

                if (pcLogin.Checked)
                {
                    var pcSendSmsCode = await YouPin898Api.PcSendSmsCode(SessionId, area, phone, default);
                    if (pcSendSmsCode.Body == null)
                    {
                        MessageBox.Show($"发送验证码失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var pcSendSmsCodeBody = pcSendSmsCode.Body;
                    if (pcSendSmsCodeBody.IsSuccess())
                    {
                        pcSendSmsCodeResponse = pcSendSmsCodeBody.GetData();

                        MessageBox.Show($"验证码发送成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    MessageBox.Show($"发送验证码失败" +
                        $"{Environment.NewLine}" +
                        $"{pcSendSmsCodeBody.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"验证码发送成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                           $"请使用手机号 {phone} " +
                           $"{Environment.NewLine}编辑发送短信 {smsConfigData.SmsUpContent} " +
                           $"{Environment.NewLine}到号码 {smsConfigData.SmsUpNumber} 完成登录" +
                           $"{Environment.NewLine}" +
                           $"发送完成后直接点击 登录 即可", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                string token;
                if (pcLogin.Checked)
                {
                    var pcSmsLogin = await YouPin898Api.PcSmsCodeLogin(SessionId, area, phone, code, pcSendSmsCodeResponse?.loginReqTicket, default);
                    if (pcSmsLogin.Body == null)
                    {
                        MessageBox.Show($"登录失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var pcSmsLoginBody = pcSmsLogin.Body;
                    var pcSmsLoginData = pcSmsLoginBody.GetData();
                    if (!pcSmsLoginBody.IsSuccess() || string.IsNullOrWhiteSpace(pcSmsLoginData.token))
                    {
                        MessageBox.Show($"登录失败" +
                            $"{Environment.NewLine}" +
                            $"{pcSmsLoginBody.GetMsg()}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    token = pcSmsLoginData.token;
                    goto LoginSuccess;
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
                    return;
                }
                token = smsLoginData.Token;

            LoginSuccess:
                Token = token;

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

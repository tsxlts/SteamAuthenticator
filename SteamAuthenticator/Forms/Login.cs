
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;

namespace Steam_Authenticator.Forms
{
    public partial class Login : Form
    {
        public Login(string account)
        {
            InitializeComponent();

            User.Text = account ?? "";
        }

        private async void loginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                loginBtn.Enabled = cancelBtn.Enabled = false;

                string user = User.Text;
                string password = Password.Text;

                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show($"用户名和密码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var steamWebClient = new SteamCommunityClient();
                var login = await steamWebClient.BeginLoginAsync(user, password, null, platformType: AuthTokenPlatformType.MobileApp);
                if (string.IsNullOrWhiteSpace(login?.SteamId) || string.IsNullOrWhiteSpace(login?.ClientId) || string.IsNullOrWhiteSpace(login?.RequestId))
                {
                    MessageBox.Show($"登录失败，请检查用户名和密码是否正确");
                    return;
                }

                Input input = null;
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.Token.Register(() => { input?.Close(); });

                var task = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            bool loginSuccess = steamWebClient.PollLoginStatusAsync(login.SteamId!, login.ClientId!, login.RequestId!, default).GetAwaiter().GetResult();
                            if (loginSuccess)
                            {
                                cts.Cancel();
                                Client = steamWebClient;
                                DialogResult = DialogResult.OK;
                                return true;
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                            Thread.Sleep(500);
                        }
                    }

                    return false;
                });

                var guard = Appsetting.Instance.Manifest.GetGuard(user);
                int index = 5;
                while (index > 0 && !cts.IsCancellationRequested)
                {
                    index--;

                    try
                    {
                        if (login.AllowedConfirmations!.Any(c => c.ConfirmationType == AuthConfirmationType.DeviceCode))
                        {
                            string code = null;
                            if (!string.IsNullOrWhiteSpace(guard?.SharedSecret))
                            {
                                code = GuardCodeGenerator.GenerateSteamGuardCode(Extension.GetSteamTimestampAsync().Result, guard.SharedSecret);
                            }
                            else
                            {
                                input = new Input("确认登录",
                                    $"请进行手机令牌确认登录" +
                                    $"{Environment.NewLine}" +
                                    $"或者输入手机令牌", required: true, errorMsg: "请输入手机令牌");
                                if (input.ShowDialog() != DialogResult.OK)
                                {
                                    return;
                                }

                                code = input.InputValue;
                            }

                            await steamWebClient.ConfirmLoginWithGuardCodeAsync(login.SteamId, login.ClientId, AuthConfirmationType.DeviceCode, code, default);
                            continue;
                        }

                        if (login.AllowedConfirmations!.Any(c => c.ConfirmationType == AuthConfirmationType.EmailCode))
                        {
                            input = new Input("确认登录", "请输入邮箱令牌", required: true, errorMsg: "请输入邮箱令牌");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            string code = input.InputValue;
                            await steamWebClient.ConfirmLoginWithGuardCodeAsync(login.SteamId, login.ClientId, AuthConfirmationType.EmailCode, code, default);
                            continue;
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        await Task.Delay(2000);
                    }
                }
                cts.Cancel();

                var success = await task;
                if (!success)
                {
                    MessageBox.Show($"登录失败,请验证令牌信息失败正确");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loginBtn.Enabled = cancelBtn.Enabled = true;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private async void qrAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                loginBtn.Enabled = cancelBtn.Enabled = false;

                QrAuth qrAuth = new QrAuth();
                qrAuth.ShowDialog();
                if (string.IsNullOrWhiteSpace(qrAuth.RefreshToken))
                {
                    return;
                }

                var steamWebClient = new SteamCommunityClient();
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool success = await steamWebClient.LoginAsync(qrAuth.RefreshToken, cts.Token);
                    if (!success)
                    {
                        MessageBox.Show("登录失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                Client = steamWebClient;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loginBtn.Enabled = cancelBtn.Enabled = true;
            }
        }

        public SteamCommunityClient Client { get; private set; }
    }
}

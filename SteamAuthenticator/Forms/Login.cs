using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;

namespace Steam_Authenticator.Forms
{
    public partial class Login : Form
    {
        private bool closed = false;

        public Login(string account)
        {
            InitializeComponent();

            User.Text = account ?? "";
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
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

                string error = "";
                var guard = Appsetting.Instance.Manifest.GetGuard(user);
                int errorTimes = 0;
                int waitTime = 0;
                while (!cts.IsCancellationRequested && !closed)
                {
                    try
                    {
                        if (errorTimes >= 5)
                        {
                            cts.Cancel();
                            break;
                        }

                        waitTime = 1200;

                        if (login.AllowedConfirmations!.Any(c => c.ConfirmationType == AuthConfirmationType.DeviceCode))
                        {
                            string code = null;
                            if (!string.IsNullOrWhiteSpace(guard?.SharedSecret))
                            {
                                error = $"请确认本设备上的令牌信息是否有效," +
                                    $"{Environment.NewLine}" +
                                    $"如果你已经转移了你的令牌, 请在令牌页面删除本设备上的令牌";

                                code = GuardCodeGenerator.GenerateSteamGuardCode(Extension.GetSteamTimestampAsync().Result, guard.SharedSecret);

                                if (errorTimes > 3)
                                {
                                    input = new Input("确认登录",
                                    $"你的令牌似乎已经失效了" +
                                    $"{Environment.NewLine}" +
                                    $"如果你已经转移了你的令牌, 请在新的设备上确认登录" +
                                    $"{Environment.NewLine}" +
                                    $"或者输入手机令牌", required: true, errorMsg: "请输入手机令牌");

                                    if (input.ShowDialog() == DialogResult.OK)
                                    {
                                        code = input.InputValue;
                                    }
                                }
                                ;
                            }
                            else
                            {
                                input = new Input("确认登录",
                                    $"请进行手机令牌确认登录" +
                                    $"{Environment.NewLine}" +
                                    $"或者输入手机令牌", required: true, errorMsg: "请输入手机令牌");
                                if (input.ShowDialog() != DialogResult.OK)
                                {
                                    waitTime = 0;
                                    break;
                                }

                                error = "请确认你输入的手机令牌是否有效";

                                code = input.InputValue;
                            }

                            bool checkCode = await steamWebClient.ConfirmLoginWithGuardCodeAsync(login.SteamId, login.ClientId, AuthConfirmationType.DeviceCode, code, default);
                            if (checkCode)
                            {
                                waitTime = 0;
                                break;
                            }

                            errorTimes++;
                            continue;
                        }

                        if (login.AllowedConfirmations!.Any(c => c.ConfirmationType == AuthConfirmationType.EmailCode))
                        {
                            input = new Input("确认登录", "请输入邮箱令牌", required: true, errorMsg: "请输入邮箱令牌");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                waitTime = 0;
                                break;
                            }

                            error = "请确认你输入的邮箱令牌是否有效";

                            string code = input.InputValue;
                            bool checkCode = await steamWebClient.ConfirmLoginWithGuardCodeAsync(login.SteamId, login.ClientId, AuthConfirmationType.EmailCode, code, default);
                            if (checkCode)
                            {
                                waitTime = 0;
                                break;
                            }

                            errorTimes++;
                            continue;
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        await Task.Delay(waitTime);
                    }
                }

                var success = await task;
                if (!success && !closed)
                {
                    MessageBox.Show($"登录失败{Environment.NewLine}{error}", "登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"{ex.Message}" +
                    $"{Environment.NewLine}" +
                    $"请确保你已经开启加速器, 并将加速器设置为“路由模式”", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (closed)
                {
                    return;
                }

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
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"{ex.Message}" +
                    $"{Environment.NewLine}" +
                    $"请确保你已经开启加速器, 并将加速器设置为“路由模式”", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (closed)
                {
                    return;
                }

                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loginBtn.Enabled = cancelBtn.Enabled = true;
            }
        }

        private async void tokenAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                loginBtn.Enabled = cancelBtn.Enabled = false;

                RichTextInput richTextInput = new RichTextInput("登录", "请输入RefreshToken或者AccessToken", required: true, "请输入RefreshToken或者AccessToken");
                richTextInput.ShowDialog();

                string token = richTextInput.InputValue;
                if (string.IsNullOrWhiteSpace(token))
                {
                    return;
                }
                token = Uri.UnescapeDataString(token);

                var steamWebClient = new SteamCommunityClient();
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool success = await steamWebClient.LoginAsync(token, cts.Token);
                    if (!success)
                    {
                        MessageBox.Show("登录失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                Client = steamWebClient;
                DialogResult = DialogResult.OK;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"{ex.Message}" +
                    $"{Environment.NewLine}" +
                    $"请确保你已经开启加速器, 并将加速器设置为“路由模式”", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (closed)
                {
                    return;
                }

                MessageBox.Show($"登录失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loginBtn.Enabled = cancelBtn.Enabled = true;
            }
        }

        public SteamCommunityClient Client { get; private set; }

    }
}

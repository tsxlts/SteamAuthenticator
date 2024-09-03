
using Newtonsoft.Json;
using SteamKit;
using SteamKit.Model;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Model;
using System.Text;
using System.Web;
using static SteamKit.SteamEnum;
using Steam_Authenticator.Internal;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly System.Threading.Timer timer;
        private readonly SemaphoreSlim loginConfirmLocker = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim confirmationPopupLocker = new SemaphoreSlim(1, 1);
        private readonly TimeSpan timerInterval = TimeSpan.FromSeconds(1);
        private readonly TimeSpan timerPeriod = TimeSpan.FromSeconds(20);

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            timer = new System.Threading.Timer(RefreshClientMsg, null, -1, -1);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            IEnumerable<string> accounts = Appsetting.Instance.Manifest.GetUsers();
            if (accounts.Any())
            {
                User user = Appsetting.Instance.Manifest.GetUser(accounts.Last());
                if (!string.IsNullOrWhiteSpace(user.RefreshToken))
                {
                    await Login(user.RefreshToken);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Dispose();
            Appsetting.Instance.CurrentClient.Dispose();
        }

        private async void loginMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                loginMenuItem.Enabled = false;

                ChooseAccount chooseAccount = new ChooseAccount();
                if (chooseAccount.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var user = chooseAccount.User;
                if (user != null)
                {
                    await Login(user.RefreshToken);
                    return;
                }

                Login confirmation = new Login();
                if (confirmation.ShowDialog() == DialogResult.OK)
                {
                    await ResetUser();
                }
            }
            catch
            {

            }
            finally
            {
                loginMenuItem.Enabled = true;
            }
        }

        private void settingMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }

        private void proxySettingMenuItem_Click(object sender, EventArgs e)
        {
            ProxySetting proxySetting = new ProxySetting();
            proxySetting.ShowDialog();
        }

        private void copyCookieMenuItem_Click(object sender, EventArgs e)
        {
            if (!Appsetting.Instance.CurrentClient.LoggedIn)
            {
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Appsetting.Instance.CurrentClient.WebCookie)
            {
                stringBuilder.Append($"{item.Name}={HttpUtility.UrlEncode(item.Value)}; ");
            }
            Clipboard.SetText(stringBuilder.ToString());
        }

        private void copyRefreshTokenMenuItem_Click(object sender, EventArgs e)
        {
            if (!Appsetting.Instance.CurrentClient.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(Appsetting.Instance.CurrentClient.RefreshToken);
        }

        private void copyAccessTokenMenuItem_Click(object sender, EventArgs e)
        {
            if (!Appsetting.Instance.CurrentClient.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(Appsetting.Instance.CurrentClient.AccessToken);
        }

        private void passwordMenuItem_Click(object sender, EventArgs e)
        {
            string olaPassword = "";
            if (Appsetting.Instance.Manifest.Encrypted)
            {
                string passwordTips = "请输入原密码";
                Input passwordInput;
                while (true)
                {
                    passwordInput = new Input("设置密码", passwordTips, true);
                    if (passwordInput.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    olaPassword = passwordInput.InputValue;
                    if (!Appsetting.Instance.Manifest.CheckPassword(olaPassword))
                    {
                        passwordTips = "密码错误，请重新输入";
                        continue;
                    }

                    break;
                }
            }

            Input input = new Input("设置密码", "请输入新的访问密码", true);
            if (input.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string newPassword = input.InputValue;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                while (true)
                {
                    input = new Input("设置密码", "请再次确认新的访问密码", true);
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    if (newPassword != input.InputValue)
                    {
                        MessageBox.Show("两次密码不一致", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    break;
                }
            }

            if (!Appsetting.Instance.Manifest.ChangePassword(olaPassword, newPassword))
            {
                MessageBox.Show("设置密码失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Appsetting.Instance.AppSetting.Password = newPassword;
            MessageBox.Show(Appsetting.Instance.Manifest.Encrypted ? "密码设置成功" : "已移除访问密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guardMenuItem_Click(object sender, EventArgs e)
        {
            StreamGuard streamGuard = new StreamGuard(Appsetting.Instance.CurrentClient.Account);
            streamGuard.Show();
        }

        private async void addAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            var webClient = Appsetting.Instance.CurrentClient;
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme == SteamGuardScheme.Device)
            {
                MessageBox.Show($"当前帐号已绑定令牌" +
                    $"{Environment.NewLine}" +
                    $"如果你已在其他地方绑定令牌，你可以选择移动令牌令牌验证器到当前设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            while (true)
            {
                var addAuthenticator = await SteamAuthenticator.AddAuthenticatorAsync(webClient.WebApiToken, webClient.SteamId, Extension.GetDeviceId(webClient.SteamId));
                if (addAuthenticator.ResultCode != SteamKit.Model.ErrorCodes.OK)
                {
                    MessageBox.Show($"绑定令牌失败[{addAuthenticator.ResultCode}]", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var addAuthenticatorResponse = addAuthenticator.Body;
                if (addAuthenticator == null)
                {
                    MessageBox.Show($"绑定令牌失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Guard guard = null;

                switch (addAuthenticatorResponse.Status)
                {
                    case AddAuthenticatorStatus.MustProvidePhoneNumber:
                        {
                            PhoneInput input = new PhoneInput("添加手机号", $"请输入手机号" +
                                $"{Environment.NewLine}" +
                                $"例如:+86 13100000000");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            string phone = input.Phone;
                            string country = input.Country;
                            var setAccountPhone = await SteamApi.SetAccountPhoneNumberAsync(webClient.WebApiToken, phone, country);
                            if (string.IsNullOrWhiteSpace(setAccountPhone.Body?.ConfirmationEmailAddress))
                            {
                                return;
                            }

                            while (true)
                            {
                                if (MessageBox.Show($"请进行邮箱({setAccountPhone.Body?.ConfirmationEmailAddress})确认",
                                    "提示",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                                {
                                    return;
                                }

                                var waitingForEmailConfirmation = await SteamApi.IsAccountWaitingForEmailConfirmationAsync(webClient.WebApiToken);
                                if (!waitingForEmailConfirmation.Body.AwaitingEmailConfirmation)
                                {
                                    break;
                                }

                                await Task.Delay(1000);
                            }
                        }
                        break;

                    case AddAuthenticatorStatus.AwaitingFinalization:
                        {
                            authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                            authenticatorStatusResponse = authenticatorStatus.Body;

                            guard = new Guard
                            {
                                AccountName = addAuthenticatorResponse.AccountName,
                                IdentitySecret = addAuthenticatorResponse.IdentitySecret,
                                SharedSecret = addAuthenticatorResponse.SharedSecret,
                                RevocationCode = addAuthenticatorResponse.RevocationCode,
                                Secret1 = addAuthenticatorResponse.Secret1,
                                SerialNumber = addAuthenticatorResponse.SerialNumber,
                                TokenGID = addAuthenticatorResponse.TokenGID,
                                URI = addAuthenticatorResponse.URI,

                                DeviceId = authenticatorStatusResponse?.DeviceId,
                                GuardScheme = authenticatorStatusResponse?.GuardScheme ?? 0,
                            };

                        InputSmsCode:
                            string tips = "请输入短信验证码";
                            switch (addAuthenticatorResponse.ConfirmType)
                            {
                                case AddAuthenticatorConfirmType.SmsCode:
                                    tips = "请输入短信验证码";
                                    break;

                                case AddAuthenticatorConfirmType.EmailCode:
                                    tips = "请输入邮箱验证码";
                                    break;
                            }
                            Input input = new Input("输入验证码", tips);
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            string smsCode = input.InputValue;

                        GenerateSteamGuardCode:
                            string guardCode = GuardCodeGenerator.GenerateSteamGuardCode(await Extension.GetSteamTimestampAsync(), guard.SharedSecret);
                            var finalizeAddAuthenticator = await SteamAuthenticator.FinalizeAddAuthenticatorAsync(webClient.WebApiToken, webClient.SteamId, smsCode, guardCode);
                            var finalizeAddAuthenticatorResponse = finalizeAddAuthenticator.Body;
                            switch (finalizeAddAuthenticatorResponse?.Status)
                            {
                                case null:
                                case SteamKit.Model.FinalizeAddAuthenticatorResponse.FinalizeAuthenticatorStatus.UnableToGenerateCorrectCodes:
                                    await Task.Delay(500);
                                    goto GenerateSteamGuardCode;

                                case SteamKit.Model.FinalizeAddAuthenticatorResponse.FinalizeAuthenticatorStatus.BadSMSCode:
                                    MessageBox.Show($"短信验证码错误，请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    goto InputSmsCode;

                                case SteamKit.Model.FinalizeAddAuthenticatorResponse.FinalizeAuthenticatorStatus.Finalize:
                                    authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                                    authenticatorStatusResponse = authenticatorStatus.Body;
                                    guard = new Guard
                                    {
                                        AccountName = guard.AccountName,
                                        IdentitySecret = guard.IdentitySecret,
                                        SharedSecret = guard.SharedSecret,
                                        RevocationCode = guard.RevocationCode,
                                        Secret1 = guard.Secret1,
                                        SerialNumber = guard.SerialNumber,
                                        TokenGID = guard.TokenGID,
                                        URI = guard.URI,

                                        DeviceId = authenticatorStatusResponse?.DeviceId ?? guard.DeviceId,
                                        GuardScheme = authenticatorStatusResponse?.GuardScheme ?? guard.GuardScheme,
                                    };

                                    Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);
                                    MessageBox.Show($"令牌添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                            }
                        }
                        break;

                    case AddAuthenticatorStatus.AuthenticatorPresent:
                        {
                            MessageBox.Show($"您已绑定Steam令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    default:
                        MessageBox.Show($"绑定令牌失败[{addAuthenticatorResponse?.Status}]", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }
        }

        private async void moveAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            var webClient = Appsetting.Instance.CurrentClient;
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
            {
                MessageBox.Show($"当前帐号未绑定令牌" +
                    $"{Environment.NewLine}" +
                    $"你可以直接添加令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(Appsetting.Instance.CurrentClient.Account);
            if (authenticatorStatusResponse.TokenGID == guard?.TokenGID)
            {
                MessageBox.Show($"当前帐号令牌验证器已绑定至此设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string deviceId = authenticatorStatusResponse.DeviceId;

            MoveAuthenticatorResult moveAuthenticatorResult = MoveAuthenticatorResult.Begin;
            while (true)
            {
                switch (moveAuthenticatorResult)
                {
                    case MoveAuthenticatorResult.Begin:
                        {
                            var accountPhoneStatus = await SteamApi.QueryAccountPhoneStatusAsync(webClient.WebApiToken);
                            if (accountPhoneStatus?.Body?.VerifiedPhone ?? true)
                            {
                                moveAuthenticatorResult = MoveAuthenticatorResult.AddPhoneFailure;
                                continue;
                            }

                            moveAuthenticatorResult = MoveAuthenticatorResult.WaitAddPhone;
                        }
                        break;

                    case MoveAuthenticatorResult.WaitAddPhone:
                        {
                            DialogResult res = MessageBox.Show($"检测到当前Steam帐号未绑定手机号" +
                            $"{Environment.NewLine}" +
                            $"你需要先绑定手机号才能继续移动令牌，是否继续绑定手机号？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (res != DialogResult.Yes)
                            {
                                return;
                            }

                            PhoneInput phoneInput = new PhoneInput("添加手机号", $"请输入手机号" +
                                $"{Environment.NewLine}" +
                                $"例如:+86 13100000000");
                            if (phoneInput.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            string phone = phoneInput.Phone;
                            string country = phoneInput.Country;
                            var setAccountPhone = await SteamApi.SetAccountPhoneNumberAsync(webClient.WebApiToken, phone, country);
                            if (string.IsNullOrWhiteSpace(setAccountPhone.Body?.ConfirmationEmailAddress))
                            {
                                MessageBox.Show($"添加手机号失败",
                                    "提示",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            moveAuthenticatorResult = MoveAuthenticatorResult.WaitEmailConfirm;
                        }
                        break;

                    case MoveAuthenticatorResult.WaitEmailConfirm:
                        {
                            MessageBox.Show($"请进行邮箱确认",
                                "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            var waitingForEmailConfirmation = await SteamApi.IsAccountWaitingForEmailConfirmationAsync(webClient.WebApiToken);
                            if (waitingForEmailConfirmation.Body.AwaitingEmailConfirmation)
                            {
                                await Task.Delay(1000);
                                break;
                            }

                            var sendSmsCode = await SteamApi.SendPhoneVerificationCodeAsync(webClient.WebApiToken);
                            if (sendSmsCode.ResultCode != SteamKit.Model.ErrorCodes.OK)
                            {
                                MessageBox.Show($"发送验证码失败,{sendSmsCode.ResultCode}");
                                return;
                            }

                            moveAuthenticatorResult = MoveAuthenticatorResult.WaitFinalizationAddPhone;
                        }
                        break;

                    case MoveAuthenticatorResult.WaitFinalizationAddPhone:
                        {
                            Input input = new Input("添加手机号", $"你正在绑定手机号, 验证短信已发送至你的手机," +
                                $"{Environment.NewLine}" +
                                $"请输入你收到的手机验证码");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                if (MessageBox.Show("是否要退出绑定令牌？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    return;
                                }

                                continue;
                            }

                            string smsCode = input.InputValue;
                            var finalizationAddPhone = await SteamApi.VerifyAccountPhoneWithCodeAsync(webClient.WebApiToken, smsCode);
                            switch (finalizationAddPhone.ResultCode)
                            {
                                case ErrorCodes.OK:
                                    break;

                                case ErrorCodes.NoMatch:
                                case ErrorCodes.SMSCodeFailed:
                                    MessageBox.Show($"验证码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;

                                default:
                                    MessageBox.Show($"添加手机号失败,{finalizationAddPhone.ResultCode}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                            }

                            MessageBox.Show($"添加手机号成功，继续为你移动令牌验证器");
                            moveAuthenticatorResult = MoveAuthenticatorResult.AddPhoneFailure;
                        }
                        break;

                    case MoveAuthenticatorResult.AddPhoneFailure:
                        {
                            var beginMoveAuthenticator = await SteamAuthenticator.BeginMoveAuthenticatorAsync(webClient.WebApiToken);
                            switch (beginMoveAuthenticator.ResultCode)
                            {
                                case ErrorCodes.OK:
                                    moveAuthenticatorResult = MoveAuthenticatorResult.WaitFinalization;
                                    break;

                                case ErrorCodes.RateLimitExceeded:
                                    MessageBox.Show($"发送验证码太频繁了，请稍后再试");
                                    return;

                                default:
                                    MessageBox.Show($"发送验证码失败,{beginMoveAuthenticator.ResultCode}");
                                    return;
                            }
                        }
                        break;

                    case MoveAuthenticatorResult.WaitFinalization:
                        {
                            Input input = new Input("移动令牌验证器", $"你正在移动令牌验证器, 验证短信已发送至你的手机" +
                                $"{Environment.NewLine}" +
                                $"请输入你收到的手机验证码");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                continue;
                            }

                            string smsCode = input.InputValue;
                            var finalizeMoveAuthenticator = await SteamAuthenticator.FinalizeMoveAuthenticatorAsync(webClient.WebApiToken, smsCode);
                            if (string.IsNullOrWhiteSpace(finalizeMoveAuthenticator.Body?.ReplacementToken?.SharedSecret))
                            {
                                switch (finalizeMoveAuthenticator.ResultCode)
                                {
                                    case ErrorCodes.OK:
                                        break;

                                    case ErrorCodes.NoMatch:
                                    case ErrorCodes.SMSCodeFailed:
                                        MessageBox.Show($"验证码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;

                                    default:
                                        MessageBox.Show($"移动令牌验证器失败,{finalizeMoveAuthenticator.ResultCode}");
                                        return;
                                }
                            }

                            try
                            {
                                authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                                authenticatorStatusResponse = authenticatorStatus.Body;
                            }
                            catch
                            {

                            }

                            var authenticatorResponse = finalizeMoveAuthenticator.Body.ReplacementToken;
                            guard = new Guard
                            {
                                AccountName = authenticatorResponse.AccountName,
                                IdentitySecret = authenticatorResponse.IdentitySecret,
                                SharedSecret = authenticatorResponse.SharedSecret,
                                RevocationCode = authenticatorResponse.RevocationCode,
                                Secret1 = authenticatorResponse.Secret1,
                                SerialNumber = authenticatorResponse.SerialNumber,
                                TokenGID = authenticatorResponse.TokenGID,
                                URI = authenticatorResponse.URI,
                                GuardScheme = authenticatorResponse.GuardScheme,

                                DeviceId = authenticatorStatusResponse?.DeviceId ?? deviceId,
                            };

                            Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);
                            MessageBox.Show($"令牌添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                }
            }
        }

        private async void removeAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            var webClient = Appsetting.Instance.CurrentClient;
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme == SteamGuardScheme.None)
            {
                MessageBox.Show("当前帐号未设置令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SteamGuardScheme? guardScheme = null;
            string code = null;
            string msg = null;

            DialogResult res = MessageBox.Show($"{webClient.Account}\n" +
                $"你想要完全移除Steam令牌码？\n" +
                $"是 - 完全移除Steam令牌\n" +
                $"否 - 切换到邮箱验证",
                "提示",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (res)
            {
                case DialogResult.Yes:
                    guardScheme = SteamGuardScheme.None;
                    msg = "令牌已移除";
                    break;

                case DialogResult.No:
                    guardScheme = SteamGuardScheme.Email;
                    msg = "已切换到邮箱令牌";
                    break;
            }

            if (guardScheme == null)
            {
                return;
            }

            if (authenticatorStatusResponse.GuardScheme == SteamGuardScheme.Device)
            {
                var guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                code = guard?.RevocationCode;
                if (string.IsNullOrWhiteSpace(code))
                {
                    Input input = new Input("输入撤销码", "请输入撤销码");
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    code = input.InputValue;
                }
            }

            if (MessageBox.Show($"{webClient.Account}\n" +
                $"你确认要{(guardScheme == SteamGuardScheme.None ?
                "移除全部令牌吗？" :
                "切换到邮箱令牌吗？")}",
                "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var removeAuthenticator = await SteamAuthenticator.RemoveAuthenticatorAsync(webClient.WebApiToken, guardScheme.Value, code);

                if (removeAuthenticator.ResultCode == SteamKit.Model.ErrorCodes.OK)
                {
                    MessageBox.Show(msg, "提示");
                    return;
                }

                if (removeAuthenticator.ResultCode == SteamKit.Model.ErrorCodes.Pending)
                {
                    while (true)
                    {
                        if (MessageBox.Show($"请进行邮箱确认",
                            "提示",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                        {
                            return;
                        }

                        authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                        authenticatorStatusResponse = authenticatorStatus.Body;
                        if (authenticatorStatusResponse?.GuardScheme == SteamGuardScheme.None)
                        {
                            MessageBox.Show(msg, "提示");
                            break;
                        }

                        await Task.Delay(1000);
                    }

                    return;
                }

                MessageBox.Show($"操作失败[{removeAuthenticator.ResultCode}]", "提示");
            }
            finally
            {
                authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                authenticatorStatusResponse = authenticatorStatus.Body;
                if (authenticatorStatusResponse != null && authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
                {
                    Appsetting.Instance.Manifest.RemoveGuard(webClient.Account, out var guard);
                }
            }
        }

        private void confirmMenuItem_Click(object sender, EventArgs e)
        {
            if (!Appsetting.Instance.CurrentClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            Forms.Confirmation confirmation = new Forms.Confirmation(Appsetting.Instance.CurrentClient);
            confirmation.Show();
        }

        private void importAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "导入令牌",
                Filter = "JSON (*.json)|*.json",
                DefaultExt = ".json",
                InitialDirectory = AppContext.BaseDirectory,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!openFileDialog.FileNames.Any())
                {
                    return;
                }

                List<string> error = new List<string>();
                List<string> success = new List<string>();
                foreach (var item in openFileDialog.FileNames)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    try
                    {
                        string filePath = item;// openFileDialog.FileName;
                        string json = File.ReadAllText(filePath, Encoding.UTF8);
                        Guard guard = JsonConvert.DeserializeObject<Guard>(json);
                        Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);

                        success.Add(fileInfo.Name);
                    }
                    catch
                    {
                        MessageBox.Show($"文件格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        error.Add(fileInfo.Name);
                    }
                }

                MessageBox.Show($"令牌导入结果" +
                    $"{Environment.NewLine}" +
                    $"导入成功{success.Count}个{Environment.NewLine}{(success.Any() ? string.Join(Environment.NewLine, success) : "")}" +
                    $"{Environment.NewLine}" +
                    $"导入失败{error.Count}个{Environment.NewLine}{(error.Any() ? string.Join(Environment.NewLine, error) : "")}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshClientMsg(object _)
        {
            try
            {
                List<Task> tasks = new List<Task>();

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    tasks.Add(QueryAuthSessionsForAccount(tokenSource.Token));
                    tasks.Add(NotificationsReceived(tokenSource.Token));
                    tasks.Add(QueryConfirmations(tokenSource.Token));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch
            {

            }
            finally
            {
                ResetTimer(timerInterval, timerPeriod);
            }
        }

        private async Task Login(string token)
        {
            try
            {
                UserImg.Image = Properties.Resources.userimg;
                loginMenuItem.Enabled = false;
                UserName.ForeColor = Color.FromArgb(128, 128, 128);
                UserName.Text = "正在登录...";
                Balance.Text = "￥0.00";

                await Appsetting.Instance.CurrentClient.LoginAsync(token);

                await ResetUser();
            }
            catch
            {
                UserName.ForeColor = Color.Black;
                UserName.Text = "---";
                Balance.Text = "￥0.00";
            }
            finally
            {
                loginMenuItem.Enabled = true;
            }
        }

        private async Task NotificationsReceived(CancellationToken cancellationToken)
        {
            try
            {
                var webClient = Appsetting.Instance.CurrentClient;

                var querySteamNotifications = await SteamApi.QuerySteamNotificationsAsync(webClient.WebApiToken, includeHidden: false, countOnly: false,
                      includeConfirmation: true, includePinned: false, includeRead: false,
                      cancellationToken: cancellationToken);

                var body = querySteamNotifications.Body;
                var notifications = body?.Notifications ?? new List<Notification>();
                var offers = notifications.Where(c => c.NotificationType == SteamNotificationType.ReceivedTradeOffer && !c.Read && !c.Hidden).ToList();

                OfferCountLabel.Text = $"{offers.Count}";
                ConfirmationCountLable.Text = $"{body?.ConfirmationCount ?? 0}";

            }
            catch
            {
            }
        }

        private async Task QueryAuthSessionsForAccount(CancellationToken cancellationToken)
        {
            if (!loginConfirmLocker.Wait(0))
            {
                return;
            }
            try
            {
                var webClient = Appsetting.Instance.CurrentClient;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard == null)
                {
                    return;
                }

                var queryAuthSessions = await SteamAuthentication.QueryAuthSessionsForAccountAsync(webClient.WebApiToken, cancellationToken);
                var clients = queryAuthSessions.Body?.ClientIds;
                if (clients?.Count > 0)
                {
                    var querySession = await SteamAuthentication.QueryAuthSessionInfoAsync(webClient.WebApiToken, clients[0], cancellationToken);
                    var sessionInfo = querySession.Body;
                    if (sessionInfo == null)
                    {
                        return;
                    }

                    string clientType = sessionInfo.PlatformType switch
                    {
                        var platform when platform == AuthTokenPlatformType.SteamClient => "SteamClient",
                        var platform when platform == AuthTokenPlatformType.MobileApp => "Steam App",
                        var platform when platform == AuthTokenPlatformType.WebBrowser => "网页浏览器",
                        _ => "未知设备"
                    };
                    var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                    MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(webClient, (ulong)clients[0], sessionInfo.Version);
                    mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{webClient.SteamId} 有新的登录请求";
                    mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                    mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP 地址：{sessionInfo.IP}";
                    mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("，", regions)}";

                    mobileConfirmationLogin.ShowDialog();
                }
            }
            catch
            {
            }
            finally
            {
                loginConfirmLocker.Release();
            }
        }

        private async Task QueryConfirmations(CancellationToken cancellationToken)
        {
            if (!confirmationPopupLocker.Wait(0))
            {
                return;
            }

            try
            {
                var setting = Appsetting.Instance.AppSetting.Entry;
                if (!setting.PeriodicCheckingConfirmation)
                {
                    return;
                }

                var webClient = Appsetting.Instance.CurrentClient;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard == null)
                {
                    return;
                }

                var queryConfirmations = await webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret, cancellationToken);
                var confirmations = queryConfirmations.Confirmations ?? new List<SteamKit.Model.Confirmation>();

                List<SteamKit.Model.Confirmation> autoConfirm = new List<SteamKit.Model.Confirmation>();
                List<SteamKit.Model.Confirmation> waitConfirm = new List<SteamKit.Model.Confirmation>();

                foreach (var conf in confirmations)
                {
                    if ((conf.ConfType == ConfirmationType.MarketListing && setting.AutoConfirmMarket) ||
                      (conf.ConfType == ConfirmationType.Trade && setting.AutoConfirmTrade))
                    {
                        autoConfirm.Add(conf);
                        continue;
                    }

                    waitConfirm.Add(conf);
                }

                if (autoConfirm.Any())
                {
                    try
                    {
                        var autoConfirmResult = await webClient.Confirmation.AllowConfirmationAsync(autoConfirm, guard.DeviceId, guard.IdentitySecret, cancellationToken);
                    }
                    catch
                    {

                    }
                }

                if (waitConfirm.Any())
                {
                    ConfirmationPopup confirmationPopup = new ConfirmationPopup(webClient, waitConfirm);
                    confirmationPopup.ShowDialog();
                }
            }
            catch
            {
            }
            finally
            {
                confirmationPopupLocker.Release();
            }
        }

        private async Task ResetUser()
        {
            ResetTimer(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

            Text = $"Steam验证器";
            loginMenuItem.Text = "登录";

            copyCookieMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Enabled = false;
            copyAccessTokenToolItem.Enabled = false;

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            Balance.Text = "￥0.00";

            OfferCountLabel.Text = "0";
            ConfirmationCountLable.Text = "0";

            await Task.Run(() =>
            {
                if (Appsetting.Instance.CurrentClient.LoggedIn)
                {
                    Text = $"Steam验证器[{Appsetting.Instance.CurrentClient.Account}]";
                    loginMenuItem.Text = "切换帐号";

                    copyCookieMenuItem.Enabled = true;
                    copyRefreshTokenMenuItem.Enabled = true;
                    copyAccessTokenToolItem.Enabled = true;

                    UserImg.Image = Properties.Resources.userimg;
                    UserName.ForeColor = Color.Green;
                    UserName.Text = Appsetting.Instance.CurrentClient.SteamId;
                    Balance.Text = "￥0.00";

                    SteamApi.QueryPlayerSummariesAsync(null, Appsetting.Instance.CurrentClient.WebApiToken, new[] { Appsetting.Instance.CurrentClient.SteamId }).ContinueWith(async reuslt =>
                    {
                        var players = await reuslt;
                        var player = players.Body?.Players?.FirstOrDefault();

                        if (player != null)
                        {
                            UserImg.LoadAsync(player.AvatarFull);
                            UserName.Text = player.SteamName;
                        }

                        Appsetting.Instance.Manifest.AddUser(Appsetting.Instance.CurrentClient.SteamId, new User
                        {
                            Account = Appsetting.Instance.CurrentClient.Account,
                            SteamId = Appsetting.Instance.CurrentClient.SteamId,
                            RefreshToken = Appsetting.Instance.CurrentClient.RefreshToken,
                            NickName = player?.SteamName ?? Appsetting.Instance.CurrentClient.SteamId,
                            Avatar = player?.AvatarFull ?? ""
                        });
                    });

                    Appsetting.Instance.CurrentClient.User.QueryWalletDetailsAsync().ContinueWith(async result =>
                    {
                        var walletDetails = await result;
                        if (walletDetails?.HasWallet ?? false)
                        {
                            Balance.Text = $"{walletDetails.FormattedBalance}";
                            if (!string.IsNullOrWhiteSpace(walletDetails.FormattedDelayedBalance))
                            {
                                Balance.Text = $"{Balance.Text} ({walletDetails.FormattedDelayedBalance})";
                            }
                        }
                    });

                    ResetTimer(TimeSpan.Zero, timerPeriod);
                }
            });
        }

        private void ResetTimer(TimeSpan dueTime, TimeSpan period)
        {
            timer.Change(dueTime, period);
        }
    }
}


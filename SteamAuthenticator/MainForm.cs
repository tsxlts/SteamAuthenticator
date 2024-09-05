
using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly System.Threading.Timer timer;
        private readonly TimeSpan timerInterval = TimeSpan.FromSeconds(1);
        private readonly TimeSpan timerPeriod = TimeSpan.FromSeconds(20);
        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);
        private readonly ContextMenuStrip contextMenuStrip;

        private UserClient currentClient = null;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            timer = new System.Threading.Timer(RefreshClientMsg, null, -1, -1);

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("切换").Click += setCurrentClientMenuItem_Click;
            contextMenuStrip.Items.Add("重新登录").Click += loginMenuItem_Click;
            contextMenuStrip.Items.Add("注销登录").Click += removeUserMenuItem_Click;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await LoadUsers();

            var user = Appsetting.Instance.Clients?.FirstOrDefault(c => c.User.SteamId == Appsetting.Instance.AppSetting.Entry.CurrentUser);
            user = user ?? Appsetting.Instance.Clients?.FirstOrDefault();
            if (user != null)
            {
                SetCurrentClient(user);
            }

            CheckVersion();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Dispose();
            foreach (var client in Appsetting.Instance.Clients)
            {
                client.Client.Dispose();
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
            if (!currentClient.Client.LoggedIn)
            {
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in currentClient.Client.WebCookie)
            {
                stringBuilder.Append($"{item.Name}={HttpUtility.UrlEncode(item.Value)}; ");
            }
            Clipboard.SetText(stringBuilder.ToString());
        }

        private void copyRefreshTokenMenuItem_Click(object sender, EventArgs e)
        {
            if (!currentClient.Client.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(currentClient.Client.RefreshToken);
        }

        private void copyAccessTokenMenuItem_Click(object sender, EventArgs e)
        {
            if (!currentClient.Client.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(currentClient.Client.AccessToken);
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

        private async void checkVersionMenuItem_Click(object sender, EventArgs e)
        {
            await CheckVersion();
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guardMenuItem_Click(object sender, EventArgs e)
        {
            StreamGuard streamGuard = new StreamGuard(currentClient?.Client?.Account);
            streamGuard.Show();
        }

        private async void addAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            var webClient = currentClient.Client;
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
                                    if (MessageBox.Show("是否要退出绑定令牌？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        return;
                                    }
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
            var webClient = currentClient.Client;
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

            Guard guard = Appsetting.Instance.Manifest.GetGuard(currentClient.Client.Account);
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
                                if (MessageBox.Show("是否要退出绑定令牌？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    return;
                                }

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
            var webClient = currentClient.Client;
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
            var webClient = currentClient.Client;
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (guard == null)
            {
                MessageBox.Show($"用户[{webClient.Account}]未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Forms.Confirmations confirmation = new Forms.Confirmations(webClient);
            confirmation.Show();
        }

        private void importAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "导入令牌",
                Filter = "令牌文件 (*.entry)|*.entry",
                DefaultExt = ".entry",
                InitialDirectory = AppContext.BaseDirectory,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!openFileDialog.FileNames.Any())
                {
                    return;
                }
                string tips = "请输入访问密码";
                Input input;

                List<string> error = new List<string>();
                List<string> success = new List<string>();
                foreach (var item in openFileDialog.FileNames)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    try
                    {
                        using (FileStream stream = fileInfo.OpenRead())
                        {
                            bool encrypt = stream.ReadBoolean();
                            byte[] dataBuffer = new byte[0];

                            if (encrypt)
                            {
                                tips = $"请输入解密密码" +
                                    $"{Environment.NewLine}" +
                                    $"{fileInfo.Name}";

                                input = new Input($"导出令牌[{fileInfo.Name}]", tips, true);
                                input.ShowDialog();
                                string password = input.InputValue;

                                byte[] iv = new byte[stream.ReadInt32()];
                                stream.Read(iv);

                                byte[] salt = new byte[stream.ReadInt32()];
                                stream.Read(salt);

                                dataBuffer = new byte[stream.ReadInt32()];
                                stream.Read(dataBuffer);

                                dataBuffer = FileEncryptor.DecryptData(password, salt, iv, dataBuffer);
                            }
                            else
                            {
                                dataBuffer = new byte[stream.ReadInt32()];
                                stream.Read(dataBuffer);
                            }

                            Guard guard = new Guard();
                            guard.Deserialize(new MemoryStream(dataBuffer));
                            guard = guard.Value;

                            Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);

                            success.Add(fileInfo.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{fileInfo.FullName}" +
                            $"{Environment.NewLine}" +
                            $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void offersBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient.Client;
                if (!webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录Steam帐号");
                    return;
                }

                Offers offersForm = new Offers(webClient);
                offersForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void acceptOfferBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient.Client;
                if (!webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录Steam帐号");
                    return;
                }

                if (MessageBox.Show("你确定要接受所有报价吗？", "接受报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                IEnumerable<Offer> offers = OfferCountLabel.Tag as IEnumerable<Offer>;
                if (offers == null || !offers.Any())
                {
                    return;
                }

                await HandleOffer(webClient, offers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void declineOfferBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient.Client;
                if (!webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录Steam帐号");
                    return;
                }

                if (MessageBox.Show("你确定要拒绝所有报价吗？", "拒绝报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                IEnumerable<Offer> offers = OfferCountLabel.Tag as IEnumerable<Offer>;
                if (offers == null || !offers.Any())
                {
                    return;
                }

                await HandleOffer(webClient, offers, false, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UsersPanel_SizeChanged(object sender, EventArgs e)
        {
            ResetUserPanel();
        }

        private async void addUserBtn_Click(object sender, EventArgs e)
        {
            await Login(false, null);
        }

        private async void btnUser_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Control control = sender as Control;
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await SwitchUser(userClient);
        }

        private async void setCurrentClientMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await SwitchUser(userClient);
        }

        private async void loginMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await Login(false, userClient.User.Account);
        }

        private async void removeUserMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            UserPanel panel = menuStrip.SourceControl.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            await userClient.Client.LogoutAsync();
            userClient.Client.Dispose();

            Appsetting.Instance.Manifest.RemoveUser(userClient.User.SteamId, out var entry);
            Appsetting.Instance.Clients.Remove(userClient);
            UsersPanel.Controls.Remove(panel);
            ResetUserPanel();

            if (userClient.User.SteamId == currentClient?.User?.SteamId)
            {
                SetCurrentClient(Appsetting.Instance.Clients.FirstOrDefault() ?? userClient, true);
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

                    tasks.Add(QueryOffers(tokenSource.Token));
                    tasks.Add(QueryConfirmations(tokenSource.Token));

                    tasks.Add(QueryWalletDetails(tokenSource.Token));
                    tasks.Add(RefreshUser(tokenSource.Token));
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

        private async Task NotificationsReceived(CancellationToken cancellationToken)
        {
            try
            {
                var webClient = currentClient.Client;

                var querySteamNotifications = await SteamApi.QuerySteamNotificationsAsync(webClient.WebApiToken, includeHidden: false, countOnly: true,
                      includeConfirmation: true, includePinned: false, includeRead: false,
                      cancellationToken: cancellationToken);

                var body = querySteamNotifications.Body;
                ConfirmationCountLable.Text = $"{body?.ConfirmationCount ?? 0}";
            }
            catch
            {
            }
        }

        private async Task QueryAuthSessionsForAccount(CancellationToken cancellationToken)
        {
            if (!currentClient.LoginConfirmLocker.Wait(0))
            {
                return;
            }
            try
            {
                var webClient = currentClient.Client;

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
                currentClient.LoginConfirmLocker.Release();
            }
        }

        private async Task QueryOffers(CancellationToken cancellationToken)
        {
            try
            {
                var setting = Appsetting.Instance.AppSetting.Entry;

                var webClient = currentClient.Client;

                var queryOffers = await webClient.TradeOffer.QueryOffersAsync(sentOffer: false, receivedOffer: true, onlyActive: true,
                      cancellationToken: cancellationToken);

                var offers = queryOffers?.TradeOffersReceived ?? new List<Offer>();

                OfferCountLabel.Text = $"{offers.Count}";
                OfferCountLabel.Tag = offers;

                if (setting.AutoAcceptOffer)
                {
                    await HandleOffer(webClient, offers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
                }
            }
            catch
            {
            }
        }

        private async Task QueryConfirmations(CancellationToken cancellationToken)
        {
            var setting = Appsetting.Instance.AppSetting.Entry;
            if (!setting.PeriodicCheckingConfirmation)
            {
                return;
            }

            List<Task> tasks = new List<Task>();
            var clients = setting.CheckAllConfirmation ? Appsetting.Instance.Clients : new List<UserClient> { currentClient };
            foreach (var client in clients)
            {
                var task = Task.Run(() =>
                {
                    if (!client.ConfirmationPopupLocker.Wait(0))
                    {
                        return;
                    }

                    try
                    {
                        var webClient = client.Client;

                        Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                        if (guard == null)
                        {
                            return;
                        }

                        var queryConfirmations = webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret, cancellationToken).Result;
                        var confirmations = queryConfirmations.Confirmations ?? new List<Confirmation>();

                        List<Confirmation> autoConfirm = new List<Confirmation>();
                        List<Confirmation> waitConfirm = new List<Confirmation>();

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
                                bool accept = HandleConfirmation(webClient, guard, autoConfirm, true, cancellationToken).Result;
                            }
                            catch
                            {

                            }
                        }

                        if (waitConfirm.Any())
                        {
                            ConfirmationsPopup confirmationPopup = new ConfirmationsPopup(webClient, waitConfirm);
                            confirmationPopup.ShowDialog();
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        client.ConfirmationPopupLocker.Release();
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private async Task QueryWalletDetails(CancellationToken cancellationToken)
        {
            try
            {
                var webClient = currentClient.Client;

                var walletDetails = await webClient.User.QueryWalletDetailsAsync();

                if (walletDetails?.HasWallet ?? false)
                {
                    Balance.Text = $"{walletDetails.FormattedBalance}";
                    if (!string.IsNullOrWhiteSpace(walletDetails.FormattedDelayedBalance))
                    {
                        Balance.Text = $"{Balance.Text} ({walletDetails.FormattedDelayedBalance})";
                    }
                }
            }
            catch
            {
            }
        }

        private async Task RefreshUser(CancellationToken cancellationToken)
        {
            try
            {
                var controlCollection = UsersPanel.Controls.Cast<UserPanel>().ToArray();
                foreach (UserPanel userPanel in controlCollection)
                {
                    //var nameLabel = userPanel.Controls.Cast<Control>().FirstOrDefault(c => c.Name == "username") as Label;
                    var nameLabel = userPanel.Controls.Find("username", false)?.FirstOrDefault() as Label;
                    if (nameLabel == null)
                    {
                        continue;
                    }

                    var client = userPanel.UserClient.Client;
                    var user = userPanel.UserClient.User;

                    var palyerSummaries = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId }, cancellationToken: cancellationToken);
                    if (palyerSummaries.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        await client.LogoutAsync();
                    }

                    bool reloadCurrent = false;
                    if (client.LoggedIn)
                    {
                        nameLabel.ForeColor = Color.Green;
                    }
                    else
                    {
                        nameLabel.ForeColor = Color.Red;

                        reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                    }

                    var player = palyerSummaries.Body?.Players?.FirstOrDefault();
                    if (player != null)
                    {
                        if (player.SteamName != user.NickName || player.AvatarFull != user.Avatar)
                        {
                            user.NickName = player.SteamName;
                            user.Avatar = player.AvatarFull;
                            Appsetting.Instance.Manifest.AddUser(client.SteamId, user);

                            PictureBox pictureBox = userPanel.Controls.Find("useravatar", false)?.FirstOrDefault() as PictureBox;
                            pictureBox?.LoadAsync(user.Avatar);

                            reloadCurrent = user.SteamId == currentClient?.User?.SteamId;
                        }
                    }

                    if (reloadCurrent)
                    {
                        SetCurrentClient(userPanel.UserClient, true);
                    }
                }
            }
            catch
            {

            }
        }

        private async Task<UserClient> SaveUser(SteamCommunityClient client)
        {
            if (client?.LoggedIn ?? false)
            {
                client.SetLanguage(Language.Schinese);

                var players = await SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId });
                var player = players.Body?.Players?.FirstOrDefault();

                var user = new User
                {
                    Account = client.Account,
                    SteamId = client.SteamId,
                    RefreshToken = client.RefreshToken,
                    NickName = player?.SteamName ?? client.SteamId,
                    Avatar = player?.AvatarFull ?? ""
                };

                UserClient userClient = new UserClient(user, client);

                Appsetting.Instance.Manifest.AddUser(client.SteamId, user);

                Appsetting.Instance.Clients.RemoveAll(c => c.User.SteamId == user.SteamId);
                Appsetting.Instance.Clients.Add(userClient);
                return userClient;

            }
            return null;
        }

        private async Task SwitchUser(UserClient userClient)
        {
            if (!userClient.Client.LoggedIn)
            {
                if (!await userClient.LoginAsync())
                {
                    userClient = await Login(true, userClient.User.Account);
                }
            }

            SetCurrentClient(userClient);
        }

        private void SetCurrentClient(UserClient userClient, bool reload = false)
        {
            if (!reload)
            {
                if (userClient?.User?.SteamId == currentClient?.User?.SteamId && userClient.Client.LoggedIn == currentClient?.Client.LoggedIn)
                {
                    return;
                }
            }

            ResetTimer(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            Text = $"Steam验证器";

            copyCookieMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Enabled = false;
            copyAccessTokenToolItem.Enabled = false;

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            Balance.Text = "￥0.00";

            OfferCountLabel.Text = "0";
            ConfirmationCountLable.Text = "0";

            if (userClient?.Client?.LoggedIn ?? false)
            {
                Text = $"Steam验证器[{userClient.Client.Account}]";

                copyCookieMenuItem.Enabled = true;
                copyRefreshTokenMenuItem.Enabled = true;
                copyAccessTokenToolItem.Enabled = true;

                UserImg.Image = Properties.Resources.userimg;
                if (!string.IsNullOrWhiteSpace(userClient.User.Avatar))
                {
                    UserImg.LoadAsync(userClient.User.Avatar);
                }
                UserName.ForeColor = Color.Green;
                UserName.Text = $"{userClient.User.Account} [{userClient.User.NickName}]";
                Balance.Text = "￥0.00";

                ResetTimer(TimeSpan.Zero, timerPeriod);
            }

            currentClient = userClient;
            Appsetting.Instance.AppSetting.Entry.CurrentUser = currentClient.User.SteamId;
            Appsetting.Instance.AppSetting.Save();
        }

        private void ResetTimer(TimeSpan dueTime, TimeSpan period)
        {
            timer.Change(dueTime, period);
        }

        private async Task LoadUsers()
        {
            UsersPanel.Controls.Clear();

            int startX = GetUserControlStartPointX(out int cells);

            Appsetting.Instance.Clients.RemoveAll(c => !c.Client.LoggedIn);

            IEnumerable<string> accounts = Appsetting.Instance.Manifest.GetUsers();
            int index = 0;
            if (accounts.Any())
            {
                foreach (string account in accounts)
                {
                    User user = Appsetting.Instance.Manifest.GetUser(account);

                    UserPanel panel = CreateUserPanel(startX, cells, index, new UserClient(user, new SteamCommunityClient()));
                    UsersPanel.Controls.Add(panel);

                    index++;

                    Appsetting.Instance.Clients.Add(panel.UserClient);
                }
            }

            {
                UserPanel panel = new UserPanel()
                {
                    Size = new Size(80, 110),
                    Location = new Point(startX * (index % cells) + 10, 130 * (index / cells) + 10),
                    UserClient = UserClient.None
                };

                PictureBox pictureBox = new PictureBox() { Width = 80, Height = 80, Location = new Point(0, 0), SizeMode = PictureBoxSizeMode.Zoom };
                pictureBox.Image = Properties.Resources.add;
                panel.Controls.Add(pictureBox);

                Label nameLabel = new Label()
                {
                    Text = $"添加帐号",
                    AutoSize = false,
                    AutoEllipsis = true,
                    Size = new Size(80, 30),
                    TextAlign = ContentAlignment.TopCenter,
                    ForeColor = Color.FromArgb(244, 164, 96),
                    Location = new Point(0, 80)
                };
                panel.Controls.Add(nameLabel);

                pictureBox.Cursor = Cursors.Hand;
                nameLabel.Cursor = Cursors.Hand;
                pictureBox.Click += addUserBtn_Click;
                nameLabel.Click += addUserBtn_Click;

                UsersPanel.Controls.Add(panel);
            }

            var tasks = Appsetting.Instance.Clients.Select(c => c.LoginAsync());
            await Task.WhenAll(tasks);
        }

        private async Task<UserClient> Login(bool relogin, string account)
        {
            if (relogin)
            {
                MessageBox.Show($"帐号 {account} 已掉线，请重新登录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Login login = new Login(account);
            if (login.ShowDialog() == DialogResult.OK && (login.Client?.LoggedIn ?? false))
            {
                var userClient = await SaveUser(login.Client);
                var user = userClient.User;

                if (Appsetting.Instance.Clients.Count == 1)
                {
                    SetCurrentClient(Appsetting.Instance.Clients[0]);
                }

                var controlCollection = UsersPanel.Controls.Cast<UserPanel>().ToList();
                var index = controlCollection.FindIndex(c => c.UserClient.User.SteamId == user.SteamId);

                if (index < 0)
                {
                    index = controlCollection.Count - 1;
                }
                else
                {
                    controlCollection.RemoveAt(index);
                }

                int startX = GetUserControlStartPointX(out int cells);
                UserPanel panel = CreateUserPanel(startX, cells, index, userClient);
                controlCollection.Insert(index, panel);

                UsersPanel.Controls.Clear();
                UsersPanel.Controls.AddRange(controlCollection.ToArray());
                ResetUserPanel();

                return panel.UserClient;
            }

            return null;
        }

        private void ResetUserPanel()
        {
            try
            {
                var controlCollection = UsersPanel.Controls.Cast<Control>().ToArray();

                int x = GetUserControlStartPointX(out int cells);

                int index = 0;
                foreach (Control control in controlCollection)
                {
                    control.Location = new Point(x * (index % cells) + 10, 130 * (index / cells) + 10);
                    index++;
                }

                UsersPanel.Controls.Clear();
                UsersPanel.Controls.AddRange(controlCollection);
            }
            catch
            {

            }
        }

        private int GetUserControlStartPointX(out int cells)
        {
            cells = (UsersPanel.Size.Width - 30) / 80;
            int size = (UsersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            if (size < 85)
            {
                cells = cells - 1;
                size = (UsersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            }
            return size;
        }

        private UserPanel CreateUserPanel(int startX, int cells, int index, UserClient userClient)
        {
            UserPanel panel = new UserPanel()
            {
                Size = new Size(80, 110),
                Location = new Point(startX * (index % cells) + 10, 130 * (index / cells) + 10),
                UserClient = userClient
            };

            PictureBox pictureBox = new PictureBox()
            {
                Name = "useravatar",
                Width = 80,
                Height = 80,
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            string avatar = userClient.User.Avatar;
            pictureBox.Image = Properties.Resources.userimg;
            if (!string.IsNullOrEmpty(avatar))
            {
                pictureBox.LoadAsync(avatar);
            }
            panel.Controls.Add(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{userClient.User.Account} [{userClient.User.NickName}]",
                AutoSize = false,
                AutoEllipsis = true,
                Size = new Size(80, 30),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, 80)
            };
            panel.Controls.Add(nameLabel);

            pictureBox.Cursor = Cursors.Hand;
            nameLabel.Cursor = Cursors.Hand;
            pictureBox.MouseClick += btnUser_Click;
            nameLabel.MouseClick += btnUser_Click;

            pictureBox.ContextMenuStrip = contextMenuStrip;
            nameLabel.ContextMenuStrip = contextMenuStrip;

            panel.UserClient
                .WithStartLogin(() => nameLabel.ForeColor = Color.FromArgb(128, 128, 128))
                .WithEndLogin(loggined =>
                {
                    nameLabel.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private async Task CheckVersion()
        {
            if (!checkVersionLocker.Wait(0))
            {
                return;
            }

            try
            {
                var result = await SteamApi.GetAsync<JObject>("https://api.github.com/repos/tsxlts/SteamAuthenticator/releases/latest");
                var resultObj = result.Body;
                if (!(resultObj?.TryGetValue("tag_name", out var tag_name) ?? false))
                {
                    return;
                }

                var match = Regex.Match(Application.ProductVersion, @"[\d.]+");
                var currentVersion = new Version(match.Value);

                match = Regex.Match(tag_name.Value<string>(), @"[\d.]+");
                var newVersion = new Version(match.Value);

                if (currentVersion < newVersion)
                {
                    var assets = resultObj.Value<JArray>("assets");
                    string updateUrl = assets.FirstOrDefault()?.Value<string>("browser_download_url");

                    if (!string.IsNullOrWhiteSpace(updateUrl))
                    {
                        DialogResult updateDialog = MessageBox.Show($"有最新版本可用（{tag_name}），是否立即更新", "版本更新", MessageBoxButtons.YesNo);
                        if (updateDialog == DialogResult.Yes)
                        {
                            Process.Start("explorer.exe", updateUrl);
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                checkVersionLocker.Release();
            }
        }
    }
}


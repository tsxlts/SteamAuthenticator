
using Newtonsoft.Json.Linq;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using SteamKit;
using SteamKit.Model;
using SteamKit.WebClient;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm : Form
    {
        private readonly Version currentVersion;
        private readonly System.Threading.Timer timer;
        private readonly System.Threading.Timer refreshUserTimer;
        private readonly TimeSpan timerMinPeriod = TimeSpan.FromSeconds(20);
        private readonly SemaphoreSlim checkVersionLocker = new SemaphoreSlim(1, 1);
        private readonly ContextMenuStrip contextMenuStrip;

        private string initialDirectory = null;
        private UserClient currentClient = null;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            var match = Regex.Match(Application.ProductVersion, @"[\d.]+");
            currentVersion = new Version(match.Value);
            versionLabel.Text = $"v{currentVersion}";

            timer = new System.Threading.Timer(RefreshClientMsg, null, -1, -1);
            refreshUserTimer = new System.Threading.Timer(RefreshUser, null, -1, -1);

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("切换").Click += setCurrentClientMenuItem_Click;
            contextMenuStrip.Items.Add("重新登录").Click += loginMenuItem_Click;
            contextMenuStrip.Items.Add("退出登录").Click += removeUserMenuItem_Click;
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

            await CheckVersion();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshUserTimer.Dispose();
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
            if (currentClient == null || !currentClient.Client.LoggedIn)
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
            if (currentClient == null || !currentClient.Client.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(currentClient.Client.RefreshToken);
        }

        private void copyAccessTokenMenuItem_Click(object sender, EventArgs e)
        {
            if (currentClient == null || !currentClient.Client.LoggedIn)
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
                    passwordInput = new Input("设置密码", passwordTips, password: true, required: true, errorMsg: "请输入密码");
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

            Input input = new Input("设置密码", $"请输入新的访问密码" +
                $"{Environment.NewLine}" +
                $"如果你想移除密码，则不需要输入任何文本，直接点击确定即可", password: true);
            if (input.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string newPassword = input.InputValue;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                while (true)
                {
                    input = new Input("设置密码", "请再次确认新的访问密码", password: true, required: true, errorMsg: "请输入密码");
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
            if (!await CheckVersion())
            {
                MessageBox.Show("当前客户端已是最新版本", "版本更新", MessageBoxButtons.OK);
            }
        }

        private async void versionLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!await CheckVersion())
            {
                MessageBox.Show("当前客户端已是最新版本", "版本更新", MessageBoxButtons.OK);
            }
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
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
                            Input input = new Input("绑定令牌", tips, required: true, errorMsg: "请输入验证码");
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
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

            var dialogResult = MessageBox.Show($"你正在移动你的Steam令牌到当前设备" +
                     $"{Environment.NewLine}" +
                     $"Steam将发送一条验证短信到你绑定的安全手机号" +
                    $"{Environment.NewLine}" +
                     $"移动令牌后你在48小时内产生的交易报价将被Steam暂挂，但是你可以正常进行交易" +
                     $"{Environment.NewLine}" +
                     $"你是否要继续移动你的Steam令牌？", "移动令牌", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
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
                                $"请输入你收到的手机验证码", required: true, errorMsg: "请输入验证码");
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
                                $"请输入你收到的手机验证码", required: true, errorMsg: "请输入验证码");
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
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
                    Input input = new Input("删除令牌", "请输入恢复码", required: true, errorMsg: "请输入恢复码");
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{webClient.Account} 未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmation = new Confirmations(webClient);
            confirmation.Show();
        }

        private void importFileAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "导入令牌",
                Filter = "令牌文件 (*.entry)|*.entry",
                DefaultExt = ".entry",
                InitialDirectory = initialDirectory ?? AppContext.BaseDirectory,
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
                    initialDirectory = fileInfo.DirectoryName;
                    try
                    {
                        using (FileStream stream = fileInfo.OpenRead())
                        {
                            bool encrypt = stream.ReadBoolean();
                            byte[] dataBuffer = new byte[0];

                            string password = null;
                            byte[] iv = [];
                            byte[] salt = [];
                            if (encrypt)
                            {
                                tips = $"请输入解密密码" +
                                    $"{Environment.NewLine}" +
                                    $"{fileInfo.Name}";

                                input = new Input($"导出令牌[{fileInfo.Name}]", tips, password: true, required: true, errorMsg: "请输入密码");
                                input.ShowDialog();
                                password = input.InputValue;

                                iv = new byte[stream.ReadInt32()];
                                stream.Read(iv);

                                salt = new byte[stream.ReadInt32()];
                                stream.Read(salt);
                            }

                            while (stream.Position != stream.Length)
                            {
                                dataBuffer = new byte[stream.ReadInt32()];
                                stream.Read(dataBuffer);
                                if (encrypt)
                                {
                                    dataBuffer = FileEncryptor.DecryptData(password, salt, iv, dataBuffer);
                                }

                                Guard guard = new Guard();
                                guard.Deserialize(new MemoryStream(dataBuffer));
                                guard = guard.Value;

                                Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);

                                success.Add($"{fileInfo.Name}[{guard.AccountName}]");
                            }
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

        private async void importSecretAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
                {
                    MessageBox.Show("请先登录你需要导入令牌的Steam帐号");
                    return;
                }

                var guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard != null)
                {
                    MessageBox.Show($"帐号 {webClient.Account} 已在当前设备绑定令牌" +
                        $"{Environment.NewLine}" +
                        $"如果当前设备的令牌信息已失效，请先前往 [令牌验证器 -> 令牌] 删除令牌",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                var authenticatorStatusResponse = authenticatorStatus.Body;
                if (authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
                {
                    MessageBox.Show($"帐号 {webClient.Account} 未绑定手机令牌" +
                        $"{Environment.NewLine}" +
                        $"你可以选择添加令牌为帐号 {webClient.Account} 添加令牌",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ImportAuthenticator importAuthenticator = new ImportAuthenticator(webClient);
                if (importAuthenticator.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                guard = new Guard
                {
                    AccountName = webClient.Account,
                    DeviceId = authenticatorStatusResponse.DeviceId,
                    TokenGID = authenticatorStatusResponse.TokenGID,
                    GuardScheme = authenticatorStatusResponse.GuardScheme,

                    SerialNumber = null,
                    URI = null,
                    Secret1 = null,

                    RevocationCode = importAuthenticator.RevocationCode,
                    IdentitySecret = importAuthenticator.IdentitySecret,
                    SharedSecret = importAuthenticator.SharedSecret,
                };

                if (!string.IsNullOrWhiteSpace(guard.SharedSecret))
                {
                    string code = GuardCodeGenerator.GenerateSteamGuardCode(await Extension.GetSteamTimestampAsync(), guard.SharedSecret);
                    var validateToken = await SteamAuthenticator.ValidateTokenAsync(webClient.WebApiToken, code);
                    if (!validateToken.Body.Valid)
                    {
                        MessageBox.Show($"你提供的登录秘钥似乎有误，请确认登录秘钥是否可用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(guard.IdentitySecret))
                {
                    var queryConfirmations = await webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret);
                    if (!queryConfirmations.Success)
                    {
                        MessageBox.Show($"你提供的身份秘钥似乎有误，请确认身份秘钥是否可用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                Appsetting.Instance.Manifest.AddGuard(webClient.Account, guard);

                MessageBox.Show($"令牌导入成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show($"你填写的秘钥格式有误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void offersBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
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
                acceptOfferBtn.Enabled = false;

                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
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
            finally
            {
                acceptOfferBtn.Enabled = true;
            }
        }

        private async void declineOfferBtn_Click(object sender, EventArgs e)
        {
            try
            {
                declineOfferBtn.Enabled = false;

                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
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
            finally
            {
                declineOfferBtn.Enabled = true;
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

            if (await userClient.LoginAsync())
            {
                return;
            }

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
            usersPanel.Controls.Remove(panel);
            ResetUserPanel();

            if (userClient.User.SteamId == currentClient?.User?.SteamId)
            {
                SetCurrentClient(Appsetting.Instance.Clients.FirstOrDefault() ?? userClient, true);
            }
        }

        private void buffAuthMenuItem_Click_1(object sender, EventArgs e)
        {
            var buffAuth = new BuffAuth("请扫码登录 BUFF 帐号");
            if (buffAuth.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var clients = Appsetting.Instance.Clients.Where(c => c.Client.SteamId == buffAuth.Result.Body.data.steamid);
            if (!clients.Any())
            {
                MessageBox.Show($"未在当前设备已登录的Steam帐号中找到你的BUFF帐号绑定的Steam账号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var item in clients)
            {
                item.BuffCookies = buffAuth.Result.Cookies;
            }
        }

        private void offersNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;

            Offers offersForm = new Offers(userClient.Client);
            offersForm.ShowDialog();
        }

        private void confirmationNumberBtn_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            UserPanel panel = control.Parent as UserPanel;
            UserClient userClient = panel.UserClient;
            var webClient = userClient.Client;

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{webClient.Account} 未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmations = new Confirmations(webClient);
            confirmations.ShowDialog();
        }

        private void RefreshClientMsg(object _)
        {
            var setting = Appsetting.Instance.AppSetting.Entry;

            try
            {
                List<Task> tasks = new List<Task>();

                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    tasks.Add(QueryAuthSessionsForAccount(tokenSource.Token));

                    tasks.Add(QueryOffers(tokenSource.Token));
                    tasks.Add(QueryConfirmations(tokenSource.Token));

                    tasks.Add(QueryWalletDetails(tokenSource.Token));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch
            {

            }
            finally
            {
                TimeSpan dueTime = TimeSpan.FromSeconds(Math.Max(1, setting.AutoRefreshInternal));
                TimeSpan period = TimeSpan.FromSeconds(Math.Max(timerMinPeriod.TotalSeconds, setting.AutoRefreshInternal * 2));
                ResetTimer(dueTime, period);
            }
        }

        private async Task QueryAuthSessionsForAccount(CancellationToken cancellationToken)
        {
            if (currentClient == null || !currentClient.LoginConfirmLocker.Wait(0))
            {
                return;
            }
            try
            {
                var webClient = currentClient.Client;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
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
                currentClient?.LoginConfirmLocker.Release();
            }
        }

        private async Task QueryOffers(CancellationToken cancellationToken)
        {
            try
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
                    if (client == null)
                    {
                        continue;
                    }

                    var task = Task.Run(() =>
                    {
                        try
                        {
                            var webClient = client.Client;

                            var queryOffers = webClient.TradeOffer.QueryOffersAsync(sentOffer: false, receivedOffer: true, onlyActive: true, cancellationToken: cancellationToken).Result;
                            var offers = queryOffers?.TradeOffersReceived ?? new List<Offer>();

                            if (client.User.SteamId == currentClient?.User.SteamId)
                            {
                                OfferCountLabel.Text = $"{offers.Count}";
                                OfferCountLabel.Tag = offers;
                            }

                            UserPanel userPanel = usersPanel.Controls.Find(webClient.SteamId, false).FirstOrDefault() as UserPanel;
                            if (userPanel != null)
                            {
                                Label offerLabel = userPanel.Controls.Find("offer", false).FirstOrDefault() as Label;
                                if (offerLabel != null)
                                {
                                    offerLabel.Text = $"{offers.Count}";
                                }
                            }

                            var receiveOffers = offers.Where(c => !(c.ItemsToGive?.Any() ?? false)).ToList();
                            var giveOffers = offers.Where(c => c.ItemsToGive?.Any() ?? false).ToList();

                            if (receiveOffers.Any() && setting.AutoAcceptReceiveOffer)
                            {
                                HandleOffer(webClient, receiveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }
                            if (giveOffers.Any(c => c.ConfirmationMethod == TradeOfferConfirmationMethod.Invalid) && setting.AutoAcceptGiveOffer)
                            {
                                HandleOffer(webClient, giveOffers, true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token).GetAwaiter().GetResult();
                            }
                        }
                        catch
                        {
                        }
                    });
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
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
                if (client == null)
                {
                    continue;
                }

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
                        if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                        {
                            return;
                        }

                        var queryConfirmations = webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret, cancellationToken).Result;
                        if (!queryConfirmations.Success)
                        {
                            return;
                        }

                        var confirmations = queryConfirmations.Confirmations ?? new List<Confirmation>();
                        if (client.User.SteamId == currentClient?.User.SteamId)
                        {
                            ConfirmationCountLable.Text = $"{confirmations.Count}";
                        }

                        UserPanel userPanel = usersPanel.Controls.Find(webClient.SteamId, false).FirstOrDefault() as UserPanel;
                        if (userPanel != null)
                        {
                            Label confirmationLabel = userPanel.Controls.Find("confirmation", false).FirstOrDefault() as Label;
                            if (confirmationLabel != null)
                            {
                                confirmationLabel.Text = $"{confirmations.Count}";
                            }
                        }

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

                        if (waitConfirm.Any() && setting.ConfirmationAutoPopup)
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
            if (currentClient == null)
            {
                return;
            }

            try
            {
                var webClient = currentClient.Client;

                var walletDetails = await webClient.User.QueryWalletDetailsAsync(cancellationToken);

                if (walletDetails?.HasWallet ?? false)
                {
                    Balance.Text = $"{walletDetails.FormattedBalance}";

                    DelayedBalance.Text = "￥0.00";
                    if (!string.IsNullOrWhiteSpace(walletDetails.FormattedDelayedBalance))
                    {
                        DelayedBalance.Text = $"{walletDetails.FormattedDelayedBalance}";
                    }
                }
            }
            catch
            {
            }
        }

        private void RefreshUser(object _)
        {
            try
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    var controlCollection = usersPanel.Controls.Cast<UserPanel>().ToArray();
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

                        var palyerSummaries = SteamApi.QueryPlayerSummariesAsync(null, client.WebApiToken, new[] { client.SteamId }, cancellationToken: tokenSource.Token).GetAwaiter().GetResult();
                        if (palyerSummaries.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            client.LogoutAsync().GetAwaiter().GetResult();
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
            }
            catch
            {

            }
            finally
            {
                ResetRefreshUserTimer(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"切换用户失败{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            Text = $"Steam验证器";

            copyCookieMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Enabled = false;
            copyAccessTokenToolItem.Enabled = false;

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            Balance.Text = "￥0.00";
            DelayedBalance.Text = "￥0.00";

            OfferCountLabel.Text = "---";
            ConfirmationCountLable.Text = "---";

            if (userClient?.Client?.LoggedIn ?? false)
            {
                Text = $"Steam验证器 {userClient.User.Account}[{userClient.User.NickName}]";

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
                DelayedBalance.Text = "￥0.00";
            }

            currentClient = userClient;
            Appsetting.Instance.AppSetting.Entry.CurrentUser = currentClient.User.SteamId;
            Appsetting.Instance.AppSetting.Save();
        }

        private void ResetTimer(TimeSpan dueTime, TimeSpan period)
        {
            timer.Change(dueTime, period);
        }

        private void ResetRefreshUserTimer(TimeSpan dueTime, TimeSpan period)
        {
            refreshUserTimer.Change(dueTime, period);
        }

        private async Task LoadUsers()
        {
            try
            {
                usersPanel.Controls.Clear();

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
                        usersPanel.Controls.Add(panel);

                        index++;

                        Appsetting.Instance.Clients.Add(panel.UserClient);
                    }
                }

                {
                    UserPanel panel = new UserPanel()
                    {
                        Size = new Size(80, 116),
                        Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                        UserClient = UserClient.None
                    };

                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = 80,
                        Height = 80,
                        Location = new Point(0, 0),
                        Cursor = Cursors.Hand,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    pictureBox.Image = Properties.Resources.add;
                    pictureBox.Click += addUserBtn_Click;
                    panel.Controls.Add(pictureBox);

                    Label nameLabel = new Label()
                    {
                        Text = $"添加帐号",
                        AutoSize = false,
                        AutoEllipsis = true,
                        Cursor = Cursors.Hand,
                        Size = new Size(80, 18),
                        TextAlign = ContentAlignment.TopCenter,
                        ForeColor = Color.FromArgb(244, 164, 96),
                        Location = new Point(0, 80)
                    };
                    nameLabel.Click += addUserBtn_Click;
                    panel.Controls.Add(nameLabel);
                    usersPanel.Controls.Add(panel);
                }

                var tasks = Appsetting.Instance.Clients.Select(c => c.LoginAsync());
                await Task.WhenAll(tasks);
            }
            catch
            {

            }
            finally
            {
                ResetTimer(TimeSpan.Zero, timerMinPeriod);
                ResetRefreshUserTimer(TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }
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

                var controlCollection = usersPanel.Controls.Cast<UserPanel>().ToList();
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

                usersPanel.Controls.Clear();
                usersPanel.Controls.AddRange(controlCollection.ToArray());
                ResetUserPanel();

                return panel.UserClient;
            }

            return null;
        }

        private void ResetUserPanel()
        {
            try
            {
                var controlCollection = usersPanel.Controls.Cast<Control>().ToArray();

                int x = GetUserControlStartPointX(out int cells);

                int index = 0;
                foreach (Control control in controlCollection)
                {
                    control.Location = new Point(x * (index % cells) + 10, 126 * (index / cells) + 10);
                    index++;
                }

                usersPanel.Controls.Clear();
                usersPanel.Controls.AddRange(controlCollection);
            }
            catch
            {

            }
        }

        private int GetUserControlStartPointX(out int cells)
        {
            cells = (usersPanel.Size.Width - 30) / 80;
            int size = (usersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            if (size < 85)
            {
                cells = cells - 1;
                size = (usersPanel.Size.Width - 30 - cells * 80) / (cells - 1) + 80;
            }
            return size;
        }

        private UserPanel CreateUserPanel(int startX, int cells, int index, UserClient userClient)
        {
            userClient.Client.SetLanguage(Language.Schinese);
            UserPanel panel = new UserPanel()
            {
                Name = userClient.User.SteamId,
                Size = new Size(80, 116),
                Location = new Point(startX * (index % cells) + 10, 126 * (index / cells) + 10),
                UserClient = userClient
            };

            PictureBox pictureBox = new PictureBox()
            {
                Name = "useravatar",
                Width = 80,
                Height = 80,
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.Zoom,
                InitialImage = Properties.Resources.loading,
            };
            string avatar = userClient.User.Avatar;
            pictureBox.Image = Properties.Resources.userimg;
            if (!string.IsNullOrEmpty(avatar))
            {
                pictureBox.LoadAsync(avatar);
            }
            pictureBox.MouseClick += btnUser_Click;
            pictureBox.ContextMenuStrip = contextMenuStrip;
            panel.Controls.Add(pictureBox);

            Label nameLabel = new Label()
            {
                Name = "username",
                Text = $"{userClient.User.Account} [{userClient.User.NickName}]",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(80, 18),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(128, 128, 128),
                Location = new Point(0, 80)
            };
            nameLabel.MouseClick += btnUser_Click;
            nameLabel.ContextMenuStrip = contextMenuStrip;
            panel.Controls.Add(nameLabel);

            Label offerLabel = new Label()
            {
                Name = "offer",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopRight,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(255, 128, 0),
                Location = new Point(0, 98)
            };
            offerLabel.Click += offersNumberBtn_Click;
            panel.Controls.Add(offerLabel);

            Label confirmationLabel = new Label()
            {
                Name = "confirmation",
                Text = $"---",
                AutoSize = false,
                AutoEllipsis = true,
                Cursor = Cursors.Hand,
                Size = new Size(38, 18),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = userClient.Client.LoggedIn ? Color.Green : Color.FromArgb(0, 128, 255),
                Location = new Point(42, 98)
            };
            confirmationLabel.Click += confirmationNumberBtn_Click;
            panel.Controls.Add(confirmationLabel);

            panel.UserClient
                .WithStartLogin(() => nameLabel.ForeColor = Color.FromArgb(128, 128, 128))
                .WithEndLogin(loggined =>
                {
                    nameLabel.ForeColor = loggined ? Color.Green : Color.Red;
                });

            return panel;
        }

        private async Task<bool> CheckVersion()
        {
            if (!checkVersionLocker.Wait(0))
            {
                return true;
            }

            try
            {
                var result = await SteamApi.GetAsync<JObject>("https://api.github.com/repos/tsxlts/SteamAuthenticator/releases/latest");
                var resultObj = result.Body;
                if (!(resultObj?.TryGetValue("tag_name", out var tag_name) ?? false))
                {
                    return false;
                }

                var match = Regex.Match(tag_name.Value<string>(), @"[\d.]+");
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
                    return true;
                }
            }
            catch
            {

            }
            finally
            {
                checkVersionLocker.Release();
            }

            return false;
        }
    }
}


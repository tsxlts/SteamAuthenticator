using Newtonsoft.Json;
using Steam_Authenticator.Forms;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.Other;
using SteamKit;
using SteamKit.Model;
using System.Diagnostics;
using static Steam_Authenticator.Internal.Utils;
using static SteamKit.SteamEnum;

namespace Steam_Authenticator
{
    public partial class MainForm
    {

        private void SteamId_Click(object sender, EventArgs e)
        {
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
            {
                return;
            }

            Clipboard.SetText(webClient.SteamId);
            Process.Start(new ProcessStartInfo($"{Appsetting.Instance.AppSetting.Entry.Domain.SteamCommunity}/profiles/{webClient.SteamId}") { UseShellExecute = true });
        }

        private void globalSettingMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }

        private void proxySettingMenuItem_Click(object sender, EventArgs e)
        {
            ProxySetting proxySetting = new ProxySetting();
            proxySetting.ShowDialog();
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

            Appsetting.Instance.AppSetting.Password = newPassword;
            if (!Appsetting.Instance.Manifest.ChangePassword(olaPassword, newPassword))
            {
                Appsetting.Instance.AppSetting.Password = olaPassword;

                MessageBox.Show("设置密码失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(Appsetting.Instance.Manifest.Encrypted ? "密码设置成功" : "已移除访问密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void checkVersionMenuItem_Click(object sender, EventArgs e)
        {
            if (!await CheckVersion())
            {
                MessageBox.Show("当前客户端已是最新版本", "版本更新", MessageBoxButtons.OK);
            }
        }

        private void quitMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void guardMenuItem_Click(object sender, EventArgs e)
        {
            StreamGuard streamGuard = new StreamGuard(currentClient?.GetAccount());
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
                MessageBox.Show($"{currentClient.GetAccount()} 已绑定令牌" +
                    $"{Environment.NewLine}" +
                    $"如果你已在其他地方绑定令牌，你可以选择移动令牌验证器到当前设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(currentClient.GetAccount()))
            {
                MessageBox.Show("获取你的登陆账户名失败");
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
                            string country = input.CountryCode;
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
                MessageBox.Show($"{currentClient.GetAccount()} 未绑定令牌" +
                    $"{Environment.NewLine}" +
                    $"你可以直接添加令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(currentClient.GetAccount()))
            {
                MessageBox.Show("获取你的登陆账户名失败");
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(currentClient.GetAccount());
            if (authenticatorStatusResponse.TokenGID == guard?.TokenGID)
            {
                MessageBox.Show($"{currentClient.GetAccount()} 令牌验证器已绑定至此设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dialogResult = MessageBox.Show($"你正在移动你的Steam令牌到当前设备" +
                     $"{Environment.NewLine}" +
                     $"Steam将发送一条验证短信到你绑定的安全手机号" +
                    $"{Environment.NewLine}" +
                     $"移动令牌后你在48小时内不能使用交易报价功能" +
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
                            string country = phoneInput.CountryCode;
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
                MessageBox.Show($"{currentClient.GetAccount()} 未设置令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SteamGuardScheme? guardScheme = null;
            string code = null;
            string msg = null;

            DialogResult res = MessageBox.Show($"{currentClient.GetAccount()}\n" +
                $"你想要完全移除Steam令牌码？\n" +
                $"是 - 完全移除Steam令牌\n" +
                $"否 - 切换到邮箱令牌",
                "提示",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

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
                var guard = Appsetting.Instance.Manifest.GetGuard(currentClient.GetAccount());
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

            if (MessageBox.Show($"{currentClient.GetAccount()}\n" +
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
                    Appsetting.Instance.Manifest.RemoveGuard(currentClient.GetAccount(), out var guard);
                }
            }
        }

        private void importFileAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "导入令牌",
                Filter = $"令牌文件 (*.entry;*.saEntry;*.maFile)|*.entry;*.saEntry;*.maFile" +
                $"|SA文件 (*.entry;*.saEntry;)|*.entry;*.saEntry" +
                $"|maFile文件 (*.maFile)|*.maFile",
                InitialDirectory = Appsetting.Instance.AppSetting.Entry.InitialDirectory ?? AppContext.BaseDirectory,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                return;
            }

            Appsetting.Instance.AppSetting.Entry.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
            Appsetting.Instance.AppSetting.Save();

            FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
            try
            {
                List<string> success = new List<string>();

                bool isSAFile = fileInfo.Extension.Equals(".saEntry", StringComparison.OrdinalIgnoreCase);
                isSAFile = isSAFile || fileInfo.Extension.Equals(".entry", StringComparison.OrdinalIgnoreCase);

                bool isMaFile = !isSAFile && fileInfo.Extension.Equals(".maFile", StringComparison.OrdinalIgnoreCase);

                if (isSAFile)
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
                            string tips = $"请输入解密密码" +
                                $"{Environment.NewLine}" +
                                $"{fileInfo.Name}";

                            Input input = new Input($"导出令牌[{fileInfo.Name}]", tips, password: true, required: true, errorMsg: "请输入密码");
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
                            Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);

                            success.Add(guard.AccountName);
                        }
                    }
                }

                if (isMaFile)
                {
                    using (FileStream stream = fileInfo.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string json = reader.ReadToEnd();
                            MaFile maFile = JsonConvert.DeserializeObject<MaFile>(json);
                            if (string.IsNullOrWhiteSpace(maFile.account_name))
                            {
                                throw new FileFormatException("account_name missing");
                            }
                            if (string.IsNullOrWhiteSpace(maFile.device_id))
                            {
                                throw new FileFormatException("device_id missing");
                            }
                            if (string.IsNullOrWhiteSpace(maFile.shared_secret))
                            {
                                throw new FileFormatException("shared_secret missing");
                            }
                            if (string.IsNullOrWhiteSpace(maFile.identity_secret))
                            {
                                throw new FileFormatException("identity_secret missing");
                            }

                            Guard guard = new Guard
                            {
                                AccountName = maFile.account_name,
                                DeviceId = maFile.device_id,
                                SharedSecret = maFile.shared_secret,
                                IdentitySecret = maFile.identity_secret,
                                Secret1 = maFile.secret1,
                                RevocationCode = maFile.revocation_code,
                                GuardScheme = SteamGuardScheme.Device,
                                SerialNumber = maFile.serial_number,
                                TokenGID = maFile.token_gid,
                                URI = maFile.uri,
                            };
                            Appsetting.Instance.Manifest.AddGuard(guard.AccountName, guard);

                            success.Add(guard.AccountName);
                        }
                    }
                }

                MessageBox.Show($"令牌导入结果" +
                    $"{Environment.NewLine}" +
                    $"成功导入令牌 {success.Count} 个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{fileInfo.FullName}" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            string account = currentClient?.GetAccount();
            ExportGuardOptions exportGuardOptions = new ExportGuardOptions(account);
            exportGuardOptions.ShowDialog();
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

                if (string.IsNullOrWhiteSpace(currentClient.GetAccount()))
                {
                    MessageBox.Show("获取你的登陆账户名失败");
                    return;
                }

                var guard = Appsetting.Instance.Manifest.GetGuard(currentClient.GetAccount());
                if (guard != null)
                {
                    MessageBox.Show($"帐号 {currentClient.GetAccount()} 已在当前设备绑定令牌" +
                        $"{Environment.NewLine}" +
                        $"如果当前设备的令牌信息已失效，请先前往 [令牌验证器 -> 令牌] 删除令牌",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                var authenticatorStatusResponse = authenticatorStatus.Body;
                if (authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
                {
                    MessageBox.Show($"帐号 {currentClient.GetAccount()} 未绑定手机令牌" +
                        $"{Environment.NewLine}" +
                        $"你可以选择添加令牌为帐号 {currentClient.GetAccount()} 添加令牌",
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ImportAuthenticator importAuthenticator = new ImportAuthenticator(currentClient);
                if (importAuthenticator.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                guard = new Guard
                {
                    AccountName = currentClient.GetAccount(),
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

                Appsetting.Instance.Manifest.AddGuard(currentClient.GetAccount(), guard);

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

        private void submitRequirementsMenuItem_Click(object sender, EventArgs e)
        {
            var submitRequirements = new SubmitRequirements(this);
            submitRequirements.ShowDialog();
        }

        private async void submitBugMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RichTextInput richTextInput = new RichTextInput("反馈问题", "请输入您遇到的问题", true, "请输入您遇到的问题");
                richTextInput.ShowDialog();
                string body = richTextInput.InputValue;
                if (string.IsNullOrWhiteSpace(body))
                {
                    return;
                }

                var response = await AuthenticatorApi.SubmitBug(currentVersion, body);
                if (response == null)
                {
                    MessageBox.Show($"提交失败，请前往SteamAuthenticator项目主页反馈问题", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(new ProcessStartInfo(ProjectInfo.Issues) { UseShellExecute = true });
                    DialogResult = DialogResult.OK;
                    return;
                }

                if (!response.IsSuccess())
                {
                    MessageBox.Show($"提交失败{Environment.NewLine}{response.ResultMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show(response?.ResultData?.Msg ?? $"提交成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交失败{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About(currentVersion);
            about.ShowDialog();
        }

        private void confirmationBtn_Click(object sender, EventArgs e)
        {
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(currentClient.GetAccount());
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{currentClient.GetAccount()} 未提供令牌信息，无法获取待确认数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmation = new Confirmations(this, currentClient);
            confirmation.Show();
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

                Offers offersForm = new Offers(this, currentClient);
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

                IEnumerable<Offer> offers = currentClient.ReceivedOffers;
                if (offers == null || !offers.Any(c => !c.IsOurOffer))
                {
                    return;
                }

                if (MessageBox.Show("你确定要接受所有报价吗？", "接受报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                await HandleOffer(webClient, offers.Where(c => !c.IsOurOffer), true, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
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

                IEnumerable<Offer> offers = currentClient.ReceivedOffers;
                if (offers == null || !offers.Any(c => !c.IsOurOffer))
                {
                    return;
                }

                if (MessageBox.Show("你确定要拒绝所有报价吗？", "拒绝报价", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                await HandleOffer(webClient, offers.Where(c => !c.IsOurOffer), false, new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token);
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
    }
}

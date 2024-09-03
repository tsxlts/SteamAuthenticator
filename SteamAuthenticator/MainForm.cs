
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
                string passwordTips = "������ԭ����";
                Input passwordInput;
                while (true)
                {
                    passwordInput = new Input("��������", passwordTips, true);
                    if (passwordInput.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    olaPassword = passwordInput.InputValue;
                    if (!Appsetting.Instance.Manifest.CheckPassword(olaPassword))
                    {
                        passwordTips = "�����������������";
                        continue;
                    }

                    break;
                }
            }

            Input input = new Input("��������", "�������µķ�������", true);
            if (input.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string newPassword = input.InputValue;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                while (true)
                {
                    input = new Input("��������", "���ٴ�ȷ���µķ�������", true);
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    if (newPassword != input.InputValue)
                    {
                        MessageBox.Show("�������벻һ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    break;
                }
            }

            if (!Appsetting.Instance.Manifest.ChangePassword(olaPassword, newPassword))
            {
                MessageBox.Show("��������ʧ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Appsetting.Instance.AppSetting.Password = newPassword;
            MessageBox.Show(Appsetting.Instance.Manifest.Encrypted ? "�������óɹ�" : "���Ƴ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("���ȵ�¼Steam�ʺ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme == SteamGuardScheme.Device)
            {
                MessageBox.Show($"��ǰ�ʺ��Ѱ�����" +
                    $"{Environment.NewLine}" +
                    $"��������������ط������ƣ������ѡ���ƶ�����������֤������ǰ�豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            while (true)
            {
                var addAuthenticator = await SteamAuthenticator.AddAuthenticatorAsync(webClient.WebApiToken, webClient.SteamId, Extension.GetDeviceId(webClient.SteamId));
                if (addAuthenticator.ResultCode != SteamKit.Model.ErrorCodes.OK)
                {
                    MessageBox.Show($"������ʧ��[{addAuthenticator.ResultCode}]", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var addAuthenticatorResponse = addAuthenticator.Body;
                if (addAuthenticator == null)
                {
                    MessageBox.Show($"������ʧ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Guard guard = null;

                switch (addAuthenticatorResponse.Status)
                {
                    case AddAuthenticatorStatus.MustProvidePhoneNumber:
                        {
                            PhoneInput input = new PhoneInput("����ֻ���", $"�������ֻ���" +
                                $"{Environment.NewLine}" +
                                $"����:+86 13100000000");
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
                                if (MessageBox.Show($"���������({setAccountPhone.Body?.ConfirmationEmailAddress})ȷ��",
                                    "��ʾ",
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
                            string tips = "�����������֤��";
                            switch (addAuthenticatorResponse.ConfirmType)
                            {
                                case AddAuthenticatorConfirmType.SmsCode:
                                    tips = "�����������֤��";
                                    break;

                                case AddAuthenticatorConfirmType.EmailCode:
                                    tips = "������������֤��";
                                    break;
                            }
                            Input input = new Input("������֤��", tips);
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
                                    MessageBox.Show($"������֤���������������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    MessageBox.Show($"������ӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                            }
                        }
                        break;

                    case AddAuthenticatorStatus.AuthenticatorPresent:
                        {
                            MessageBox.Show($"���Ѱ�Steam����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    default:
                        MessageBox.Show($"������ʧ��[{addAuthenticatorResponse?.Status}]", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }
        }

        private async void moveAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            var webClient = Appsetting.Instance.CurrentClient;
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("���ȵ�¼Steam�ʺ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
            {
                MessageBox.Show($"��ǰ�ʺ�δ������" +
                    $"{Environment.NewLine}" +
                    $"�����ֱ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(Appsetting.Instance.CurrentClient.Account);
            if (authenticatorStatusResponse.TokenGID == guard?.TokenGID)
            {
                MessageBox.Show($"��ǰ�ʺ�������֤���Ѱ������豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            DialogResult res = MessageBox.Show($"��⵽��ǰSteam�ʺ�δ���ֻ���" +
                            $"{Environment.NewLine}" +
                            $"����Ҫ�Ȱ��ֻ��Ų��ܼ����ƶ����ƣ��Ƿ�������ֻ��ţ�", "��ʾ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (res != DialogResult.Yes)
                            {
                                return;
                            }

                            PhoneInput phoneInput = new PhoneInput("����ֻ���", $"�������ֻ���" +
                                $"{Environment.NewLine}" +
                                $"����:+86 13100000000");
                            if (phoneInput.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            string phone = phoneInput.Phone;
                            string country = phoneInput.Country;
                            var setAccountPhone = await SteamApi.SetAccountPhoneNumberAsync(webClient.WebApiToken, phone, country);
                            if (string.IsNullOrWhiteSpace(setAccountPhone.Body?.ConfirmationEmailAddress))
                            {
                                MessageBox.Show($"����ֻ���ʧ��",
                                    "��ʾ",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            moveAuthenticatorResult = MoveAuthenticatorResult.WaitEmailConfirm;
                        }
                        break;

                    case MoveAuthenticatorResult.WaitEmailConfirm:
                        {
                            MessageBox.Show($"���������ȷ��",
                                "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            var waitingForEmailConfirmation = await SteamApi.IsAccountWaitingForEmailConfirmationAsync(webClient.WebApiToken);
                            if (waitingForEmailConfirmation.Body.AwaitingEmailConfirmation)
                            {
                                await Task.Delay(1000);
                                break;
                            }

                            var sendSmsCode = await SteamApi.SendPhoneVerificationCodeAsync(webClient.WebApiToken);
                            if (sendSmsCode.ResultCode != SteamKit.Model.ErrorCodes.OK)
                            {
                                MessageBox.Show($"������֤��ʧ��,{sendSmsCode.ResultCode}");
                                return;
                            }

                            moveAuthenticatorResult = MoveAuthenticatorResult.WaitFinalizationAddPhone;
                        }
                        break;

                    case MoveAuthenticatorResult.WaitFinalizationAddPhone:
                        {
                            Input input = new Input("����ֻ���", $"�����ڰ��ֻ���, ��֤�����ѷ���������ֻ�," +
                                $"{Environment.NewLine}" +
                                $"���������յ����ֻ���֤��");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                if (MessageBox.Show("�Ƿ�Ҫ�˳������ƣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                                    MessageBox.Show($"��֤�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;

                                default:
                                    MessageBox.Show($"����ֻ���ʧ��,{finalizationAddPhone.ResultCode}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                            }

                            MessageBox.Show($"����ֻ��ųɹ�������Ϊ���ƶ�������֤��");
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
                                    MessageBox.Show($"������֤��̫Ƶ���ˣ����Ժ�����");
                                    return;

                                default:
                                    MessageBox.Show($"������֤��ʧ��,{beginMoveAuthenticator.ResultCode}");
                                    return;
                            }
                        }
                        break;

                    case MoveAuthenticatorResult.WaitFinalization:
                        {
                            Input input = new Input("�ƶ�������֤��", $"�������ƶ�������֤��, ��֤�����ѷ���������ֻ�" +
                                $"{Environment.NewLine}" +
                                $"���������յ����ֻ���֤��");
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
                                        MessageBox.Show($"��֤�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;

                                    default:
                                        MessageBox.Show($"�ƶ�������֤��ʧ��,{finalizeMoveAuthenticator.ResultCode}");
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
                            MessageBox.Show($"������ӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("���ȵ�¼Steam�ʺ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
            var authenticatorStatusResponse = authenticatorStatus.Body;
            if (authenticatorStatusResponse.GuardScheme == SteamGuardScheme.None)
            {
                MessageBox.Show("��ǰ�ʺ�δ��������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SteamGuardScheme? guardScheme = null;
            string code = null;
            string msg = null;

            DialogResult res = MessageBox.Show($"{webClient.Account}\n" +
                $"����Ҫ��ȫ�Ƴ�Steam�����룿\n" +
                $"�� - ��ȫ�Ƴ�Steam����\n" +
                $"�� - �л���������֤",
                "��ʾ",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (res)
            {
                case DialogResult.Yes:
                    guardScheme = SteamGuardScheme.None;
                    msg = "�������Ƴ�";
                    break;

                case DialogResult.No:
                    guardScheme = SteamGuardScheme.Email;
                    msg = "���л�����������";
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
                    Input input = new Input("���볷����", "�����볷����");
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    code = input.InputValue;
                }
            }

            if (MessageBox.Show($"{webClient.Account}\n" +
                $"��ȷ��Ҫ{(guardScheme == SteamGuardScheme.None ?
                "�Ƴ�ȫ��������" :
                "�л�������������")}",
                "����", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var removeAuthenticator = await SteamAuthenticator.RemoveAuthenticatorAsync(webClient.WebApiToken, guardScheme.Value, code);

                if (removeAuthenticator.ResultCode == SteamKit.Model.ErrorCodes.OK)
                {
                    MessageBox.Show(msg, "��ʾ");
                    return;
                }

                if (removeAuthenticator.ResultCode == SteamKit.Model.ErrorCodes.Pending)
                {
                    while (true)
                    {
                        if (MessageBox.Show($"���������ȷ��",
                            "��ʾ",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                        {
                            return;
                        }

                        authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                        authenticatorStatusResponse = authenticatorStatus.Body;
                        if (authenticatorStatusResponse?.GuardScheme == SteamGuardScheme.None)
                        {
                            MessageBox.Show(msg, "��ʾ");
                            break;
                        }

                        await Task.Delay(1000);
                    }

                    return;
                }

                MessageBox.Show($"����ʧ��[{removeAuthenticator.ResultCode}]", "��ʾ");
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
                MessageBox.Show("���ȵ�¼Steam�ʺ�");
                return;
            }

            Forms.Confirmation confirmation = new Forms.Confirmation(Appsetting.Instance.CurrentClient);
            confirmation.Show();
        }

        private void importAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "��������",
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
                        MessageBox.Show($"�ļ���ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        error.Add(fileInfo.Name);
                    }
                }

                MessageBox.Show($"���Ƶ�����" +
                    $"{Environment.NewLine}" +
                    $"����ɹ�{success.Count}��{Environment.NewLine}{(success.Any() ? string.Join(Environment.NewLine, success) : "")}" +
                    $"{Environment.NewLine}" +
                    $"����ʧ��{error.Count}��{Environment.NewLine}{(error.Any() ? string.Join(Environment.NewLine, error) : "")}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                UserName.Text = "���ڵ�¼...";
                Balance.Text = "��0.00";

                await Appsetting.Instance.CurrentClient.LoginAsync(token);

                await ResetUser();
            }
            catch
            {
                UserName.ForeColor = Color.Black;
                UserName.Text = "---";
                Balance.Text = "��0.00";
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
                        var platform when platform == AuthTokenPlatformType.WebBrowser => "��ҳ�����",
                        _ => "δ֪�豸"
                    };
                    var regions = new[] { sessionInfo.Country, sessionInfo.State, sessionInfo.City }.Where(c => !string.IsNullOrWhiteSpace(c));

                    MobileConfirmationLogin mobileConfirmationLogin = new MobileConfirmationLogin(webClient, (ulong)clients[0], sessionInfo.Version);
                    mobileConfirmationLogin.ConfirmLoginTitle.Text = $"{webClient.SteamId} ���µĵ�¼����";
                    mobileConfirmationLogin.ConfirmLoginClientType.Text = clientType;
                    mobileConfirmationLogin.ConfirmLoginIP.Text = $"IP ��ַ��{sessionInfo.IP}";
                    mobileConfirmationLogin.ConfirmLoginRegion.Text = $"{string.Join("��", regions)}";

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

            Text = $"Steam��֤��";
            loginMenuItem.Text = "��¼";

            copyCookieMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Enabled = false;
            copyAccessTokenToolItem.Enabled = false;

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            Balance.Text = "��0.00";

            OfferCountLabel.Text = "0";
            ConfirmationCountLable.Text = "0";

            await Task.Run(() =>
            {
                if (Appsetting.Instance.CurrentClient.LoggedIn)
                {
                    Text = $"Steam��֤��[{Appsetting.Instance.CurrentClient.Account}]";
                    loginMenuItem.Text = "�л��ʺ�";

                    copyCookieMenuItem.Enabled = true;
                    copyRefreshTokenMenuItem.Enabled = true;
                    copyAccessTokenToolItem.Enabled = true;

                    UserImg.Image = Properties.Resources.userimg;
                    UserName.ForeColor = Color.Green;
                    UserName.Text = Appsetting.Instance.CurrentClient.SteamId;
                    Balance.Text = "��0.00";

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


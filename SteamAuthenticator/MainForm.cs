
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
            contextMenuStrip.Items.Add("�л�").Click += setCurrentClientMenuItem_Click;
            contextMenuStrip.Items.Add("���µ�¼").Click += loginMenuItem_Click;
            contextMenuStrip.Items.Add("�˳���¼").Click += removeUserMenuItem_Click;
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
                string passwordTips = "������ԭ����";
                Input passwordInput;
                while (true)
                {
                    passwordInput = new Input("��������", passwordTips, password: true, required: true, errorMsg: "����������");
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

            Input input = new Input("��������", $"�������µķ�������" +
                $"{Environment.NewLine}" +
                $"��������Ƴ����룬����Ҫ�����κ��ı���ֱ�ӵ��ȷ������", password: true);
            if (input.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string newPassword = input.InputValue;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                while (true)
                {
                    input = new Input("��������", "���ٴ�ȷ���µķ�������", password: true, required: true, errorMsg: "����������");
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

        private async void checkVersionMenuItem_Click(object sender, EventArgs e)
        {
            if (!await CheckVersion())
            {
                MessageBox.Show("��ǰ�ͻ����������°汾", "�汾����", MessageBoxButtons.OK);
            }
        }

        private async void versionLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!await CheckVersion())
            {
                MessageBox.Show("��ǰ�ͻ����������°汾", "�汾����", MessageBoxButtons.OK);
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
                                    if (MessageBox.Show("�Ƿ�Ҫ�˳������ƣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            Input input = new Input("������", tips, required: true, errorMsg: "��������֤��");
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
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

            Guard guard = Appsetting.Instance.Manifest.GetGuard(currentClient.Client.Account);
            if (authenticatorStatusResponse.TokenGID == guard?.TokenGID)
            {
                MessageBox.Show($"��ǰ�ʺ�������֤���Ѱ������豸", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dialogResult = MessageBox.Show($"�������ƶ����Steam���Ƶ���ǰ�豸" +
                     $"{Environment.NewLine}" +
                     $"Steam������һ����֤���ŵ���󶨵İ�ȫ�ֻ���" +
                    $"{Environment.NewLine}" +
                     $"�ƶ����ƺ�����48Сʱ�ڲ����Ľ��ױ��۽���Steam�ݹң�����������������н���" +
                     $"{Environment.NewLine}" +
                     $"���Ƿ�Ҫ�����ƶ����Steam���ƣ�", "�ƶ�����", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                                $"���������յ����ֻ���֤��", required: true, errorMsg: "��������֤��");
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
                                $"���������յ����ֻ���֤��", required: true, errorMsg: "��������֤��");
                            if (input.ShowDialog() != DialogResult.OK)
                            {
                                if (MessageBox.Show("�Ƿ�Ҫ�˳������ƣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
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
                    Input input = new Input("ɾ������", "������ָ���", required: true, errorMsg: "������ָ���");
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
            var webClient = currentClient?.Client;
            if (webClient == null || !webClient.LoggedIn)
            {
                MessageBox.Show("���ȵ�¼Steam�ʺ�");
                return;
            }

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
            {
                MessageBox.Show($"{webClient.Account} δ�ṩ������Ϣ���޷���ȡ��ȷ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Confirmations confirmation = new Confirmations(webClient);
            confirmation.Show();
        }

        private void importFileAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "��������",
                Filter = "�����ļ� (*.entry)|*.entry",
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
                string tips = "�������������";
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
                                tips = $"�������������" +
                                    $"{Environment.NewLine}" +
                                    $"{fileInfo.Name}";

                                input = new Input($"��������[{fileInfo.Name}]", tips, password: true, required: true, errorMsg: "����������");
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
                            $"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void importSecretAuthenticatorMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
                {
                    MessageBox.Show("���ȵ�¼����Ҫ�������Ƶ�Steam�ʺ�");
                    return;
                }

                var guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard != null)
                {
                    MessageBox.Show($"�ʺ� {webClient.Account} ���ڵ�ǰ�豸������" +
                        $"{Environment.NewLine}" +
                        $"�����ǰ�豸��������Ϣ��ʧЧ������ǰ�� [������֤�� -> ����] ɾ������",
                        "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var authenticatorStatus = await SteamAuthenticator.QueryAuthenticatorStatusAsync(webClient.WebApiToken, webClient.SteamId);
                var authenticatorStatusResponse = authenticatorStatus.Body;
                if (authenticatorStatusResponse.GuardScheme != SteamGuardScheme.Device)
                {
                    MessageBox.Show($"�ʺ� {webClient.Account} δ���ֻ�����" +
                        $"{Environment.NewLine}" +
                        $"�����ѡ���������Ϊ�ʺ� {webClient.Account} �������",
                        "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show($"���ṩ�ĵ�¼��Կ�ƺ�������ȷ�ϵ�¼��Կ�Ƿ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(guard.IdentitySecret))
                {
                    var queryConfirmations = await webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret);
                    if (!queryConfirmations.Success)
                    {
                        MessageBox.Show($"���ṩ�������Կ�ƺ�������ȷ�������Կ�Ƿ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                Appsetting.Instance.Manifest.AddGuard(webClient.Account, guard);

                MessageBox.Show($"���Ƶ���ɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show($"����д����Կ��ʽ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void offersBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var webClient = currentClient?.Client;
                if (webClient == null || !webClient.LoggedIn)
                {
                    MessageBox.Show("���ȵ�¼Steam�ʺ�");
                    return;
                }

                Offers offersForm = new Offers(webClient);
                offersForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("���ȵ�¼Steam�ʺ�");
                    return;
                }

                if (MessageBox.Show("��ȷ��Ҫ�������б�����", "���ܱ���", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                MessageBox.Show($"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("���ȵ�¼Steam�ʺ�");
                    return;
                }

                if (MessageBox.Show("��ȷ��Ҫ�ܾ����б�����", "�ܾ�����", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                MessageBox.Show($"{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var buffAuth = new BuffAuth("��ɨ���¼ BUFF �ʺ�");
            if (buffAuth.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var clients = Appsetting.Instance.Clients.Where(c => c.Client.SteamId == buffAuth.Result.Body.data.steamid);
            if (!clients.Any())
            {
                MessageBox.Show($"δ�ڵ�ǰ�豸�ѵ�¼��Steam�ʺ����ҵ����BUFF�ʺŰ󶨵�Steam�˺�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"{webClient.Account} δ�ṩ������Ϣ���޷���ȡ��ȷ������", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    DelayedBalance.Text = "��0.00";
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
                MessageBox.Show($"�л��û�ʧ��{Environment.NewLine}{ex.Message}", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            Text = $"Steam��֤��";

            copyCookieMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Enabled = false;
            copyAccessTokenToolItem.Enabled = false;

            UserImg.Image = Properties.Resources.userimg;
            UserName.ForeColor = Color.Black;
            UserName.Text = "---";
            Balance.Text = "��0.00";
            DelayedBalance.Text = "��0.00";

            OfferCountLabel.Text = "---";
            ConfirmationCountLable.Text = "---";

            if (userClient?.Client?.LoggedIn ?? false)
            {
                Text = $"Steam��֤�� {userClient.User.Account}[{userClient.User.NickName}]";

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
                Balance.Text = "��0.00";
                DelayedBalance.Text = "��0.00";
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
                        Text = $"����ʺ�",
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
                MessageBox.Show($"�ʺ� {account} �ѵ��ߣ������µ�¼", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        DialogResult updateDialog = MessageBox.Show($"�����°汾���ã�{tag_name}�����Ƿ���������", "�汾����", MessageBoxButtons.YesNo);
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


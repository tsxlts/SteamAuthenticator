using Newtonsoft.Json.Linq;
using QRCoder;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model.BUFF;
using SteamKit;
using SteamKit.Model;
using System.Net.Http.Json;

namespace Steam_Authenticator.Forms
{
    public partial class BuffAuth : Form
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private string codeId = null;
        private CookieCollection currentCookies = new CookieCollection();

        public IWebResponse<BuffResponse<QrCodeLoginResponse>> Result { get; private set; }

        public BuffAuth(string tips)
        {
            InitializeComponent();

            msg.Text = tips;
        }

        private void BuffAuth_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                QrCodeLoginOpen().Wait();
                CreateQrCode().Wait();

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        Task.Delay(1000).Wait();

                        if (string.IsNullOrWhiteSpace(codeId))
                        {
                            continue;
                        }

                        var qrCodePollResult = QrCodePoll().GetAwaiter().GetResult();
                        var body = qrCodePollResult.Body;

                        if (!"OK".Equals(body?.code, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var data = body.data;
                        int state = data.Value<int>("state");

                        switch (state)
                        {
                            case 1:
                                continue;

                            case 2:
                                msg.Text = "请在 BUFF App 上确认登录";
                                continue;

                            case 5:
                                CreateQrCode().Wait();
                                continue;

                            case 3:
                                var loginResult = QrCodeLogin().GetAwaiter().GetResult();
                                Result = loginResult;
                                DialogResult = DialogResult.OK;
                                return;

                            case 4:
                            default:
                                MessageBox.Show($"登录失败", "BUFF 登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                DialogResult = DialogResult.Cancel;
                                return;
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }

        private void BuffAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private async Task QrCodeLoginOpen()
        {
            var qrCodePollResult = await SteamApi.GetAsync<BuffResponse<JObject>>($"{BuffApi.Api}/account/api/qr_code_login_open", currentCookies: currentCookies);
            currentCookies.Add(qrCodePollResult.Cookies);
        }

        private async Task CreateQrCode()
        {
            try
            {
                var createQrCode = await SteamApi.PostAsync<BuffResponse<JObject>>($"{BuffApi.Api}/account/api/qr_code_create", JsonContent.Create(new
                {
                    code_type = 1,
                    extra_param = "{}"
                }), currentCookies: currentCookies);

                currentCookies.Add(createQrCode.Cookies);
                var body = createQrCode.Body;

                if (!"OK".Equals(body?.code, StringComparison.OrdinalIgnoreCase))
                {
                    msg.Text = "获取登录二维码失败";
                    return;
                }

                var data = body.data;
                codeId = data.Value<string>("code_id");
                string url = data.Value<string>("url");

                LoadQrCode(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"二维码加载失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "BUFF 登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<IWebResponse<BuffResponse<JObject>>> QrCodePoll()
        {
            var qrCodePollResult = await SteamApi.GetAsync<BuffResponse<JObject>>($"{BuffApi.Api}/account/api/qr_code_poll?item_id={Uri.EscapeDataString(codeId)}",
                currentCookies: currentCookies);

            currentCookies.Add(qrCodePollResult.Cookies);

            return qrCodePollResult;
        }

        private async Task<IWebResponse<BuffResponse<QrCodeLoginResponse>>> QrCodeLogin()
        {
            string deviceId = $"{Guid.NewGuid()}".Replace("-", "");

            IDictionary<string, string> headers = new Dictionary<string, string>
            {
                {"X-CSRFToken",currentCookies["csrf_token"]?.Value },
                {"Referer","https://buff.163.com/" },
                {"Origin","https://buff.163.com/" },
                {"Accept","*/*" }
            };

            var login = await SteamApi.PostAsync<BuffResponse<QrCodeLoginResponse>>($"{BuffApi.Api}/account/api/qr_code_login", JsonContent.Create(new
            {
                item_id = codeId,
                web_device_id = deviceId
            }), headers: headers, currentCookies: currentCookies);

            currentCookies.Add(login.Cookies);
            login.Cookies = currentCookies;
            return login;
        }

        private void LoadQrCode(string plainText)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.L))
                {
                    using (var qrCode = new PngByteQRCode(qrCodeData))
                    {
                        var qrCodeBuffer = qrCode.GetGraphic(10, drawQuietZones: false);
                        qrBox.Image = Image.FromStream(new MemoryStream(qrCodeBuffer));
                    }
                }
            }
        }
    }
}

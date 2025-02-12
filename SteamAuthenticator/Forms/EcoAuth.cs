using Newtonsoft.Json.Linq;
using QRCoder;
using SkiaSharp;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model.ECO;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class EcoAuth : Form
    {
        private readonly string clientId = Guid.NewGuid().ToString();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private string qrCodeId = null;
        private int refreshTime = 300;

        public Model.ECO.LoginResponse Result { get; private set; }

        public EcoAuth(string tips)
        {
            InitializeComponent();

            msg.Text = tips;
        }

        private void EcoAuth_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                CreateQrCode().Wait();

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        Task.Delay(1000).Wait();

                        if (string.IsNullOrWhiteSpace(qrCodeId))
                        {
                            continue;
                        }

                        var qrCodePollResult = QrCodePoll().GetAwaiter().GetResult();
                        var body = qrCodePollResult.Body;

                        if (!"0".Equals(body?.StatusCode, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        if (!"0".Equals(body.StatusData?.ResultCode, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var data = body.StatusData.ResultData;
                        int state = data.State;

                        switch (state)
                        {
                            case 0:
                                continue;

                            case 1:
                                msg.Text = "请在 ECO App 上确认登录";
                                continue;

                            case 3:
                                CreateQrCode().Wait();
                                continue;

                            case 2:
                                var refreshToken = EcoApi.RefreshTokenAsync(data.ClientId, data.Token.RefreshToken).GetAwaiter().GetResult();
                                var refreshTokenBody = refreshToken.Body;
                                if (!"0".Equals(refreshTokenBody?.StatusCode, StringComparison.OrdinalIgnoreCase))
                                {
                                    CreateQrCode().Wait();
                                    continue;
                                }
                                if (!"0".Equals(refreshTokenBody.StatusData?.ResultCode, StringComparison.OrdinalIgnoreCase))
                                {
                                    CreateQrCode().Wait();
                                    continue;
                                }

                                var refreshTokenData = refreshTokenBody.StatusData.ResultData;

                                var queryUserBody = EcoApi.QueryUserAsync(refreshTokenData.Token).GetAwaiter().GetResult();
                                var queryUserData = queryUserBody.StatusData.ResultData;

                                Result = new Model.ECO.LoginResponse
                                {
                                    UserId = data.Token.UserId,
                                    UserName = data.Token.UserName,
                                    Avatar = queryUserData.UserHead,

                                    ClientId = refreshTokenData.ClientId,

                                    Token = refreshTokenData.Token,
                                    TokenExpireDate = refreshTokenData.TokenExpireDate,
                                    TokenExpireDateForCST = refreshTokenData.TokenExpireDateForCST,
                                    TokenRelativeExpireTime = refreshTokenData.TokenRelativeExpireTime,

                                    RefreshToken = refreshTokenData.RefreshToken,
                                    RefreshTokenExpireDate = refreshTokenData.RefreshTokenExpireDate,
                                    RefreshTokenExpireDateForCST = refreshTokenData.RefreshTokenExpireDateForCST,
                                    RefreshTokenRelativeExpireTime = refreshTokenData.RefreshTokenRelativeExpireTime,
                                };
                                DialogResult = DialogResult.OK;
                                return;

                            default:
                                MessageBox.Show($"登录失败", "ECO 登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void EcoAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private async Task CreateQrCode()
        {
            try
            {
                var createQrCode = await SteamApi.GetAsync<EcoResponse<JObject>>($"{EcoApi.Api}/Api/Login/GetLoginQrCodeInfo?ClientId={clientId}");

                var body = createQrCode.Body;

                if (!"0".Equals(body?.StatusCode, StringComparison.OrdinalIgnoreCase))
                {
                    msg.Text = "获取登录二维码失败";
                    return;
                }
                if (!"0".Equals(body.StatusData?.ResultCode, StringComparison.OrdinalIgnoreCase))
                {
                    msg.Text = "获取登录二维码失败";
                    return;
                }

                var data = body.StatusData.ResultData;
                qrCodeId = data.Value<string>("QrCodeId");
                refreshTime = data.Value<int>("RefreshTime");
                string action = data.Value<string>("Action");

                LoadQrCode(action);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"二维码加载失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "ECO 登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<IWebResponse<EcoResponse<QrCodeLoginResponse>>> QrCodePoll()
        {
            var qrCodePollResult = await SteamApi.GetAsync<EcoResponse<QrCodeLoginResponse>>($"{EcoApi.Api}/Api/Login/QueryScanState?QRCodeId={Uri.EscapeDataString(qrCodeId)}");
            return qrCodePollResult;
        }

        private void LoadQrCode(string plainText)
        {
            plainText = plainText.Replace("data:image/png;base64,", "");
            var buffer = Convert.FromBase64String(plainText);
            using (var skiaImage = SKBitmap.Decode(buffer))
            {
                var skiaReader = new ZXing.SkiaSharp.BarcodeReader();
                var skiaResult = skiaReader.Decode(skiaImage);
                plainText = skiaResult?.Text;
            }

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

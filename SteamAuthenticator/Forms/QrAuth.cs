using QRCoder;
using SteamKit;
using SteamKit.Api;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class QrAuth : Form
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private (byte[] RequestId, ulong ClientId) authSessionViaQRResponse = default;

        public QrAuth()
        {
            InitializeComponent();
        }

        private void QrAuth_Load(object sender, EventArgs e)
        {
            qrBox.SizeMode = PictureBoxSizeMode.Zoom;
            LoadQrCode();

            Task.Run(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        Task.Delay(1000).Wait();

                        if (authSessionViaQRResponse.ClientId == 0)
                        {
                            continue;
                        }

                        var sessionStatusReault = SteamAuthentication.PollAuthSessionStatusAsync(authSessionViaQRResponse.ClientId, authSessionViaQRResponse.RequestId).GetAwaiter().GetResult();
                        if (!string.IsNullOrWhiteSpace(sessionStatusReault.Body?.refresh_token))
                        {
                            RefreshToken = sessionStatusReault.Body.refresh_token;
                            DialogResult = DialogResult.OK;
                            break;
                        }

                        if (sessionStatusReault.Body?.new_client_id == 0)
                        {
                            authSessionViaQRResponse.ClientId = sessionStatusReault.Body.new_client_id;
                        }
                        if (!string.IsNullOrWhiteSpace(sessionStatusReault?.Body?.new_challenge_url))
                        {
                            LoadQrCode(sessionStatusReault?.Body.new_challenge_url);
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }

        private async void LoadQrCode()
        {
            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
                {
                    var qrResult = await SteamAuthentication.BeginAuthSessionViaQRAsync(SteamKit.Proto.EAuthTokenPlatformType.k_EAuthTokenPlatformType_MobileApp, cts.Token);
                    if (!string.IsNullOrWhiteSpace(qrResult.Body?.challenge_url))
                    {
                        LoadQrCode(qrResult.Body?.challenge_url);
                        authSessionViaQRResponse = (qrResult.Body.request_id, qrResult.Body.client_id);
                    }
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
                MessageBox.Show($"二维码加载失败" +
                    $"{Environment.NewLine}" +
                    $"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QrAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
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

        public string RefreshToken { get; private set; }
    }
}

using QRCoder;
using SteamKit;
using SteamKit.Model;

namespace Steam_Authenticator.Forms
{
    public partial class QrAuth : Form
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private (string RequestId, string ClientId) authSessionViaQRResponse = default;

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

                        if (string.IsNullOrWhiteSpace(authSessionViaQRResponse.ClientId))
                        {
                            continue;
                        }

                        var sessionStatusReault = SteamAuthentication.PollAuthSessionStatusAsync(authSessionViaQRResponse.ClientId, authSessionViaQRResponse.RequestId).GetAwaiter().GetResult();
                        if (!string.IsNullOrWhiteSpace(sessionStatusReault.Body?.RefreshToken))
                        {
                            RefreshToken = sessionStatusReault.Body.RefreshToken;
                            DialogResult = DialogResult.OK;
                            break;
                        }

                        if (!string.IsNullOrWhiteSpace(sessionStatusReault.Body?.NewClientId))
                        {
                            authSessionViaQRResponse.ClientId = sessionStatusReault.Body.NewClientId;
                        }
                        if (!string.IsNullOrWhiteSpace(sessionStatusReault?.Body?.NewChallengeUrl))
                        {
                            LoadQrCode(sessionStatusReault?.Body.NewChallengeUrl);
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
                    var qrResult = await SteamAuthentication.BeginAuthSessionViaQRAsync(AuthTokenPlatformType.MobileApp, cts.Token);
                    if (!string.IsNullOrWhiteSpace(qrResult.Body?.ChallengeUrl))
                    {
                        LoadQrCode(qrResult.Body?.ChallengeUrl);
                        authSessionViaQRResponse = (qrResult.Body.RequestId, qrResult.Body.ClientId);
                    }
                }
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

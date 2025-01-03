
namespace Steam_Authenticator.Forms
{
    public partial class QrBox : Form
    {
        private readonly Action<PictureBox> loadQrcode;

        public QrBox(string tips, Action<PictureBox> loadQrcode)
        {
            InitializeComponent();

            msg.Text = tips;

            this.loadQrcode = loadQrcode;
        }

        private void QrBox_Load(object sender, EventArgs e)
        {
            loadQrcode.Invoke(qrcodeBox);
        }
    }
}

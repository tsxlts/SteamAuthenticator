
namespace Steam_Authenticator.Forms
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Continue;
        }

        private void declineBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}

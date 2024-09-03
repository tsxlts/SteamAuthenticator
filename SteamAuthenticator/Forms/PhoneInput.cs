using System.Text.RegularExpressions;

namespace Steam_Authenticator.Forms
{
    public partial class PhoneInput : Form
    {
        private readonly string title;
        private readonly string tips;
        public PhoneInput(string title, string tips)
        {
            InitializeComponent();

            this.title = title;
            this.tips = tips;
        }

        private void Input_Load(object sender, EventArgs e)
        {
            Text = title;
            TipsLabel.Text = tips;
            PhoneBox.Text = Phone;
            CountryBox.Text = Country;
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            Phone = PhoneBox.Text;
            if (Phone[0] != '+')
            {
                MessageBox.Show("电话号码必须以+和国家代码开头。", "手机号", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Country = CountryBox.Text;
            if (string.IsNullOrWhiteSpace(Phone))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CountryBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            var regex = new Regex(@"[^a-zA-Z]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void PhoneBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            var regex = new Regex(@"[^0-9\s\+]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        public string Phone { get; set; }

        public string Country { get; set; }
    }
}

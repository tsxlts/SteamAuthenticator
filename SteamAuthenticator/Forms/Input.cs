namespace Steam_Authenticator.Forms
{
    public partial class Input : Form
    {
        private readonly string title;
        private readonly string tips;
        private readonly bool required;
        private readonly string errorMsg;

        public Input(string title, string tips, bool password = false, bool required = false, string errorMsg = null)
        {
            InitializeComponent();

            this.title = title;
            this.tips = tips;
            this.required = required;
            this.errorMsg = errorMsg;

            if (password)
            {
                InputBox.PasswordChar = '*';
            }
        }

        private void Input_Load(object sender, EventArgs e)
        {
            Text = title;
            TipsLabel.Text = tips;
            InputBox.Text = InputValue;
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            InputValue = InputBox.Text;
            if (string.IsNullOrWhiteSpace(InputValue) && required)
            {
                MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public string InputValue { get; set; }
    }
}

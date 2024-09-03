namespace Steam_Authenticator.Forms
{
    public partial class Input : Form
    {
        private readonly string title;
        private readonly string tips;
        public Input(string title, string tips, bool password = false)
        {
            InitializeComponent();

            this.title = title;
            this.tips = tips;

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
            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public string InputValue { get; set; }
    }
}

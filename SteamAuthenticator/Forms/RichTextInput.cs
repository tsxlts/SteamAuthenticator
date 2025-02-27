namespace Steam_Authenticator.Forms
{
    public partial class RichTextInput : Form
    {
        private readonly string title;
        private readonly string tips;
        private readonly bool required;
        private readonly string errorMsg;
        public RichTextInput(string title, string tips, bool required = false, string errorMsg = null)
        {
            InitializeComponent();

            this.title = title;
            this.tips = tips;
            this.required = required;
            this.errorMsg = errorMsg;
        }

        private void RichTextInput_Load(object sender, EventArgs e)
        {
            Text = title;
            TipsLabel.Text = tips;
            InputBox.Text = InputValue;

            InputBox.Focus();
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

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InputBox.SelectedText = Environment.NewLine;
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        public string InputValue { get; set; }
    }
}

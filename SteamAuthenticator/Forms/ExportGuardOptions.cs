using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class ExportGuardOptions : Form
    {
        private readonly string current;

        public ExportGuardOptions(string current)
        {
            InitializeComponent();

            currentName.Text = current;
            this.current = current;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            SelectGuards.Clear();

            if (exportAll.Checked)
            {
                var accounts = Appsetting.Instance.Manifest.GetGuards();
                foreach (var item in accounts)
                {
                    SelectGuards.Add(Appsetting.Instance.Manifest.GetGuard(item));
                }
            }
            else
            {
                SelectGuards.Add(Appsetting.Instance.Manifest.GetGuard(current));
            }

            DialogResult = DialogResult.OK;
        }

        public readonly List<Guard> SelectGuards = new List<Guard>();

        public string EncryptPassword => passwordBox.Text;
    }
}

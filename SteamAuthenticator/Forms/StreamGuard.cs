using Newtonsoft.Json;
using SteamKit;

namespace Steam_Authenticator.Forms
{
    public partial class StreamGuard : Form
    {
        private readonly string account;
        private readonly System.Threading.Timer timer;
        public StreamGuard(string account)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            timer = new System.Threading.Timer(Refresh, null, Timeout.Infinite, Timeout.Infinite);
            this.account = account;
        }

        private void StreamGuard_Load(object sender, EventArgs e)
        {
            var users = Appsetting.Instance.Manifest.GetGuards().OrderBy(c => c);
            if (users.Any())
            {
                int selectIndex = 0;
                int index = 0;
                foreach (var item in users)
                {
                    Users.Items.Add(item);
                    if (item.Equals(account, StringComparison.OrdinalIgnoreCase))
                    {
                        selectIndex = index;
                    }

                    index++;
                }
                Users.SelectedIndex = selectIndex;
            }

            timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public void Refresh(object _)
        {
            string account = Users.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(account))
            {
                return;
            }

            var guard = Appsetting.Instance.Manifest.GetGuard(account);
            if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
            {
                return;
            }

            long timestamp = Extension.GetSystemTimestamp();

            string code = GuardCodeGenerator.GenerateSteamGuardCode(timestamp, guard.SharedSecret);
            GuardText.Text = code;
            ExpireText.Text = $"{30 - (timestamp % 30)}秒";
        }

        private void Users_SelectedValueChanged(object sender, EventArgs e)
        {
            timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void GuardText_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GuardText.Text);
        }

        private void exportGuardBtn_Click(object sender, EventArgs e)
        {
            string account = Users.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(account))
            {
                MessageBox.Show("请选择要导出的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var guard = Appsetting.Instance.Manifest.GetGuard(account);

            string json = JsonConvert.SerializeObject(guard, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "导出令牌",
                Filter = "JSON (*.json)|*.json",
                DefaultExt = ".json",
                FileName = $"{account}.guard.json",
                InitialDirectory = @"C:\"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, json);
            }
        }
    }
}

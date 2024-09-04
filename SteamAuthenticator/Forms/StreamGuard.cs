using Steam_Authenticator.Internal;
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
            if (Appsetting.Instance.Manifest.Encrypted)
            {
                string tips = "请输入访问密码";
                Input input;
                while (true)
                {
                    input = new Input("导出令牌", tips, true);
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    string password = input.InputValue;
                    if (!Appsetting.Instance.Manifest.CheckPassword(password))
                    {
                        tips = "访问密码错误，请重新输入";
                        continue;
                    }
                    break;
                }
            }

            string account = Users.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(account))
            {
                MessageBox.Show("请选择要导出的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var guard = Appsetting.Instance.Manifest.GetGuard(account);

            string encryptPassword = null;
            if (MessageBox.Show($"导出的令牌文件是否需要加密", "导出令牌", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string tips = "请输入加密密码";
                Input input;
                while (true)
                {
                    input = new Input("导出令牌", tips, true);
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    encryptPassword = input.InputValue;
                    if (string.IsNullOrWhiteSpace(encryptPassword))
                    {
                        tips = "加密密码不能为空，请重新输入";
                        continue;
                    }

                    break;
                }
            }

            bool encrypt = !string.IsNullOrWhiteSpace(encryptPassword);

            byte[] dataBuffer;
            using (var stream = guard.Serialize())
            {
                dataBuffer = new byte[stream.Length];
                stream.Read(dataBuffer);
            }

            using (var stream = new MemoryStream())
            {
                stream.WriteBoolean(encrypt);

                if (encrypt)
                {
                    var iv = FileEncryptor.GetInitializationVector();
                    var salt = FileEncryptor.GetRandomSalt();
                    dataBuffer = FileEncryptor.EncryptData(encryptPassword, salt, iv, dataBuffer);

                    stream.WriteInt32(iv.Length);
                    stream.Write(iv);

                    stream.WriteInt32(salt.Length);
                    stream.Write(salt);
                }

                stream.WriteInt32(dataBuffer.Length);
                stream.Write(dataBuffer);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "导出令牌",
                    Filter = "令牌文件 (*.entry)|*.entry",
                    DefaultExt = ".entry",
                    FileName = $"{account}.guard.entry",
                    InitialDirectory = AppContext.BaseDirectory,
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, stream.ToArray());
                }
            }
        }
    }
}

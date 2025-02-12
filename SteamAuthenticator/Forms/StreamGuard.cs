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
                GuardText.Text = "*****";
                return;
            }

            var guard = Appsetting.Instance.Manifest.GetGuard(account);
            if (string.IsNullOrWhiteSpace(guard?.SharedSecret))
            {
                GuardText.Text = "*****";
                return;
            }

            long timestamp = Extension.GetSystemTimestamp();

            string code = GuardCodeGenerator.GenerateSteamGuardCode(timestamp, guard.SharedSecret);
            GuardText.Text = code;
            ExpireText.Text = $"{30 - (timestamp % 30)}秒";
        }

        private void Users_SelectedValueChanged(object sender, EventArgs e)
        {
            RevocationCode.Text = "******";
            timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void GuardText_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GuardText.Text);
        }

        private void RevocationCode_Click(object sender, EventArgs e)
        {
            string account = Users.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(account))
            {
                return;
            }

            var guard = Appsetting.Instance.Manifest.GetGuard(account);
            if (string.IsNullOrWhiteSpace(guard?.RevocationCode))
            {
                return;
            }

            if (RevocationCode.Text == "******")
            {
                RevocationCode.Text = guard.RevocationCode;
            }
            else
            {
                RevocationCode.Text = "******";
            }
        }

        private void CopyRevocationCode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(RevocationCode.Text);
        }

        private void deleteGuardBtn_Click(object sender, EventArgs e)
        {
            try
            {
                deleteGuardBtn.Enabled = false;

                string account = Users.SelectedItem?.ToString();
                if (string.IsNullOrWhiteSpace(account))
                {
                    MessageBox.Show("请选择要删除的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Appsetting.Instance.Manifest.Encrypted)
                {
                    string tips = "请输入访问密码";
                    Input input;
                    while (true)
                    {
                        input = new Input("删除令牌", tips, password: true, required: true, errorMsg: "请输入密码");
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

                if (MessageBox.Show($"删除令牌前请确保已将令牌移动至其他设备" +
                    $"{Environment.NewLine}" +
                    $"令牌删除后不可恢复" +
                    $"{Environment.NewLine}" +
                    $"你确认要删除帐号 {account} 的令牌吗？", "删除令牌", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                Appsetting.Instance.Manifest.RemoveGuard(account, out var _);
                Users.Items.Remove(account);

                if (Users.Items.Count > 0)
                {
                    Users.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                deleteGuardBtn.Enabled = true;
            }
        }

        private void exportGuardBtn_Click(object sender, EventArgs e)
        {
            try
            {
                exportGuardBtn.Enabled = false;

                if (!Appsetting.Instance.Manifest.GetGuards().Any())
                {
                    MessageBox.Show("没有可以导出的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string account = Users.SelectedItem?.ToString();
                ExportGuardOptions exportGuardOptions = new ExportGuardOptions(account);
                exportGuardOptions.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                exportGuardBtn.Enabled = true;
            }
        }
    }
}

using Steam_Authenticator.Internal;
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

            if (Appsetting.Instance.Manifest.GetGuard(current) == null)
            {
                exportCurrent.Enabled = false;
                exportAll.Checked = true;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (Appsetting.Instance.Manifest.Encrypted)
            {
                string tips = "请输入访问密码";
                Input input;
                while (true)
                {
                    input = new Input("导出令牌", tips, password: true, required: true, errorMsg: "请输入密码");
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

            List<Guard> guards = new List<Guard>();

            if (exportAll.Checked)
            {
                var accounts = Appsetting.Instance.Manifest.GetGuards();
                foreach (var item in accounts)
                {
                    guards.Add(Appsetting.Instance.Manifest.GetGuard(item));
                }
            }
            else
            {
                guards.Add(Appsetting.Instance.Manifest.GetGuard(current));
            }

            if (guards.Count == 0)
            {
                MessageBox.Show("请选择要导出的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string encryptPassword = passwordBox.Text;
            bool encrypt = !string.IsNullOrWhiteSpace(encryptPassword);

            using (var stream = new MemoryStream())
            {
                stream.WriteBoolean(encrypt);

                var iv = new byte[0];
                var salt = new byte[0];
                if (encrypt)
                {
                    iv = FileEncryptor.GetInitializationVector();
                    salt = FileEncryptor.GetRandomSalt();

                    stream.WriteInt32(iv.Length);
                    stream.Write(iv);

                    stream.WriteInt32(salt.Length);
                    stream.Write(salt);
                }

                foreach (var guard in guards)
                {
                    using (var guardStream = guard.Serialize())
                    {
                        byte[] dataBuffer = new byte[guardStream.Length];
                        guardStream.Read(dataBuffer);
                        if (encrypt)
                        {
                            dataBuffer = FileEncryptor.EncryptData(encryptPassword, salt, iv, dataBuffer);
                        }

                        stream.WriteInt32(dataBuffer.Length);
                        stream.Write(dataBuffer);
                    }
                }

                string initialDirectory = Appsetting.Instance.AppSetting.Entry.InitialDirectory ?? AppContext.BaseDirectory;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "导出令牌",
                    Filter = "令牌文件 (*.entry)|*.entry",
                    DefaultExt = ".entry",
                    FileName = $"{DateTime.Now:yyyyMMddHHmmss}.guard.entry",
                    InitialDirectory = initialDirectory,
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, stream.ToArray());

                    initialDirectory = new FileInfo(filePath).DirectoryName;

                    Appsetting.Instance.AppSetting.Entry.InitialDirectory = initialDirectory;
                    Appsetting.Instance.AppSetting.Save();
                }
            }

            Close();
        }
    }
}

using System.Diagnostics;
using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.Other;

namespace Steam_Authenticator.Forms
{
    public partial class ExportGuardOptions : Form
    {
        private readonly string current;
        private readonly List<string> selected;

        public ExportGuardOptions(string current)
        {
            InitializeComponent();

            selected = new List<string>();
        }
        private void selectAccountBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<SelectOption> selectOptions = new List<SelectOption>();

            var users = Appsetting.Instance.Manifest.GetGuards();
            foreach (var user in users)
            {
                selectOptions.Add(new SelectOption
                {
                    Value = user,
                    Text = $"{user}",
                    Checked = selected.Contains(user)
                });
            }

            var options = new Options("选择令牌", $"请选择你需要导出的令牌")
            {
                Width = this.Width - 20,
                Height = this.Height - 20,
                ItemSize = new Size(100, 20),
                Multiselect = true,
                Datas = selectOptions.OrderBy(c => c.Text).ToList(),
            };
            if (options.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            selected.Clear();
            selected.AddRange(options.Selected.Select(c => c.Value));

            ReloadAccounts();
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
            foreach (var item in selected)
            {
                guards.Add(Appsetting.Instance.Manifest.GetGuard(item));
            }

            if (guards.Count == 0)
            {
                MessageBox.Show("请选择要导出的令牌", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (saFile.Checked)
            {
                Input input = new Input("设置密码", $"请输入文件加密密码{Environment.NewLine}留空不设置密码", true);
                if (input.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                string encryptPassword = input.InputValue;
                bool encrypt = !string.IsNullOrWhiteSpace(encryptPassword);

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "导出令牌",
                    Filter = "SA文件 (*.saEntry)|*.saEntry",
                    DefaultExt = ".saEntry",
                    FileName = $"SA_{DateTime.Now:yyyyMMdd.HHmm}.saEntry",
                    InitialDirectory = Appsetting.Instance.AppSetting.Entry.InitialDirectory ?? AppContext.BaseDirectory,
                    CheckPathExists = true
                };
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Appsetting.Instance.AppSetting.Entry.InitialDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                Appsetting.Instance.AppSetting.Save();

                string filePath = saveFileDialog.FileName;

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

                    File.WriteAllBytes(filePath, stream.ToArray());
                }

                Process.Start("Explorer", string.Format("/select,\"{0}\"", filePath));
            }

            if (maFile.Checked)
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    InitialDirectory = Appsetting.Instance.AppSetting.Entry.InitialDirectory ?? AppContext.BaseDirectory,
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Appsetting.Instance.AppSetting.Entry.InitialDirectory = folderBrowserDialog.SelectedPath;
                Appsetting.Instance.AppSetting.Save();

                MaFile maFile;
                string filePath = folderBrowserDialog.SelectedPath;
                foreach (var guard in guards)
                {
                    maFile = new MaFile
                    {
                        account_name = guard.AccountName,
                        steamid = guard.SteamId,
                        device_id = guard.DeviceId,
                        shared_secret = guard.SharedSecret,
                        identity_secret = guard.IdentitySecret,
                        secret1 = guard.Secret1,
                        revocation_code = guard.RevocationCode,
                        serial_number = guard.SerialNumber,
                        steamguard_scheme = $"{(int)guard.GuardScheme}",
                        token_gid = guard.TokenGID,
                        uri = guard.URI,
                    };

                    filePath = Path.Combine(folderBrowserDialog.SelectedPath, $"{guard.AccountName}.maFile");
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(maFile));
                }

                Process.Start("Explorer", string.Format("/select,\"{0}\"", filePath));
            }

            Close();
        }

        private void ReloadAccounts()
        {
            accountPanel.Controls.Clear();
            int lineWith = accountPanel.Width - 25;
            Color[] colors = [Color.PaleTurquoise, Color.Wheat];
            int y = 0;
            int index = 0;
            foreach (var account in selected)
            {
                var line = new Panel
                {
                    Name = account,
                    Location = new Point(5, y),
                    Width = lineWith,
                    Height = 23,
                    BackColor = colors[(index++) % colors.Length]
                };

                var userLabel = new Label
                {
                    Name = "user",
                    Text = $"{account}",
                    AutoSize = false,
                    AutoEllipsis = true,
                    Size = new Size(250, 16),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(5, 3),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                };
                line.Controls.Add(userLabel);

                var deleteBtn = new Label
                {
                    Name = $"{account}",
                    Text = $"删除",
                    AutoSize = false,
                    Size = new Size(40, 16),
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(line.Width - 45, 3),
                    Cursor = Cursors.Hand,
                    ForeColor = Color.Gray,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                };
                deleteBtn.Click += (object sender, EventArgs e) =>
                {
                    selected.Remove(account);
                    ReloadAccounts();
                };
                line.Controls.Add(deleteBtn);

                accountPanel.Controls.Add(line);

                y = y + 25;
            }
        }
    }
}

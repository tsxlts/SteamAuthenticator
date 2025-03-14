﻿using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.Other;
using System.Diagnostics;

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

            exportCurrent.Checked = true;
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
                    FileName = $"{guards[0].AccountName}.saEntry",
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
    }
}

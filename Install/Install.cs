using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using static Install.WindowsApi;

namespace Install
{
    public partial class Install : Form
    {
        public readonly RunParams runParams;

        private bool completed = false;

        public Install(RunParams runParams)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            this.runParams = runParams;
        }

        private async void Install_Load(object sender, EventArgs e)
        {
            installPathBox.Text = runParams.InstallPath;

            await AppInstall();
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            if (completed)
            {
                Close();
                return;
            }

            await AppInstall();
        }

        private Task AppInstall()
        {
            return Task.Run(() =>
            {
                try
                {
                    installBtn.Enabled = false;
                    installBtn.Text = "У����";

                    try
                    {
                        if (runParams.RunningProcessId > 0)
                        {
                            var runningProcess = Process.GetProcessById(runParams.RunningProcessId);
                            if (!(runningProcess?.HasExited ?? true))
                            {
                                runningProcess.Kill();
                            }
                            Task.Delay(3000).GetAwaiter().GetResult();
                        }
                    }
                    catch
                    {

                    }

                    installBtn.Text = "���ڰ�װ";

                    using (ZipArchive archive = ZipFile.Open(runParams.InstallPackage, ZipArchiveMode.Read))
                    {
                        msgBox.ForeColor = Color.Gray;
                        this.progressBar.Minimum = 0;
                        this.progressBar.Maximum = archive.Entries.Count;
                        this.progressBar.Value = 0;

                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            msgBox.Text = entry.FullName;

                            var nodes = entry.FullName.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(1);
                            if (!nodes.Any())
                            {
                                continue;
                            }

                            string filePath = Path.Combine([runParams.InstallPath, .. nodes]);

                            try
                            {
                                this.progressBar.Value++;

                                if (entry.FullName.EndsWith('/'))
                                {
                                    Directory.CreateDirectory(filePath);
                                    continue;
                                }

                                entry.ExtractToFile(filePath, true);
                            }
                            catch (IOException)
                            {
                                var runningProcess = GetProcessByFile(filePath);
                                if (runningProcess != null)
                                {
                                    runParams.RunningProcessId = runningProcess.Id;

                                    var killProcess = MessageBox.Show($"��⵽ Steam��֤�� �ͻ���δ�ر� ,{Environment.NewLine}" +
                                         $"�Ƿ������˳� Steam��֤�� �ͻ�����ɸ���",
                                         "����Steam��֤��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (killProcess == DialogResult.Yes)
                                    {
                                        AppInstall().GetAwaiter().GetResult();
                                    }

                                    return;
                                }

                                MessageBox.Show($"��⵽ Steam��֤�� �ͻ���δ�ر� ,{Environment.NewLine}" +
                                $"���˳� Steam��֤�� �ͻ��˺����°�װ",
                                "����", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                return;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"��װʧ��, {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    File.Delete(runParams.InstallPackage);
                    completed = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"��װʧ��, {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.progressBar.Minimum = 0;
                    this.progressBar.Maximum = 1;
                    this.progressBar.Value = 1;

                    msgBox.ForeColor = completed ? Color.Green : Color.Red;

                    installBtn.Text = completed ? "��װ���" : "���°�װ";
                    installBtn.Enabled = true;
                }
            });
        }

        private static Process GetProcessByFile(string filePath)
        {
            IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (hSnapshot != IntPtr.Zero)
            {
                PROCESSENTRY32 pe32 = new PROCESSENTRY32();
                pe32.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
                if (Process32First(hSnapshot, ref pe32))
                {
                    do
                    {
                        try
                        {
                            Process process = Process.GetProcessById(unchecked((int)pe32.th32ProcessID));
                            foreach (ProcessModule module in process.Modules)
                            {
                                if (module.FileName.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                                {
                                    CloseHandle(hSnapshot);
                                    return process;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    while (Process32Next(hSnapshot, ref pe32));
                }
                CloseHandle(hSnapshot);
            }
            return null;
        }
    }
}

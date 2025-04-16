
using System.Diagnostics;
using Newtonsoft.Json;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class ApplicationUpgrade : Form
    {
        private readonly Version currentVersion;
        private readonly Version latestVersion;
        private readonly DateTime latestVersionTime;
        private readonly string versionSummary;
        private readonly string updateUrl;
        private readonly string downloadUrl;
        private readonly string appName;

        private TaskCompletionSource downloadTask;
        private string tempfile;

        private CancellationTokenSource downloadCts = new CancellationTokenSource();

        public ApplicationUpgrade(Version currentVersion, Version latestVersion, DateTime latestVersionTime, string versionSummary, string updateUrl, string downloadUrl, string name)
        {
            InitializeComponent();

            this.currentVersion = currentVersion;
            this.latestVersion = latestVersion;
            this.latestVersionTime = latestVersionTime;
            this.versionSummary = versionSummary;
            this.updateUrl = updateUrl;
            this.downloadUrl = downloadUrl;
            this.appName = name;
        }

        private void ApplicationUpgrade_Load(object sender, EventArgs e)
        {
            currentVersionBox.Text = currentVersion.ToString();
            latestVersionBox.Text = latestVersion.ToString();
            latestVersionTimeBox.Text = latestVersionTime.ToString("yyyy年MM月dd日 HH时mm分");
            versionSummaryBox.Text = versionSummary;
        }

        private void giteeDownload_Click(object sender, EventArgs e)
        {
            Utils.CopyText(downloadUrl);
        }

        private void githubDownload_Click(object sender, EventArgs e)
        {
            Utils.CopyText(updateUrl);
        }

        private async void upgradeBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                upgradeBtn.Enabled = false;

                var installPackage = Path.Combine(Appsetting.Instance.DownloadDirectory, appName);
                if (File.Exists(installPackage))
                {
                    StratInstall(installPackage);
                    return;
                }

                this.progress.Text = "0%";
                tempfile = Path.Combine(Appsetting.Instance.DownloadDirectory, $"{appName}.bin");
                if (File.Exists(tempfile))
                {
                    File.Delete(tempfile);
                }

                downloadTask = new TaskCompletionSource();
                downloadCts = new CancellationTokenSource();
                using (var client = new HttpClient(new HttpClientHandler()))
                {
                    var httpResponseMessage = await client.GetAsync(updateUrl, HttpCompletionOption.ResponseHeadersRead, downloadCts.Token).ConfigureAwait(true);
                    var contentLength = httpResponseMessage.Content.Headers.ContentLength.Value;

                    this.progressBar.Minimum = 0;
                    this.progressBar.Maximum = 100;

                    using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync(downloadCts.Token).ConfigureAwait(true))
                    {
                        long downloadSize = 0;
                        var readLength = 1024000;
                        byte[] bytes = new byte[readLength];
                        int writeLength;
                        while ((writeLength = await stream.ReadAsync(bytes, 0, readLength, downloadCts.Token).ConfigureAwait(true)) > 0)
                        {
                            if (downloadCts.IsCancellationRequested)
                            {
                                break;
                            }

                            using (FileStream fs = new FileStream(tempfile, FileMode.Append, FileAccess.Write))
                            {
                                await fs.WriteAsync(bytes, 0, writeLength, downloadCts.Token).ConfigureAwait(true);
                            }
                            downloadSize += writeLength;

                            int progressPercentage = (int)Math.Floor(Math.Min(downloadSize * 100.00m / contentLength, 100.00m));
                            this.progressBar.Value = progressPercentage;
                            this.progress.Text = $"{progressPercentage}%";
                        }

                        if (downloadSize >= contentLength)
                        {
                            File.Move(tempfile, installPackage);

                            this.progress.Text = $"下载完成";
                            if (MessageBox.Show($"最新版程序已下载完成, 是否立即更新", "检查更新", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                StratInstall(installPackage);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.progress.Text = $"下载失败";
            }
            finally
            {
                upgradeBtn.Enabled = true;
                downloadTask.TrySetResult();
            }
        }

        private void downloadBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show($"如遇到无法更新的情况, 你可以按照以下操作进行更新{Environment.NewLine}" +
                $"1、复制国内或者国际下载链接{Environment.NewLine}" +
                $"2、在浏览器打开并下载最新安装包{Environment.NewLine}" +
                $"3、退出应用程序, 解压安装包{Environment.NewLine}" +
                $"4、将解压后的文件覆盖原安装目录下所有文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void ApplicationUpgrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                downloadCts.Cancel();
                if (downloadTask != null)
                {
                    await downloadTask.Task;
                }

                if (File.Exists(tempfile))
                {
                    File.Delete(tempfile);
                }
            }
            catch
            {

            }
            finally
            {
                this.Dispose();
            }
        }

        private void StratInstall(string installPackage)
        {
            string install = Appsetting.Instance.SetupApplication;
            if (!File.Exists(install))
            {
                MessageBox.Show($"未找到更新程序, 你可以按照以下操作进行更新{Environment.NewLine}" +
                $"1、复制国内或者国际下载链接{Environment.NewLine}" +
                $"2、在浏览器打开并下载最新安装包{Environment.NewLine}" +
                $"3、退出应用程序, 解压安装包{Environment.NewLine}" +
                $"4、将解压后的文件覆盖原安装目录下所有文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = install
            };
            startInfo.ArgumentList.Add(JsonConvert.SerializeObject(new InstallRunParams
            {
                InstallPath = AppContext.BaseDirectory,
                InstallPackage = installPackage,
                RunningProcessId = Process.GetCurrentProcess().Id
            }));

            Process.Start(startInfo);
        }
    }
}

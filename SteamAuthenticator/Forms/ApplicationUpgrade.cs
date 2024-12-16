
using Newtonsoft.Json;
using Steam_Authenticator.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Steam_Authenticator.Forms
{
    public partial class ApplicationUpgrade : Form
    {
        private readonly Version currentVersion;
        private readonly Version latestVersion;
        private readonly DateTime latestVersionTime;
        private readonly string versionSummary;
        private readonly string updateUrl;
        private readonly string appName;

        private Task downloadTask = null;
        private string downloadFileName = null;

        public ApplicationUpgrade(Version currentVersion, Version latestVersion, DateTime latestVersionTime, string versionSummary, string updateUrl, string name)
        {
            InitializeComponent();

            this.currentVersion = currentVersion;
            this.latestVersion = latestVersion;
            this.latestVersionTime = latestVersionTime;
            this.versionSummary = versionSummary;
            this.updateUrl = updateUrl;
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

        }

        private void githubDownload_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(updateUrl);
        }

        private void upgradeBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                upgradeBtn.Enabled = false;
                if (downloadFileName != null)
                {
                    StratInstall();
                    return;
                }

                this.progress.Text = "0%";
                downloadFileName = Path.Combine(Appsetting.Instance.DownloadDirectory, appName);

                using (var client = new WebClient())
                {
                    client.DownloadFileCompleted += client_DownloadFileCompleted;
                    client.DownloadProgressChanged += client_DownloadProgressChanged;
                    downloadTask = client.DownloadFileTaskAsync(new Uri(updateUrl), downloadFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新失败, {ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                upgradeBtn.Enabled = true;
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

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.progress.Text = $"下载完成";

            if (MessageBox.Show($"最新版程序已下载完成, 是否立即更新", "检查更新", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StratInstall();
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = (int)e.TotalBytesToReceive;
            this.progressBar.Value = (int)e.BytesReceived;
            this.progress.Text = $"{e.ProgressPercentage}%";
        }

        private void ApplicationUpgrade_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void StratInstall()
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
                InstallPackage = downloadFileName,
                RunningProcessId = Process.GetCurrentProcess().Id
            }));

            Process.Start(startInfo);
        }
    }
}

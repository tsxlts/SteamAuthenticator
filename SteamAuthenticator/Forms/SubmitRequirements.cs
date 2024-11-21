using Newtonsoft.Json.Linq;
using SteamKit;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Steam_Authenticator.Forms
{
    public partial class SubmitRequirements : Form
    {
        private readonly Form mainForm;

        public SubmitRequirements(Form mainForm)
        {
            InitializeComponent();

            this.mainForm = mainForm;
        }

        private void SubmitRequirements_Load(object sender, EventArgs e)
        {
            this.Width = this.mainForm.Width - 100;
            this.Height = this.mainForm.Height - 100;

            Location = new Point(this.mainForm.Location.X + 50, this.mainForm.Location.Y + 50);
        }

        private async void submit_Click(object sender, EventArgs e)
        {
            try
            {
                submit.Enabled = false;

                var subject = subjectBox.Text;
                var body = bodyBox.Text;
                var contactInfo = contactInfoBox.Text;

                if (string.IsNullOrWhiteSpace(subject))
                {
                    MessageBox.Show("请输入需求主题", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    subjectBox.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(body))
                {
                    MessageBox.Show("请输入需求描述", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bodyBox.Focus();
                    return;
                }

                string api = "http://api.vdaima.cn/steam/Api/SteamAuthenticator/SubmitRequirements";
                var response = await SteamApi.PostAsync<JObject>(api, JsonContent.Create(new
                {
                    Project = "SteamAuthenticator",
                    Subject = subject,
                    Body = body,
                    ContactInfo = contactInfo,
                }));
                var responseBody = response.Body;
                if (responseBody == null)
                {
                    MessageBox.Show($"提交失败，请前往SteamAuthenticator项目主页提交需求", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("explorer.exe", $"https://github.com/tsxlts/SteamAuthenticator/issues");
                    return;
                }

                var resultCode = responseBody.Value<string>("ResultCode");
                var resultMsg = responseBody.Value<string>("ResultMsg");
                if (!"0".Equals(resultCode))
                {
                    MessageBox.Show($"提交失败{Environment.NewLine}{resultMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show($"提交成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"提交失败，请前往SteamAuthenticator项目主页提交需求", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start("explorer.exe", $"https://github.com/tsxlts/SteamAuthenticator/issues");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交失败{Environment.NewLine}{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                submit.Enabled = true;
            }
        }
    }
}

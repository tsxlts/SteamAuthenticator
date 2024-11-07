using Steam_Authenticator.Controls;
using Steam_Authenticator.Model;
using SteamKit.Model;
using SteamKit.WebClient;
using System.Drawing.Drawing2D;
using static Steam_Authenticator.Internal.Utils;

namespace Steam_Authenticator.Forms
{
    public partial class Confirmations : Form
    {
        private readonly Form preForm;
        private readonly SteamCommunityClient webClient;
        private bool refreshing = false;

        public Confirmations(Form preForm, SteamCommunityClient webClient)
        {
            InitializeComponent();
            this.preForm = preForm;
            this.webClient = webClient;

            Width = this.preForm.Width;
            Height = this.preForm.Height;
        }

        private async void Confirmations_Load(object sender, EventArgs e)
        {
            Location = this.preForm.Location;
            preForm.Hide();

            if (!webClient.LoggedIn)
            {
                return;
            }

            Text = $"令牌确认 [{webClient.Account}]";

            await RefreshConfirmations(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Enabled = true;

            ConfirmationsView.Panel2.AutoScroll = true;
        }
       
        private void Confirmations_FormClosed(object sender, FormClosedEventArgs e)
        {
            preForm.Show();
        }

        private async void autoRefreshTimer_Tick(object sender, EventArgs e)
        {
            return;

            autoRefreshTimer.Enabled = false;

            await RefreshConfirmations(false, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);

            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Enabled = true;
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            if (!webClient.LoggedIn)
            {
                MessageBox.Show("请先登录Steam帐号");
                return;
            }

            await RefreshConfirmations(true, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            var button = (ConfirmationButton)sender;
            try
            {
                button.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = button.Confirmation;

                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool accept = await HandleConfirmation(webClient, guard, new[] { confirmation }, true, cts.Token);
                    if (!accept)
                    {
                        MessageBox.Show("操作失败,请重新操作");
                    }
                }

                await RefreshConfirmations(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button.Enabled = true;
            }
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            var button = (ConfirmationButton)sender;
            try
            {
                button.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = button.Confirmation;
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool cancel = await HandleConfirmation(webClient, guard, new[] { confirmation }, false, cts.Token);
                    if (!cancel)
                    {
                        MessageBox.Show("操作失败,请重新操作");
                    }
                }

                await RefreshConfirmations(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                button.Enabled = true;
            }
        }

        private async void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                var button = (ConfirmationButton)sender;
                var confirmation = button.Confirmation;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Browser browser = new Browser()
                {
                    Width = 600,
                    Height = 400
                };
                var detail = await webClient.Confirmation.ConfirmationDetailAsync(confirmation.Id, guard.DeviceId, guard.IdentitySecret);
                browser.Text = confirmation.ConfTypeName;
                browser.Show();
                browser.LoadHtlm(detail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task RefreshConfirmations(bool showError, CancellationToken cancellationToken)
        {
            if (refreshing)
            {
                return;
            }

            try
            {
                refreshing = true;

                refreshBtn.Text = "正在刷新...";
                refreshBtn.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (string.IsNullOrWhiteSpace(guard?.IdentitySecret))
                {
                    if (showError)
                    {
                        MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    using (CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token))
                    {
                        QueryConfirmationsResponse confirm;
                        while (true)
                        {
                            confirm = await webClient.Confirmation.QueryConfirmationsAsync(guard.DeviceId, guard.IdentitySecret);

                            if (tokenSource.IsCancellationRequested || (confirm?.Success ?? false))
                            {
                                break;
                            }
                            await Task.Delay(TimeSpan.FromSeconds(2));
                        }

                        if (confirm == null)
                        {
                            if (showError)
                            {
                                MessageBox.Show("查询失败,请重新刷新");
                            }
                            return;
                        }
                        if (!confirm.Success)
                        {
                            if (showError)
                            {
                                MessageBox.Show($"{confirm.Detail}", confirm.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            return;
                        }

                        ConfirmationsView.Panel2.Controls.Clear();

                        if (confirm.Confirmations.Count == 0)
                        {
                            Label errorLabel = new Label()
                            {
                                Text = "当前没有任何确认信息",
                                AutoSize = false,
                                ForeColor = Color.FromArgb(255, 140, 0),
                                TextAlign = ContentAlignment.TopCenter,
                                Dock = DockStyle.Fill
                            };
                            ConfirmationsView.Panel2.Controls.Add(errorLabel);
                            return;
                        }

                        foreach (var confirmation in confirm.Confirmations)
                        {
                            Panel panel = new Panel() { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding { Bottom = 10 } };
                            panel.Paint += (s, e) =>
                            {
                                using (LinearGradientBrush brush = new LinearGradientBrush(panel.ClientRectangle, Color.FromArgb(255, 228, 181), Color.FromArgb(255, 255, 224), 90F))
                                {
                                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                                }
                            };

                            if (!string.IsNullOrEmpty(confirmation.Icon))
                            {
                                PictureBox pictureBox = new PictureBox() { Width = 60, Height = 60, Location = new Point(20, 20), SizeMode = PictureBoxSizeMode.Zoom };
                                pictureBox.LoadAsync(confirmation.Icon);
                                panel.Controls.Add(pictureBox);
                            }

                            Label nameLabel = new Label()
                            {
                                Text = $"{confirmation.Headline}\n{confirmation.CreatorId}",
                                AutoSize = true,
                                ForeColor = Color.Green,
                                Location = new Point(90, 20),
                                BackColor = Color.Transparent
                            };
                            panel.Controls.Add(nameLabel);

                            Label summaryLabel = new Label()
                            {
                                Text = $"[{new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(confirmation.CreationTime).ToLocalTime():yyyy/MM/dd HH:mm:ss}]" +
                                $"\n{string.Join("\n", confirmation.Summary)}",
                                AutoSize = true,
                                ForeColor = Color.Green,
                                Location = new Point(90, nameLabel.Height + nameLabel.Location.Y + 10),
                                BackColor = Color.Transparent
                            };
                            panel.Controls.Add(summaryLabel);

                            ConfirmationButton acceptButton = new ConfirmationButton()
                            {
                                Text = confirmation.Accept,
                                Location = new Point(90, summaryLabel.Height + summaryLabel.Location.Y + 10),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(102, 153, 255),
                                ForeColor = Color.Snow,
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowOnly,
                                Confirmation = confirmation
                            };
                            acceptButton.Click += btnAccept_Click;
                            panel.Controls.Add(acceptButton);

                            ConfirmationButton cancelButton = new ConfirmationButton()
                            {
                                Text = confirmation.Cancel,
                                Location = new Point(180, summaryLabel.Height + summaryLabel.Location.Y + 10),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(102, 153, 255),
                                ForeColor = Color.Snow,
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowOnly,
                                Confirmation = confirmation
                            };
                            cancelButton.Click += btnCancel_Click;
                            panel.Controls.Add(cancelButton);

                            ConfirmationButton detailButton = new ConfirmationButton()
                            {
                                Text = "查看",
                                Location = new Point(270, summaryLabel.Height + summaryLabel.Location.Y + 10),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(102, 153, 255),
                                ForeColor = Color.Snow,
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowOnly,
                                Confirmation = confirmation
                            };
                            detailButton.Click += btnDetail_Click;
                            panel.Controls.Add(detailButton);

                            ConfirmationsView.Panel2.Controls.Add(panel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (showError)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                refreshing = false;

                refreshBtn.Text = "刷新";
                refreshBtn.Enabled = true;
            }
        }
    }
}

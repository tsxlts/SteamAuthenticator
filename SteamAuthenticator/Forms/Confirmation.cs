using SteamKit.Model;
using Steam_Authenticator.Controls;
using Steam_Authenticator.Model;
using System.Drawing.Drawing2D;
using SteamKit.WebClient;

namespace Steam_Authenticator.Forms
{
    public partial class Confirmation : Form
    {
        private readonly SteamCommunityClient webClient;
        bool refreshing = false;

        public Confirmation(SteamCommunityClient webClient)
        {
            InitializeComponent();
            this.webClient = webClient;
        }

        private async void Confirmation_Load(object sender, EventArgs e)
        {
            if (!webClient.LoggedIn)
            {
                return;
            }

            await RefreshConfirmation(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Enabled = true;

            ConfirmationsView.Panel2.AutoScroll = true;
        }

        private async void autoRefreshTimer_Tick(object sender, EventArgs e)
        {
            return;

            autoRefreshTimer.Enabled = false;

            await RefreshConfirmation(false, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);

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

            await RefreshConfirmation(true, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            var button = (ConfirmationButton)sender;
            try
            {
                button.Enabled = false;

                Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
                if (guard == null)
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = button.Confirmation;

                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool accept;
                    while (true)
                    {
                        accept = await webClient.Confirmation.AllowConfirmationAsync(new[]
                        {
                            new ConfirmationBase
                            {
                                Id = confirmation.Id,
                                Key=confirmation.Key
                            }
                        }, guard.DeviceId, guard.IdentitySecret);

                        if (cts.IsCancellationRequested || accept)
                        {
                            break;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                    if (!accept)
                    {
                        MessageBox.Show("操作失败,请重新操作");
                    }
                }

                await RefreshConfirmation(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
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
                if (guard == null)
                {
                    MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = button.Confirmation;
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    bool cancel;
                    while (true)
                    {
                        cancel = await webClient.Confirmation.CancelConfirmationAsync(new[]
                        {
                            new ConfirmationBase
                            {
                                Id = confirmation.Id,
                                Key=confirmation.Key
                            }
                        }, guard.DeviceId, guard.IdentitySecret);

                        if (cts.IsCancellationRequested || cancel)
                        {
                            break;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                    if (!cancel)
                    {
                        MessageBox.Show("操作失败,请重新操作");
                    }
                }

                await RefreshConfirmation(false, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
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
            var button = (ConfirmationButton)sender;
            var confirmation = button.Confirmation;

            Guard guard = Appsetting.Instance.Manifest.GetGuard(webClient.Account);
            if (guard == null)
            {
                MessageBox.Show($"用户[{webClient.Account}]未提供登录令牌信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Browser browser = new Browser()
            {
                Width = 400,
                Height = 500
            };
            var detail = await webClient.Confirmation.ConfirmationDetailAsync(confirmation.Id, guard.DeviceId, guard.IdentitySecret);
            browser.Text = confirmation.ConfTypeName;
            browser.Show();
            browser.LoadHtlm(detail);
        }

        private async Task RefreshConfirmation(bool showError, CancellationToken cancellationToken)
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
                if (guard == null)
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
                                AutoSize = true,
                                ForeColor = Color.FromArgb(255, 140, 0),
                                Location = new Point(10, 10)
                            };
                            ConfirmationsView.Panel2.Controls.Add(errorLabel);
                            return;
                        }

                        foreach (var confirmation in confirm.Confirmations)
                        {
                            Panel panel = new Panel() { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding { Bottom = 10 } };
                            panel.Paint += (s, e) =>
                            {
                                using (LinearGradientBrush brush = new LinearGradientBrush(panel.ClientRectangle, Color.FromArgb(30, 144, 255), Color.FromArgb(0, 191, 255), 90F))
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
                                ForeColor = Color.Snow,
                                Location = new Point(90, 20),
                                BackColor = Color.Transparent
                            };
                            panel.Controls.Add(nameLabel);

                            ConfirmationButton acceptButton = new ConfirmationButton()
                            {
                                Text = confirmation.Accept,
                                Location = new Point(90, 60),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(30, 144, 255),
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
                                Location = new Point(180, 60),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(30, 144, 255),
                                ForeColor = Color.Snow,
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowOnly,
                                Confirmation = confirmation
                            };
                            cancelButton.Click += btnCancel_Click;
                            panel.Controls.Add(cancelButton);

                            ConfirmationButton detailButton = new ConfirmationButton()
                            {
                                Text = "详情",
                                Location = new Point(270, 60),
                                FlatStyle = FlatStyle.Flat,
                                FlatAppearance = { BorderSize = 0 },
                                BackColor = Color.FromArgb(30, 144, 255),
                                ForeColor = Color.Snow,
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowOnly,
                                Confirmation = confirmation
                            };
                            detailButton.Click += btnDetail_Click;
                            panel.Controls.Add(detailButton);

                            Label summaryLabel = new Label()
                            {
                                Text = $"[{new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(confirmation.CreationTime).ToLocalTime():yyyy/MM/dd HH:mm:ss}]" +
                                $"\n{string.Join("\n", confirmation.Summary)}",
                                AutoSize = true,
                                ForeColor = Color.Snow,
                                Location = new Point(90, 100),
                                BackColor = Color.Transparent
                            };
                            panel.Controls.Add(summaryLabel);

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

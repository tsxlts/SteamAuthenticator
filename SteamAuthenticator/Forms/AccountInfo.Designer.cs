﻿namespace Steam_Authenticator.Forms
{
    partial class AccountInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountInfo));
            copyTradeLinkBtn = new Label();
            tradeLinkBox = new Label();
            label1 = new Label();
            steamNameBox = new Label();
            label3 = new Label();
            steamIdBox = new Label();
            label4 = new Label();
            copySteamIdBtn = new Label();
            copyApikeyBtn = new Label();
            apikeyBox = new Label();
            label6 = new Label();
            label5 = new Label();
            tradeStatusBox = new Label();
            tradeStatusBtn = new Label();
            tradePermissionBox = new Label();
            label7 = new Label();
            loginStatusBox = new Label();
            label9 = new Label();
            guardStatusBox = new Label();
            label10 = new Label();
            bansStatusBox = new Label();
            label8 = new Label();
            tradeLinkLoading = new PictureBox();
            apikeyLoading = new PictureBox();
            tradeStatusLoading = new PictureBox();
            tradePermissionLoading = new PictureBox();
            bansStatusLoading = new PictureBox();
            guardStatusLoading = new PictureBox();
            refreshBtn = new Label();
            steamAccountBox = new Label();
            label11 = new Label();
            emailBox = new Label();
            label12 = new Label();
            accountSettingLoading = new PictureBox();
            phoneBox = new Label();
            label13 = new Label();
            label2 = new Label();
            guardTimeBox = new Label();
            label15 = new Label();
            ((System.ComponentModel.ISupportInitialize)tradeLinkLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)apikeyLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tradeStatusLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tradePermissionLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bansStatusLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)guardStatusLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)accountSettingLoading).BeginInit();
            SuspendLayout();
            // 
            // copyTradeLinkBtn
            // 
            copyTradeLinkBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyTradeLinkBtn.Cursor = Cursors.Hand;
            copyTradeLinkBtn.ForeColor = Color.Gray;
            copyTradeLinkBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyTradeLinkBtn.Location = new Point(482, 181);
            copyTradeLinkBtn.Name = "copyTradeLinkBtn";
            copyTradeLinkBtn.Size = new Size(36, 23);
            copyTradeLinkBtn.TabIndex = 15;
            copyTradeLinkBtn.Text = "复制";
            copyTradeLinkBtn.TextAlign = ContentAlignment.MiddleLeft;
            copyTradeLinkBtn.Click += copyTradeLinkBtn_Click;
            // 
            // tradeLinkBox
            // 
            tradeLinkBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tradeLinkBox.AutoEllipsis = true;
            tradeLinkBox.ImageAlign = ContentAlignment.MiddleLeft;
            tradeLinkBox.Location = new Point(107, 185);
            tradeLinkBox.Name = "tradeLinkBox";
            tradeLinkBox.Size = new Size(351, 16);
            tradeLinkBox.TabIndex = 14;
            tradeLinkBox.Text = "**********";
            tradeLinkBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(16, 181);
            label1.Name = "label1";
            label1.Size = new Size(85, 23);
            label1.TabIndex = 13;
            label1.Text = "交易链接：";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // steamNameBox
            // 
            steamNameBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            steamNameBox.ForeColor = Color.Green;
            steamNameBox.ImageAlign = ContentAlignment.MiddleLeft;
            steamNameBox.Location = new Point(107, 38);
            steamNameBox.Name = "steamNameBox";
            steamNameBox.Size = new Size(336, 23);
            steamNameBox.TabIndex = 17;
            steamNameBox.Text = "**********";
            steamNameBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.ForeColor = Color.Green;
            label3.ImageAlign = ContentAlignment.MiddleLeft;
            label3.Location = new Point(16, 38);
            label3.Name = "label3";
            label3.Size = new Size(85, 23);
            label3.TabIndex = 16;
            label3.Text = "Steam昵称：";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // steamIdBox
            // 
            steamIdBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            steamIdBox.ForeColor = SystemColors.ControlText;
            steamIdBox.ImageAlign = ContentAlignment.MiddleLeft;
            steamIdBox.Location = new Point(107, 66);
            steamIdBox.Name = "steamIdBox";
            steamIdBox.Size = new Size(351, 23);
            steamIdBox.TabIndex = 19;
            steamIdBox.Text = "**********";
            steamIdBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.ForeColor = SystemColors.ControlText;
            label4.ImageAlign = ContentAlignment.MiddleLeft;
            label4.Location = new Point(16, 66);
            label4.Name = "label4";
            label4.Size = new Size(85, 23);
            label4.TabIndex = 18;
            label4.Text = "SteamId：";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // copySteamIdBtn
            // 
            copySteamIdBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copySteamIdBtn.Cursor = Cursors.Hand;
            copySteamIdBtn.ForeColor = Color.Gray;
            copySteamIdBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copySteamIdBtn.Location = new Point(482, 66);
            copySteamIdBtn.Name = "copySteamIdBtn";
            copySteamIdBtn.Size = new Size(36, 23);
            copySteamIdBtn.TabIndex = 20;
            copySteamIdBtn.Text = "复制";
            copySteamIdBtn.TextAlign = ContentAlignment.MiddleLeft;
            copySteamIdBtn.Click += copySteamIdBtn_Click;
            // 
            // copyApikeyBtn
            // 
            copyApikeyBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyApikeyBtn.Cursor = Cursors.Hand;
            copyApikeyBtn.ForeColor = Color.Gray;
            copyApikeyBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyApikeyBtn.Location = new Point(482, 210);
            copyApikeyBtn.Name = "copyApikeyBtn";
            copyApikeyBtn.Size = new Size(36, 23);
            copyApikeyBtn.TabIndex = 23;
            copyApikeyBtn.Text = "复制";
            copyApikeyBtn.TextAlign = ContentAlignment.MiddleLeft;
            copyApikeyBtn.Click += copyApikeyBtn_Click;
            // 
            // apikeyBox
            // 
            apikeyBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            apikeyBox.AutoEllipsis = true;
            apikeyBox.ImageAlign = ContentAlignment.MiddleLeft;
            apikeyBox.Location = new Point(107, 214);
            apikeyBox.Name = "apikeyBox";
            apikeyBox.Size = new Size(351, 16);
            apikeyBox.TabIndex = 22;
            apikeyBox.Text = "**********";
            apikeyBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.ImageAlign = ContentAlignment.MiddleLeft;
            label6.Location = new Point(16, 210);
            label6.Name = "label6";
            label6.Size = new Size(85, 23);
            label6.TabIndex = 21;
            label6.Text = "ApiKey：";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.ForeColor = SystemColors.ControlText;
            label5.ImageAlign = ContentAlignment.MiddleLeft;
            label5.Location = new Point(16, 239);
            label5.Name = "label5";
            label5.Size = new Size(85, 23);
            label5.TabIndex = 24;
            label5.Text = "交易状态：";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tradeStatusBox
            // 
            tradeStatusBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tradeStatusBox.AutoEllipsis = true;
            tradeStatusBox.ImageAlign = ContentAlignment.MiddleLeft;
            tradeStatusBox.Location = new Point(107, 243);
            tradeStatusBox.Name = "tradeStatusBox";
            tradeStatusBox.Size = new Size(351, 16);
            tradeStatusBox.TabIndex = 25;
            tradeStatusBox.Text = "**********";
            tradeStatusBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tradeStatusBtn
            // 
            tradeStatusBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tradeStatusBtn.Cursor = Cursors.Hand;
            tradeStatusBtn.ForeColor = Color.Gray;
            tradeStatusBtn.ImageAlign = ContentAlignment.MiddleLeft;
            tradeStatusBtn.Location = new Point(482, 239);
            tradeStatusBtn.Name = "tradeStatusBtn";
            tradeStatusBtn.Size = new Size(36, 23);
            tradeStatusBtn.TabIndex = 26;
            tradeStatusBtn.Text = "详情";
            tradeStatusBtn.TextAlign = ContentAlignment.MiddleLeft;
            tradeStatusBtn.Click += tradeStatusBtn_Click;
            // 
            // tradePermissionBox
            // 
            tradePermissionBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tradePermissionBox.AutoEllipsis = true;
            tradePermissionBox.ImageAlign = ContentAlignment.MiddleLeft;
            tradePermissionBox.Location = new Point(107, 271);
            tradePermissionBox.Name = "tradePermissionBox";
            tradePermissionBox.Size = new Size(351, 16);
            tradePermissionBox.TabIndex = 28;
            tradePermissionBox.Text = "**********";
            tradePermissionBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.ForeColor = SystemColors.ControlText;
            label7.ImageAlign = ContentAlignment.MiddleLeft;
            label7.Location = new Point(16, 267);
            label7.Name = "label7";
            label7.Size = new Size(85, 23);
            label7.TabIndex = 27;
            label7.Text = "交易权限：";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // loginStatusBox
            // 
            loginStatusBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            loginStatusBox.ForeColor = SystemColors.ControlText;
            loginStatusBox.ImageAlign = ContentAlignment.MiddleLeft;
            loginStatusBox.Location = new Point(107, 95);
            loginStatusBox.Name = "loginStatusBox";
            loginStatusBox.Size = new Size(351, 23);
            loginStatusBox.TabIndex = 30;
            loginStatusBox.Text = "**********";
            loginStatusBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.ForeColor = SystemColors.ControlText;
            label9.ImageAlign = ContentAlignment.MiddleLeft;
            label9.Location = new Point(16, 95);
            label9.Name = "label9";
            label9.Size = new Size(85, 23);
            label9.TabIndex = 29;
            label9.Text = "登录状态：";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // guardStatusBox
            // 
            guardStatusBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            guardStatusBox.AutoEllipsis = true;
            guardStatusBox.ImageAlign = ContentAlignment.MiddleLeft;
            guardStatusBox.Location = new Point(107, 157);
            guardStatusBox.Name = "guardStatusBox";
            guardStatusBox.Size = new Size(126, 16);
            guardStatusBox.TabIndex = 32;
            guardStatusBox.Text = "**********";
            guardStatusBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            label10.ForeColor = SystemColors.ControlText;
            label10.ImageAlign = ContentAlignment.MiddleLeft;
            label10.Location = new Point(16, 153);
            label10.Name = "label10";
            label10.Size = new Size(85, 23);
            label10.TabIndex = 31;
            label10.Text = "帐号安全：";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // bansStatusBox
            // 
            bansStatusBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            bansStatusBox.AutoEllipsis = true;
            bansStatusBox.ImageAlign = ContentAlignment.MiddleLeft;
            bansStatusBox.Location = new Point(107, 299);
            bansStatusBox.Name = "bansStatusBox";
            bansStatusBox.Size = new Size(351, 16);
            bansStatusBox.TabIndex = 34;
            bansStatusBox.Text = "**********";
            bansStatusBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.ForeColor = SystemColors.ControlText;
            label8.ImageAlign = ContentAlignment.MiddleLeft;
            label8.Location = new Point(16, 295);
            label8.Name = "label8";
            label8.Size = new Size(85, 23);
            label8.TabIndex = 33;
            label8.Text = "帐号封禁：";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tradeLinkLoading
            // 
            tradeLinkLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tradeLinkLoading.BackColor = SystemColors.Control;
            tradeLinkLoading.Image = Properties.Resources.loading_full;
            tradeLinkLoading.Location = new Point(464, 185);
            tradeLinkLoading.Name = "tradeLinkLoading";
            tradeLinkLoading.Size = new Size(16, 16);
            tradeLinkLoading.SizeMode = PictureBoxSizeMode.Zoom;
            tradeLinkLoading.TabIndex = 35;
            tradeLinkLoading.TabStop = false;
            tradeLinkLoading.Visible = false;
            // 
            // apikeyLoading
            // 
            apikeyLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            apikeyLoading.BackColor = SystemColors.Control;
            apikeyLoading.Image = Properties.Resources.loading_full;
            apikeyLoading.Location = new Point(464, 214);
            apikeyLoading.Name = "apikeyLoading";
            apikeyLoading.Size = new Size(16, 16);
            apikeyLoading.SizeMode = PictureBoxSizeMode.Zoom;
            apikeyLoading.TabIndex = 36;
            apikeyLoading.TabStop = false;
            apikeyLoading.Visible = false;
            // 
            // tradeStatusLoading
            // 
            tradeStatusLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tradeStatusLoading.BackColor = SystemColors.Control;
            tradeStatusLoading.Image = Properties.Resources.loading_full;
            tradeStatusLoading.Location = new Point(464, 243);
            tradeStatusLoading.Name = "tradeStatusLoading";
            tradeStatusLoading.Size = new Size(16, 16);
            tradeStatusLoading.SizeMode = PictureBoxSizeMode.Zoom;
            tradeStatusLoading.TabIndex = 37;
            tradeStatusLoading.TabStop = false;
            tradeStatusLoading.Visible = false;
            // 
            // tradePermissionLoading
            // 
            tradePermissionLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tradePermissionLoading.BackColor = SystemColors.Control;
            tradePermissionLoading.Image = Properties.Resources.loading_full;
            tradePermissionLoading.Location = new Point(464, 271);
            tradePermissionLoading.Name = "tradePermissionLoading";
            tradePermissionLoading.Size = new Size(16, 16);
            tradePermissionLoading.SizeMode = PictureBoxSizeMode.Zoom;
            tradePermissionLoading.TabIndex = 38;
            tradePermissionLoading.TabStop = false;
            tradePermissionLoading.Visible = false;
            // 
            // bansStatusLoading
            // 
            bansStatusLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            bansStatusLoading.BackColor = SystemColors.Control;
            bansStatusLoading.Image = Properties.Resources.loading_full;
            bansStatusLoading.Location = new Point(464, 299);
            bansStatusLoading.Name = "bansStatusLoading";
            bansStatusLoading.Size = new Size(16, 16);
            bansStatusLoading.SizeMode = PictureBoxSizeMode.Zoom;
            bansStatusLoading.TabIndex = 39;
            bansStatusLoading.TabStop = false;
            bansStatusLoading.Visible = false;
            // 
            // guardStatusLoading
            // 
            guardStatusLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guardStatusLoading.BackColor = SystemColors.Control;
            guardStatusLoading.Image = Properties.Resources.loading_full;
            guardStatusLoading.Location = new Point(464, 157);
            guardStatusLoading.Name = "guardStatusLoading";
            guardStatusLoading.Size = new Size(16, 16);
            guardStatusLoading.SizeMode = PictureBoxSizeMode.Zoom;
            guardStatusLoading.TabIndex = 40;
            guardStatusLoading.TabStop = false;
            guardStatusLoading.Visible = false;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            refreshBtn.Cursor = Cursors.Hand;
            refreshBtn.ForeColor = Color.DeepSkyBlue;
            refreshBtn.Location = new Point(458, 9);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(60, 23);
            refreshBtn.TabIndex = 41;
            refreshBtn.Text = "重新检测";
            refreshBtn.TextAlign = ContentAlignment.MiddleCenter;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // steamAccountBox
            // 
            steamAccountBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            steamAccountBox.ForeColor = Color.Green;
            steamAccountBox.ImageAlign = ContentAlignment.MiddleLeft;
            steamAccountBox.Location = new Point(107, 9);
            steamAccountBox.Name = "steamAccountBox";
            steamAccountBox.Size = new Size(336, 23);
            steamAccountBox.TabIndex = 16;
            steamAccountBox.Text = "**********";
            steamAccountBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            label11.ForeColor = Color.Green;
            label11.ImageAlign = ContentAlignment.MiddleLeft;
            label11.Location = new Point(16, 9);
            label11.Name = "label11";
            label11.Size = new Size(85, 23);
            label11.TabIndex = 42;
            label11.Text = "登录帐号名：";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // emailBox
            // 
            emailBox.AutoEllipsis = true;
            emailBox.ForeColor = SystemColors.ControlText;
            emailBox.ImageAlign = ContentAlignment.MiddleLeft;
            emailBox.Location = new Point(157, 127);
            emailBox.Name = "emailBox";
            emailBox.Size = new Size(128, 16);
            emailBox.TabIndex = 44;
            emailBox.Text = "**********";
            emailBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.ForeColor = SystemColors.ControlText;
            label12.ImageAlign = ContentAlignment.MiddleLeft;
            label12.Location = new Point(16, 124);
            label12.Name = "label12";
            label12.Size = new Size(85, 23);
            label12.TabIndex = 43;
            label12.Text = "联系信息：";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // accountSettingLoading
            // 
            accountSettingLoading.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            accountSettingLoading.BackColor = SystemColors.Control;
            accountSettingLoading.Image = Properties.Resources.loading_full;
            accountSettingLoading.Location = new Point(464, 127);
            accountSettingLoading.Name = "accountSettingLoading";
            accountSettingLoading.Size = new Size(16, 16);
            accountSettingLoading.SizeMode = PictureBoxSizeMode.Zoom;
            accountSettingLoading.TabIndex = 45;
            accountSettingLoading.TabStop = false;
            accountSettingLoading.Visible = false;
            // 
            // phoneBox
            // 
            phoneBox.AutoEllipsis = true;
            phoneBox.ForeColor = SystemColors.ControlText;
            phoneBox.ImageAlign = ContentAlignment.MiddleLeft;
            phoneBox.Location = new Point(374, 127);
            phoneBox.Name = "phoneBox";
            phoneBox.Size = new Size(84, 16);
            phoneBox.TabIndex = 46;
            phoneBox.Text = "**********";
            phoneBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            label13.ForeColor = Color.RoyalBlue;
            label13.ImageAlign = ContentAlignment.MiddleLeft;
            label13.Location = new Point(309, 124);
            label13.Name = "label13";
            label13.Size = new Size(59, 23);
            label13.TabIndex = 47;
            label13.Text = "手机号：";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.ForeColor = Color.RoyalBlue;
            label2.ImageAlign = ContentAlignment.MiddleLeft;
            label2.Location = new Point(107, 124);
            label2.Name = "label2";
            label2.Size = new Size(44, 23);
            label2.TabIndex = 48;
            label2.Text = "邮箱：";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // guardTimeBox
            // 
            guardTimeBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guardTimeBox.AutoEllipsis = true;
            guardTimeBox.ImageAlign = ContentAlignment.MiddleLeft;
            guardTimeBox.Location = new Point(321, 157);
            guardTimeBox.Name = "guardTimeBox";
            guardTimeBox.Size = new Size(137, 16);
            guardTimeBox.TabIndex = 50;
            guardTimeBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label15.ForeColor = SystemColors.ControlText;
            label15.ImageAlign = ContentAlignment.MiddleLeft;
            label15.Location = new Point(239, 153);
            label15.Name = "label15";
            label15.Size = new Size(76, 23);
            label15.TabIndex = 49;
            label15.Text = "绑定时间：";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AccountInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 338);
            Controls.Add(guardTimeBox);
            Controls.Add(label15);
            Controls.Add(label2);
            Controls.Add(label13);
            Controls.Add(phoneBox);
            Controls.Add(accountSettingLoading);
            Controls.Add(emailBox);
            Controls.Add(label12);
            Controls.Add(steamAccountBox);
            Controls.Add(label11);
            Controls.Add(refreshBtn);
            Controls.Add(guardStatusLoading);
            Controls.Add(bansStatusLoading);
            Controls.Add(tradePermissionLoading);
            Controls.Add(tradeStatusLoading);
            Controls.Add(apikeyLoading);
            Controls.Add(tradeLinkLoading);
            Controls.Add(bansStatusBox);
            Controls.Add(label8);
            Controls.Add(guardStatusBox);
            Controls.Add(label10);
            Controls.Add(loginStatusBox);
            Controls.Add(label9);
            Controls.Add(tradePermissionBox);
            Controls.Add(label7);
            Controls.Add(tradeStatusBtn);
            Controls.Add(tradeStatusBox);
            Controls.Add(label5);
            Controls.Add(copyApikeyBtn);
            Controls.Add(apikeyBox);
            Controls.Add(label6);
            Controls.Add(copySteamIdBtn);
            Controls.Add(steamIdBox);
            Controls.Add(label4);
            Controls.Add(steamNameBox);
            Controls.Add(label3);
            Controls.Add(copyTradeLinkBtn);
            Controls.Add(tradeLinkBox);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(536, 377);
            Name = "AccountInfo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "帐号信息";
            Load += AccountInfo_Load;
            ((System.ComponentModel.ISupportInitialize)tradeLinkLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)apikeyLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)tradeStatusLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)tradePermissionLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)bansStatusLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)guardStatusLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)accountSettingLoading).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label copyTradeLinkBtn;
        private Label tradeLinkBox;
        private Label label1;
        private Label steamNameBox;
        private Label label3;
        private Label steamIdBox;
        private Label label4;
        private Label copySteamIdBtn;
        private Label copyApikeyBtn;
        private Label apikeyBox;
        private Label label6;
        private Label label5;
        private Label tradeStatusBox;
        private Label tradeStatusBtn;
        private Label tradePermissionBox;
        private Label label7;
        private Label loginStatusBox;
        private Label label9;
        private Label guardStatusBox;
        private Label label10;
        private Label bansStatusBox;
        private Label label8;
        private PictureBox tradeLinkLoading;
        private PictureBox apikeyLoading;
        private PictureBox tradeStatusLoading;
        private PictureBox tradePermissionLoading;
        private PictureBox bansStatusLoading;
        private PictureBox guardStatusLoading;
        private Label refreshBtn;
        private Label steamAccountBox;
        private Label label11;
        private Label emailBox;
        private Label label12;
        private PictureBox accountSettingLoading;
        private Label phoneBox;
        private Label label13;
        private Label label2;
        private Label guardTimeBox;
        private Label label15;
    }
}
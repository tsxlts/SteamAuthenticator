namespace Steam_Authenticator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            UserToolStripMenuItem = new ToolStripMenuItem();
            settingMenuItem = new ToolStripMenuItem();
            proxySettingMenuItem = new ToolStripMenuItem();
            passwordMenuItem = new ToolStripMenuItem();
            copyCookieMenuItem = new ToolStripMenuItem();
            copyRefreshTokenMenuItem = new ToolStripMenuItem();
            copyAccessTokenToolItem = new ToolStripMenuItem();
            checkVersionMenuItem = new ToolStripMenuItem();
            quitMenuItem = new ToolStripMenuItem();
            authenticatorMenu = new ToolStripMenuItem();
            guardMenuItem = new ToolStripMenuItem();
            confirmMenuItem = new ToolStripMenuItem();
            addAuthenticatorMenuItem = new ToolStripMenuItem();
            moveAuthenticatorMenuItem = new ToolStripMenuItem();
            importAuthenticatorMenuItem = new ToolStripMenuItem();
            importFileAuthenticatorMenuItem = new ToolStripMenuItem();
            importSecretAuthenticatorMenuItem = new ToolStripMenuItem();
            removeAuthenticatorMenuItem = new ToolStripMenuItem();
            UserImg = new PictureBox();
            UserName = new Label();
            Balance = new Label();
            panel1 = new Panel();
            confirmationBtn = new Label();
            declineOfferBtn = new Label();
            acceptOfferBtn = new Label();
            offersBtn = new Label();
            ConfirmationCountLable = new Label();
            OfferCountLabel = new Label();
            label2 = new Label();
            label1 = new Label();
            usersPanel = new Panel();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            versionLabel = new LinkLabel();
            label6 = new Label();
            label7 = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)UserImg).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { UserToolStripMenuItem, authenticatorMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(684, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // UserToolStripMenuItem
            // 
            UserToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingMenuItem, proxySettingMenuItem, passwordMenuItem, copyCookieMenuItem, copyRefreshTokenMenuItem, copyAccessTokenToolItem, checkVersionMenuItem, quitMenuItem });
            UserToolStripMenuItem.Name = "UserToolStripMenuItem";
            UserToolStripMenuItem.Size = new Size(44, 21);
            UserToolStripMenuItem.Text = "文件";
            // 
            // settingMenuItem
            // 
            settingMenuItem.Name = "settingMenuItem";
            settingMenuItem.Size = new Size(180, 22);
            settingMenuItem.Text = "用户设置";
            settingMenuItem.Click += settingMenuItem_Click;
            // 
            // proxySettingMenuItem
            // 
            proxySettingMenuItem.Name = "proxySettingMenuItem";
            proxySettingMenuItem.Size = new Size(180, 22);
            proxySettingMenuItem.Text = "代理设置";
            proxySettingMenuItem.Click += proxySettingMenuItem_Click;
            // 
            // passwordMenuItem
            // 
            passwordMenuItem.Name = "passwordMenuItem";
            passwordMenuItem.Size = new Size(180, 22);
            passwordMenuItem.Text = "访问密码";
            passwordMenuItem.Click += passwordMenuItem_Click;
            // 
            // copyCookieMenuItem
            // 
            copyCookieMenuItem.Enabled = false;
            copyCookieMenuItem.Name = "copyCookieMenuItem";
            copyCookieMenuItem.Size = new Size(180, 22);
            copyCookieMenuItem.Text = "复制Cookie";
            copyCookieMenuItem.Click += copyCookieMenuItem_Click;
            // 
            // copyRefreshTokenMenuItem
            // 
            copyRefreshTokenMenuItem.Enabled = false;
            copyRefreshTokenMenuItem.Name = "copyRefreshTokenMenuItem";
            copyRefreshTokenMenuItem.Size = new Size(180, 22);
            copyRefreshTokenMenuItem.Text = "复制RefreshToken";
            copyRefreshTokenMenuItem.Click += copyRefreshTokenMenuItem_Click;
            // 
            // copyAccessTokenToolItem
            // 
            copyAccessTokenToolItem.Enabled = false;
            copyAccessTokenToolItem.Name = "copyAccessTokenToolItem";
            copyAccessTokenToolItem.Size = new Size(180, 22);
            copyAccessTokenToolItem.Text = "复制AccessToken";
            copyAccessTokenToolItem.Click += copyAccessTokenMenuItem_Click;
            // 
            // checkVersionMenuItem
            // 
            checkVersionMenuItem.Name = "checkVersionMenuItem";
            checkVersionMenuItem.Size = new Size(180, 22);
            checkVersionMenuItem.Text = "检查更新";
            checkVersionMenuItem.Click += checkVersionMenuItem_Click;
            // 
            // quitMenuItem
            // 
            quitMenuItem.Name = "quitMenuItem";
            quitMenuItem.Size = new Size(180, 22);
            quitMenuItem.Text = "退出";
            quitMenuItem.Click += quitMenuItem_Click;
            // 
            // authenticatorMenu
            // 
            authenticatorMenu.DropDownItems.AddRange(new ToolStripItem[] { guardMenuItem, confirmMenuItem, addAuthenticatorMenuItem, moveAuthenticatorMenuItem, importAuthenticatorMenuItem, removeAuthenticatorMenuItem });
            authenticatorMenu.Name = "authenticatorMenu";
            authenticatorMenu.Size = new Size(80, 21);
            authenticatorMenu.Text = "令牌验证器";
            // 
            // guardMenuItem
            // 
            guardMenuItem.Name = "guardMenuItem";
            guardMenuItem.Size = new Size(124, 22);
            guardMenuItem.Text = "令牌";
            guardMenuItem.Click += guardMenuItem_Click;
            // 
            // confirmMenuItem
            // 
            confirmMenuItem.Name = "confirmMenuItem";
            confirmMenuItem.Size = new Size(124, 22);
            confirmMenuItem.Text = "确认";
            confirmMenuItem.Click += confirmMenuItem_Click;
            // 
            // addAuthenticatorMenuItem
            // 
            addAuthenticatorMenuItem.Name = "addAuthenticatorMenuItem";
            addAuthenticatorMenuItem.Size = new Size(124, 22);
            addAuthenticatorMenuItem.Text = "添加令牌";
            addAuthenticatorMenuItem.Click += addAuthenticatorMenuItem_Click;
            // 
            // moveAuthenticatorMenuItem
            // 
            moveAuthenticatorMenuItem.Name = "moveAuthenticatorMenuItem";
            moveAuthenticatorMenuItem.Size = new Size(124, 22);
            moveAuthenticatorMenuItem.Text = "移动令牌";
            moveAuthenticatorMenuItem.Click += moveAuthenticatorMenuItem_Click;
            // 
            // importAuthenticatorMenuItem
            // 
            importAuthenticatorMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importFileAuthenticatorMenuItem, importSecretAuthenticatorMenuItem });
            importAuthenticatorMenuItem.Name = "importAuthenticatorMenuItem";
            importAuthenticatorMenuItem.Size = new Size(124, 22);
            importAuthenticatorMenuItem.Text = "导入令牌";
            // 
            // importFileAuthenticatorMenuItem
            // 
            importFileAuthenticatorMenuItem.Name = "importFileAuthenticatorMenuItem";
            importFileAuthenticatorMenuItem.Size = new Size(124, 22);
            importFileAuthenticatorMenuItem.Text = "文件导入";
            importFileAuthenticatorMenuItem.Click += importFileAuthenticatorMenuItem_Click;
            // 
            // importSecretAuthenticatorMenuItem
            // 
            importSecretAuthenticatorMenuItem.Name = "importSecretAuthenticatorMenuItem";
            importSecretAuthenticatorMenuItem.Size = new Size(124, 22);
            importSecretAuthenticatorMenuItem.Text = "秘钥导入";
            importSecretAuthenticatorMenuItem.Click += importSecretAuthenticatorMenuItem_Click;
            // 
            // removeAuthenticatorMenuItem
            // 
            removeAuthenticatorMenuItem.Name = "removeAuthenticatorMenuItem";
            removeAuthenticatorMenuItem.Size = new Size(124, 22);
            removeAuthenticatorMenuItem.Text = "解绑令牌";
            removeAuthenticatorMenuItem.Click += removeAuthenticatorMenuItem_Click;
            // 
            // UserImg
            // 
            UserImg.Image = Properties.Resources.userimg;
            UserImg.InitialImage = Properties.Resources.loading;
            UserImg.Location = new Point(12, 38);
            UserImg.Name = "UserImg";
            UserImg.Size = new Size(100, 100);
            UserImg.SizeMode = PictureBoxSizeMode.Zoom;
            UserImg.TabIndex = 1;
            UserImg.TabStop = false;
            // 
            // UserName
            // 
            UserName.AutoEllipsis = true;
            UserName.Location = new Point(118, 40);
            UserName.Name = "UserName";
            UserName.Size = new Size(200, 23);
            UserName.TabIndex = 2;
            UserName.Text = "---";
            UserName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Balance
            // 
            Balance.AutoEllipsis = true;
            Balance.ForeColor = Color.Green;
            Balance.Location = new Point(118, 68);
            Balance.Name = "Balance";
            Balance.Size = new Size(200, 23);
            Balance.TabIndex = 4;
            Balance.Text = "￥0.00";
            Balance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel1.Controls.Add(confirmationBtn);
            panel1.Controls.Add(declineOfferBtn);
            panel1.Controls.Add(acceptOfferBtn);
            panel1.Controls.Add(offersBtn);
            panel1.Controls.Add(ConfirmationCountLable);
            panel1.Controls.Add(OfferCountLabel);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(464, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(208, 100);
            panel1.TabIndex = 6;
            // 
            // confirmationBtn
            // 
            confirmationBtn.AutoSize = true;
            confirmationBtn.Cursor = Cursors.Hand;
            confirmationBtn.ForeColor = Color.Green;
            confirmationBtn.Location = new Point(97, 33);
            confirmationBtn.Name = "confirmationBtn";
            confirmationBtn.Size = new Size(32, 17);
            confirmationBtn.TabIndex = 7;
            confirmationBtn.Text = "查看";
            confirmationBtn.Click += confirmMenuItem_Click;
            // 
            // declineOfferBtn
            // 
            declineOfferBtn.AutoSize = true;
            declineOfferBtn.Cursor = Cursors.Hand;
            declineOfferBtn.ForeColor = Color.Red;
            declineOfferBtn.Location = new Point(173, 5);
            declineOfferBtn.Name = "declineOfferBtn";
            declineOfferBtn.Size = new Size(32, 17);
            declineOfferBtn.TabIndex = 6;
            declineOfferBtn.Text = "拒绝";
            declineOfferBtn.Click += declineOfferBtn_Click;
            // 
            // acceptOfferBtn
            // 
            acceptOfferBtn.AutoSize = true;
            acceptOfferBtn.Cursor = Cursors.Hand;
            acceptOfferBtn.ForeColor = Color.Green;
            acceptOfferBtn.Location = new Point(136, 5);
            acceptOfferBtn.Name = "acceptOfferBtn";
            acceptOfferBtn.Size = new Size(32, 17);
            acceptOfferBtn.TabIndex = 5;
            acceptOfferBtn.Text = "接受";
            acceptOfferBtn.Click += acceptOfferBtn_Click;
            // 
            // offersBtn
            // 
            offersBtn.AutoSize = true;
            offersBtn.Cursor = Cursors.Hand;
            offersBtn.ForeColor = Color.Green;
            offersBtn.Location = new Point(97, 5);
            offersBtn.Name = "offersBtn";
            offersBtn.Size = new Size(32, 17);
            offersBtn.TabIndex = 4;
            offersBtn.Text = "查看";
            offersBtn.Click += offersBtn_Click;
            // 
            // ConfirmationCountLable
            // 
            ConfirmationCountLable.AutoSize = true;
            ConfirmationCountLable.ForeColor = Color.FromArgb(0, 128, 255);
            ConfirmationCountLable.Location = new Point(44, 33);
            ConfirmationCountLable.Name = "ConfirmationCountLable";
            ConfirmationCountLable.Size = new Size(23, 17);
            ConfirmationCountLable.TabIndex = 3;
            ConfirmationCountLable.Text = "---";
            ConfirmationCountLable.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // OfferCountLabel
            // 
            OfferCountLabel.AutoSize = true;
            OfferCountLabel.ForeColor = Color.FromArgb(255, 128, 0);
            OfferCountLabel.Location = new Point(44, 5);
            OfferCountLabel.Name = "OfferCountLabel";
            OfferCountLabel.Size = new Size(23, 17);
            OfferCountLabel.TabIndex = 2;
            OfferCountLabel.Text = "---";
            OfferCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(0, 128, 255);
            label2.Location = new Point(3, 33);
            label2.Name = "label2";
            label2.Size = new Size(35, 17);
            label2.TabIndex = 1;
            label2.Text = "确认:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(255, 128, 0);
            label1.Location = new Point(3, 5);
            label1.Name = "label1";
            label1.Size = new Size(35, 17);
            label1.TabIndex = 0;
            label1.Text = "报价:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // usersPanel
            // 
            usersPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            usersPanel.AutoScroll = true;
            usersPanel.BorderStyle = BorderStyle.FixedSingle;
            usersPanel.Location = new Point(12, 156);
            usersPanel.Name = "usersPanel";
            usersPanel.Size = new Size(660, 297);
            usersPanel.TabIndex = 7;
            usersPanel.SizeChanged += UsersPanel_SizeChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.ForeColor = Color.Green;
            label3.Location = new Point(12, 456);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 8;
            label3.Text = "登录成功";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.ForeColor = Color.Red;
            label4.Location = new Point(74, 456);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 9;
            label4.Text = "登录失败";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(136, 456);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 10;
            label5.Text = "正在登录";
            // 
            // versionLabel
            // 
            versionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            versionLabel.Location = new Point(572, 456);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(100, 23);
            versionLabel.TabIndex = 12;
            versionLabel.TabStop = true;
            versionLabel.Text = "v1.0.0";
            versionLabel.TextAlign = ContentAlignment.TopRight;
            versionLabel.LinkClicked += versionLabel_LinkClicked;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label6.AutoSize = true;
            label6.ForeColor = Color.FromArgb(255, 128, 0);
            label6.Location = new Point(198, 456);
            label6.Name = "label6";
            label6.Size = new Size(44, 17);
            label6.TabIndex = 13;
            label6.Text = "报价数";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label7.AutoSize = true;
            label7.ForeColor = Color.FromArgb(0, 128, 255);
            label7.Location = new Point(248, 456);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 14;
            label7.Text = "确认数";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 479);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(versionLabel);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(usersPanel);
            Controls.Add(panel1);
            Controls.Add(Balance);
            Controls.Add(UserName);
            Controls.Add(UserImg);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(700, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Steam验证器";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)UserImg).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem UserToolStripMenuItem;
        private PictureBox UserImg;
        private Label UserName;
        private ToolStripMenuItem copyCookieMenuItem;
        private ToolStripMenuItem copyRefreshTokenMenuItem;
        private Label Balance;
        private Panel panel1;
        private Label ConfirmationCountLable;
        private Label OfferCountLabel;
        private Label label2;
        private Label label1;
        private ToolStripMenuItem copyAccessTokenToolItem;
        private ToolStripMenuItem authenticatorMenu;
        private ToolStripMenuItem guardMenuItem;
        private ToolStripMenuItem addAuthenticatorMenuItem;
        private ToolStripMenuItem removeAuthenticatorMenuItem;
        private ToolStripMenuItem confirmMenuItem;
        private ToolStripMenuItem importAuthenticatorMenuItem;
        private ToolStripMenuItem moveAuthenticatorMenuItem;
        private ToolStripMenuItem proxySettingMenuItem;
        private ToolStripMenuItem passwordMenuItem;
        private ToolStripMenuItem settingMenuItem;
        private Label offersBtn;
        private Label acceptOfferBtn;
        private Label declineOfferBtn;
        private Label confirmationBtn;
        private Panel usersPanel;
        private ToolStripMenuItem checkVersionMenuItem;
        private ToolStripMenuItem quitMenuItem;
        private Label label3;
        private Label label4;
        private Label label5;
        private LinkLabel versionLabel;
        private Label label6;
        private Label label7;
        private ToolStripMenuItem importFileAuthenticatorMenuItem;
        private ToolStripMenuItem importSecretAuthenticatorMenuItem;
    }
}

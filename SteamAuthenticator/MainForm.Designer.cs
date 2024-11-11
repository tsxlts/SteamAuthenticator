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
            checkVersionMenuItem = new ToolStripMenuItem();
            quitMenuItem = new ToolStripMenuItem();
            authenticatorMenu = new ToolStripMenuItem();
            guardMenuItem = new ToolStripMenuItem();
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
            pictureBox1 = new PictureBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            versionLabel = new LinkLabel();
            label6 = new Label();
            label7 = new Label();
            DelayedBalance = new Label();
            tabControl = new TabControl();
            steamPage = new TabPage();
            buffPage = new TabPage();
            buffUsersPanel = new Panel();
            pictureBox2 = new PictureBox();
            statusPanel = new Panel();
            SteamId = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)UserImg).BeginInit();
            panel1.SuspendLayout();
            usersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl.SuspendLayout();
            steamPage.SuspendLayout();
            buffPage.SuspendLayout();
            buffUsersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            statusPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { UserToolStripMenuItem, authenticatorMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(690, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // UserToolStripMenuItem
            // 
            UserToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingMenuItem, proxySettingMenuItem, passwordMenuItem, checkVersionMenuItem, quitMenuItem });
            UserToolStripMenuItem.Name = "UserToolStripMenuItem";
            UserToolStripMenuItem.Size = new Size(44, 21);
            UserToolStripMenuItem.Text = "文件";
            // 
            // settingMenuItem
            // 
            settingMenuItem.Name = "settingMenuItem";
            settingMenuItem.Size = new Size(124, 22);
            settingMenuItem.Text = "全局设置";
            settingMenuItem.Click += globalSettingMenuItem_Click;
            // 
            // proxySettingMenuItem
            // 
            proxySettingMenuItem.Name = "proxySettingMenuItem";
            proxySettingMenuItem.Size = new Size(124, 22);
            proxySettingMenuItem.Text = "代理设置";
            proxySettingMenuItem.Click += proxySettingMenuItem_Click;
            // 
            // passwordMenuItem
            // 
            passwordMenuItem.Name = "passwordMenuItem";
            passwordMenuItem.Size = new Size(124, 22);
            passwordMenuItem.Text = "访问密码";
            passwordMenuItem.Click += passwordMenuItem_Click;
            // 
            // checkVersionMenuItem
            // 
            checkVersionMenuItem.Name = "checkVersionMenuItem";
            checkVersionMenuItem.Size = new Size(124, 22);
            checkVersionMenuItem.Text = "检查更新";
            checkVersionMenuItem.Click += checkVersionMenuItem_Click;
            // 
            // quitMenuItem
            // 
            quitMenuItem.Name = "quitMenuItem";
            quitMenuItem.Size = new Size(124, 22);
            quitMenuItem.Text = "退出";
            quitMenuItem.Click += quitMenuItem_Click;
            // 
            // authenticatorMenu
            // 
            authenticatorMenu.DropDownItems.AddRange(new ToolStripItem[] { guardMenuItem, addAuthenticatorMenuItem, moveAuthenticatorMenuItem, importAuthenticatorMenuItem, removeAuthenticatorMenuItem });
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
            Balance.Location = new Point(118, 94);
            Balance.Name = "Balance";
            Balance.Size = new Size(200, 18);
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
            panel1.Location = new Point(470, 38);
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
            confirmationBtn.Click += confirmationBtn_Click;
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
            usersPanel.BackgroundImageLayout = ImageLayout.Zoom;
            usersPanel.Controls.Add(pictureBox1);
            usersPanel.Location = new Point(3, 3);
            usersPanel.Name = "usersPanel";
            usersPanel.Size = new Size(656, 274);
            usersPanel.TabIndex = 7;
            usersPanel.SizeChanged += UsersPanel_SizeChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.loading;
            pictureBox1.Location = new Point(278, 87);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.ForeColor = Color.Green;
            label3.Location = new Point(3, 7);
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
            label4.Location = new Point(65, 7);
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
            label5.Location = new Point(127, 7);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 10;
            label5.Text = "正在登录";
            // 
            // versionLabel
            // 
            versionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            versionLabel.Location = new Point(578, 464);
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
            label6.Location = new Point(189, 7);
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
            label7.Location = new Point(239, 7);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 14;
            label7.Text = "确认数";
            // 
            // DelayedBalance
            // 
            DelayedBalance.AutoEllipsis = true;
            DelayedBalance.ForeColor = Color.Gray;
            DelayedBalance.Location = new Point(118, 116);
            DelayedBalance.Name = "DelayedBalance";
            DelayedBalance.Size = new Size(200, 18);
            DelayedBalance.TabIndex = 15;
            DelayedBalance.Text = "￥0.00";
            DelayedBalance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(steamPage);
            tabControl.Controls.Add(buffPage);
            tabControl.Location = new Point(12, 144);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(670, 310);
            tabControl.TabIndex = 16;
            // 
            // steamPage
            // 
            steamPage.Controls.Add(usersPanel);
            steamPage.Location = new Point(4, 26);
            steamPage.Name = "steamPage";
            steamPage.Padding = new Padding(3);
            steamPage.Size = new Size(662, 280);
            steamPage.TabIndex = 0;
            steamPage.Text = "Steam 帐号";
            steamPage.UseVisualStyleBackColor = true;
            // 
            // buffPage
            // 
            buffPage.Controls.Add(buffUsersPanel);
            buffPage.Location = new Point(4, 26);
            buffPage.Name = "buffPage";
            buffPage.Padding = new Padding(3);
            buffPage.Size = new Size(662, 280);
            buffPage.TabIndex = 1;
            buffPage.Text = "BUFF 帐号";
            buffPage.UseVisualStyleBackColor = true;
            // 
            // buffUsersPanel
            // 
            buffUsersPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            buffUsersPanel.AutoScroll = true;
            buffUsersPanel.BackgroundImageLayout = ImageLayout.Zoom;
            buffUsersPanel.Controls.Add(pictureBox2);
            buffUsersPanel.Location = new Point(3, 3);
            buffUsersPanel.Name = "buffUsersPanel";
            buffUsersPanel.Size = new Size(656, 274);
            buffUsersPanel.TabIndex = 8;
            buffUsersPanel.SizeChanged += buffUserPanel_SizeChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.loading;
            pictureBox2.Location = new Point(278, 87);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(100, 100);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // statusPanel
            // 
            statusPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusPanel.Controls.Add(label3);
            statusPanel.Controls.Add(label4);
            statusPanel.Controls.Add(label6);
            statusPanel.Controls.Add(label7);
            statusPanel.Controls.Add(label5);
            statusPanel.Location = new Point(12, 453);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new Size(608, 30);
            statusPanel.TabIndex = 17;
            // 
            // SteamId
            // 
            SteamId.AutoEllipsis = true;
            SteamId.Cursor = Cursors.Hand;
            SteamId.ForeColor = Color.FromArgb(0, 0, 238);
            SteamId.Location = new Point(118, 67);
            SteamId.Name = "SteamId";
            SteamId.Size = new Size(200, 18);
            SteamId.TabIndex = 18;
            SteamId.Text = "---";
            SteamId.TextAlign = ContentAlignment.MiddleLeft;
            SteamId.Click += SteamId_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(690, 487);
            Controls.Add(SteamId);
            Controls.Add(statusPanel);
            Controls.Add(tabControl);
            Controls.Add(DelayedBalance);
            Controls.Add(versionLabel);
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
            usersPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl.ResumeLayout(false);
            steamPage.ResumeLayout(false);
            buffPage.ResumeLayout(false);
            buffUsersPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            statusPanel.ResumeLayout(false);
            statusPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem UserToolStripMenuItem;
        private PictureBox UserImg;
        private Label UserName;
        private Label Balance;
        private Panel panel1;
        private Label ConfirmationCountLable;
        private Label OfferCountLabel;
        private Label label2;
        private Label label1;
        private ToolStripMenuItem authenticatorMenu;
        private ToolStripMenuItem guardMenuItem;
        private ToolStripMenuItem addAuthenticatorMenuItem;
        private ToolStripMenuItem removeAuthenticatorMenuItem;
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
        private Label DelayedBalance;
        private TabControl tabControl;
        private TabPage steamPage;
        private TabPage buffPage;
        private Panel statusPanel;
        private Panel buffUsersPanel;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label SteamId;
    }
}

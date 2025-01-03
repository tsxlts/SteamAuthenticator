using Steam_Authenticator.Controls;

namespace Steam_Authenticator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            UserToolStripMenuItem = new ToolStripMenuItem();
            settingMenuItem = new ToolStripMenuItem();
            proxySettingMenuItem = new ToolStripMenuItem();
            passwordMenuItem = new ToolStripMenuItem();
            checkVersionMenuItem = new ToolStripMenuItem();
            quitMenuItem = new ToolStripMenuItem();
            authenticatorMenuItem = new ToolStripMenuItem();
            guardMenuItem = new ToolStripMenuItem();
            addAuthenticatorMenuItem = new ToolStripMenuItem();
            moveAuthenticatorMenuItem = new ToolStripMenuItem();
            importAuthenticatorMenuItem = new ToolStripMenuItem();
            importFileAuthenticatorMenuItem = new ToolStripMenuItem();
            importSecretAuthenticatorMenuItem = new ToolStripMenuItem();
            exportAuthenticatorMenuItem = new ToolStripMenuItem();
            removeAuthenticatorMenuItem = new ToolStripMenuItem();
            submitRequirementsMenuItem = new ToolStripMenuItem();
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
            usersPanel = new SteamUserCollectionPanel();
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
            buffUsersPanel = new BuffUserCollectionPanel();
            pictureBox2 = new PictureBox();
            ecoPage = new TabPage();
            ecoUsersPanel = new EcoUserCollectionPanel();
            pictureBox6 = new PictureBox();
            statusPanel = new Panel();
            label10 = new Label();
            pictureBox5 = new PictureBox();
            label9 = new Label();
            label8 = new Label();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            SteamId = new Label();
            submitRequirementsLabel = new LinkLabel();
            mainNotifyIcon = new NotifyIcon(components);
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
            ecoPage.SuspendLayout();
            ecoUsersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { UserToolStripMenuItem, authenticatorMenuItem, submitRequirementsMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(764, 25);
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
            // authenticatorMenuItem
            // 
            authenticatorMenuItem.DropDownItems.AddRange(new ToolStripItem[] { guardMenuItem, addAuthenticatorMenuItem, moveAuthenticatorMenuItem, importAuthenticatorMenuItem, exportAuthenticatorMenuItem, removeAuthenticatorMenuItem });
            authenticatorMenuItem.Name = "authenticatorMenuItem";
            authenticatorMenuItem.Size = new Size(80, 21);
            authenticatorMenuItem.Text = "令牌验证器";
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
            // exportAuthenticatorMenuItem
            // 
            exportAuthenticatorMenuItem.Name = "exportAuthenticatorMenuItem";
            exportAuthenticatorMenuItem.Size = new Size(124, 22);
            exportAuthenticatorMenuItem.Text = "导出令牌";
            exportAuthenticatorMenuItem.Click += exportAuthenticatorMenuItem_Click;
            // 
            // removeAuthenticatorMenuItem
            // 
            removeAuthenticatorMenuItem.Name = "removeAuthenticatorMenuItem";
            removeAuthenticatorMenuItem.Size = new Size(124, 22);
            removeAuthenticatorMenuItem.Text = "解绑令牌";
            removeAuthenticatorMenuItem.Click += removeAuthenticatorMenuItem_Click;
            // 
            // submitRequirementsMenuItem
            // 
            submitRequirementsMenuItem.Name = "submitRequirementsMenuItem";
            submitRequirementsMenuItem.Size = new Size(140, 21);
            submitRequirementsMenuItem.Text = "联系我们（提交需求）";
            submitRequirementsMenuItem.Click += submitRequirementsMenuItem_Click;
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
            panel1.Location = new Point(544, 38);
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
            usersPanel.AutoScroll = true;
            usersPanel.BackgroundImageLayout = ImageLayout.Zoom;
            usersPanel.Controls.Add(pictureBox1);
            usersPanel.Dock = DockStyle.Fill;
            usersPanel.Location = new Point(3, 3);
            usersPanel.Name = "usersPanel";
            usersPanel.Size = new Size(730, 298);
            usersPanel.TabIndex = 7;
            usersPanel.SizeChanged += UsersPanel_SizeChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.Image = Properties.Resources.loading;
            pictureBox1.Location = new Point(313, 87);
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
            versionLabel.Location = new Point(692, 481);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(60, 23);
            versionLabel.TabIndex = 12;
            versionLabel.TabStop = true;
            versionLabel.Text = "v1.0.0";
            versionLabel.TextAlign = ContentAlignment.MiddleRight;
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
            tabControl.Controls.Add(ecoPage);
            tabControl.Location = new Point(12, 144);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(744, 334);
            tabControl.TabIndex = 16;
            // 
            // steamPage
            // 
            steamPage.Controls.Add(usersPanel);
            steamPage.Location = new Point(4, 26);
            steamPage.Name = "steamPage";
            steamPage.Padding = new Padding(3);
            steamPage.Size = new Size(736, 304);
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
            buffPage.Size = new Size(736, 304);
            buffPage.TabIndex = 1;
            buffPage.Text = "BUFF 帐号";
            buffPage.UseVisualStyleBackColor = true;
            // 
            // buffUsersPanel
            // 
            buffUsersPanel.AutoScroll = true;
            buffUsersPanel.BackgroundImageLayout = ImageLayout.Zoom;
            buffUsersPanel.Controls.Add(pictureBox2);
            buffUsersPanel.Dock = DockStyle.Fill;
            buffUsersPanel.Location = new Point(3, 3);
            buffUsersPanel.Name = "buffUsersPanel";
            buffUsersPanel.Size = new Size(730, 298);
            buffUsersPanel.TabIndex = 8;
            buffUsersPanel.SizeChanged += buffUserPanel_SizeChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.None;
            pictureBox2.Image = Properties.Resources.loading;
            pictureBox2.Location = new Point(313, 87);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(100, 100);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // ecoPage
            // 
            ecoPage.Controls.Add(ecoUsersPanel);
            ecoPage.Location = new Point(4, 26);
            ecoPage.Name = "ecoPage";
            ecoPage.Padding = new Padding(3);
            ecoPage.Size = new Size(736, 304);
            ecoPage.TabIndex = 2;
            ecoPage.Text = "ECO 帐号";
            ecoPage.UseVisualStyleBackColor = true;
            // 
            // ecoUsersPanel
            // 
            ecoUsersPanel.AutoScroll = true;
            ecoUsersPanel.BackgroundImageLayout = ImageLayout.Zoom;
            ecoUsersPanel.Controls.Add(pictureBox6);
            ecoUsersPanel.Dock = DockStyle.Fill;
            ecoUsersPanel.Location = new Point(3, 3);
            ecoUsersPanel.Name = "ecoUsersPanel";
            ecoUsersPanel.Size = new Size(730, 298);
            ecoUsersPanel.TabIndex = 8;
            // 
            // pictureBox6
            // 
            pictureBox6.Anchor = AnchorStyles.None;
            pictureBox6.Image = Properties.Resources.loading;
            pictureBox6.Location = new Point(313, 87);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(100, 100);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.TabIndex = 0;
            pictureBox6.TabStop = false;
            // 
            // statusPanel
            // 
            statusPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusPanel.Controls.Add(label10);
            statusPanel.Controls.Add(pictureBox5);
            statusPanel.Controls.Add(label9);
            statusPanel.Controls.Add(label8);
            statusPanel.Controls.Add(pictureBox4);
            statusPanel.Controls.Add(pictureBox3);
            statusPanel.Controls.Add(label3);
            statusPanel.Controls.Add(label4);
            statusPanel.Controls.Add(label6);
            statusPanel.Controls.Add(label7);
            statusPanel.Controls.Add(label5);
            statusPanel.Location = new Point(12, 477);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new Size(599, 30);
            statusPanel.TabIndex = 17;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label10.AutoSize = true;
            label10.ForeColor = Color.FromArgb(57, 89, 220);
            label10.Location = new Point(491, 7);
            label10.Name = "label10";
            label10.Size = new Size(56, 17);
            label10.TabIndex = 20;
            label10.Text = "自动收货";
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.auto_accept;
            pictureBox5.Location = new Point(467, 5);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(21, 21);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 19;
            pictureBox5.TabStop = false;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label9.AutoSize = true;
            label9.ForeColor = Color.FromArgb(121, 193, 38);
            label9.Location = new Point(405, 7);
            label9.Name = "label9";
            label9.Size = new Size(56, 17);
            label9.TabIndex = 18;
            label9.Text = "自动确认";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label8.AutoSize = true;
            label8.ForeColor = Color.FromArgb(93, 151, 255);
            label8.Location = new Point(320, 7);
            label8.Name = "label8";
            label8.Size = new Size(56, 17);
            label8.TabIndex = 17;
            label8.Text = "自动发货";
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.auto_deliver;
            pictureBox4.Location = new Point(296, 5);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(21, 21);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.auto_confirm;
            pictureBox3.Location = new Point(381, 5);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(21, 21);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
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
            // submitRequirementsLabel
            // 
            submitRequirementsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            submitRequirementsLabel.LinkColor = Color.FromArgb(192, 0, 192);
            submitRequirementsLabel.Location = new Point(626, 481);
            submitRequirementsLabel.Name = "submitRequirementsLabel";
            submitRequirementsLabel.Size = new Size(60, 23);
            submitRequirementsLabel.TabIndex = 19;
            submitRequirementsLabel.TabStop = true;
            submitRequirementsLabel.Text = "联系我们";
            submitRequirementsLabel.TextAlign = ContentAlignment.MiddleCenter;
            submitRequirementsLabel.LinkClicked += submitRequirementsLabel_LinkClicked;
            // 
            // mainNotifyIcon
            // 
            mainNotifyIcon.Icon = (Icon)resources.GetObject("mainNotifyIcon.Icon");
            mainNotifyIcon.Text = "Steam验证器";
            mainNotifyIcon.Visible = true;
            mainNotifyIcon.MouseDoubleClick += mainNotifyIcon_MouseDoubleClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(764, 511);
            Controls.Add(submitRequirementsLabel);
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
            MinimumSize = new Size(780, 550);
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
            ecoPage.ResumeLayout(false);
            ecoUsersPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            statusPanel.ResumeLayout(false);
            statusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
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
        private ToolStripMenuItem authenticatorMenuItem;
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
        private SteamUserCollectionPanel usersPanel;
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
        private BuffUserCollectionPanel buffUsersPanel;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label SteamId;
        private LinkLabel submitRequirementsLabel;
        private ToolStripMenuItem submitRequirementsMenuItem;
        private NotifyIcon mainNotifyIcon;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Label label8;
        private Label label9;
        private Label label10;
        private PictureBox pictureBox5;
        private TabPage ecoPage;
        private EcoUserCollectionPanel ecoUsersPanel;
        private PictureBox pictureBox6;
        private ToolStripMenuItem exportAuthenticatorMenuItem;
    }
}

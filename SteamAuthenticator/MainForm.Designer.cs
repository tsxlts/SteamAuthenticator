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
            loginMenuItem = new ToolStripMenuItem();
            proxySettingMenuItem = new ToolStripMenuItem();
            passwordMenuItem = new ToolStripMenuItem();
            copyCookieMenuItem = new ToolStripMenuItem();
            copyRefreshTokenMenuItem = new ToolStripMenuItem();
            copyAccessTokenToolItem = new ToolStripMenuItem();
            authenticatorMenu = new ToolStripMenuItem();
            confirmMenuItem = new ToolStripMenuItem();
            guardMenuItem = new ToolStripMenuItem();
            addAuthenticatorMenuItem = new ToolStripMenuItem();
            moveAuthenticatorMenuItem = new ToolStripMenuItem();
            removeAuthenticatorMenuItem = new ToolStripMenuItem();
            importAuthenticatorMenuItem = new ToolStripMenuItem();
            UserImg = new PictureBox();
            UserName = new Label();
            Balance = new Label();
            panel1 = new Panel();
            ConfirmationCountLable = new Label();
            OfferCountLabel = new Label();
            label2 = new Label();
            label1 = new Label();
            settingMenuItem = new ToolStripMenuItem();
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
            menuStrip1.Size = new Size(444, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // UserToolStripMenuItem
            // 
            UserToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginMenuItem, settingMenuItem, proxySettingMenuItem, passwordMenuItem, copyCookieMenuItem, copyRefreshTokenMenuItem, copyAccessTokenToolItem });
            UserToolStripMenuItem.Name = "UserToolStripMenuItem";
            UserToolStripMenuItem.Size = new Size(44, 21);
            UserToolStripMenuItem.Text = "用户";
            // 
            // loginMenuItem
            // 
            loginMenuItem.Name = "loginMenuItem";
            loginMenuItem.Size = new Size(180, 22);
            loginMenuItem.Text = "登录";
            loginMenuItem.Click += loginMenuItem_Click;
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
            // authenticatorMenu
            // 
            authenticatorMenu.DropDownItems.AddRange(new ToolStripItem[] { confirmMenuItem, guardMenuItem, addAuthenticatorMenuItem, moveAuthenticatorMenuItem, removeAuthenticatorMenuItem, importAuthenticatorMenuItem });
            authenticatorMenu.Name = "authenticatorMenu";
            authenticatorMenu.Size = new Size(80, 21);
            authenticatorMenu.Text = "令牌验证器";
            // 
            // confirmMenuItem
            // 
            confirmMenuItem.Name = "confirmMenuItem";
            confirmMenuItem.Size = new Size(136, 22);
            confirmMenuItem.Text = "令牌确认";
            confirmMenuItem.Click += confirmMenuItem_Click;
            // 
            // guardMenuItem
            // 
            guardMenuItem.Name = "guardMenuItem";
            guardMenuItem.Size = new Size(136, 22);
            guardMenuItem.Text = "Steam令牌";
            guardMenuItem.Click += guardMenuItem_Click;
            // 
            // addAuthenticatorMenuItem
            // 
            addAuthenticatorMenuItem.Name = "addAuthenticatorMenuItem";
            addAuthenticatorMenuItem.Size = new Size(136, 22);
            addAuthenticatorMenuItem.Text = "添加令牌";
            addAuthenticatorMenuItem.Click += addAuthenticatorMenuItem_Click;
            // 
            // moveAuthenticatorMenuItem
            // 
            moveAuthenticatorMenuItem.Name = "moveAuthenticatorMenuItem";
            moveAuthenticatorMenuItem.Size = new Size(136, 22);
            moveAuthenticatorMenuItem.Text = "移动令牌";
            moveAuthenticatorMenuItem.Click += moveAuthenticatorMenuItem_Click;
            // 
            // removeAuthenticatorMenuItem
            // 
            removeAuthenticatorMenuItem.Name = "removeAuthenticatorMenuItem";
            removeAuthenticatorMenuItem.Size = new Size(136, 22);
            removeAuthenticatorMenuItem.Text = "删除令牌";
            removeAuthenticatorMenuItem.Click += removeAuthenticatorMenuItem_Click;
            // 
            // importAuthenticatorMenuItem
            // 
            importAuthenticatorMenuItem.Name = "importAuthenticatorMenuItem";
            importAuthenticatorMenuItem.Size = new Size(136, 22);
            importAuthenticatorMenuItem.Text = "导入令牌";
            importAuthenticatorMenuItem.Click += importAuthenticatorMenuItem_Click;
            // 
            // UserImg
            // 
            UserImg.Image = Properties.Resources.userimg;
            UserImg.Location = new Point(12, 38);
            UserImg.Name = "UserImg";
            UserImg.Size = new Size(100, 100);
            UserImg.SizeMode = PictureBoxSizeMode.Zoom;
            UserImg.TabIndex = 1;
            UserImg.TabStop = false;
            // 
            // UserName
            // 
            UserName.Location = new Point(118, 40);
            UserName.Name = "UserName";
            UserName.Size = new Size(100, 23);
            UserName.TabIndex = 2;
            UserName.Text = "---";
            UserName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Balance
            // 
            Balance.ForeColor = Color.Green;
            Balance.Location = new Point(118, 68);
            Balance.Name = "Balance";
            Balance.Size = new Size(100, 23);
            Balance.TabIndex = 4;
            Balance.Text = "￥0.00";
            Balance.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel1.Controls.Add(ConfirmationCountLable);
            panel1.Controls.Add(OfferCountLabel);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(283, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(149, 100);
            panel1.TabIndex = 6;
            // 
            // ConfirmationCountLable
            // 
            ConfirmationCountLable.AutoSize = true;
            ConfirmationCountLable.Location = new Point(44, 33);
            ConfirmationCountLable.Name = "ConfirmationCountLable";
            ConfirmationCountLable.Size = new Size(15, 17);
            ConfirmationCountLable.TabIndex = 3;
            ConfirmationCountLable.Text = "0";
            ConfirmationCountLable.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // OfferCountLabel
            // 
            OfferCountLabel.AutoSize = true;
            OfferCountLabel.Location = new Point(44, 5);
            OfferCountLabel.Name = "OfferCountLabel";
            OfferCountLabel.Size = new Size(15, 17);
            OfferCountLabel.TabIndex = 2;
            OfferCountLabel.Text = "0";
            OfferCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
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
            label1.Location = new Point(3, 5);
            label1.Name = "label1";
            label1.Size = new Size(35, 17);
            label1.TabIndex = 0;
            label1.Text = "报价:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // settingMenuItem
            // 
            settingMenuItem.Name = "settingMenuItem";
            settingMenuItem.Size = new Size(180, 22);
            settingMenuItem.Text = "用户设置";
            settingMenuItem.Click += settingMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 361);
            Controls.Add(panel1);
            Controls.Add(Balance);
            Controls.Add(UserName);
            Controls.Add(UserImg);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
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
        private ToolStripMenuItem loginMenuItem;
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
    }
}

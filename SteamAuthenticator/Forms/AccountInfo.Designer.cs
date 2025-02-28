namespace Steam_Authenticator.Forms
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
            label2 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // copyTradeLinkBtn
            // 
            copyTradeLinkBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyTradeLinkBtn.Cursor = Cursors.Hand;
            copyTradeLinkBtn.ForeColor = Color.Gray;
            copyTradeLinkBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyTradeLinkBtn.Location = new Point(410, 65);
            copyTradeLinkBtn.Name = "copyTradeLinkBtn";
            copyTradeLinkBtn.Size = new Size(42, 23);
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
            tradeLinkBox.Location = new Point(107, 69);
            tradeLinkBox.Name = "tradeLinkBox";
            tradeLinkBox.Size = new Size(297, 16);
            tradeLinkBox.TabIndex = 14;
            tradeLinkBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(16, 65);
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
            steamNameBox.Location = new Point(107, 9);
            steamNameBox.Name = "steamNameBox";
            steamNameBox.Size = new Size(297, 23);
            steamNameBox.TabIndex = 17;
            steamNameBox.Text = "**********";
            steamNameBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.ForeColor = Color.Green;
            label3.ImageAlign = ContentAlignment.MiddleLeft;
            label3.Location = new Point(16, 9);
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
            steamIdBox.Location = new Point(107, 37);
            steamIdBox.Name = "steamIdBox";
            steamIdBox.Size = new Size(297, 23);
            steamIdBox.TabIndex = 19;
            steamIdBox.Text = "**********";
            steamIdBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.ForeColor = SystemColors.ControlText;
            label4.ImageAlign = ContentAlignment.MiddleLeft;
            label4.Location = new Point(16, 37);
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
            copySteamIdBtn.Location = new Point(410, 37);
            copySteamIdBtn.Name = "copySteamIdBtn";
            copySteamIdBtn.Size = new Size(42, 23);
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
            copyApikeyBtn.Location = new Point(410, 93);
            copyApikeyBtn.Name = "copyApikeyBtn";
            copyApikeyBtn.Size = new Size(42, 23);
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
            apikeyBox.Location = new Point(107, 97);
            apikeyBox.Name = "apikeyBox";
            apikeyBox.Size = new Size(297, 16);
            apikeyBox.TabIndex = 22;
            apikeyBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.ImageAlign = ContentAlignment.MiddleLeft;
            label6.Location = new Point(16, 93);
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
            label5.Location = new Point(16, 121);
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
            tradeStatusBox.Location = new Point(107, 125);
            tradeStatusBox.Name = "tradeStatusBox";
            tradeStatusBox.Size = new Size(297, 16);
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
            tradeStatusBtn.Location = new Point(410, 121);
            tradeStatusBtn.Name = "tradeStatusBtn";
            tradeStatusBtn.Size = new Size(42, 23);
            tradeStatusBtn.TabIndex = 26;
            tradeStatusBtn.Text = "详情";
            tradeStatusBtn.TextAlign = ContentAlignment.MiddleLeft;
            tradeStatusBtn.Click += tradeStatusBtn_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoEllipsis = true;
            label2.ImageAlign = ContentAlignment.MiddleLeft;
            label2.Location = new Point(107, 153);
            label2.Name = "label2";
            label2.Size = new Size(297, 16);
            label2.TabIndex = 28;
            label2.Text = "**********";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.ForeColor = SystemColors.ControlText;
            label7.ImageAlign = ContentAlignment.MiddleLeft;
            label7.Location = new Point(16, 149);
            label7.Name = "label7";
            label7.Size = new Size(85, 23);
            label7.TabIndex = 27;
            label7.Text = "交易权限：";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AccountInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 291);
            Controls.Add(label2);
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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(480, 330);
            Name = "AccountInfo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "帐号信息";
            Load += AccountInfo_Load;
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
        private Label label2;
        private Label label7;
    }
}
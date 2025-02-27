namespace Steam_Authenticator.Forms
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            versionBox = new Label();
            label3 = new Label();
            projectBox = new Label();
            label1 = new Label();
            copyProjectBtn = new Label();
            qqBox = new Label();
            label4 = new Label();
            copyQQBtn = new Label();
            copyWeChatBtn = new Label();
            wechatBox = new Label();
            label2 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // versionBox
            // 
            versionBox.ForeColor = Color.Green;
            versionBox.ImageAlign = ContentAlignment.MiddleLeft;
            versionBox.Location = new Point(91, 38);
            versionBox.Name = "versionBox";
            versionBox.Size = new Size(73, 23);
            versionBox.TabIndex = 11;
            versionBox.Text = "0.0.0";
            versionBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.ForeColor = Color.Green;
            label3.ImageAlign = ContentAlignment.MiddleLeft;
            label3.Location = new Point(12, 38);
            label3.Name = "label3";
            label3.Size = new Size(73, 23);
            label3.TabIndex = 10;
            label3.Text = "当前版本：";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // projectBox
            // 
            projectBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            projectBox.AutoEllipsis = true;
            projectBox.ImageAlign = ContentAlignment.MiddleLeft;
            projectBox.Location = new Point(91, 13);
            projectBox.Name = "projectBox";
            projectBox.Size = new Size(292, 16);
            projectBox.TabIndex = 9;
            projectBox.Text = "https://github.com/***/***";
            projectBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(73, 23);
            label1.TabIndex = 8;
            label1.Text = "项目地址：";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // copyProjectBtn
            // 
            copyProjectBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyProjectBtn.Cursor = Cursors.Hand;
            copyProjectBtn.ForeColor = Color.Gray;
            copyProjectBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyProjectBtn.Location = new Point(390, 9);
            copyProjectBtn.Name = "copyProjectBtn";
            copyProjectBtn.Size = new Size(42, 23);
            copyProjectBtn.TabIndex = 12;
            copyProjectBtn.Text = "复制";
            copyProjectBtn.TextAlign = ContentAlignment.MiddleLeft;
            copyProjectBtn.Click += copyProjectBtn_Click;
            // 
            // qqBox
            // 
            qqBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            qqBox.ForeColor = SystemColors.ControlText;
            qqBox.ImageAlign = ContentAlignment.MiddleLeft;
            qqBox.Location = new Point(158, 67);
            qqBox.Name = "qqBox";
            qqBox.Size = new Size(225, 23);
            qqBox.TabIndex = 14;
            qqBox.Text = "**********";
            qqBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.ForeColor = SystemColors.ControlText;
            label4.ImageAlign = ContentAlignment.MiddleLeft;
            label4.Location = new Point(12, 67);
            label4.Name = "label4";
            label4.Size = new Size(73, 23);
            label4.TabIndex = 13;
            label4.Text = "联系方式：";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // copyQQBtn
            // 
            copyQQBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyQQBtn.Cursor = Cursors.Hand;
            copyQQBtn.ForeColor = Color.Gray;
            copyQQBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyQQBtn.Location = new Point(390, 67);
            copyQQBtn.Name = "copyQQBtn";
            copyQQBtn.Size = new Size(42, 23);
            copyQQBtn.TabIndex = 15;
            copyQQBtn.Text = "复制";
            copyQQBtn.TextAlign = ContentAlignment.MiddleLeft;
            copyQQBtn.Click += copyQQBtn_Click;
            // 
            // copyWeChatBtn
            // 
            copyWeChatBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyWeChatBtn.Cursor = Cursors.Hand;
            copyWeChatBtn.ForeColor = Color.Gray;
            copyWeChatBtn.ImageAlign = ContentAlignment.MiddleLeft;
            copyWeChatBtn.Location = new Point(390, 95);
            copyWeChatBtn.Name = "copyWeChatBtn";
            copyWeChatBtn.Size = new Size(42, 23);
            copyWeChatBtn.TabIndex = 17;
            copyWeChatBtn.Text = "复制";
            copyWeChatBtn.TextAlign = ContentAlignment.MiddleLeft;
            copyWeChatBtn.Click += copyWeChatBtn_Click;
            // 
            // wechatBox
            // 
            wechatBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            wechatBox.ForeColor = SystemColors.ControlText;
            wechatBox.ImageAlign = ContentAlignment.MiddleLeft;
            wechatBox.Location = new Point(158, 95);
            wechatBox.Name = "wechatBox";
            wechatBox.Size = new Size(225, 23);
            wechatBox.TabIndex = 16;
            wechatBox.Text = "**********";
            wechatBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.ForeColor = Color.Gray;
            label2.ImageAlign = ContentAlignment.MiddleLeft;
            label2.Location = new Point(91, 67);
            label2.Name = "label2";
            label2.Size = new Size(61, 23);
            label2.TabIndex = 18;
            label2.Text = "QQ";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.ForeColor = Color.Gray;
            label5.ImageAlign = ContentAlignment.MiddleLeft;
            label5.Location = new Point(91, 95);
            label5.Name = "label5";
            label5.Size = new Size(61, 23);
            label5.TabIndex = 19;
            label5.Text = "WeChat";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // About
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 261);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(copyWeChatBtn);
            Controls.Add(wechatBox);
            Controls.Add(copyQQBtn);
            Controls.Add(qqBox);
            Controls.Add(label4);
            Controls.Add(copyProjectBtn);
            Controls.Add(versionBox);
            Controls.Add(label3);
            Controls.Add(projectBox);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(460, 300);
            Name = "About";
            StartPosition = FormStartPosition.CenterParent;
            Text = "关于我们";
            Load += About_Load;
            ResumeLayout(false);
        }

        #endregion
        private Label versionBox;
        private Label label3;
        private Label projectBox;
        private Label label1;
        private Label copyProjectBtn;
        private Label qqBox;
        private Label label4;
        private Label copyQQBtn;
        private Label copyWeChatBtn;
        private Label wechatBox;
        private Label label2;
        private Label label5;
    }
}
namespace Steam_Authenticator.Forms
{
    partial class MobileConfirmationLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MobileConfirmationLogin));
            ConfirmLoginTitle = new Label();
            ConfirmLoginClientType = new Label();
            ConfirmLoginIP = new Label();
            ConfirmLoginRegion = new Label();
            splitContainer1 = new SplitContainer();
            acceptBtn = new Button();
            declineBtn = new Button();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // ConfirmLoginTitle
            // 
            ConfirmLoginTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmLoginTitle.Font = new Font("Microsoft YaHei UI", 15F);
            ConfirmLoginTitle.Location = new Point(12, 22);
            ConfirmLoginTitle.Name = "ConfirmLoginTitle";
            ConfirmLoginTitle.Size = new Size(355, 31);
            ConfirmLoginTitle.TabIndex = 0;
            ConfirmLoginTitle.Text = "*** 有新的登录请求";
            ConfirmLoginTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConfirmLoginClientType
            // 
            ConfirmLoginClientType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmLoginClientType.Font = new Font("Microsoft YaHei UI", 10.5F);
            ConfirmLoginClientType.Location = new Point(12, 72);
            ConfirmLoginClientType.Name = "ConfirmLoginClientType";
            ConfirmLoginClientType.Size = new Size(355, 31);
            ConfirmLoginClientType.TabIndex = 1;
            ConfirmLoginClientType.Text = "网页浏览器";
            ConfirmLoginClientType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConfirmLoginIP
            // 
            ConfirmLoginIP.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmLoginIP.Font = new Font("Microsoft YaHei UI", 10.5F);
            ConfirmLoginIP.Location = new Point(12, 116);
            ConfirmLoginIP.Name = "ConfirmLoginIP";
            ConfirmLoginIP.Size = new Size(355, 31);
            ConfirmLoginIP.TabIndex = 2;
            ConfirmLoginIP.Text = "IP 地址：***.***.***.***";
            ConfirmLoginIP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConfirmLoginRegion
            // 
            ConfirmLoginRegion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmLoginRegion.Font = new Font("Microsoft YaHei UI", 10.5F);
            ConfirmLoginRegion.Location = new Point(12, 160);
            ConfirmLoginRegion.Name = "ConfirmLoginRegion";
            ConfirmLoginRegion.Size = new Size(355, 31);
            ConfirmLoginRegion.TabIndex = 3;
            ConfirmLoginRegion.Text = "中国，北京";
            ConfirmLoginRegion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(1, 277);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(acceptBtn);
            splitContainer1.Panel1MinSize = 185;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(declineBtn);
            splitContainer1.Panel2MinSize = 185;
            splitContainer1.Size = new Size(378, 41);
            splitContainer1.SplitterDistance = 189;
            splitContainer1.TabIndex = 4;
            // 
            // acceptBtn
            // 
            acceptBtn.Dock = DockStyle.Fill;
            acceptBtn.Location = new Point(0, 0);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(189, 41);
            acceptBtn.TabIndex = 0;
            acceptBtn.Text = "批准";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // declineBtn
            // 
            declineBtn.Dock = DockStyle.Fill;
            declineBtn.Location = new Point(0, 0);
            declineBtn.Name = "declineBtn";
            declineBtn.Size = new Size(185, 41);
            declineBtn.TabIndex = 0;
            declineBtn.Text = "拒绝";
            declineBtn.UseVisualStyleBackColor = true;
            declineBtn.Click += declineBtn_Click;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.Font = new Font("Microsoft YaHei UI", 18F);
            label5.Location = new Point(12, 221);
            label5.Name = "label5";
            label5.Size = new Size(355, 31);
            label5.TabIndex = 5;
            label5.Text = "是否批准登录？";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MobileConfirmationLogin
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = declineBtn;
            ClientSize = new Size(379, 333);
            Controls.Add(label5);
            Controls.Add(splitContainer1);
            Controls.Add(ConfirmLoginRegion);
            Controls.Add(ConfirmLoginIP);
            Controls.Add(ConfirmLoginClientType);
            Controls.Add(ConfirmLoginTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MobileConfirmationLogin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "请求登录";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public Label ConfirmLoginTitle;
        public Label ConfirmLoginClientType;
        public Label ConfirmLoginIP;
        public Label ConfirmLoginRegion;
        private SplitContainer splitContainer1;
        public Button acceptBtn;
        public Button declineBtn;
        private Label label5;
    }
}
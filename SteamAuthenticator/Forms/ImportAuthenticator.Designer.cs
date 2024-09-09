namespace Steam_Authenticator.Forms
{
    partial class ImportAuthenticator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportAuthenticator));
            label1 = new Label();
            RevocationCodeBox = new TextBox();
            SharedSecretBox = new TextBox();
            label2 = new Label();
            IdentitySecretBox = new TextBox();
            label3 = new Label();
            splitContainer1 = new SplitContainer();
            cancelBtm = new Button();
            acceptBtn = new Button();
            label4 = new Label();
            AccountNameBox = new TextBox();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // AccountNameBox
            // 
            AccountNameBox.Font = new Font("Microsoft YaHei UI", 12F);
            AccountNameBox.Location = new Point(118, 63);
            AccountNameBox.Name = "AccountNameBox";
            AccountNameBox.Size = new Size(212, 28);
            AccountNameBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.Font = new Font("Microsoft YaHei UI", 8F);
            label1.Location = new Point(12, 114);
            label1.Name = "label1";
            label1.Size = new Size(100, 38);
            label1.TabIndex = 1;
            label1.Text = "恢         复         码\r\nrevocation_code";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RevocationCodeBox
            // 
            RevocationCodeBox.Font = new Font("Microsoft YaHei UI", 12F);
            RevocationCodeBox.Location = new Point(118, 119);
            RevocationCodeBox.Name = "RevocationCodeBox";
            RevocationCodeBox.Size = new Size(212, 28);
            RevocationCodeBox.TabIndex = 2;
            // 
            // SharedSecretBox
            // 
            SharedSecretBox.Font = new Font("Microsoft YaHei UI", 12F);
            SharedSecretBox.Location = new Point(118, 174);
            SharedSecretBox.Name = "SharedSecretBox";
            SharedSecretBox.Size = new Size(212, 28);
            SharedSecretBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft YaHei UI", 8F);
            label2.Location = new Point(12, 169);
            label2.Name = "label2";
            label2.Size = new Size(100, 38);
            label2.TabIndex = 2;
            label2.Text = "登     录     秘     钥\r\nshared_secret";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // IdentitySecretBox
            // 
            IdentitySecretBox.Font = new Font("Microsoft YaHei UI", 12F);
            IdentitySecretBox.Location = new Point(118, 231);
            IdentitySecretBox.Name = "IdentitySecretBox";
            IdentitySecretBox.Size = new Size(212, 28);
            IdentitySecretBox.TabIndex = 5;
            // 
            // label3
            // 
            label3.Font = new Font("Microsoft YaHei UI", 8F);
            label3.Location = new Point(12, 226);
            label3.Name = "label3";
            label3.Size = new Size(100, 38);
            label3.TabIndex = 4;
            label3.Text = "身     份     秘     钥\r\nidentity_secret";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(0, 281);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(cancelBtm);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(acceptBtn);
            splitContainer1.Size = new Size(342, 35);
            splitContainer1.SplitterDistance = 171;
            splitContainer1.TabIndex = 6;
            // 
            // cancelBtm
            // 
            cancelBtm.Dock = DockStyle.Fill;
            cancelBtm.Location = new Point(0, 0);
            cancelBtm.Name = "cancelBtm";
            cancelBtm.Size = new Size(171, 35);
            cancelBtm.TabIndex = 0;
            cancelBtm.Text = "取消";
            cancelBtm.UseVisualStyleBackColor = true;
            cancelBtm.Click += cancelBtm_Click;
            // 
            // acceptBtn
            // 
            acceptBtn.Dock = DockStyle.Fill;
            acceptBtn.Location = new Point(0, 0);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(167, 35);
            acceptBtn.TabIndex = 0;
            acceptBtn.Text = "确定";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // label4
            // 
            label4.ForeColor = Color.FromArgb(0, 128, 255);
            label4.Location = new Point(12, 9);
            label4.Name = "label4";
            label4.Size = new Size(318, 34);
            label4.TabIndex = 7;
            label4.Text = "如果你想要使用令牌功能，必须填写登录秘钥\r\n如果你想要使用令牌确认功能，必须填写身份秘钥";
            // 
            // label5
            // 
            label5.Font = new Font("Microsoft YaHei UI", 8F);
            label5.Location = new Point(12, 58);
            label5.Name = "label5";
            label5.Size = new Size(100, 38);
            label5.TabIndex = 8;
            label5.Text = "帐                     号\r\naccount_name";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ImportAuthenticator
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelBtm;
            ClientSize = new Size(342, 320);
            Controls.Add(AccountNameBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(splitContainer1);
            Controls.Add(IdentitySecretBox);
            Controls.Add(label3);
            Controls.Add(SharedSecretBox);
            Controls.Add(label2);
            Controls.Add(RevocationCodeBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImportAuthenticator";
            StartPosition = FormStartPosition.CenterParent;
            Text = "导入令牌";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox RevocationCodeBox;
        private TextBox SharedSecretBox;
        private Label label2;
        private TextBox IdentitySecretBox;
        private Label label3;
        private SplitContainer splitContainer1;
        private Button cancelBtm;
        private Button acceptBtn;
        private Label label4;
        private TextBox AccountNameBox;
        private Label label5;
    }
}
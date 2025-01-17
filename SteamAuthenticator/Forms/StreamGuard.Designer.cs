namespace Steam_Authenticator.Forms
{
    partial class StreamGuard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamGuard));
            Users = new ComboBox();
            ExpireText = new Label();
            label2 = new Label();
            label3 = new Label();
            GuardText = new Label();
            label5 = new Label();
            label6 = new Label();
            deleteGuardBtn = new Button();
            exportGuardBtn = new Button();
            splitContainer1 = new SplitContainer();
            copyRevocationCode = new Label();
            RevocationCode = new Label();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // Users
            // 
            Users.DropDownStyle = ComboBoxStyle.DropDownList;
            Users.FormattingEnabled = true;
            Users.Location = new Point(12, 12);
            Users.Name = "Users";
            Users.Size = new Size(172, 25);
            Users.TabIndex = 0;
            Users.SelectedValueChanged += Users_SelectedValueChanged;
            // 
            // ExpireText
            // 
            ExpireText.ForeColor = Color.Green;
            ExpireText.Location = new Point(61, 86);
            ExpireText.Name = "ExpireText";
            ExpireText.Size = new Size(40, 23);
            ExpireText.TabIndex = 1;
            ExpireText.Text = "30秒";
            ExpireText.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 89);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "有效期";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 53);
            label3.Name = "label3";
            label3.Size = new Size(32, 17);
            label3.TabIndex = 3;
            label3.Text = "令牌";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // GuardText
            // 
            GuardText.Cursor = Cursors.Hand;
            GuardText.ForeColor = Color.Red;
            GuardText.Location = new Point(61, 53);
            GuardText.Name = "GuardText";
            GuardText.Size = new Size(55, 17);
            GuardText.TabIndex = 4;
            GuardText.Text = "*****";
            GuardText.TextAlign = ContentAlignment.MiddleLeft;
            GuardText.Click += GuardText_Click;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.ForeColor = Color.FromArgb(128, 128, 128);
            label5.Location = new Point(128, 53);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 5;
            label5.Text = "点击复制";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.ForeColor = Color.FromArgb(128, 128, 128);
            label6.Location = new Point(104, 89);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 6;
            label6.Text = "到期自动刷新";
            // 
            // deleteGuardBtn
            // 
            deleteGuardBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            deleteGuardBtn.Location = new Point(0, 0);
            deleteGuardBtn.Name = "deleteGuardBtn";
            deleteGuardBtn.Size = new Size(97, 30);
            deleteGuardBtn.TabIndex = 1;
            deleteGuardBtn.Text = "删除令牌";
            deleteGuardBtn.UseVisualStyleBackColor = true;
            deleteGuardBtn.Click += deleteGuardBtn_Click;
            // 
            // exportGuardBtn
            // 
            exportGuardBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exportGuardBtn.Location = new Point(0, 0);
            exportGuardBtn.Name = "exportGuardBtn";
            exportGuardBtn.Size = new Size(93, 30);
            exportGuardBtn.TabIndex = 0;
            exportGuardBtn.Text = "导出令牌";
            exportGuardBtn.UseVisualStyleBackColor = true;
            exportGuardBtn.Click += exportGuardBtn_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(1, 162);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(deleteGuardBtn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(exportGuardBtn);
            splitContainer1.Size = new Size(194, 30);
            splitContainer1.SplitterDistance = 97;
            splitContainer1.TabIndex = 8;
            // 
            // copyRevocationCode
            // 
            copyRevocationCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyRevocationCode.Cursor = Cursors.Hand;
            copyRevocationCode.ForeColor = Color.FromArgb(128, 128, 128);
            copyRevocationCode.Location = new Point(128, 124);
            copyRevocationCode.Name = "copyRevocationCode";
            copyRevocationCode.Size = new Size(56, 17);
            copyRevocationCode.TabIndex = 11;
            copyRevocationCode.Text = "点此复制";
            copyRevocationCode.TextAlign = ContentAlignment.MiddleLeft;
            copyRevocationCode.Click += CopyRevocationCode_Click;
            // 
            // RevocationCode
            // 
            RevocationCode.Cursor = Cursors.Hand;
            RevocationCode.ForeColor = Color.Red;
            RevocationCode.Location = new Point(61, 124);
            RevocationCode.Name = "RevocationCode";
            RevocationCode.Size = new Size(55, 17);
            RevocationCode.TabIndex = 10;
            RevocationCode.Text = "******";
            RevocationCode.TextAlign = ContentAlignment.MiddleLeft;
            RevocationCode.Click += RevocationCode_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 124);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 9;
            label7.Text = "恢复码";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // StreamGuard
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(196, 193);
            Controls.Add(copyRevocationCode);
            Controls.Add(RevocationCode);
            Controls.Add(label7);
            Controls.Add(splitContainer1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(GuardText);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(ExpireText);
            Controls.Add(Users);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StreamGuard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Steam令牌";
            Load += StreamGuard_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox Users;
        private Label ExpireText;
        private Label label2;
        private Label label3;
        private Label GuardText;
        private Label label5;
        private Label label6;
        private Button exportGuardBtn;
        private Button deleteGuardBtn;
        private SplitContainer splitContainer1;
        private Label copyRevocationCode;
        private Label RevocationCode;
        private Label label7;
    }
}
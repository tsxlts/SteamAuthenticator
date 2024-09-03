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
            panel1 = new Panel();
            exportGuardBtn = new Button();
            panel1.SuspendLayout();
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
            ExpireText.Size = new Size(61, 23);
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
            label3.Size = new Size(44, 17);
            label3.TabIndex = 3;
            label3.Text = "令牌码";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // GuardText
            // 
            GuardText.Cursor = Cursors.Hand;
            GuardText.ForeColor = Color.Red;
            GuardText.Location = new Point(61, 50);
            GuardText.Name = "GuardText";
            GuardText.Size = new Size(61, 23);
            GuardText.TabIndex = 4;
            GuardText.Text = "*****";
            GuardText.TextAlign = ContentAlignment.MiddleLeft;
            GuardText.Click += GuardText_Click;
            // 
            // label5
            // 
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
            label6.ForeColor = Color.FromArgb(128, 128, 128);
            label6.Location = new Point(104, 89);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 6;
            label6.Text = "到期自动刷新";
            // 
            // panel1
            // 
            panel1.Controls.Add(exportGuardBtn);
            panel1.Location = new Point(0, 123);
            panel1.Name = "panel1";
            panel1.Size = new Size(195, 33);
            panel1.TabIndex = 7;
            // 
            // exportGuardBtn
            // 
            exportGuardBtn.Dock = DockStyle.Fill;
            exportGuardBtn.Location = new Point(0, 0);
            exportGuardBtn.Name = "exportGuardBtn";
            exportGuardBtn.Size = new Size(195, 33);
            exportGuardBtn.TabIndex = 0;
            exportGuardBtn.Text = "导出令牌";
            exportGuardBtn.UseVisualStyleBackColor = true;
            exportGuardBtn.Click += exportGuardBtn_Click;
            // 
            // StreamGuard
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(195, 158);
            Controls.Add(panel1);
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
            panel1.ResumeLayout(false);
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
        private Panel panel1;
        private Button exportGuardBtn;
    }
}
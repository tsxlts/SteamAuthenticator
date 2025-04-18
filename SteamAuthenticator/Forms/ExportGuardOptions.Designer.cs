namespace Steam_Authenticator.Forms
{
    partial class ExportGuardOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportGuardOptions));
            saveBtn = new Button();
            maFile = new RadioButton();
            saFile = new RadioButton();
            groupBox1 = new GroupBox();
            accountPanel = new Panel();
            label3 = new Label();
            selectAccountBtn = new LinkLabel();
            groupBox2 = new GroupBox();
            panel1 = new Panel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // saveBtn
            // 
            saveBtn.Dock = DockStyle.Fill;
            saveBtn.Location = new Point(0, 0);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(368, 33);
            saveBtn.TabIndex = 6;
            saveBtn.Text = "导出令牌";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // maFile
            // 
            maFile.AutoSize = true;
            maFile.Location = new Point(6, 49);
            maFile.Name = "maFile";
            maFile.Size = new Size(183, 21);
            maFile.TabIndex = 2;
            maFile.TabStop = true;
            maFile.Text = "导出maFile文件（SDA文件）";
            maFile.UseVisualStyleBackColor = true;
            // 
            // saFile
            // 
            saFile.AutoSize = true;
            saFile.Checked = true;
            saFile.Location = new Point(6, 22);
            saFile.Name = "saFile";
            saFile.Size = new Size(89, 21);
            saFile.TabIndex = 1;
            saFile.TabStop = true;
            saFile.Text = "导出SA文件";
            saFile.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(accountPanel);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(selectAccountBtn);
            groupBox1.Location = new Point(12, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(368, 170);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "选择帐号";
            // 
            // accountPanel
            // 
            accountPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            accountPanel.AutoScroll = true;
            accountPanel.BackColor = Color.White;
            accountPanel.Location = new Point(6, 39);
            accountPanel.Name = "accountPanel";
            accountPanel.Size = new Size(356, 125);
            accountPanel.TabIndex = 7;
            // 
            // label3
            // 
            label3.ForeColor = Color.FromArgb(127, 127, 127);
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(80, 17);
            label3.TabIndex = 6;
            label3.Text = "导出帐号：";
            // 
            // selectAccountBtn
            // 
            selectAccountBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectAccountBtn.AutoSize = true;
            selectAccountBtn.Location = new Point(306, 19);
            selectAccountBtn.Name = "selectAccountBtn";
            selectAccountBtn.Size = new Size(56, 17);
            selectAccountBtn.TabIndex = 5;
            selectAccountBtn.TabStop = true;
            selectAccountBtn.Text = "选择帐号";
            selectAccountBtn.VisitedLinkColor = Color.Blue;
            selectAccountBtn.LinkClicked += selectAccountBtn_LinkClicked;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(maFile);
            groupBox2.Controls.Add(saFile);
            groupBox2.Location = new Point(12, 186);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(368, 84);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "文件选格式";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(saveBtn);
            panel1.Location = new Point(12, 276);
            panel1.Name = "panel1";
            panel1.Size = new Size(368, 33);
            panel1.TabIndex = 10;
            // 
            // ExportGuardOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 314);
            Controls.Add(panel1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportGuardOptions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "导出令牌";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button saveBtn;
        private RadioButton maFile;
        private RadioButton saFile;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Panel panel1;
        private LinkLabel selectAccountBtn;
        private Label label3;
        private Panel accountPanel;
    }
}
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
            exportAll = new RadioButton();
            exportCurrent = new RadioButton();
            currentName = new Label();
            saveBtn = new Button();
            maFile = new RadioButton();
            saFile = new RadioButton();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            panel1 = new Panel();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // exportAll
            // 
            exportAll.AutoSize = true;
            exportAll.Location = new Point(6, 22);
            exportAll.Name = "exportAll";
            exportAll.Size = new Size(98, 21);
            exportAll.TabIndex = 0;
            exportAll.TabStop = true;
            exportAll.Text = "所有帐号令牌";
            exportAll.UseVisualStyleBackColor = true;
            // 
            // exportCurrent
            // 
            exportCurrent.AutoSize = true;
            exportCurrent.Location = new Point(6, 49);
            exportCurrent.Name = "exportCurrent";
            exportCurrent.Size = new Size(98, 21);
            exportCurrent.TabIndex = 1;
            exportCurrent.TabStop = true;
            exportCurrent.Text = "当前帐号令牌";
            exportCurrent.UseVisualStyleBackColor = true;
            // 
            // currentName
            // 
            currentName.AutoEllipsis = true;
            currentName.ForeColor = Color.Green;
            currentName.Location = new Point(110, 51);
            currentName.Name = "currentName";
            currentName.Size = new Size(100, 17);
            currentName.TabIndex = 2;
            currentName.Text = "            ";
            // 
            // saveBtn
            // 
            saveBtn.Dock = DockStyle.Fill;
            saveBtn.Location = new Point(0, 0);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(221, 33);
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
            groupBox1.Controls.Add(currentName);
            groupBox1.Controls.Add(exportAll);
            groupBox1.Controls.Add(exportCurrent);
            groupBox1.Location = new Point(12, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(221, 82);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "选择帐号";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(maFile);
            groupBox2.Controls.Add(saFile);
            groupBox2.Location = new Point(12, 99);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(221, 84);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "文件选格式";
            // 
            // panel1
            // 
            panel1.Controls.Add(saveBtn);
            panel1.Location = new Point(12, 189);
            panel1.Name = "panel1";
            panel1.Size = new Size(221, 33);
            panel1.TabIndex = 10;
            // 
            // ExportGuardOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(243, 227);
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

        private RadioButton exportAll;
        private RadioButton exportCurrent;
        private Label currentName;
        private Button saveBtn;
        private RadioButton maFile;
        private RadioButton saFile;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Panel panel1;
    }
}
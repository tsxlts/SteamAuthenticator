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
            radioGroup = new Panel();
            currentName = new Label();
            passwordBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            saveBtn = new Button();
            radioGroup.SuspendLayout();
            SuspendLayout();
            // 
            // exportAll
            // 
            exportAll.AutoSize = true;
            exportAll.Location = new Point(3, 8);
            exportAll.Name = "exportAll";
            exportAll.Size = new Size(122, 21);
            exportAll.TabIndex = 0;
            exportAll.TabStop = true;
            exportAll.Text = "导出所有帐号令牌";
            exportAll.UseVisualStyleBackColor = true;
            // 
            // exportCurrent
            // 
            exportCurrent.AutoSize = true;
            exportCurrent.Location = new Point(3, 46);
            exportCurrent.Name = "exportCurrent";
            exportCurrent.Size = new Size(122, 21);
            exportCurrent.TabIndex = 1;
            exportCurrent.TabStop = true;
            exportCurrent.Text = "导出当前帐号令牌";
            exportCurrent.UseVisualStyleBackColor = true;
            // 
            // radioGroup
            // 
            radioGroup.Controls.Add(currentName);
            radioGroup.Controls.Add(exportAll);
            radioGroup.Controls.Add(exportCurrent);
            radioGroup.Location = new Point(12, 12);
            radioGroup.Name = "radioGroup";
            radioGroup.Size = new Size(243, 84);
            radioGroup.TabIndex = 2;
            // 
            // currentName
            // 
            currentName.AutoEllipsis = true;
            currentName.ForeColor = Color.Green;
            currentName.Location = new Point(131, 48);
            currentName.Name = "currentName";
            currentName.Size = new Size(100, 17);
            currentName.TabIndex = 2;
            currentName.Text = "            ";
            // 
            // passwordBox
            // 
            passwordBox.Location = new Point(72, 102);
            passwordBox.Name = "passwordBox";
            passwordBox.PasswordChar = '*';
            passwordBox.Size = new Size(85, 23);
            passwordBox.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 104);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 4;
            label1.Text = "设置密码";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(163, 105);
            label2.Name = "label2";
            label2.Size = new Size(92, 17);
            label2.TabIndex = 5;
            label2.Text = "留空不设置密码";
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(12, 157);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(243, 30);
            saveBtn.TabIndex = 6;
            saveBtn.Text = "导出令牌";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // ExportGuardOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 192);
            Controls.Add(saveBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(passwordBox);
            Controls.Add(radioGroup);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportGuardOptions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "导出令牌";
            radioGroup.ResumeLayout(false);
            radioGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton exportAll;
        private RadioButton exportCurrent;
        private Panel radioGroup;
        private Label currentName;
        private TextBox passwordBox;
        private Label label1;
        private Label label2;
        private Button saveBtn;
    }
}
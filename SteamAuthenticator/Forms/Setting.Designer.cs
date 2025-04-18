﻿namespace Steam_Authenticator.Forms
{
    partial class Setting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting));
            periodicChecking = new CheckBox();
            saveBtn = new Button();
            confirmationAutoPopup = new CheckBox();
            autoRefreshInternal = new NumericUpDown();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)autoRefreshInternal).BeginInit();
            SuspendLayout();
            // 
            // periodicChecking
            // 
            periodicChecking.AutoSize = true;
            periodicChecking.Font = new Font("Microsoft YaHei UI", 9F);
            periodicChecking.Location = new Point(12, 12);
            periodicChecking.Name = "periodicChecking";
            periodicChecking.Size = new Size(160, 21);
            periodicChecking.TabIndex = 0;
            periodicChecking.Text = "自动刷新 报价/确认 信息";
            periodicChecking.UseVisualStyleBackColor = true;
            periodicChecking.CheckedChanged += periodicChecking_CheckedChanged;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(1, 117);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(196, 33);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // confirmationAutoPopup
            // 
            confirmationAutoPopup.AutoSize = true;
            confirmationAutoPopup.Location = new Point(12, 68);
            confirmationAutoPopup.Name = "confirmationAutoPopup";
            confirmationAutoPopup.Size = new Size(183, 21);
            confirmationAutoPopup.TabIndex = 6;
            confirmationAutoPopup.Text = "有新的确认信息时自动提示我";
            confirmationAutoPopup.UseVisualStyleBackColor = true;
            // 
            // autoRefreshInternal
            // 
            autoRefreshInternal.Location = new Point(12, 39);
            autoRefreshInternal.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            autoRefreshInternal.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            autoRefreshInternal.Name = "autoRefreshInternal";
            autoRefreshInternal.Size = new Size(56, 23);
            autoRefreshInternal.TabIndex = 7;
            autoRefreshInternal.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(74, 42);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 8;
            label1.Text = "秒刷新一次";
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(197, 150);
            Controls.Add(label1);
            Controls.Add(autoRefreshInternal);
            Controls.Add(confirmationAutoPopup);
            Controls.Add(saveBtn);
            Controls.Add(periodicChecking);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Setting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "全局 报价/确认 设置";
            Load += Setting_Load;
            ((System.ComponentModel.ISupportInitialize)autoRefreshInternal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox periodicChecking;
        private Button saveBtn;
        private CheckBox confirmationAutoPopup;
        private NumericUpDown autoRefreshInternal;
        private Label label1;
    }
}
namespace Steam_Authenticator.Forms
{
    partial class ECOSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ECOSetting));
            saveBtn = new Button();
            groupBox1 = new GroupBox();
            autoSendOffer = new CheckBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(12, 108);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(197, 33);
            saveBtn.TabIndex = 7;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(autoSendOffer);
            groupBox1.Location = new Point(12, 7);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(197, 87);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "订单发货 设置";
            // 
            // autoSendOffer
            // 
            autoSendOffer.AutoSize = true;
            autoSendOffer.Location = new Point(6, 22);
            autoSendOffer.Name = "autoSendOffer";
            autoSendOffer.Size = new Size(99, 21);
            autoSendOffer.TabIndex = 0;
            autoSendOffer.Text = "自动发送报价";
            autoSendOffer.UseVisualStyleBackColor = true;
            // 
            // ECOSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(221, 149);
            Controls.Add(saveBtn);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ECOSetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ECO 设置";
            Load += ECOSetting_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button saveBtn;
        private GroupBox groupBox1;
        private CheckBox autoSendOffer;
    }
}
namespace Steam_Authenticator.Forms
{
    partial class BuffSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuffSetting));
            autoAcceptGiveOffer = new CheckBox();
            saveBtn = new Button();
            SuspendLayout();
            // 
            // autoAcceptGiveOffer
            // 
            autoAcceptGiveOffer.AutoSize = true;
            autoAcceptGiveOffer.Location = new Point(12, 12);
            autoAcceptGiveOffer.Name = "autoAcceptGiveOffer";
            autoAcceptGiveOffer.Size = new Size(123, 21);
            autoAcceptGiveOffer.TabIndex = 11;
            autoAcceptGiveOffer.Text = "自动接收发货报价";
            autoAcceptGiveOffer.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(1, 92);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(173, 33);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // BuffSetting
            // 
            AcceptButton = saveBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(174, 128);
            Controls.Add(autoAcceptGiveOffer);
            Controls.Add(saveBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BuffSetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "BUFF 设置";
            Load += BuffSetting_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox autoAcceptGiveOffer;
        private Button saveBtn;
    }
}
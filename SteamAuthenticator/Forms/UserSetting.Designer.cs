namespace Steam_Authenticator.Forms
{
    partial class UserSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSetting));
            autoAcceptGiveOffer = new CheckBox();
            autoAcceptReceiveOffer = new CheckBox();
            saveBtn = new Button();
            autoConfirmMarket = new CheckBox();
            autoConfirmTrade = new CheckBox();
            periodicChecking = new CheckBox();
            SuspendLayout();
            // 
            // autoAcceptGiveOffer
            // 
            autoAcceptGiveOffer.AutoSize = true;
            autoAcceptGiveOffer.Location = new Point(12, 120);
            autoAcceptGiveOffer.Name = "autoAcceptGiveOffer";
            autoAcceptGiveOffer.Size = new Size(195, 21);
            autoAcceptGiveOffer.TabIndex = 14;
            autoAcceptGiveOffer.Text = "自动接收索取报价（发货报价）";
            autoAcceptGiveOffer.UseVisualStyleBackColor = true;
            // 
            // autoAcceptReceiveOffer
            // 
            autoAcceptReceiveOffer.AutoSize = true;
            autoAcceptReceiveOffer.Location = new Point(12, 93);
            autoAcceptReceiveOffer.Name = "autoAcceptReceiveOffer";
            autoAcceptReceiveOffer.Size = new Size(195, 21);
            autoAcceptReceiveOffer.TabIndex = 13;
            autoAcceptReceiveOffer.Text = "自动接收赠送报价（收货报价）";
            autoAcceptReceiveOffer.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(1, 159);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(206, 33);
            saveBtn.TabIndex = 12;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // autoConfirmMarket
            // 
            autoConfirmMarket.AutoSize = true;
            autoConfirmMarket.Location = new Point(12, 66);
            autoConfirmMarket.Name = "autoConfirmMarket";
            autoConfirmMarket.Size = new Size(123, 21);
            autoConfirmMarket.TabIndex = 11;
            autoConfirmMarket.Text = "自动确认市场上架";
            autoConfirmMarket.UseVisualStyleBackColor = true;
            autoConfirmMarket.CheckedChanged += autoConfirmMarket_CheckedChanged;
            // 
            // autoConfirmTrade
            // 
            autoConfirmTrade.AutoSize = true;
            autoConfirmTrade.Location = new Point(12, 39);
            autoConfirmTrade.Name = "autoConfirmTrade";
            autoConfirmTrade.Size = new Size(99, 21);
            autoConfirmTrade.TabIndex = 10;
            autoConfirmTrade.Text = "自动确认报价";
            autoConfirmTrade.UseVisualStyleBackColor = true;
            autoConfirmTrade.CheckedChanged += autoConfirmTrade_CheckedChanged;
            // 
            // periodicChecking
            // 
            periodicChecking.AutoSize = true;
            periodicChecking.Font = new Font("Microsoft YaHei UI", 9F);
            periodicChecking.Location = new Point(12, 12);
            periodicChecking.Name = "periodicChecking";
            periodicChecking.Size = new Size(147, 21);
            periodicChecking.TabIndex = 15;
            periodicChecking.Text = "自动刷新报价确认信息";
            periodicChecking.UseVisualStyleBackColor = true;
            periodicChecking.CheckedChanged += periodicChecking_CheckedChanged;
            // 
            // UserSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(207, 193);
            Controls.Add(periodicChecking);
            Controls.Add(autoAcceptGiveOffer);
            Controls.Add(autoAcceptReceiveOffer);
            Controls.Add(saveBtn);
            Controls.Add(autoConfirmMarket);
            Controls.Add(autoConfirmTrade);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UserSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "用户设置";
            Load += UserSetting_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox autoAcceptGiveOffer;
        private CheckBox autoAcceptReceiveOffer;
        private Button saveBtn;
        private CheckBox autoConfirmMarket;
        private CheckBox autoConfirmTrade;
        private CheckBox periodicChecking;
    }
}
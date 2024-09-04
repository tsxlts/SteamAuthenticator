namespace Steam_Authenticator.Forms
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
            periodicChecking = new CheckBox();
            checkAll = new CheckBox();
            autoConfirmTrade = new CheckBox();
            autoConfirmMarket = new CheckBox();
            saveBtn = new Button();
            autoAcceptOffer = new CheckBox();
            SuspendLayout();
            // 
            // periodicChecking
            // 
            periodicChecking.AutoSize = true;
            periodicChecking.Font = new Font("Microsoft YaHei UI", 8F);
            periodicChecking.Location = new Point(12, 12);
            periodicChecking.Name = "periodicChecking";
            periodicChecking.Size = new Size(257, 36);
            periodicChecking.TabIndex = 0;
            periodicChecking.Text = "自动刷新报价确认信息\r\n有新的确认信息时自动提示我或者自动帮我确认";
            periodicChecking.UseVisualStyleBackColor = true;
            periodicChecking.CheckedChanged += periodicChecking_CheckedChanged;
            // 
            // checkAll
            // 
            checkAll.AutoSize = true;
            checkAll.Location = new Point(12, 54);
            checkAll.Name = "checkAll";
            checkAll.Size = new Size(159, 21);
            checkAll.TabIndex = 1;
            checkAll.Text = "检测所有账号报价和确认";
            checkAll.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade
            // 
            autoConfirmTrade.AutoSize = true;
            autoConfirmTrade.Location = new Point(12, 81);
            autoConfirmTrade.Name = "autoConfirmTrade";
            autoConfirmTrade.Size = new Size(99, 21);
            autoConfirmTrade.TabIndex = 2;
            autoConfirmTrade.Text = "自动确认报价";
            autoConfirmTrade.UseVisualStyleBackColor = true;
            autoConfirmTrade.CheckedChanged += autoConfirmTrade_CheckedChanged;
            // 
            // autoConfirmMarket
            // 
            autoConfirmMarket.AutoSize = true;
            autoConfirmMarket.Location = new Point(12, 109);
            autoConfirmMarket.Name = "autoConfirmMarket";
            autoConfirmMarket.Size = new Size(123, 21);
            autoConfirmMarket.TabIndex = 3;
            autoConfirmMarket.Text = "自动确认市场上架";
            autoConfirmMarket.UseVisualStyleBackColor = true;
            autoConfirmMarket.CheckedChanged += autoConfirmMarket_CheckedChanged;
            // 
            // saveBtn
            // 
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(12, 171);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(258, 33);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // autoAcceptOffer
            // 
            autoAcceptOffer.AutoSize = true;
            autoAcceptOffer.Location = new Point(12, 136);
            autoAcceptOffer.Name = "autoAcceptOffer";
            autoAcceptOffer.Size = new Size(99, 21);
            autoAcceptOffer.TabIndex = 5;
            autoAcceptOffer.Text = "自动接收报价";
            autoAcceptOffer.UseVisualStyleBackColor = true;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(282, 215);
            Controls.Add(autoAcceptOffer);
            Controls.Add(saveBtn);
            Controls.Add(autoConfirmMarket);
            Controls.Add(autoConfirmTrade);
            Controls.Add(checkAll);
            Controls.Add(periodicChecking);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Setting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "用户设置";
            Load += Setting_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox periodicChecking;
        private CheckBox checkAll;
        private CheckBox autoConfirmTrade;
        private CheckBox autoConfirmMarket;
        private Button saveBtn;
        private CheckBox autoAcceptOffer;
    }
}
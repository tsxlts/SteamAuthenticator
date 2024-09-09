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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting));
            periodicChecking = new CheckBox();
            checkAll = new CheckBox();
            autoConfirmTrade = new CheckBox();
            autoConfirmMarket = new CheckBox();
            saveBtn = new Button();
            autoAcceptReceiveOffer = new CheckBox();
            confirmationAutoPopup = new CheckBox();
            autoRefreshInternal = new NumericUpDown();
            label1 = new Label();
            autoAcceptGiveOffer = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)autoRefreshInternal).BeginInit();
            SuspendLayout();
            // 
            // periodicChecking
            // 
            periodicChecking.AutoSize = true;
            periodicChecking.Font = new Font("Microsoft YaHei UI", 9F);
            periodicChecking.Location = new Point(12, 12);
            periodicChecking.Name = "periodicChecking";
            periodicChecking.Size = new Size(147, 21);
            periodicChecking.TabIndex = 0;
            periodicChecking.Text = "自动刷新报价确认信息";
            periodicChecking.UseVisualStyleBackColor = true;
            periodicChecking.CheckedChanged += periodicChecking_CheckedChanged;
            // 
            // checkAll
            // 
            checkAll.AutoSize = true;
            checkAll.Location = new Point(12, 68);
            checkAll.Name = "checkAll";
            checkAll.Size = new Size(159, 21);
            checkAll.TabIndex = 1;
            checkAll.Text = "检测所有账号报价和确认";
            checkAll.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade
            // 
            autoConfirmTrade.AutoSize = true;
            autoConfirmTrade.Location = new Point(12, 122);
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
            autoConfirmMarket.Location = new Point(12, 149);
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
            saveBtn.Location = new Point(12, 244);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(258, 33);
            saveBtn.TabIndex = 4;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // autoAcceptReceiveOffer
            // 
            autoAcceptReceiveOffer.AutoSize = true;
            autoAcceptReceiveOffer.Location = new Point(12, 176);
            autoAcceptReceiveOffer.Name = "autoAcceptReceiveOffer";
            autoAcceptReceiveOffer.Size = new Size(195, 21);
            autoAcceptReceiveOffer.TabIndex = 5;
            autoAcceptReceiveOffer.Text = "自动接收赠送报价（收货报价）";
            autoAcceptReceiveOffer.UseVisualStyleBackColor = true;
            // 
            // confirmationAutoPopup
            // 
            confirmationAutoPopup.AutoSize = true;
            confirmationAutoPopup.Location = new Point(12, 95);
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
            // autoAcceptGiveOffer
            // 
            autoAcceptGiveOffer.AutoSize = true;
            autoAcceptGiveOffer.Location = new Point(12, 203);
            autoAcceptGiveOffer.Name = "autoAcceptGiveOffer";
            autoAcceptGiveOffer.Size = new Size(195, 21);
            autoAcceptGiveOffer.TabIndex = 9;
            autoAcceptGiveOffer.Text = "自动接收索取报价（发货报价）";
            autoAcceptGiveOffer.UseVisualStyleBackColor = true;
            // 
            // Setting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(282, 289);
            Controls.Add(autoAcceptGiveOffer);
            Controls.Add(label1);
            Controls.Add(autoRefreshInternal);
            Controls.Add(confirmationAutoPopup);
            Controls.Add(autoAcceptReceiveOffer);
            Controls.Add(saveBtn);
            Controls.Add(autoConfirmMarket);
            Controls.Add(autoConfirmTrade);
            Controls.Add(checkAll);
            Controls.Add(periodicChecking);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Setting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "用户设置";
            Load += Setting_Load;
            ((System.ComponentModel.ISupportInitialize)autoRefreshInternal).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox periodicChecking;
        private CheckBox checkAll;
        private CheckBox autoConfirmTrade;
        private CheckBox autoConfirmMarket;
        private Button saveBtn;
        private CheckBox autoAcceptReceiveOffer;
        private CheckBox confirmationAutoPopup;
        private NumericUpDown autoRefreshInternal;
        private Label label1;
        private CheckBox autoAcceptGiveOffer;
    }
}
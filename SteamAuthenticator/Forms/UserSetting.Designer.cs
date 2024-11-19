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
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            autoAcceptGiveOffer_Other = new CheckBox();
            autoAcceptGiveOffer_Buff = new CheckBox();
            groupBox3 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // autoAcceptGiveOffer
            // 
            autoAcceptGiveOffer.AutoSize = true;
            autoAcceptGiveOffer.Location = new Point(6, 23);
            autoAcceptGiveOffer.Name = "autoAcceptGiveOffer";
            autoAcceptGiveOffer.Size = new Size(75, 21);
            autoAcceptGiveOffer.TabIndex = 14;
            autoAcceptGiveOffer.Text = "全部报价";
            autoAcceptGiveOffer.UseVisualStyleBackColor = true;
            autoAcceptGiveOffer.CheckedChanged += autoAcceptGiveOffer_All_CheckedChanged;
            // 
            // autoAcceptReceiveOffer
            // 
            autoAcceptReceiveOffer.AutoSize = true;
            autoAcceptReceiveOffer.Location = new Point(6, 23);
            autoAcceptReceiveOffer.Name = "autoAcceptReceiveOffer";
            autoAcceptReceiveOffer.Size = new Size(75, 21);
            autoAcceptReceiveOffer.TabIndex = 13;
            autoAcceptReceiveOffer.Text = "全部报价";
            autoAcceptReceiveOffer.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(1, 306);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(352, 33);
            saveBtn.TabIndex = 12;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // autoConfirmMarket
            // 
            autoConfirmMarket.AutoSize = true;
            autoConfirmMarket.Location = new Point(6, 49);
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
            autoConfirmTrade.Location = new Point(6, 22);
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
            periodicChecking.Location = new Point(18, 12);
            periodicChecking.Name = "periodicChecking";
            periodicChecking.Size = new Size(147, 21);
            periodicChecking.TabIndex = 15;
            periodicChecking.Text = "自动刷新报价确认信息";
            periodicChecking.UseVisualStyleBackColor = true;
            periodicChecking.CheckedChanged += periodicChecking_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.BackColor = SystemColors.Control;
            groupBox1.Controls.Add(autoConfirmTrade);
            groupBox1.Controls.Add(autoConfirmMarket);
            groupBox1.Location = new Point(12, 39);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(329, 84);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "自动令牌确认";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(autoAcceptGiveOffer_Other);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Buff);
            groupBox2.Controls.Add(autoAcceptGiveOffer);
            groupBox2.Location = new Point(12, 138);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(329, 66);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "自动接受 索取 报价（发货 报价）";
            // 
            // autoAcceptGiveOffer_Other
            // 
            autoAcceptGiveOffer_Other.AutoSize = true;
            autoAcceptGiveOffer_Other.Location = new Point(173, 23);
            autoAcceptGiveOffer_Other.Name = "autoAcceptGiveOffer_Other";
            autoAcceptGiveOffer_Other.Size = new Size(75, 21);
            autoAcceptGiveOffer_Other.TabIndex = 16;
            autoAcceptGiveOffer_Other.Text = "其他报价";
            autoAcceptGiveOffer_Other.UseVisualStyleBackColor = true;
            autoAcceptGiveOffer_Other.CheckedChanged += autoAcceptGiveOffer_CheckedChanged;
            // 
            // autoAcceptGiveOffer_Buff
            // 
            autoAcceptGiveOffer_Buff.AutoSize = true;
            autoAcceptGiveOffer_Buff.Location = new Point(87, 23);
            autoAcceptGiveOffer_Buff.Name = "autoAcceptGiveOffer_Buff";
            autoAcceptGiveOffer_Buff.Size = new Size(80, 21);
            autoAcceptGiveOffer_Buff.TabIndex = 15;
            autoAcceptGiveOffer_Buff.Text = "BUFF报价";
            autoAcceptGiveOffer_Buff.UseVisualStyleBackColor = true;
            autoAcceptGiveOffer_Buff.CheckedChanged += autoAcceptGiveOffer_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(autoAcceptReceiveOffer);
            groupBox3.Location = new Point(12, 221);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(329, 66);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "自动接受 赠送 报价（收货 报价）";
            // 
            // UserSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(353, 340);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(periodicChecking);
            Controls.Add(saveBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UserSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "用户设置";
            Load += UserSetting_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
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
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private CheckBox autoAcceptGiveOffer_Buff;
        private CheckBox autoAcceptGiveOffer_Other;
        private GroupBox groupBox3;
    }
}
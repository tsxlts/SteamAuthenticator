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
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            setAcceptGiveOfferRoleBtn = new LinkLabel();
            autoAcceptGiveOffer_Custom = new CheckBox();
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
            autoAcceptGiveOffer.Location = new Point(6, 76);
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
            saveBtn.Location = new Point(1, 375);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(489, 33);
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
            periodicChecking.Size = new Size(160, 21);
            periodicChecking.TabIndex = 15;
            periodicChecking.Text = "自动刷新 报价/确认 信息";
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
            groupBox1.Size = new Size(466, 84);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "自动令牌确认";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(setAcceptGiveOfferRoleBtn);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Custom);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Other);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Buff);
            groupBox2.Controls.Add(autoAcceptGiveOffer);
            groupBox2.Location = new Point(12, 129);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(466, 168);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "自动接受 索取 报价（发货 报价）";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(99, 50);
            label4.Name = "label4";
            label4.Size = new Size(296, 17);
            label4.TabIndex = 22;
            label4.Text = "除了以上报价以外, 通过其他平台/方式向你发起的报价";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(99, 23);
            label3.Name = "label3";
            label3.Size = new Size(289, 17);
            label3.TabIndex = 21;
            label3.Text = "BUFF平台向你发起的报价, 仅在登录BUFF帐号后生效";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(99, 77);
            label2.Name = "label2";
            label2.Size = new Size(310, 17);
            label2.TabIndex = 20;
            label2.Text = "勾选后自动接受所有报价, 其余开关全部忽略, 请谨慎勾选";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(161, 104);
            label1.Name = "label1";
            label1.Size = new Size(291, 17);
            label1.TabIndex = 19;
            label1.Text = "勾选后仅自动接受满足规则的报价, 其余开关全部忽略";
            // 
            // setAcceptGiveOfferRoleBtn
            // 
            setAcceptGiveOfferRoleBtn.AutoSize = true;
            setAcceptGiveOfferRoleBtn.Location = new Point(99, 104);
            setAcceptGiveOfferRoleBtn.Name = "setAcceptGiveOfferRoleBtn";
            setAcceptGiveOfferRoleBtn.Size = new Size(56, 17);
            setAcceptGiveOfferRoleBtn.TabIndex = 18;
            setAcceptGiveOfferRoleBtn.TabStop = true;
            setAcceptGiveOfferRoleBtn.Text = "规则设置";
            setAcceptGiveOfferRoleBtn.LinkClicked += setAcceptGiveOfferRoleBtn_LinkClicked;
            // 
            // autoAcceptGiveOffer_Custom
            // 
            autoAcceptGiveOffer_Custom.AutoSize = true;
            autoAcceptGiveOffer_Custom.Location = new Point(6, 103);
            autoAcceptGiveOffer_Custom.Name = "autoAcceptGiveOffer_Custom";
            autoAcceptGiveOffer_Custom.Size = new Size(87, 21);
            autoAcceptGiveOffer_Custom.TabIndex = 17;
            autoAcceptGiveOffer_Custom.Text = "自定义规则";
            autoAcceptGiveOffer_Custom.UseVisualStyleBackColor = true;
            autoAcceptGiveOffer_Custom.CheckedChanged += autoAcceptGiveOffer_Custom_CheckedChanged;
            // 
            // autoAcceptGiveOffer_Other
            // 
            autoAcceptGiveOffer_Other.AutoSize = true;
            autoAcceptGiveOffer_Other.Location = new Point(6, 49);
            autoAcceptGiveOffer_Other.Name = "autoAcceptGiveOffer_Other";
            autoAcceptGiveOffer_Other.Size = new Size(75, 21);
            autoAcceptGiveOffer_Other.TabIndex = 16;
            autoAcceptGiveOffer_Other.Text = "其他报价";
            autoAcceptGiveOffer_Other.UseVisualStyleBackColor = true;
            // 
            // autoAcceptGiveOffer_Buff
            // 
            autoAcceptGiveOffer_Buff.AutoSize = true;
            autoAcceptGiveOffer_Buff.Location = new Point(6, 22);
            autoAcceptGiveOffer_Buff.Name = "autoAcceptGiveOffer_Buff";
            autoAcceptGiveOffer_Buff.Size = new Size(80, 21);
            autoAcceptGiveOffer_Buff.TabIndex = 15;
            autoAcceptGiveOffer_Buff.Text = "BUFF报价";
            autoAcceptGiveOffer_Buff.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(autoAcceptReceiveOffer);
            groupBox3.Location = new Point(12, 303);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(466, 66);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "自动接受 赠送 报价（收货 报价）";
            // 
            // UserSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(490, 409);
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
            StartPosition = FormStartPosition.CenterParent;
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
        private CheckBox autoAcceptGiveOffer_Custom;
        private LinkLabel setAcceptGiveOfferRoleBtn;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
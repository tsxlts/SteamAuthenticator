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
            autoConfirmTrade_C5 = new CheckBox();
            label19 = new Label();
            label20 = new Label();
            autoAcceptGiveOffer_C5 = new CheckBox();
            autoConfirmTrade_YouPin = new CheckBox();
            label13 = new Label();
            label14 = new Label();
            autoAcceptGiveOffer_YouPin = new CheckBox();
            label12 = new Label();
            autoConfirmTrade_Eco = new CheckBox();
            label10 = new Label();
            label11 = new Label();
            autoAcceptGiveOffer_Eco = new CheckBox();
            autoConfirmTrade_Custom = new CheckBox();
            autoConfirmTrade_Other = new CheckBox();
            autoConfirmTrade_Buff = new CheckBox();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            setAcceptGiveOfferRoleBtn = new LinkLabel();
            autoAcceptGiveOffer_Custom = new CheckBox();
            autoAcceptGiveOffer_Other = new CheckBox();
            autoAcceptGiveOffer_Buff = new CheckBox();
            groupBox3 = new GroupBox();
            label9 = new Label();
            label15 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // autoAcceptGiveOffer
            // 
            autoAcceptGiveOffer.AutoSize = true;
            autoAcceptGiveOffer.Location = new Point(86, 50);
            autoAcceptGiveOffer.Name = "autoAcceptGiveOffer";
            autoAcceptGiveOffer.Size = new Size(75, 21);
            autoAcceptGiveOffer.TabIndex = 14;
            autoAcceptGiveOffer.Text = "自动接受";
            autoAcceptGiveOffer.UseVisualStyleBackColor = true;
            autoAcceptGiveOffer.CheckedChanged += autoAcceptGiveOffer_All_CheckedChanged;
            // 
            // autoAcceptReceiveOffer
            // 
            autoAcceptReceiveOffer.AutoSize = true;
            autoAcceptReceiveOffer.Location = new Point(86, 23);
            autoAcceptReceiveOffer.Name = "autoAcceptReceiveOffer";
            autoAcceptReceiveOffer.Size = new Size(75, 21);
            autoAcceptReceiveOffer.TabIndex = 13;
            autoAcceptReceiveOffer.Text = "自动接受";
            autoAcceptReceiveOffer.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(1, 445);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(754, 33);
            saveBtn.TabIndex = 12;
            saveBtn.Text = "保存设置";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // autoConfirmMarket
            // 
            autoConfirmMarket.AutoSize = true;
            autoConfirmMarket.Location = new Point(6, 22);
            autoConfirmMarket.Name = "autoConfirmMarket";
            autoConfirmMarket.Size = new Size(123, 21);
            autoConfirmMarket.TabIndex = 11;
            autoConfirmMarket.Text = "自动确认市场上架";
            autoConfirmMarket.UseVisualStyleBackColor = true;
            autoConfirmMarket.CheckedChanged += autoConfirm_CheckedChanged;
            // 
            // autoConfirmTrade
            // 
            autoConfirmTrade.AutoSize = true;
            autoConfirmTrade.ForeColor = Color.Red;
            autoConfirmTrade.Location = new Point(167, 50);
            autoConfirmTrade.Name = "autoConfirmTrade";
            autoConfirmTrade.Size = new Size(75, 21);
            autoConfirmTrade.TabIndex = 10;
            autoConfirmTrade.Text = "自动确认";
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
            groupBox1.Controls.Add(autoConfirmMarket);
            groupBox1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            groupBox1.Location = new Point(12, 39);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(731, 66);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "自动令牌确认";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(autoConfirmTrade_C5);
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(autoAcceptGiveOffer_C5);
            groupBox2.Controls.Add(autoConfirmTrade_YouPin);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(autoAcceptGiveOffer_YouPin);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(autoConfirmTrade_Eco);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Eco);
            groupBox2.Controls.Add(autoConfirmTrade);
            groupBox2.Controls.Add(autoConfirmTrade_Custom);
            groupBox2.Controls.Add(autoConfirmTrade_Other);
            groupBox2.Controls.Add(autoConfirmTrade_Buff);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(setAcceptGiveOfferRoleBtn);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Custom);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Other);
            groupBox2.Controls.Add(autoAcceptGiveOffer_Buff);
            groupBox2.Controls.Add(autoAcceptGiveOffer);
            groupBox2.Location = new Point(12, 111);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(731, 246);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "自动发货 设置（发送的/收到的 发货报价）";
            // 
            // autoConfirmTrade_C5
            // 
            autoConfirmTrade_C5.AutoSize = true;
            autoConfirmTrade_C5.ForeColor = Color.Red;
            autoConfirmTrade_C5.Location = new Point(167, 185);
            autoConfirmTrade_C5.Name = "autoConfirmTrade_C5";
            autoConfirmTrade_C5.Size = new Size(75, 21);
            autoConfirmTrade_C5.TabIndex = 43;
            autoConfirmTrade_C5.Text = "自动确认";
            autoConfirmTrade_C5.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            label19.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label19.Location = new Point(6, 186);
            label19.Name = "label19";
            label19.Size = new Size(74, 17);
            label19.TabIndex = 42;
            label19.Text = "C5GAME 报价";
            label19.TextAlign = ContentAlignment.TopRight;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label20.ForeColor = Color.Gray;
            label20.Location = new Point(249, 186);
            label20.Name = "label20";
            label20.Size = new Size(376, 17);
            label20.TabIndex = 41;
            label20.Text = "C5Game平台产生的报价, 仅在设置C5GAME开放平台AppKey后生效";
            // 
            // autoAcceptGiveOffer_C5
            // 
            autoAcceptGiveOffer_C5.AutoSize = true;
            autoAcceptGiveOffer_C5.Location = new Point(86, 185);
            autoAcceptGiveOffer_C5.Name = "autoAcceptGiveOffer_C5";
            autoAcceptGiveOffer_C5.Size = new Size(75, 21);
            autoAcceptGiveOffer_C5.TabIndex = 40;
            autoAcceptGiveOffer_C5.Text = "自动接受";
            autoAcceptGiveOffer_C5.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade_YouPin
            // 
            autoConfirmTrade_YouPin.AutoSize = true;
            autoConfirmTrade_YouPin.ForeColor = Color.Red;
            autoConfirmTrade_YouPin.Location = new Point(167, 158);
            autoConfirmTrade_YouPin.Name = "autoConfirmTrade_YouPin";
            autoConfirmTrade_YouPin.Size = new Size(75, 21);
            autoConfirmTrade_YouPin.TabIndex = 39;
            autoConfirmTrade_YouPin.Text = "自动确认";
            autoConfirmTrade_YouPin.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            label13.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label13.Location = new Point(6, 159);
            label13.Name = "label13";
            label13.Size = new Size(74, 17);
            label13.TabIndex = 38;
            label13.Text = "悠悠 报价";
            label13.TextAlign = ContentAlignment.TopRight;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label14.ForeColor = Color.Gray;
            label14.Location = new Point(249, 159);
            label14.Name = "label14";
            label14.Size = new Size(303, 17);
            label14.TabIndex = 37;
            label14.Text = "悠悠有品平台产生的报价, 仅在登录悠悠有品帐号后生效";
            // 
            // autoAcceptGiveOffer_YouPin
            // 
            autoAcceptGiveOffer_YouPin.AutoSize = true;
            autoAcceptGiveOffer_YouPin.Location = new Point(86, 158);
            autoAcceptGiveOffer_YouPin.Name = "autoAcceptGiveOffer_YouPin";
            autoAcceptGiveOffer_YouPin.Size = new Size(75, 21);
            autoAcceptGiveOffer_YouPin.TabIndex = 36;
            autoAcceptGiveOffer_YouPin.Text = "自动接受";
            autoAcceptGiveOffer_YouPin.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label12.ForeColor = Color.FromArgb(57, 89, 220);
            label12.Location = new Point(6, 25);
            label12.Name = "label12";
            label12.Size = new Size(359, 17);
            label12.TabIndex = 35;
            label12.Text = "自动接收 对收到的报价生效, 自动确认 对发送的和收到的报价生效\r\n";
            // 
            // autoConfirmTrade_Eco
            // 
            autoConfirmTrade_Eco.AutoSize = true;
            autoConfirmTrade_Eco.ForeColor = Color.Red;
            autoConfirmTrade_Eco.Location = new Point(167, 131);
            autoConfirmTrade_Eco.Name = "autoConfirmTrade_Eco";
            autoConfirmTrade_Eco.Size = new Size(75, 21);
            autoConfirmTrade_Eco.TabIndex = 34;
            autoConfirmTrade_Eco.Text = "自动确认";
            autoConfirmTrade_Eco.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label10.Location = new Point(6, 132);
            label10.Name = "label10";
            label10.Size = new Size(74, 17);
            label10.TabIndex = 33;
            label10.Text = "ECO 报价";
            label10.TextAlign = ContentAlignment.TopRight;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label11.ForeColor = Color.Gray;
            label11.Location = new Point(249, 132);
            label11.Name = "label11";
            label11.Size = new Size(257, 17);
            label11.TabIndex = 32;
            label11.Text = "ECO平台产生的报价, 仅在登录ECO帐号后生效";
            // 
            // autoAcceptGiveOffer_Eco
            // 
            autoAcceptGiveOffer_Eco.AutoSize = true;
            autoAcceptGiveOffer_Eco.Location = new Point(86, 131);
            autoAcceptGiveOffer_Eco.Name = "autoAcceptGiveOffer_Eco";
            autoAcceptGiveOffer_Eco.Size = new Size(75, 21);
            autoAcceptGiveOffer_Eco.TabIndex = 31;
            autoAcceptGiveOffer_Eco.Text = "自动接受";
            autoAcceptGiveOffer_Eco.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade_Custom
            // 
            autoConfirmTrade_Custom.AutoSize = true;
            autoConfirmTrade_Custom.ForeColor = Color.Red;
            autoConfirmTrade_Custom.Location = new Point(167, 77);
            autoConfirmTrade_Custom.Name = "autoConfirmTrade_Custom";
            autoConfirmTrade_Custom.Size = new Size(75, 21);
            autoConfirmTrade_Custom.TabIndex = 30;
            autoConfirmTrade_Custom.Text = "自动确认";
            autoConfirmTrade_Custom.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade_Other
            // 
            autoConfirmTrade_Other.AutoSize = true;
            autoConfirmTrade_Other.ForeColor = Color.Red;
            autoConfirmTrade_Other.Location = new Point(167, 212);
            autoConfirmTrade_Other.Name = "autoConfirmTrade_Other";
            autoConfirmTrade_Other.Size = new Size(75, 21);
            autoConfirmTrade_Other.TabIndex = 29;
            autoConfirmTrade_Other.Text = "自动确认";
            autoConfirmTrade_Other.UseVisualStyleBackColor = true;
            // 
            // autoConfirmTrade_Buff
            // 
            autoConfirmTrade_Buff.AutoSize = true;
            autoConfirmTrade_Buff.ForeColor = Color.Red;
            autoConfirmTrade_Buff.Location = new Point(167, 104);
            autoConfirmTrade_Buff.Name = "autoConfirmTrade_Buff";
            autoConfirmTrade_Buff.Size = new Size(75, 21);
            autoConfirmTrade_Buff.TabIndex = 28;
            autoConfirmTrade_Buff.Text = "自动确认";
            autoConfirmTrade_Buff.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label8.Location = new Point(6, 77);
            label8.Name = "label8";
            label8.Size = new Size(74, 17);
            label8.TabIndex = 26;
            label8.Text = "自定义规则";
            label8.TextAlign = ContentAlignment.TopRight;
            // 
            // label7
            // 
            label7.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label7.Location = new Point(6, 51);
            label7.Name = "label7";
            label7.Size = new Size(74, 17);
            label7.TabIndex = 25;
            label7.Text = "全部 报价";
            label7.TextAlign = ContentAlignment.TopRight;
            // 
            // label6
            // 
            label6.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label6.Location = new Point(6, 213);
            label6.Name = "label6";
            label6.Size = new Size(74, 17);
            label6.TabIndex = 24;
            label6.Text = "其他 报价";
            label6.TextAlign = ContentAlignment.TopRight;
            // 
            // label5
            // 
            label5.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label5.Location = new Point(6, 105);
            label5.Name = "label5";
            label5.Size = new Size(74, 17);
            label5.TabIndex = 23;
            label5.Text = "BUFF 报价";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(249, 213);
            label4.Name = "label4";
            label4.Size = new Size(272, 17);
            label4.TabIndex = 22;
            label4.Text = "除了以上报价以外, 通过其他平台/方式产生的报价";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(249, 105);
            label3.Name = "label3";
            label3.Size = new Size(265, 17);
            label3.TabIndex = 21;
            label3.Text = "BUFF平台产生的报价, 仅在登录BUFF帐号后生效";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.ForeColor = Color.Red;
            label2.Location = new Point(249, 51);
            label2.Name = "label2";
            label2.Size = new Size(310, 17);
            label2.TabIndex = 20;
            label2.Text = "勾选后自动处理所有报价, 其余开关全部忽略, 请谨慎勾选";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(311, 77);
            label1.Name = "label1";
            label1.Size = new Size(176, 17);
            label1.TabIndex = 19;
            label1.Text = "勾选后自动处理满足规则的报价";
            // 
            // setAcceptGiveOfferRoleBtn
            // 
            setAcceptGiveOfferRoleBtn.AutoSize = true;
            setAcceptGiveOfferRoleBtn.Location = new Point(249, 77);
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
            autoAcceptGiveOffer_Custom.Location = new Point(86, 77);
            autoAcceptGiveOffer_Custom.Name = "autoAcceptGiveOffer_Custom";
            autoAcceptGiveOffer_Custom.Size = new Size(75, 21);
            autoAcceptGiveOffer_Custom.TabIndex = 17;
            autoAcceptGiveOffer_Custom.Text = "自动接受";
            autoAcceptGiveOffer_Custom.UseVisualStyleBackColor = true;
            // 
            // autoAcceptGiveOffer_Other
            // 
            autoAcceptGiveOffer_Other.AutoSize = true;
            autoAcceptGiveOffer_Other.Location = new Point(86, 212);
            autoAcceptGiveOffer_Other.Name = "autoAcceptGiveOffer_Other";
            autoAcceptGiveOffer_Other.Size = new Size(75, 21);
            autoAcceptGiveOffer_Other.TabIndex = 16;
            autoAcceptGiveOffer_Other.Text = "自动接受";
            autoAcceptGiveOffer_Other.UseVisualStyleBackColor = true;
            // 
            // autoAcceptGiveOffer_Buff
            // 
            autoAcceptGiveOffer_Buff.AutoSize = true;
            autoAcceptGiveOffer_Buff.Location = new Point(86, 104);
            autoAcceptGiveOffer_Buff.Name = "autoAcceptGiveOffer_Buff";
            autoAcceptGiveOffer_Buff.Size = new Size(75, 21);
            autoAcceptGiveOffer_Buff.TabIndex = 15;
            autoAcceptGiveOffer_Buff.Text = "自动接受";
            autoAcceptGiveOffer_Buff.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(autoAcceptReceiveOffer);
            groupBox3.Location = new Point(12, 363);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(731, 66);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "自动收货 设置（收到的 收货报价）";
            // 
            // label9
            // 
            label9.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label9.Location = new Point(6, 24);
            label9.Name = "label9";
            label9.Size = new Size(74, 17);
            label9.TabIndex = 26;
            label9.Text = "全部 报价";
            label9.TextAlign = ContentAlignment.TopRight;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Gray;
            label15.Location = new Point(184, 13);
            label15.Name = "label15";
            label15.Size = new Size(68, 17);
            label15.TabIndex = 19;
            label15.Text = "请确保已在";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Red;
            label16.Location = new Point(392, 13);
            label16.Name = "label16";
            label16.Size = new Size(141, 17);
            label16.TabIndex = 20;
            label16.Text = "自动刷新 报价/确认 信息";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.Red;
            label17.Location = new Point(258, 13);
            label17.Name = "label17";
            label17.Size = new Size(56, 17);
            label17.TabIndex = 21;
            label17.Text = "全局设置";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ForeColor = Color.Gray;
            label18.Location = new Point(320, 13);
            label18.Name = "label18";
            label18.Size = new Size(68, 17);
            label18.TabIndex = 21;
            label18.Text = "菜单下开启";
            // 
            // UserSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(755, 479);
            Controls.Add(label18);
            Controls.Add(label16);
            Controls.Add(label17);
            Controls.Add(label15);
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
        private Label label5;
        private Label label6;
        private Label label8;
        private Label label7;
        private CheckBox autoConfirmTrade_Custom;
        private CheckBox autoConfirmTrade_Other;
        private CheckBox autoConfirmTrade_Buff;
        private Label label9;
        private CheckBox autoConfirmTrade_Eco;
        private Label label10;
        private Label label11;
        private CheckBox autoAcceptGiveOffer_Eco;
        private Label label12;
        private CheckBox autoConfirmTrade_YouPin;
        private Label label13;
        private Label label14;
        private CheckBox autoAcceptGiveOffer_YouPin;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private CheckBox autoConfirmTrade_C5;
        private Label label19;
        private Label label20;
        private CheckBox autoAcceptGiveOffer_C5;
    }
}
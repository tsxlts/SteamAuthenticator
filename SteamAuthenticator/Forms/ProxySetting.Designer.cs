namespace Steam_Authenticator.Forms
{
    partial class ProxySetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxySetting));
            label1 = new Label();
            communityBox = new TextBox();
            loginBox = new TextBox();
            label2 = new Label();
            storeBox = new TextBox();
            label3 = new Label();
            apiBox = new TextBox();
            label4 = new Label();
            saveBtn = new Button();
            groupBox1 = new GroupBox();
            useCustomerDomain = new CheckBox();
            groupBox2 = new GroupBox();
            useCustomerProxy = new CheckBox();
            proxyPortBox = new NumericUpDown();
            label5 = new Label();
            proxyAddressBox = new TextBox();
            label6 = new Label();
            proxyHostBox = new TextBox();
            panel1 = new Panel();
            helpBox = new TextBox();
            label7 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)proxyPortBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 58);
            label1.Name = "label1";
            label1.Size = new Size(92, 17);
            label1.TabIndex = 0;
            label1.Text = "Steam社区地址";
            // 
            // communityBox
            // 
            communityBox.Location = new Point(104, 56);
            communityBox.Name = "communityBox";
            communityBox.Size = new Size(280, 23);
            communityBox.TabIndex = 2002;
            // 
            // loginBox
            // 
            loginBox.Location = new Point(104, 85);
            loginBox.Name = "loginBox";
            loginBox.Size = new Size(280, 23);
            loginBox.TabIndex = 2003;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 88);
            label2.Name = "label2";
            label2.Size = new Size(92, 17);
            label2.TabIndex = 2;
            label2.Text = "Steam登录地址";
            // 
            // storeBox
            // 
            storeBox.Location = new Point(104, 114);
            storeBox.Name = "storeBox";
            storeBox.Size = new Size(280, 23);
            storeBox.TabIndex = 2004;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 116);
            label3.Name = "label3";
            label3.Size = new Size(92, 17);
            label3.TabIndex = 4;
            label3.Text = "Steam商店地址";
            // 
            // apiBox
            // 
            apiBox.Location = new Point(104, 172);
            apiBox.Name = "apiBox";
            apiBox.Size = new Size(280, 23);
            apiBox.TabIndex = 2005;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 174);
            label4.Name = "label4";
            label4.Size = new Size(87, 17);
            label4.TabIndex = 6;
            label4.Text = "SteamApi地址";
            // 
            // saveBtn
            // 
            saveBtn.Dock = DockStyle.Fill;
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(0, 0);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(415, 45);
            saveBtn.TabIndex = 99999;
            saveBtn.Text = "保存";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(helpBox);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(useCustomerDomain);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(communityBox);
            groupBox1.Controls.Add(apiBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(loginBox);
            groupBox1.Controls.Add(storeBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(12, 158);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(394, 209);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "域名配置";
            // 
            // useCustomerDomain
            // 
            useCustomerDomain.AutoSize = true;
            useCustomerDomain.Location = new Point(9, 24);
            useCustomerDomain.Name = "useCustomerDomain";
            useCustomerDomain.Size = new Size(99, 21);
            useCustomerDomain.TabIndex = 2001;
            useCustomerDomain.Text = "使用以下域名";
            useCustomerDomain.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(useCustomerProxy);
            groupBox2.Controls.Add(proxyPortBox);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(proxyAddressBox);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(proxyHostBox);
            groupBox2.Location = new Point(12, 18);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(394, 134);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "代理配置";
            // 
            // useCustomerProxy
            // 
            useCustomerProxy.AutoSize = true;
            useCustomerProxy.Location = new Point(9, 24);
            useCustomerProxy.Name = "useCustomerProxy";
            useCustomerProxy.Size = new Size(99, 21);
            useCustomerProxy.TabIndex = 1001;
            useCustomerProxy.Text = "使用以下代理";
            useCustomerProxy.UseVisualStyleBackColor = true;
            // 
            // proxyPortBox
            // 
            proxyPortBox.Location = new Point(306, 91);
            proxyPortBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            proxyPortBox.Name = "proxyPortBox";
            proxyPortBox.Size = new Size(78, 23);
            proxyPortBox.TabIndex = 1004;
            proxyPortBox.Value = new decimal(new int[] { 8080, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 57);
            label5.Name = "label5";
            label5.Size = new Size(80, 17);
            label5.TabIndex = 6;
            label5.Text = "指定代理地址";
            // 
            // proxyAddressBox
            // 
            proxyAddressBox.Location = new Point(104, 54);
            proxyAddressBox.Name = "proxyAddressBox";
            proxyAddressBox.Size = new Size(280, 23);
            proxyAddressBox.TabIndex = 1002;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 94);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 8;
            label6.Text = "指定代理主机";
            // 
            // proxyHostBox
            // 
            proxyHostBox.Location = new Point(104, 91);
            proxyHostBox.Name = "proxyHostBox";
            proxyHostBox.Size = new Size(196, 23);
            proxyHostBox.TabIndex = 1003;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(saveBtn);
            panel1.Location = new Point(1, 380);
            panel1.Name = "panel1";
            panel1.Size = new Size(415, 45);
            panel1.TabIndex = 13;
            // 
            // helpBox
            // 
            helpBox.Location = new Point(104, 143);
            helpBox.Name = "helpBox";
            helpBox.Size = new Size(280, 23);
            helpBox.TabIndex = 2007;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 145);
            label7.Name = "label7";
            label7.Size = new Size(92, 17);
            label7.TabIndex = 2006;
            label7.Text = "Steam客服地址";
            // 
            // ProxySetting
            // 
            AcceptButton = saveBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(418, 426);
            Controls.Add(panel1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProxySetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "代理设置";
            Load += ProxySetting_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)proxyPortBox).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private TextBox communityBox;
        private TextBox loginBox;
        private Label label2;
        private TextBox storeBox;
        private Label label3;
        private TextBox apiBox;
        private Label label4;
        private Button saveBtn;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label5;
        private TextBox proxyAddressBox;
        private Label label6;
        private NumericUpDown proxyPortBox;
        private TextBox proxyHostBox;
        private CheckBox useCustomerDomain;
        private CheckBox useCustomerProxy;
        private Panel panel1;
        private TextBox helpBox;
        private Label label7;
    }
}
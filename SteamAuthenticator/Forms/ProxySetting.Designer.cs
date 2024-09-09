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
            cancelBtn = new Button();
            splitContainer1 = new SplitContainer();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            proxyPortBox = new NumericUpDown();
            label5 = new Label();
            proxyAddressBox = new TextBox();
            label6 = new Label();
            proxyHostBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)proxyPortBox).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 31);
            label1.Name = "label1";
            label1.Size = new Size(92, 17);
            label1.TabIndex = 0;
            label1.Text = "Steam社区地址";
            // 
            // communityBox
            // 
            communityBox.Location = new Point(104, 29);
            communityBox.Name = "communityBox";
            communityBox.Size = new Size(255, 23);
            communityBox.TabIndex = 1;
            // 
            // loginBox
            // 
            loginBox.Location = new Point(104, 72);
            loginBox.Name = "loginBox";
            loginBox.Size = new Size(255, 23);
            loginBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 75);
            label2.Name = "label2";
            label2.Size = new Size(92, 17);
            label2.TabIndex = 2;
            label2.Text = "Steam登录地址";
            // 
            // storeBox
            // 
            storeBox.Location = new Point(104, 115);
            storeBox.Name = "storeBox";
            storeBox.Size = new Size(255, 23);
            storeBox.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 117);
            label3.Name = "label3";
            label3.Size = new Size(92, 17);
            label3.TabIndex = 4;
            label3.Text = "Steam商店地址";
            // 
            // apiBox
            // 
            apiBox.Location = new Point(104, 159);
            apiBox.Name = "apiBox";
            apiBox.Size = new Size(255, 23);
            apiBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 161);
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
            saveBtn.Size = new Size(191, 40);
            saveBtn.TabIndex = 8;
            saveBtn.Text = "保存";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Dock = DockStyle.Fill;
            cancelBtn.Font = new Font("Microsoft YaHei UI", 12F);
            cancelBtn.Location = new Point(0, 0);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(195, 40);
            cancelBtn.TabIndex = 9;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(1, 384);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(cancelBtn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(saveBtn);
            splitContainer1.Size = new Size(390, 40);
            splitContainer1.SplitterDistance = 195;
            splitContainer1.TabIndex = 10;
            // 
            // groupBox1
            // 
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
            groupBox1.Size = new Size(365, 196);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "域名配置";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(proxyPortBox);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(proxyAddressBox);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(proxyHostBox);
            groupBox2.Location = new Point(12, 18);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(365, 114);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "代理配置";
            // 
            // proxyPortBox
            // 
            proxyPortBox.Location = new Point(281, 70);
            proxyPortBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            proxyPortBox.Name = "proxyPortBox";
            proxyPortBox.Size = new Size(78, 23);
            proxyPortBox.TabIndex = 11;
            proxyPortBox.Value = new decimal(new int[] { 8080, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 30);
            label5.Name = "label5";
            label5.Size = new Size(80, 17);
            label5.TabIndex = 6;
            label5.Text = "指定代理地址";
            // 
            // proxyAddressBox
            // 
            proxyAddressBox.Location = new Point(104, 27);
            proxyAddressBox.Name = "proxyAddressBox";
            proxyAddressBox.Size = new Size(255, 23);
            proxyAddressBox.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 73);
            label6.Name = "label6";
            label6.Size = new Size(80, 17);
            label6.TabIndex = 8;
            label6.Text = "指定代理主机";
            // 
            // proxyHostBox
            // 
            proxyHostBox.Location = new Point(104, 70);
            proxyHostBox.Name = "proxyHostBox";
            proxyHostBox.Size = new Size(171, 23);
            proxyHostBox.TabIndex = 9;
            // 
            // ProxySetting
            // 
            AcceptButton = saveBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelBtn;
            ClientSize = new Size(391, 426);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProxySetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "代理设置";
            Load += ProxySetting_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)proxyPortBox).EndInit();
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
        private Button cancelBtn;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label5;
        private TextBox proxyAddressBox;
        private Label label6;
        private NumericUpDown proxyPortBox;
        private TextBox proxyHostBox;
    }
}
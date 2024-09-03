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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(92, 17);
            label1.TabIndex = 0;
            label1.Text = "Steam社区地址";
            // 
            // communityBox
            // 
            communityBox.Location = new Point(110, 19);
            communityBox.Name = "communityBox";
            communityBox.Size = new Size(267, 23);
            communityBox.TabIndex = 1;
            // 
            // loginBox
            // 
            loginBox.Location = new Point(110, 62);
            loginBox.Name = "loginBox";
            loginBox.Size = new Size(267, 23);
            loginBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(92, 17);
            label2.TabIndex = 2;
            label2.Text = "Steam登录地址";
            // 
            // storeBox
            // 
            storeBox.Location = new Point(110, 105);
            storeBox.Name = "storeBox";
            storeBox.Size = new Size(267, 23);
            storeBox.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 107);
            label3.Name = "label3";
            label3.Size = new Size(92, 17);
            label3.TabIndex = 4;
            label3.Text = "Steam商店地址";
            // 
            // apiBox
            // 
            apiBox.Location = new Point(110, 149);
            apiBox.Name = "apiBox";
            apiBox.Size = new Size(267, 23);
            apiBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 151);
            label4.Name = "label4";
            label4.Size = new Size(87, 17);
            label4.TabIndex = 6;
            label4.Text = "SteamApi地址";
            // 
            // saveBtn
            // 
            saveBtn.Font = new Font("Microsoft YaHei UI", 12F);
            saveBtn.Location = new Point(207, 246);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(172, 30);
            saveBtn.TabIndex = 8;
            saveBtn.Text = "保存";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Font = new Font("Microsoft YaHei UI", 12F);
            cancelBtn.Location = new Point(12, 246);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(172, 30);
            cancelBtn.TabIndex = 9;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // ProxySetting
            // 
            AcceptButton = saveBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(391, 284);
            Controls.Add(cancelBtn);
            Controls.Add(saveBtn);
            Controls.Add(apiBox);
            Controls.Add(label4);
            Controls.Add(storeBox);
            Controls.Add(label3);
            Controls.Add(loginBox);
            Controls.Add(label2);
            Controls.Add(communityBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProxySetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "代理设置";
            Load += ProxySetting_Load;
            ResumeLayout(false);
            PerformLayout();
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
    }
}
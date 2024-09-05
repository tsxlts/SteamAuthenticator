namespace Steam_Authenticator.Forms
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            User = new TextBox();
            label1 = new Label();
            label2 = new Label();
            Password = new TextBox();
            loginBtn = new Button();
            cancelBtn = new Button();
            SuspendLayout();
            // 
            // User
            // 
            User.Location = new Point(62, 20);
            User.Name = "User";
            User.Size = new Size(135, 23);
            User.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 23);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 1;
            label1.Text = "用户名";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(32, 17);
            label2.TabIndex = 3;
            label2.Text = "密码";
            // 
            // Password
            // 
            Password.Location = new Point(62, 62);
            Password.Name = "Password";
            Password.PasswordChar = '*';
            Password.Size = new Size(135, 23);
            Password.TabIndex = 2;
            // 
            // loginBtn
            // 
            loginBtn.Location = new Point(127, 110);
            loginBtn.Name = "loginBtn";
            loginBtn.Size = new Size(70, 28);
            loginBtn.TabIndex = 4;
            loginBtn.Text = "登录";
            loginBtn.UseVisualStyleBackColor = true;
            loginBtn.Click += loginBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(47, 110);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(70, 28);
            cancelBtn.TabIndex = 5;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // Login
            // 
            AcceptButton = loginBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(209, 150);
            Controls.Add(cancelBtn);
            Controls.Add(loginBtn);
            Controls.Add(label2);
            Controls.Add(Password);
            Controls.Add(label1);
            Controls.Add(User);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "登录";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox User;
        private Label label1;
        private Label label2;
        private TextBox Password;
        private Button loginBtn;
        private Button cancelBtn;
    }
}
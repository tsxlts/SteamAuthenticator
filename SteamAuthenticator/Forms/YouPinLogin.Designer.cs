namespace Steam_Authenticator.Forms
{
    partial class YouPinLogin
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
            label1 = new Label();
            codeBox = new TextBox();
            label2 = new Label();
            msg = new Label();
            sendSmsCodeBtn = new LinkLabel();
            okBtn = new Button();
            phoneBox = new TextBox();
            panel1 = new Panel();
            label4 = new Label();
            label3 = new Label();
            appLogin = new RadioButton();
            pcLogin = new RadioButton();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 111);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 0;
            label1.Text = "手机号";
            // 
            // codeBox
            // 
            codeBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            codeBox.BorderStyle = BorderStyle.None;
            codeBox.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            codeBox.Location = new Point(62, 142);
            codeBox.Name = "codeBox";
            codeBox.Size = new Size(107, 25);
            codeBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 145);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "验证码";
            // 
            // msg
            // 
            msg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            msg.ForeColor = Color.Red;
            msg.Location = new Point(12, 6);
            msg.Name = "msg";
            msg.Size = new Size(231, 38);
            msg.TabIndex = 10;
            msg.Text = "请登录你的 悠悠有品 帐号";
            msg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sendSmsCodeBtn
            // 
            sendSmsCodeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            sendSmsCodeBtn.AutoSize = true;
            sendSmsCodeBtn.Location = new Point(175, 145);
            sendSmsCodeBtn.Name = "sendSmsCodeBtn";
            sendSmsCodeBtn.Size = new Size(68, 17);
            sendSmsCodeBtn.TabIndex = 11;
            sendSmsCodeBtn.TabStop = true;
            sendSmsCodeBtn.Text = "发送验证码";
            sendSmsCodeBtn.LinkClicked += sendSmsCodeBtn_LinkClicked;
            // 
            // okBtn
            // 
            okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            okBtn.BackColor = Color.Transparent;
            okBtn.Location = new Point(12, 185);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(231, 43);
            okBtn.TabIndex = 12;
            okBtn.Text = "登录/注册";
            okBtn.UseVisualStyleBackColor = false;
            okBtn.Click += okBtn_Click;
            // 
            // phoneBox
            // 
            phoneBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            phoneBox.BorderStyle = BorderStyle.None;
            phoneBox.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            phoneBox.Location = new Point(62, 108);
            phoneBox.Name = "phoneBox";
            phoneBox.Size = new Size(181, 25);
            phoneBox.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(pcLogin);
            panel1.Controls.Add(appLogin);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(12, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(231, 52);
            panel1.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(3, 31);
            label4.Name = "label4";
            label4.Size = new Size(195, 17);
            label4.TabIndex = 3;
            label4.Text = "若无法登录, 请切换登录方式后再试";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 4);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 1;
            label3.Text = "登录方式";
            // 
            // appLogin
            // 
            appLogin.AutoSize = true;
            appLogin.Location = new Point(65, 2);
            appLogin.Name = "appLogin";
            appLogin.Size = new Size(50, 21);
            appLogin.TabIndex = 4;
            appLogin.TabStop = true;
            appLogin.Text = "App";
            appLogin.UseVisualStyleBackColor = true;
            // 
            // pcLogin
            // 
            pcLogin.AutoSize = true;
            pcLogin.Location = new Point(121, 2);
            pcLogin.Name = "pcLogin";
            pcLogin.Size = new Size(41, 21);
            pcLogin.TabIndex = 5;
            pcLogin.TabStop = true;
            pcLogin.Text = "PC";
            pcLogin.UseVisualStyleBackColor = true;
            // 
            // YouPinLogin
            // 
            AcceptButton = okBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(255, 240);
            Controls.Add(panel1);
            Controls.Add(phoneBox);
            Controls.Add(okBtn);
            Controls.Add(sendSmsCodeBtn);
            Controls.Add(msg);
            Controls.Add(codeBox);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "YouPinLogin";
            StartPosition = FormStartPosition.CenterParent;
            Text = "悠悠有品 登录";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox codeBox;
        private Label label2;
        private Label msg;
        private LinkLabel sendSmsCodeBtn;
        private Button okBtn;
        private TextBox phoneBox;
        private Panel panel1;
        private Label label3;
        private Label label4;
        private RadioButton pcLogin;
        private RadioButton appLogin;
    }
}
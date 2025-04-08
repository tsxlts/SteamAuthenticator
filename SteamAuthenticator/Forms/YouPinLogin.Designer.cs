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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 56);
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
            codeBox.Location = new Point(62, 95);
            codeBox.Name = "codeBox";
            codeBox.Size = new Size(107, 25);
            codeBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 98);
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
            sendSmsCodeBtn.Location = new Point(175, 98);
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
            okBtn.Location = new Point(12, 149);
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
            phoneBox.Location = new Point(62, 53);
            phoneBox.Name = "phoneBox";
            phoneBox.Size = new Size(181, 25);
            phoneBox.TabIndex = 1;
            // 
            // YouPinLogin
            // 
            AcceptButton = okBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(255, 204);
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
    }
}
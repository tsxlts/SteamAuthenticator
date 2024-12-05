namespace Steam_Authenticator.Forms
{
    partial class ConfirmationsPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmationsPopup));
            acceptBtn = new Button();
            declineBtn = new Button();
            userLabel = new Label();
            panel1 = new Panel();
            tipsLabel = new Label();
            detailBtn = new Button();
            panel2 = new Panel();
            msgLabel = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // acceptBtn
            // 
            acceptBtn.BackColor = Color.FromArgb(192, 255, 192);
            acceptBtn.Font = new Font("Microsoft YaHei UI", 12F);
            acceptBtn.Location = new Point(159, 182);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(67, 33);
            acceptBtn.TabIndex = 0;
            acceptBtn.Tag = "accept";
            acceptBtn.Text = "确认";
            acceptBtn.UseVisualStyleBackColor = false;
            acceptBtn.Click += accept_decline_Click;
            // 
            // declineBtn
            // 
            declineBtn.BackColor = Color.FromArgb(255, 192, 192);
            declineBtn.Font = new Font("Microsoft YaHei UI", 12F);
            declineBtn.Location = new Point(86, 182);
            declineBtn.Name = "declineBtn";
            declineBtn.Size = new Size(67, 33);
            declineBtn.TabIndex = 1;
            declineBtn.Tag = "decline";
            declineBtn.Text = "拒绝";
            declineBtn.UseVisualStyleBackColor = false;
            declineBtn.Click += accept_decline_Click;
            // 
            // userLabel
            // 
            userLabel.Location = new Point(12, 9);
            userLabel.Name = "userLabel";
            userLabel.Size = new Size(214, 23);
            userLabel.TabIndex = 2;
            userLabel.Text = "steam user";
            userLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(tipsLabel);
            panel1.Location = new Point(13, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(213, 111);
            panel1.TabIndex = 3;
            // 
            // tipsLabel
            // 
            tipsLabel.Dock = DockStyle.Fill;
            tipsLabel.Location = new Point(0, 0);
            tipsLabel.Name = "tipsLabel";
            tipsLabel.Size = new Size(213, 111);
            tipsLabel.TabIndex = 2;
            tipsLabel.Text = "你有0个报价待确认";
            tipsLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // detailBtn
            // 
            detailBtn.BackColor = Color.White;
            detailBtn.Font = new Font("Microsoft YaHei UI", 12F);
            detailBtn.Location = new Point(13, 182);
            detailBtn.Name = "detailBtn";
            detailBtn.Size = new Size(67, 33);
            detailBtn.TabIndex = 4;
            detailBtn.Text = "查看";
            detailBtn.UseVisualStyleBackColor = false;
            detailBtn.Click += detailBtn_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(msgLabel);
            panel2.Location = new Point(13, 152);
            panel2.Name = "panel2";
            panel2.Size = new Size(213, 24);
            panel2.TabIndex = 5;
            // 
            // msgLabel
            // 
            msgLabel.Dock = DockStyle.Fill;
            msgLabel.ForeColor = Color.Green;
            msgLabel.Location = new Point(0, 0);
            msgLabel.Name = "msgLabel";
            msgLabel.Size = new Size(213, 24);
            msgLabel.TabIndex = 0;
            msgLabel.Text = "等待确认";
            msgLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConfirmationsPopup
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(238, 224);
            Controls.Add(panel2);
            Controls.Add(detailBtn);
            Controls.Add(panel1);
            Controls.Add(userLabel);
            Controls.Add(declineBtn);
            Controls.Add(acceptBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfirmationsPopup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "令牌确认";
            Load += ConfirmationsPopup_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button acceptBtn;
        private Button declineBtn;
        private Label userLabel;
        private Panel panel1;
        private Label tipsLabel;
        private Button detailBtn;
        private Panel panel2;
        private Label msgLabel;
    }
}
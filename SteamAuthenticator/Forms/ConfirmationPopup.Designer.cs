namespace Steam_Authenticator.Forms
{
    partial class ConfirmationPopup
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
            acceptBtn = new Button();
            declineBtn = new Button();
            label1 = new Label();
            panel1 = new Panel();
            TipsLabel = new Label();
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
            // label1
            // 
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(214, 23);
            label1.TabIndex = 2;
            label1.Text = "steam user";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(TipsLabel);
            panel1.Location = new Point(13, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(213, 111);
            panel1.TabIndex = 3;
            // 
            // TipsLabel
            // 
            TipsLabel.Dock = DockStyle.Fill;
            TipsLabel.Location = new Point(0, 0);
            TipsLabel.Name = "TipsLabel";
            TipsLabel.Size = new Size(213, 111);
            TipsLabel.TabIndex = 2;
            TipsLabel.Text = "你有0个报价待确认";
            TipsLabel.TextAlign = ContentAlignment.MiddleCenter;
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
            // ConfirmationPopup
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(238, 224);
            Controls.Add(panel2);
            Controls.Add(detailBtn);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(declineBtn);
            Controls.Add(acceptBtn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfirmationPopup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "令牌确认";
            Load += ConfirmationPopup_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button acceptBtn;
        private Button declineBtn;
        private Label label1;
        private Panel panel1;
        private Label TipsLabel;
        private Button detailBtn;
        private Panel panel2;
        private Label msgLabel;
    }
}
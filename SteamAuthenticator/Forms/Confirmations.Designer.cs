namespace Steam_Authenticator.Forms
{
    partial class Confirmations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Confirmations));
            panel1 = new Panel();
            declineAllBtn = new Label();
            acceptAllBtn = new Label();
            refreshBtn = new Label();
            confirmationsPanel = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.FromArgb(255, 248, 220);
            panel1.Controls.Add(declineAllBtn);
            panel1.Controls.Add(acceptAllBtn);
            panel1.Controls.Add(refreshBtn);
            panel1.Location = new Point(0, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(353, 40);
            panel1.TabIndex = 0;
            // 
            // declineAllBtn
            // 
            declineAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            declineAllBtn.AutoSize = true;
            declineAllBtn.Cursor = Cursors.Hand;
            declineAllBtn.ForeColor = Color.Red;
            declineAllBtn.Location = new Point(291, 12);
            declineAllBtn.Name = "declineAllBtn";
            declineAllBtn.Size = new Size(56, 17);
            declineAllBtn.TabIndex = 10;
            declineAllBtn.Text = "全部取消";
            declineAllBtn.Click += declineAllBtn_Click;
            // 
            // acceptAllBtn
            // 
            acceptAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            acceptAllBtn.AutoSize = true;
            acceptAllBtn.Cursor = Cursors.Hand;
            acceptAllBtn.ForeColor = Color.Green;
            acceptAllBtn.Location = new Point(229, 12);
            acceptAllBtn.Name = "acceptAllBtn";
            acceptAllBtn.Size = new Size(56, 17);
            acceptAllBtn.TabIndex = 9;
            acceptAllBtn.Text = "全部确认";
            acceptAllBtn.Click += acceptAllBtn_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            refreshBtn.AutoSize = true;
            refreshBtn.ForeColor = Color.Green;
            refreshBtn.Location = new Point(167, 12);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(56, 17);
            refreshBtn.TabIndex = 0;
            refreshBtn.Text = "刷新确认";
            refreshBtn.Click += refreshBtn_Click;
            // 
            // confirmationsPanel
            // 
            confirmationsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            confirmationsPanel.BackColor = Color.White;
            confirmationsPanel.Location = new Point(1, 47);
            confirmationsPanel.Name = "confirmationsPanel";
            confirmationsPanel.Size = new Size(352, 220);
            confirmationsPanel.TabIndex = 1;
            // 
            // Confirmations
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(354, 267);
            Controls.Add(confirmationsPanel);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Confirmations";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "令牌确认";
            FormClosed += Confirmations_FormClosed;
            Load += Confirmations_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Label refreshBtn;
        private Panel confirmationsPanel;
        private Label declineAllBtn;
        private Label acceptAllBtn;
    }
}
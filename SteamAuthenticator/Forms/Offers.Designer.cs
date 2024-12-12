namespace Steam_Authenticator.Forms
{
    partial class Offers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Offers));
            panel1 = new Panel();
            refreshBtn = new Label();
            declineAllBtn = new Label();
            acceptAllBtn = new Label();
            offersPanel = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.FromArgb(255, 248, 220);
            panel1.Controls.Add(refreshBtn);
            panel1.Controls.Add(declineAllBtn);
            panel1.Controls.Add(acceptAllBtn);
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(365, 40);
            panel1.TabIndex = 0;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            refreshBtn.AutoSize = true;
            refreshBtn.Cursor = Cursors.Hand;
            refreshBtn.ForeColor = Color.Green;
            refreshBtn.Location = new Point(179, 13);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(56, 17);
            refreshBtn.TabIndex = 9;
            refreshBtn.Text = "刷新报价";
            refreshBtn.Click += refreshBtn_Click;
            // 
            // declineAllBtn
            // 
            declineAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            declineAllBtn.AutoSize = true;
            declineAllBtn.Cursor = Cursors.Hand;
            declineAllBtn.ForeColor = Color.Red;
            declineAllBtn.Location = new Point(303, 13);
            declineAllBtn.Name = "declineAllBtn";
            declineAllBtn.Size = new Size(56, 17);
            declineAllBtn.TabIndex = 8;
            declineAllBtn.Text = "全部拒绝";
            declineAllBtn.Click += declineAllBtn_Click;
            // 
            // acceptAllBtn
            // 
            acceptAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            acceptAllBtn.AutoSize = true;
            acceptAllBtn.Cursor = Cursors.Hand;
            acceptAllBtn.ForeColor = Color.Green;
            acceptAllBtn.Location = new Point(241, 13);
            acceptAllBtn.Name = "acceptAllBtn";
            acceptAllBtn.Size = new Size(56, 17);
            acceptAllBtn.TabIndex = 7;
            acceptAllBtn.Text = "全部接受";
            acceptAllBtn.Click += acceptAllBtn_Click;
            // 
            // offersPanel
            // 
            offersPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            offersPanel.BackColor = Color.White;
            offersPanel.Location = new Point(1, 47);
            offersPanel.Name = "offersPanel";
            offersPanel.Size = new Size(365, 228);
            offersPanel.TabIndex = 3;
            // 
            // Offers
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 276);
            Controls.Add(offersPanel);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Offers";
            StartPosition = FormStartPosition.CenterParent;
            Text = "报价";
            FormClosed += Offers_FormClosed;
            Load += Offers_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Label declineAllBtn;
        private Label acceptAllBtn;
        private Label refreshBtn;
        private Panel offersPanel;
    }
}
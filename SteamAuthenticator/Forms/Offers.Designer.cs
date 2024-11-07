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
            OffersView = new SplitContainer();
            panel1 = new Panel();
            refreshBtn = new Label();
            declineAllBtn = new Label();
            acceptAllBtn = new Label();
            ((System.ComponentModel.ISupportInitialize)OffersView).BeginInit();
            OffersView.Panel1.SuspendLayout();
            OffersView.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // OffersView
            // 
            OffersView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OffersView.FixedPanel = FixedPanel.Panel1;
            OffersView.IsSplitterFixed = true;
            OffersView.Location = new Point(1, 1);
            OffersView.Name = "OffersView";
            OffersView.Orientation = Orientation.Horizontal;
            // 
            // OffersView.Panel1
            // 
            OffersView.Panel1.Controls.Add(panel1);
            // 
            // OffersView.Panel2
            // 
            OffersView.Panel2.BackColor = Color.White;
            OffersView.Size = new Size(365, 276);
            OffersView.SplitterDistance = 40;
            OffersView.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 248, 220);
            panel1.Controls.Add(refreshBtn);
            panel1.Controls.Add(declineAllBtn);
            panel1.Controls.Add(acceptAllBtn);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
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
            refreshBtn.Location = new Point(178, 13);
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
            declineAllBtn.Location = new Point(302, 13);
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
            acceptAllBtn.Location = new Point(240, 13);
            acceptAllBtn.Name = "acceptAllBtn";
            acceptAllBtn.Size = new Size(56, 17);
            acceptAllBtn.TabIndex = 7;
            acceptAllBtn.Text = "全部接受";
            acceptAllBtn.Click += acceptAllBtn_Click;
            // 
            // Offers
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 276);
            Controls.Add(OffersView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Offers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "报价";
            FormClosed += Offers_FormClosed;
            Load += Offers_Load;
            OffersView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)OffersView).EndInit();
            OffersView.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer OffersView;
        private Panel panel1;
        private Label declineAllBtn;
        private Label acceptAllBtn;
        private Label refreshBtn;
    }
}
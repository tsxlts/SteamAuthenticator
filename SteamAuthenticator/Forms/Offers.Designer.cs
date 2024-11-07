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
            acceptAllBtn = new Button();
            declineAllBtn = new Button();
            refreshBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)OffersView).BeginInit();
            OffersView.Panel1.SuspendLayout();
            OffersView.SuspendLayout();
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
            OffersView.Panel1.Controls.Add(acceptAllBtn);
            OffersView.Panel1.Controls.Add(declineAllBtn);
            OffersView.Panel1.Controls.Add(refreshBtn);
            OffersView.Size = new Size(402, 568);
            OffersView.SplitterDistance = 45;
            OffersView.TabIndex = 2;
            // 
            // acceptAllBtn
            // 
            acceptAllBtn.Anchor = AnchorStyles.Top;
            acceptAllBtn.Location = new Point(136, 3);
            acceptAllBtn.Name = "acceptAllBtn";
            acceptAllBtn.Size = new Size(130, 39);
            acceptAllBtn.TabIndex = 1;
            acceptAllBtn.Text = "全部接受";
            acceptAllBtn.UseVisualStyleBackColor = true;
            acceptAllBtn.Click += acceptAllBtn_Click;
            // 
            // declineAllBtn
            // 
            declineAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            declineAllBtn.Location = new Point(271, 3);
            declineAllBtn.Name = "declineAllBtn";
            declineAllBtn.Size = new Size(130, 39);
            declineAllBtn.TabIndex = 2;
            declineAllBtn.Text = "全部拒绝";
            declineAllBtn.UseVisualStyleBackColor = true;
            declineAllBtn.Click += declineAllBtn_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Location = new Point(2, 3);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(130, 39);
            refreshBtn.TabIndex = 0;
            refreshBtn.Text = "刷新";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // Offers
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 568);
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
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer OffersView;
        private Button acceptAllBtn;
        private Button declineAllBtn;
        private Button refreshBtn;
    }
}
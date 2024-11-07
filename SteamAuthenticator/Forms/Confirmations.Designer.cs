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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Confirmations));
            ConfirmationsView = new SplitContainer();
            panel1 = new Panel();
            refreshBtn = new Label();
            autoRefreshTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)ConfirmationsView).BeginInit();
            ConfirmationsView.Panel1.SuspendLayout();
            ConfirmationsView.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // ConfirmationsView
            // 
            ConfirmationsView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConfirmationsView.FixedPanel = FixedPanel.Panel1;
            ConfirmationsView.IsSplitterFixed = true;
            ConfirmationsView.Location = new Point(1, 1);
            ConfirmationsView.Name = "ConfirmationsView";
            ConfirmationsView.Orientation = Orientation.Horizontal;
            // 
            // ConfirmationsView.Panel1
            // 
            ConfirmationsView.Panel1.Controls.Add(panel1);
            // 
            // ConfirmationsView.Panel2
            // 
            ConfirmationsView.Panel2.BackColor = Color.White;
            ConfirmationsView.Size = new Size(419, 278);
            ConfirmationsView.SplitterDistance = 40;
            ConfirmationsView.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 248, 220);
            panel1.Controls.Add(refreshBtn);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(419, 40);
            panel1.TabIndex = 0;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            refreshBtn.AutoSize = true;
            refreshBtn.ForeColor = Color.Green;
            refreshBtn.Location = new Point(356, 12);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(56, 17);
            refreshBtn.TabIndex = 0;
            refreshBtn.Text = "刷新确认";
            refreshBtn.Click += refreshBtn_Click;
            // 
            // autoRefreshTimer
            // 
            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Tick += autoRefreshTimer_Tick;
            // 
            // Confirmations
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 280);
            Controls.Add(ConfirmationsView);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Confirmations";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "令牌确认";
            FormClosed += Confirmations_FormClosed;
            Load += Confirmations_Load;
            ConfirmationsView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ConfirmationsView).EndInit();
            ConfirmationsView.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer ConfirmationsView;
        private System.Windows.Forms.Timer autoRefreshTimer;
        private Panel panel1;
        private Label refreshBtn;
    }
}
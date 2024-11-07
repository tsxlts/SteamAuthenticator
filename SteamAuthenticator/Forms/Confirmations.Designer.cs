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
            refreshBtn = new Button();
            autoRefreshTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)ConfirmationsView).BeginInit();
            ConfirmationsView.Panel1.SuspendLayout();
            ConfirmationsView.SuspendLayout();
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
            ConfirmationsView.Panel1.Controls.Add(refreshBtn);
            ConfirmationsView.Size = new Size(394, 566);
            ConfirmationsView.SplitterDistance = 32;
            ConfirmationsView.TabIndex = 1;
            // 
            // refreshBtn
            // 
            refreshBtn.Dock = DockStyle.Fill;
            refreshBtn.Location = new Point(0, 0);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(394, 32);
            refreshBtn.TabIndex = 0;
            refreshBtn.Text = "刷新";
            refreshBtn.UseVisualStyleBackColor = true;
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
            ClientSize = new Size(397, 568);
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
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer ConfirmationsView;
        private Button refreshBtn;
        private System.Windows.Forms.Timer autoRefreshTimer;
    }
}
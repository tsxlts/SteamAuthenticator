namespace Steam_Authenticator.Forms
{
    partial class ChooseAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseAccount));
            UsersPanel = new Panel();
            SuspendLayout();
            // 
            // UsersPanel
            // 
            UsersPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UsersPanel.Location = new Point(10, 20);
            UsersPanel.Name = "UsersPanel";
            UsersPanel.Size = new Size(480, 240);
            UsersPanel.TabIndex = 0;
            // 
            // ChooseAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 281);
            Controls.Add(UsersPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(520, 320);
            MinimizeBox = false;
            MinimumSize = new Size(520, 320);
            Name = "ChooseAccount";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "选择帐号";
            Load += ChooseAccount_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel UsersPanel;
    }
}
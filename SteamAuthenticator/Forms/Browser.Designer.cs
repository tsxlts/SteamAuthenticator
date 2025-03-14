﻿namespace Steam_Authenticator.Forms
{
    partial class Browser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            WebPanel = new Panel();
            SuspendLayout();
            // 
            // WebPanel
            // 
            WebPanel.BorderStyle = BorderStyle.FixedSingle;
            WebPanel.Dock = DockStyle.Fill;
            WebPanel.Location = new Point(0, 0);
            WebPanel.Name = "WebPanel";
            WebPanel.Size = new Size(584, 361);
            WebPanel.TabIndex = 0;
            // 
            // Browser
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(WebPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(500, 400);
            Name = "Browser";
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += Browser_FormClosing;
            Load += Browser_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel WebPanel;
    }
}
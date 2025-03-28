﻿namespace Steam_Authenticator.Forms
{
    partial class QrAuth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrAuth));
            qrBox = new PictureBox();
            msg = new Label();
            ((System.ComponentModel.ISupportInitialize)qrBox).BeginInit();
            SuspendLayout();
            // 
            // qrBox
            // 
            qrBox.Image = Properties.Resources.loading;
            qrBox.InitialImage = Properties.Resources.loading;
            qrBox.Location = new Point(12, 9);
            qrBox.Name = "qrBox";
            qrBox.Size = new Size(200, 200);
            qrBox.SizeMode = PictureBoxSizeMode.Zoom;
            qrBox.TabIndex = 0;
            qrBox.TabStop = false;
            // 
            // msg
            // 
            msg.ForeColor = Color.Red;
            msg.Location = new Point(12, 212);
            msg.Name = "msg";
            msg.Size = new Size(199, 38);
            msg.TabIndex = 8;
            msg.Text = "请使用 Steam App 扫码\r\n登录 Steam 帐号";
            msg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // QrAuth
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(223, 256);
            Controls.Add(msg);
            Controls.Add(qrBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QrAuth";
            StartPosition = FormStartPosition.CenterParent;
            Text = "扫码登录";
            FormClosing += QrAuth_FormClosing;
            Load += QrAuth_Load;
            ((System.ComponentModel.ISupportInitialize)qrBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox qrBox;
        private Label msg;
    }
}
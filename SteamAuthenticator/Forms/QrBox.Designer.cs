namespace Steam_Authenticator.Forms
{
    partial class QrBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrBox));
            msg = new Label();
            qrcodeBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)qrcodeBox).BeginInit();
            SuspendLayout();
            // 
            // msg
            // 
            msg.ForeColor = Color.Red;
            msg.Location = new Point(12, 295);
            msg.Name = "msg";
            msg.Size = new Size(275, 38);
            msg.TabIndex = 11;
            msg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // qrcodeBox
            // 
            qrcodeBox.Image = Properties.Resources.loading;
            qrcodeBox.InitialImage = Properties.Resources.loading;
            qrcodeBox.Location = new Point(12, 12);
            qrcodeBox.Name = "qrcodeBox";
            qrcodeBox.Size = new Size(275, 275);
            qrcodeBox.SizeMode = PictureBoxSizeMode.Zoom;
            qrcodeBox.TabIndex = 10;
            qrcodeBox.TabStop = false;
            // 
            // QrBox
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 343);
            Controls.Add(msg);
            Controls.Add(qrcodeBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QrBox";
            StartPosition = FormStartPosition.CenterParent;
            Load += QrBox_Load;
            ((System.ComponentModel.ISupportInitialize)qrcodeBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label msg;
        private PictureBox qrcodeBox;
    }
}
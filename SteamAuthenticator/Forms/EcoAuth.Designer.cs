namespace Steam_Authenticator.Forms
{
    partial class EcoAuth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcoAuth));
            msg = new Label();
            qrBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)qrBox).BeginInit();
            SuspendLayout();
            // 
            // msg
            // 
            msg.ForeColor = Color.Red;
            msg.Location = new Point(11, 194);
            msg.Name = "msg";
            msg.Size = new Size(180, 38);
            msg.TabIndex = 9;
            msg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // qrBox
            // 
            qrBox.Image = Properties.Resources.loading;
            qrBox.InitialImage = Properties.Resources.loading;
            qrBox.Location = new Point(12, 11);
            qrBox.Name = "qrBox";
            qrBox.Size = new Size(180, 180);
            qrBox.SizeMode = PictureBoxSizeMode.Zoom;
            qrBox.TabIndex = 8;
            qrBox.TabStop = false;
            // 
            // EcoAuth
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(203, 242);
            Controls.Add(msg);
            Controls.Add(qrBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EcoAuth";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ECO 登录";
            FormClosing += EcoAuth_FormClosing;
            Load += EcoAuth_Load;
            ((System.ComponentModel.ISupportInitialize)qrBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label msg;
        private PictureBox qrBox;
    }
}
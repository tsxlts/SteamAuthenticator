namespace Install
{
    partial class Install
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Install));
            label1 = new Label();
            label2 = new Label();
            progressBar = new ProgressBar();
            msgBox = new Label();
            installPathBox = new Label();
            panel1 = new Panel();
            installBtn = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(12, 51);
            label1.Name = "label1";
            label1.Size = new Size(69, 23);
            label1.TabIndex = 0;
            label1.Text = "安装路径：";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(393, 23);
            label2.TabIndex = 2;
            label2.Text = "正在安装Steam验证器";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 109);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(393, 23);
            progressBar.TabIndex = 3;
            // 
            // msgBox
            // 
            msgBox.AutoEllipsis = true;
            msgBox.ForeColor = Color.Gray;
            msgBox.Location = new Point(12, 83);
            msgBox.Name = "msgBox";
            msgBox.Size = new Size(393, 23);
            msgBox.TabIndex = 4;
            msgBox.Text = "准备就绪...";
            msgBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // installPathBox
            // 
            installPathBox.AutoEllipsis = true;
            installPathBox.Location = new Point(87, 51);
            installPathBox.Name = "installPathBox";
            installPathBox.Size = new Size(318, 23);
            installPathBox.TabIndex = 1;
            installPathBox.Text = "C:\\";
            installPathBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(installBtn);
            panel1.Location = new Point(12, 143);
            panel1.Name = "panel1";
            panel1.Size = new Size(393, 31);
            panel1.TabIndex = 5;
            // 
            // installBtn
            // 
            installBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            installBtn.Location = new Point(0, 0);
            installBtn.Name = "installBtn";
            installBtn.Size = new Size(393, 31);
            installBtn.TabIndex = 0;
            installBtn.Text = "开始安装";
            installBtn.UseVisualStyleBackColor = true;
            installBtn.Click += installBtn_Click;
            // 
            // Install
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(417, 183);
            Controls.Add(panel1);
            Controls.Add(msgBox);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(installPathBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Install";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SteamAuthenticator";
            Load += Install_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private ProgressBar progressBar;
        private Label msgBox;
        private Label installPathBox;
        private Panel panel1;
        private Button installBtn;
    }
}

namespace Steam_Authenticator.Forms
{
    partial class Welcome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Welcome));
            label1 = new Label();
            label2 = new Label();
            splitContainer1 = new SplitContainer();
            declineBtn = new Button();
            acceptBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(434, 34);
            label1.TabIndex = 0;
            label1.Text = "欢迎使用Steam身份验证器\r\n";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(12, 63);
            label2.Name = "label2";
            label2.Size = new Size(434, 263);
            label2.TabIndex = 1;
            label2.Text = resources.GetString("label2.Text");
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(12, 329);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(declineBtn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(acceptBtn);
            splitContainer1.Size = new Size(434, 40);
            splitContainer1.SplitterDistance = 217;
            splitContainer1.TabIndex = 2;
            // 
            // declineBtn
            // 
            declineBtn.Dock = DockStyle.Fill;
            declineBtn.Location = new Point(0, 0);
            declineBtn.Name = "declineBtn";
            declineBtn.Size = new Size(217, 40);
            declineBtn.TabIndex = 0;
            declineBtn.Text = "拒绝并退出";
            declineBtn.UseVisualStyleBackColor = true;
            declineBtn.Click += declineBtn_Click;
            // 
            // acceptBtn
            // 
            acceptBtn.Dock = DockStyle.Fill;
            acceptBtn.Location = new Point(0, 0);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(213, 40);
            acceptBtn.TabIndex = 0;
            acceptBtn.Text = "同意并继续";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // Welcome
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 379);
            Controls.Add(splitContainer1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Welcome";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "欢迎使用Steam验证器";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private SplitContainer splitContainer1;
        private Button declineBtn;
        private Button acceptBtn;
    }
}
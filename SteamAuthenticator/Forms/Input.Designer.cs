namespace Steam_Authenticator.Forms
{
    partial class Input
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Input));
            InputBox = new TextBox();
            TipsLabel = new Label();
            acceptBtn = new Button();
            cancelBtn = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // InputBox
            // 
            InputBox.Location = new Point(12, 113);
            InputBox.Name = "InputBox";
            InputBox.Size = new Size(167, 23);
            InputBox.TabIndex = 0;
            // 
            // TipsLabel
            // 
            TipsLabel.Dock = DockStyle.Fill;
            TipsLabel.Location = new Point(0, 0);
            TipsLabel.Name = "TipsLabel";
            TipsLabel.Size = new Size(167, 86);
            TipsLabel.TabIndex = 1;
            TipsLabel.Text = "请输入";
            TipsLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // acceptBtn
            // 
            acceptBtn.Location = new Point(102, 152);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(77, 23);
            acceptBtn.TabIndex = 2;
            acceptBtn.Text = "确定";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(12, 152);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(77, 23);
            cancelBtn.TabIndex = 3;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(TipsLabel);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 86);
            panel1.TabIndex = 4;
            // 
            // Input
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(191, 185);
            Controls.Add(panel1);
            Controls.Add(cancelBtn);
            Controls.Add(acceptBtn);
            Controls.Add(InputBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Input";
            StartPosition = FormStartPosition.CenterParent;
            Text = "请输入...";
            Load += Input_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputBox;
        private Label TipsLabel;
        private Button acceptBtn;
        private Button cancelBtn;
        private Panel panel1;
    }
}
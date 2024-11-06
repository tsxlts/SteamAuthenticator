namespace Steam_Authenticator.Forms
{
    partial class RichTextInput
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
            panel1 = new Panel();
            TipsLabel = new Label();
            cancelBtn = new Button();
            acceptBtn = new Button();
            InputBox = new RichTextBox();
            splitContainer1 = new SplitContainer();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(TipsLabel);
            panel1.Location = new Point(12, 11);
            panel1.Name = "panel1";
            panel1.Size = new Size(338, 24);
            panel1.TabIndex = 8;
            // 
            // TipsLabel
            // 
            TipsLabel.AutoEllipsis = true;
            TipsLabel.Dock = DockStyle.Fill;
            TipsLabel.Location = new Point(0, 0);
            TipsLabel.Name = "TipsLabel";
            TipsLabel.Size = new Size(338, 24);
            TipsLabel.TabIndex = 1;
            TipsLabel.Text = "请输入...";
            TipsLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cancelBtn
            // 
            cancelBtn.Dock = DockStyle.Fill;
            cancelBtn.Location = new Point(0, 0);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(171, 34);
            cancelBtn.TabIndex = 7;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // acceptBtn
            // 
            acceptBtn.Dock = DockStyle.Fill;
            acceptBtn.Location = new Point(0, 0);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(167, 34);
            acceptBtn.TabIndex = 6;
            acceptBtn.Text = "确定";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // InputBox
            // 
            InputBox.BorderStyle = BorderStyle.FixedSingle;
            InputBox.Location = new Point(10, 41);
            InputBox.Name = "InputBox";
            InputBox.Size = new Size(342, 104);
            InputBox.TabIndex = 9;
            InputBox.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(10, 151);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(cancelBtn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(acceptBtn);
            splitContainer1.Size = new Size(342, 34);
            splitContainer1.SplitterDistance = 171;
            splitContainer1.TabIndex = 10;
            // 
            // RichTextInput
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(362, 197);
            Controls.Add(splitContainer1);
            Controls.Add(InputBox);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RichTextInput";
            StartPosition = FormStartPosition.CenterParent;
            Text = "请输入...";
            Load += RichTextInput_Load;
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label TipsLabel;
        private Button cancelBtn;
        private Button acceptBtn;
        private RichTextBox InputBox;
        private SplitContainer splitContainer1;
    }
}
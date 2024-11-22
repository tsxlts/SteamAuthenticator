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
            panel2 = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
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
            InputBox.BorderStyle = BorderStyle.None;
            InputBox.Dock = DockStyle.Fill;
            InputBox.Location = new Point(0, 0);
            InputBox.Name = "InputBox";
            InputBox.Size = new Size(340, 102);
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
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(InputBox);
            panel2.Location = new Point(10, 41);
            panel2.Name = "panel2";
            panel2.Size = new Size(342, 104);
            panel2.TabIndex = 11;
            // 
            // RichTextInput
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(362, 197);
            Controls.Add(panel2);
            Controls.Add(splitContainer1);
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
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label TipsLabel;
        private Button cancelBtn;
        private Button acceptBtn;
        private RichTextBox InputBox;
        private SplitContainer splitContainer1;
        private Panel panel2;
    }
}
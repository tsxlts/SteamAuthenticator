namespace Steam_Authenticator.Forms
{
    partial class Search
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search));
            searchBox = new ComboBox();
            tips = new Label();
            panel1 = new Panel();
            okBtn = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // searchBox
            // 
            searchBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchBox.FormattingEnabled = true;
            searchBox.IntegralHeight = false;
            searchBox.Location = new Point(12, 58);
            searchBox.MaxDropDownItems = 10;
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(196, 25);
            searchBox.TabIndex = 0;
            // 
            // tips
            // 
            tips.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tips.AutoEllipsis = true;
            tips.Location = new Point(12, 9);
            tips.Name = "tips";
            tips.Size = new Size(196, 30);
            tips.TabIndex = 1;
            tips.Text = "请在下面输入框搜索";
            tips.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(okBtn);
            panel1.Location = new Point(12, 109);
            panel1.Name = "panel1";
            panel1.Size = new Size(196, 33);
            panel1.TabIndex = 2;
            // 
            // okBtn
            // 
            okBtn.Dock = DockStyle.Fill;
            okBtn.Location = new Point(0, 0);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(196, 33);
            okBtn.TabIndex = 0;
            okBtn.Text = "确定";
            okBtn.UseVisualStyleBackColor = true;
            okBtn.Click += okBtn_Click;
            // 
            // Search
            // 
            AcceptButton = okBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(220, 158);
            Controls.Add(panel1);
            Controls.Add(tips);
            Controls.Add(searchBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Search";
            StartPosition = FormStartPosition.CenterParent;
            Text = "搜索";
            Load += Search_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox searchBox;
        private Label tips;
        private Panel panel1;
        private Button okBtn;
    }
}
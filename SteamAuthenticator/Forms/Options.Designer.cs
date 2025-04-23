namespace Steam_Authenticator.Forms
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            msg = new Label();
            panel2 = new Panel();
            okBtn = new Button();
            form = new GroupBox();
            dataPanel = new Panel();
            panel1 = new Panel();
            reverseSelectBtn = new Label();
            selectAllBtn = new Label();
            panel2.SuspendLayout();
            form.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // msg
            // 
            msg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            msg.ForeColor = Color.Red;
            msg.Location = new Point(12, 9);
            msg.Name = "msg";
            msg.Size = new Size(371, 38);
            msg.TabIndex = 8;
            msg.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(okBtn);
            panel2.Location = new Point(12, 268);
            panel2.Name = "panel2";
            panel2.Size = new Size(371, 32);
            panel2.TabIndex = 10;
            // 
            // okBtn
            // 
            okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            okBtn.Location = new Point(0, 0);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(371, 32);
            okBtn.TabIndex = 0;
            okBtn.Text = "确认";
            okBtn.UseVisualStyleBackColor = true;
            okBtn.Click += okBtn_Click;
            // 
            // form
            // 
            form.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            form.Controls.Add(dataPanel);
            form.Controls.Add(panel1);
            form.Location = new Point(12, 50);
            form.Name = "form";
            form.Size = new Size(371, 212);
            form.TabIndex = 11;
            form.TabStop = false;
            form.Text = "请在下方选择";
            // 
            // dataPanel
            // 
            dataPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataPanel.AutoScroll = true;
            dataPanel.BackColor = Color.White;
            dataPanel.Location = new Point(6, 57);
            dataPanel.Name = "dataPanel";
            dataPanel.Size = new Size(359, 149);
            dataPanel.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(reverseSelectBtn);
            panel1.Controls.Add(selectAllBtn);
            panel1.Location = new Point(6, 22);
            panel1.Name = "panel1";
            panel1.Size = new Size(359, 29);
            panel1.TabIndex = 0;
            // 
            // reverseSelectBtn
            // 
            reverseSelectBtn.Cursor = Cursors.Hand;
            reverseSelectBtn.ForeColor = Color.FromArgb(255, 128, 0);
            reverseSelectBtn.Location = new Point(85, 3);
            reverseSelectBtn.Name = "reverseSelectBtn";
            reverseSelectBtn.Size = new Size(70, 23);
            reverseSelectBtn.TabIndex = 1;
            reverseSelectBtn.Text = "反选";
            reverseSelectBtn.TextAlign = ContentAlignment.MiddleCenter;
            reverseSelectBtn.Click += reverseSelectBtn_Click;
            // 
            // selectAllBtn
            // 
            selectAllBtn.Cursor = Cursors.Hand;
            selectAllBtn.ForeColor = Color.FromArgb(0, 128, 255);
            selectAllBtn.Location = new Point(3, 3);
            selectAllBtn.Name = "selectAllBtn";
            selectAllBtn.Size = new Size(70, 23);
            selectAllBtn.TabIndex = 0;
            selectAllBtn.Text = "全选";
            selectAllBtn.TextAlign = ContentAlignment.MiddleCenter;
            selectAllBtn.Click += selectAllBtn_Click;
            // 
            // Options
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(395, 304);
            Controls.Add(form);
            Controls.Add(panel2);
            Controls.Add(msg);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Options";
            StartPosition = FormStartPosition.CenterParent;
            Text = "选择";
            Load += Options_Load;
            panel2.ResumeLayout(false);
            form.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label msg;
        private Panel panel2;
        private Button okBtn;
        private GroupBox form;
        private Panel dataPanel;
        private Panel panel1;
        private Label selectAllBtn;
        private Label reverseSelectBtn;
    }
}
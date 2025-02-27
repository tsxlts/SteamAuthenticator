namespace Steam_Authenticator.Forms
{
    partial class SubmitRequirements
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitRequirements));
            label1 = new Label();
            subjectBox = new TextBox();
            bodyBox = new RichTextBox();
            contactInfoBox = new TextBox();
            label5 = new Label();
            submit = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            groupBox1 = new GroupBox();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(3, 5);
            label1.Name = "label1";
            label1.Size = new Size(67, 23);
            label1.TabIndex = 0;
            label1.Text = "你的需求";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // subjectBox
            // 
            subjectBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            subjectBox.Location = new Point(73, 5);
            subjectBox.Name = "subjectBox";
            subjectBox.Size = new Size(211, 23);
            subjectBox.TabIndex = 1;
            // 
            // bodyBox
            // 
            bodyBox.BorderStyle = BorderStyle.None;
            bodyBox.Dock = DockStyle.Fill;
            bodyBox.Location = new Point(3, 19);
            bodyBox.Name = "bodyBox";
            bodyBox.Size = new Size(475, 223);
            bodyBox.TabIndex = 3;
            bodyBox.Text = "";
            bodyBox.KeyDown += bodyBox_KeyDown;
            // 
            // contactInfoBox
            // 
            contactInfoBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            contactInfoBox.Location = new Point(363, 5);
            contactInfoBox.Name = "contactInfoBox";
            contactInfoBox.Size = new Size(115, 23);
            contactInfoBox.TabIndex = 2;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.Location = new Point(290, 5);
            label5.Name = "label5";
            label5.Size = new Size(67, 23);
            label5.TabIndex = 7;
            label5.Text = "联系方式";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // submit
            // 
            submit.Dock = DockStyle.Fill;
            submit.Location = new Point(0, 0);
            submit.Name = "submit";
            submit.Size = new Size(481, 36);
            submit.TabIndex = 99;
            submit.Text = "提交";
            submit.UseVisualStyleBackColor = true;
            submit.Click += submit_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(submit);
            panel2.Location = new Point(12, 301);
            panel2.Name = "panel2";
            panel2.Size = new Size(481, 36);
            panel2.TabIndex = 50;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(label1);
            panel3.Controls.Add(subjectBox);
            panel3.Controls.Add(contactInfoBox);
            panel3.Controls.Add(label5);
            panel3.Location = new Point(12, 12);
            panel3.Name = "panel3";
            panel3.Size = new Size(481, 32);
            panel3.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(bodyBox);
            groupBox1.Location = new Point(12, 50);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(481, 245);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "需求描述";
            // 
            // SubmitRequirements
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(505, 338);
            Controls.Add(groupBox1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(400, 300);
            Name = "SubmitRequirements";
            StartPosition = FormStartPosition.CenterParent;
            Text = "提交需求";
            Load += SubmitRequirements_Load;
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private TextBox subjectBox;
        private RichTextBox bodyBox;
        private TextBox contactInfoBox;
        private Label label5;
        private Button submit;
        private Panel panel2;
        private Panel panel3;
        private GroupBox groupBox1;
    }
}
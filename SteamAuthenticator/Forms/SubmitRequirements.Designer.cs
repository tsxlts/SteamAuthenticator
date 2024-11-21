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
            label2 = new Label();
            bodyBox = new RichTextBox();
            panel1 = new Panel();
            label3 = new Label();
            label4 = new Label();
            contactInfoBox = new TextBox();
            label5 = new Label();
            submit = new Button();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(67, 23);
            label1.TabIndex = 0;
            label1.Text = "你的需求";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // subjectBox
            // 
            subjectBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            subjectBox.Location = new Point(85, 17);
            subjectBox.Name = "subjectBox";
            subjectBox.Size = new Size(267, 23);
            subjectBox.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(12, 50);
            label2.Name = "label2";
            label2.Size = new Size(67, 23);
            label2.TabIndex = 2;
            label2.Text = "需求描述";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bodyBox
            // 
            bodyBox.BorderStyle = BorderStyle.None;
            bodyBox.Dock = DockStyle.Fill;
            bodyBox.Location = new Point(0, 0);
            bodyBox.Name = "bodyBox";
            bodyBox.Size = new Size(265, 119);
            bodyBox.TabIndex = 3;
            bodyBox.Text = "";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(bodyBox);
            panel1.Location = new Point(85, 50);
            panel1.Name = "panel1";
            panel1.Size = new Size(267, 121);
            panel1.TabIndex = 4;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(357, 17);
            label3.Name = "label3";
            label3.Size = new Size(19, 23);
            label3.TabIndex = 5;
            label3.Text = "*";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.ForeColor = Color.Red;
            label4.Location = new Point(358, 51);
            label4.Name = "label4";
            label4.Size = new Size(19, 23);
            label4.TabIndex = 6;
            label4.Text = "*";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contactInfoBox
            // 
            contactInfoBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            contactInfoBox.Location = new Point(86, 183);
            contactInfoBox.Name = "contactInfoBox";
            contactInfoBox.Size = new Size(267, 23);
            contactInfoBox.TabIndex = 8;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.Location = new Point(13, 183);
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
            submit.Size = new Size(382, 36);
            submit.TabIndex = 9;
            submit.Text = "提交";
            submit.UseVisualStyleBackColor = true;
            submit.Click += submit_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(submit);
            panel2.Location = new Point(1, 224);
            panel2.Name = "panel2";
            panel2.Size = new Size(382, 36);
            panel2.TabIndex = 10;
            // 
            // SubmitRequirements
            // 
            AcceptButton = submit;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 261);
            Controls.Add(panel2);
            Controls.Add(contactInfoBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(subjectBox);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(400, 300);
            Name = "SubmitRequirements";
            Text = "提交需求";
            Load += SubmitRequirements_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox subjectBox;
        private Label label2;
        private RichTextBox bodyBox;
        private Panel panel1;
        private Label label3;
        private Label label4;
        private TextBox contactInfoBox;
        private Label label5;
        private Button submit;
        private Panel panel2;
    }
}
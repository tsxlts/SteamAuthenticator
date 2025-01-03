namespace Steam_Authenticator.Forms
{
    partial class TimePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimePicker));
            startTime = new DateTimePicker();
            endTime = new DateTimePicker();
            label1 = new Label();
            addBtn = new Label();
            panel1 = new Panel();
            okBtn = new Button();
            timesBox = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // startTime
            // 
            startTime.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            startTime.Format = DateTimePickerFormat.Custom;
            startTime.Location = new Point(12, 12);
            startTime.Name = "startTime";
            startTime.ShowUpDown = true;
            startTime.Size = new Size(145, 23);
            startTime.TabIndex = 1;
            // 
            // endTime
            // 
            endTime.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            endTime.Format = DateTimePickerFormat.Custom;
            endTime.Location = new Point(189, 12);
            endTime.Name = "endTime";
            endTime.ShowUpDown = true;
            endTime.Size = new Size(145, 23);
            endTime.TabIndex = 2;
            // 
            // label1
            // 
            label1.Location = new Point(163, 12);
            label1.Name = "label1";
            label1.Size = new Size(20, 23);
            label1.TabIndex = 3;
            label1.Text = "至";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // addBtn
            // 
            addBtn.ForeColor = Color.Green;
            addBtn.Location = new Point(340, 12);
            addBtn.Name = "addBtn";
            addBtn.Size = new Size(64, 23);
            addBtn.TabIndex = 4;
            addBtn.Text = "添加";
            addBtn.TextAlign = ContentAlignment.MiddleCenter;
            addBtn.Click += addBtn_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(okBtn);
            panel1.Location = new Point(12, 228);
            panel1.Name = "panel1";
            panel1.Size = new Size(392, 32);
            panel1.TabIndex = 5;
            // 
            // okBtn
            // 
            okBtn.Dock = DockStyle.Fill;
            okBtn.Location = new Point(0, 0);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(392, 32);
            okBtn.TabIndex = 0;
            okBtn.Text = "确定";
            okBtn.UseVisualStyleBackColor = true;
            okBtn.Click += okBtn_Click;
            // 
            // timesBox
            // 
            timesBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            timesBox.AutoScroll = true;
            timesBox.BackColor = Color.White;
            timesBox.Location = new Point(12, 41);
            timesBox.Name = "timesBox";
            timesBox.Size = new Size(392, 181);
            timesBox.TabIndex = 6;
            // 
            // TimePicker
            // 
            AcceptButton = okBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 272);
            Controls.Add(timesBox);
            Controls.Add(panel1);
            Controls.Add(addBtn);
            Controls.Add(label1);
            Controls.Add(endTime);
            Controls.Add(startTime);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TimePicker";
            StartPosition = FormStartPosition.CenterParent;
            Text = "时间选择";
            Load += TimePicker_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DateTimePicker startTime;
        private DateTimePicker endTime;
        private Label label1;
        private Label addBtn;
        private Panel panel1;
        private Button okBtn;
        private Panel timesBox;
    }
}
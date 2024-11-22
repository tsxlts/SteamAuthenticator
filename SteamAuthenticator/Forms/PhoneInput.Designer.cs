namespace Steam_Authenticator.Forms
{
    partial class PhoneInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhoneInput));
            PhoneBox = new TextBox();
            TipsLabel = new Label();
            acceptBtn = new Button();
            cancelBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            CountryBox = new ComboBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // PhoneBox
            // 
            PhoneBox.Location = new Point(74, 137);
            PhoneBox.Name = "PhoneBox";
            PhoneBox.Size = new Size(105, 23);
            PhoneBox.TabIndex = 0;
            PhoneBox.Text = "+86 ";
            PhoneBox.KeyPress += PhoneBox_KeyPress;
            // 
            // TipsLabel
            // 
            TipsLabel.Dock = DockStyle.Fill;
            TipsLabel.Location = new Point(0, 0);
            TipsLabel.Name = "TipsLabel";
            TipsLabel.Size = new Size(167, 83);
            TipsLabel.TabIndex = 1;
            TipsLabel.Text = "请输入手机号";
            TipsLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // acceptBtn
            // 
            acceptBtn.Location = new Point(102, 166);
            acceptBtn.Name = "acceptBtn";
            acceptBtn.Size = new Size(77, 23);
            acceptBtn.TabIndex = 2;
            acceptBtn.Text = "确定";
            acceptBtn.UseVisualStyleBackColor = true;
            acceptBtn.Click += acceptBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(12, 166);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(77, 23);
            cancelBtn.TabIndex = 3;
            cancelBtn.Text = "取消";
            cancelBtn.UseVisualStyleBackColor = true;
            cancelBtn.Click += cancelBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 140);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 5;
            label1.Text = "手机号";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 111);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 6;
            label2.Text = "国家代码";
            // 
            // panel1
            // 
            panel1.Controls.Add(TipsLabel);
            panel1.Location = new Point(12, 7);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 83);
            panel1.TabIndex = 7;
            // 
            // CountryBox
            // 
            CountryBox.BackColor = SystemColors.Window;
            CountryBox.DropDownStyle = ComboBoxStyle.DropDownList;
            CountryBox.FormattingEnabled = true;
            CountryBox.IntegralHeight = false;
            CountryBox.Location = new Point(74, 108);
            CountryBox.MaxDropDownItems = 5;
            CountryBox.MaxLength = 5;
            CountryBox.Name = "CountryBox";
            CountryBox.Size = new Size(105, 25);
            CountryBox.TabIndex = 2;
            // 
            // PhoneInput
            // 
            AcceptButton = acceptBtn;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(191, 197);
            Controls.Add(CountryBox);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cancelBtn);
            Controls.Add(acceptBtn);
            Controls.Add(PhoneBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PhoneInput";
            StartPosition = FormStartPosition.CenterParent;
            Text = "请输入...";
            Load += Input_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox PhoneBox;
        private Label TipsLabel;
        private Button acceptBtn;
        private Button cancelBtn;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private ComboBox CountryBox;
    }
}
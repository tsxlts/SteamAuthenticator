namespace Steam_Authenticator.Forms
{
    partial class AcceptOfferRule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcceptOfferRule));
            groupBox1 = new GroupBox();
            offerMessageRegex = new RadioButton();
            offerMessageNotContains = new RadioButton();
            offerMessageNotEquals = new RadioButton();
            offerMessageContains = new RadioButton();
            offerMessageEquals = new RadioButton();
            label1 = new Label();
            offerMessage = new CheckBox();
            offerMessageBox = new TextBox();
            groupBox2 = new GroupBox();
            assetNameRegex = new RadioButton();
            assetNameNotContains = new RadioButton();
            assetNameNotEquals = new RadioButton();
            assetNameContains = new RadioButton();
            assetNameEquals = new RadioButton();
            label2 = new Label();
            assetName = new CheckBox();
            assetNameBox = new TextBox();
            panel1 = new Panel();
            saveBtn = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(offerMessageRegex);
            groupBox1.Controls.Add(offerMessageNotContains);
            groupBox1.Controls.Add(offerMessageNotEquals);
            groupBox1.Controls.Add(offerMessageContains);
            groupBox1.Controls.Add(offerMessageEquals);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(offerMessage);
            groupBox1.Controls.Add(offerMessageBox);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(552, 191);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "交易附言";
            // 
            // offerMessageRegex
            // 
            offerMessageRegex.AutoSize = true;
            offerMessageRegex.Location = new Point(7, 159);
            offerMessageRegex.Name = "offerMessageRegex";
            offerMessageRegex.Size = new Size(122, 21);
            offerMessageRegex.TabIndex = 7;
            offerMessageRegex.TabStop = true;
            offerMessageRegex.Text = "正则匹配任意一个";
            offerMessageRegex.UseVisualStyleBackColor = true;
            // 
            // offerMessageNotContains
            // 
            offerMessageNotContains.AutoSize = true;
            offerMessageNotContains.Location = new Point(7, 132);
            offerMessageNotContains.Name = "offerMessageNotContains";
            offerMessageNotContains.Size = new Size(110, 21);
            offerMessageNotContains.TabIndex = 6;
            offerMessageNotContains.TabStop = true;
            offerMessageNotContains.Text = "不包含任意一个";
            offerMessageNotContains.UseVisualStyleBackColor = true;
            // 
            // offerMessageNotEquals
            // 
            offerMessageNotEquals.AutoSize = true;
            offerMessageNotEquals.Location = new Point(7, 105);
            offerMessageNotEquals.Name = "offerMessageNotEquals";
            offerMessageNotEquals.Size = new Size(110, 21);
            offerMessageNotEquals.TabIndex = 5;
            offerMessageNotEquals.TabStop = true;
            offerMessageNotEquals.Text = "不等于任意一个";
            offerMessageNotEquals.UseVisualStyleBackColor = true;
            // 
            // offerMessageContains
            // 
            offerMessageContains.AutoSize = true;
            offerMessageContains.Location = new Point(7, 78);
            offerMessageContains.Name = "offerMessageContains";
            offerMessageContains.Size = new Size(98, 21);
            offerMessageContains.TabIndex = 4;
            offerMessageContains.TabStop = true;
            offerMessageContains.Text = "包含任意一个";
            offerMessageContains.UseVisualStyleBackColor = true;
            // 
            // offerMessageEquals
            // 
            offerMessageEquals.AutoSize = true;
            offerMessageEquals.Location = new Point(7, 51);
            offerMessageEquals.Name = "offerMessageEquals";
            offerMessageEquals.Size = new Size(98, 21);
            offerMessageEquals.TabIndex = 3;
            offerMessageEquals.TabStop = true;
            offerMessageEquals.Text = "等于任意一个";
            offerMessageEquals.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(135, 24);
            label1.Name = "label1";
            label1.Size = new Size(200, 17);
            label1.TabIndex = 2;
            label1.Text = "在输入框中输入匹配规则，每行一个";
            // 
            // offerMessage
            // 
            offerMessage.AutoSize = true;
            offerMessage.Location = new Point(7, 24);
            offerMessage.Name = "offerMessage";
            offerMessage.Size = new Size(75, 21);
            offerMessage.TabIndex = 1;
            offerMessage.Text = "交易附言";
            offerMessage.UseVisualStyleBackColor = true;
            // 
            // offerMessageBox
            // 
            offerMessageBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            offerMessageBox.Location = new Point(135, 45);
            offerMessageBox.Multiline = true;
            offerMessageBox.Name = "offerMessageBox";
            offerMessageBox.Size = new Size(411, 140);
            offerMessageBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(assetNameRegex);
            groupBox2.Controls.Add(assetNameNotContains);
            groupBox2.Controls.Add(assetNameNotEquals);
            groupBox2.Controls.Add(assetNameContains);
            groupBox2.Controls.Add(assetNameEquals);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(assetName);
            groupBox2.Controls.Add(assetNameBox);
            groupBox2.Location = new Point(12, 209);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(552, 191);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "物品名称";
            // 
            // assetNameRegex
            // 
            assetNameRegex.AutoSize = true;
            assetNameRegex.Location = new Point(7, 159);
            assetNameRegex.Name = "assetNameRegex";
            assetNameRegex.Size = new Size(122, 21);
            assetNameRegex.TabIndex = 7;
            assetNameRegex.TabStop = true;
            assetNameRegex.Text = "正则匹配任意一个";
            assetNameRegex.UseVisualStyleBackColor = true;
            // 
            // assetNameNotContains
            // 
            assetNameNotContains.AutoSize = true;
            assetNameNotContains.Location = new Point(7, 132);
            assetNameNotContains.Name = "assetNameNotContains";
            assetNameNotContains.Size = new Size(110, 21);
            assetNameNotContains.TabIndex = 6;
            assetNameNotContains.TabStop = true;
            assetNameNotContains.Text = "不包含任意一个";
            assetNameNotContains.UseVisualStyleBackColor = true;
            // 
            // assetNameNotEquals
            // 
            assetNameNotEquals.AutoSize = true;
            assetNameNotEquals.Location = new Point(7, 105);
            assetNameNotEquals.Name = "assetNameNotEquals";
            assetNameNotEquals.Size = new Size(110, 21);
            assetNameNotEquals.TabIndex = 5;
            assetNameNotEquals.TabStop = true;
            assetNameNotEquals.Text = "不等于任意一个";
            assetNameNotEquals.UseVisualStyleBackColor = true;
            // 
            // assetNameContains
            // 
            assetNameContains.AutoSize = true;
            assetNameContains.Location = new Point(7, 78);
            assetNameContains.Name = "assetNameContains";
            assetNameContains.Size = new Size(98, 21);
            assetNameContains.TabIndex = 4;
            assetNameContains.TabStop = true;
            assetNameContains.Text = "包含任意一个";
            assetNameContains.UseVisualStyleBackColor = true;
            // 
            // assetNameEquals
            // 
            assetNameEquals.AutoSize = true;
            assetNameEquals.Location = new Point(7, 51);
            assetNameEquals.Name = "assetNameEquals";
            assetNameEquals.Size = new Size(98, 21);
            assetNameEquals.TabIndex = 3;
            assetNameEquals.TabStop = true;
            assetNameEquals.Text = "等于任意一个";
            assetNameEquals.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(135, 24);
            label2.Name = "label2";
            label2.Size = new Size(200, 17);
            label2.TabIndex = 2;
            label2.Text = "在输入框中输入匹配规则，每行一个";
            // 
            // assetName
            // 
            assetName.AutoSize = true;
            assetName.Location = new Point(7, 24);
            assetName.Name = "assetName";
            assetName.Size = new Size(75, 21);
            assetName.TabIndex = 1;
            assetName.Text = "物品名称";
            assetName.UseVisualStyleBackColor = true;
            // 
            // assetNameBox
            // 
            assetNameBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            assetNameBox.Location = new Point(135, 45);
            assetNameBox.Multiline = true;
            assetNameBox.Name = "assetNameBox";
            assetNameBox.Size = new Size(411, 140);
            assetNameBox.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(saveBtn);
            panel1.Location = new Point(12, 410);
            panel1.Name = "panel1";
            panel1.Size = new Size(552, 34);
            panel1.TabIndex = 3;
            // 
            // saveBtn
            // 
            saveBtn.Dock = DockStyle.Fill;
            saveBtn.Location = new Point(0, 0);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(552, 34);
            saveBtn.TabIndex = 0;
            saveBtn.Text = "保存";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // AcceptOfferRule
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(576, 451);
            Controls.Add(panel1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AcceptOfferRule";
            StartPosition = FormStartPosition.CenterParent;
            Text = "自动接受报价规则设置";
            Load += AcceptOfferRule_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TextBox offerMessageBox;
        private CheckBox offerMessage;
        private Label label1;
        private RadioButton offerMessageRegex;
        private RadioButton offerMessageNotContains;
        private RadioButton offerMessageNotEquals;
        private RadioButton offerMessageContains;
        private RadioButton offerMessageEquals;
        private RadioButton assetNameRegex;
        private RadioButton assetNameNotContains;
        private RadioButton assetNameNotEquals;
        private RadioButton assetNameContains;
        private RadioButton assetNameEquals;
        private Label label2;
        private CheckBox assetName;
        private TextBox assetNameBox;
        private Panel panel1;
        private Button saveBtn;
    }
}
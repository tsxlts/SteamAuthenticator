namespace Steam_Authenticator.Forms
{
    partial class TransferAssets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferAssets));
            groupBox1 = new GroupBox();
            panel5 = new Panel();
            allAssets = new RadioButton();
            label6 = new Label();
            panel2 = new Panel();
            cs2Game = new RadioButton();
            label7 = new Label();
            panel1 = new Panel();
            autoConfirm = new CheckBox();
            receiverSendOffer = new RadioButton();
            autoAccept = new CheckBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            selectReceiverBtn = new LinkLabel();
            receiverBox = new Label();
            label3 = new Label();
            groupBox3 = new GroupBox();
            label5 = new Label();
            deliverersPanel = new Panel();
            selectDelivererBtn = new LinkLabel();
            panel3 = new Panel();
            okBtn = new Button();
            panel4 = new Panel();
            label4 = new Label();
            groupBox1.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(panel5);
            groupBox1.Controls.Add(panel2);
            groupBox1.Controls.Add(panel1);
            groupBox1.Location = new Point(12, 64);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(677, 93);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "报价选项";
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel5.Controls.Add(allAssets);
            panel5.Controls.Add(label6);
            panel5.Location = new Point(312, 56);
            panel5.Name = "panel5";
            panel5.Size = new Size(359, 28);
            panel5.TabIndex = 3;
            // 
            // allAssets
            // 
            allAssets.AutoSize = true;
            allAssets.Location = new Point(86, 4);
            allAssets.Name = "allAssets";
            allAssets.Size = new Size(122, 21);
            allAssets.TabIndex = 2;
            allAssets.TabStop = true;
            allAssets.Text = "全部可交易的库存";
            allAssets.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.ForeColor = Color.FromArgb(127, 127, 127);
            label6.Location = new Point(3, 5);
            label6.Name = "label6";
            label6.Size = new Size(77, 17);
            label6.TabIndex = 1;
            label6.Text = "库存：";
            label6.TextAlign = ContentAlignment.TopRight;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(cs2Game);
            panel2.Controls.Add(label7);
            panel2.Location = new Point(6, 56);
            panel2.Name = "panel2";
            panel2.Size = new Size(300, 28);
            panel2.TabIndex = 2;
            // 
            // cs2Game
            // 
            cs2Game.AutoSize = true;
            cs2Game.Location = new Point(89, 4);
            cs2Game.Name = "cs2Game";
            cs2Game.Size = new Size(48, 21);
            cs2Game.TabIndex = 3;
            cs2Game.TabStop = true;
            cs2Game.Text = "CS2";
            cs2Game.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.ForeColor = Color.FromArgb(127, 127, 127);
            label7.Location = new Point(3, 5);
            label7.Name = "label7";
            label7.Size = new Size(80, 17);
            label7.TabIndex = 2;
            label7.Text = "游戏：";
            label7.TextAlign = ContentAlignment.TopRight;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(autoConfirm);
            panel1.Controls.Add(receiverSendOffer);
            panel1.Controls.Add(autoAccept);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(6, 22);
            panel1.Name = "panel1";
            panel1.Size = new Size(665, 28);
            panel1.TabIndex = 1;
            // 
            // autoConfirm
            // 
            autoConfirm.AutoSize = true;
            autoConfirm.Location = new Point(497, 5);
            autoConfirm.Name = "autoConfirm";
            autoConfirm.Size = new Size(99, 21);
            autoConfirm.TabIndex = 3;
            autoConfirm.Text = "自动令牌确认";
            autoConfirm.UseVisualStyleBackColor = true;
            autoConfirm.CheckedChanged += autoConfirm_CheckedChanged;
            // 
            // receiverSendOffer
            // 
            receiverSendOffer.AutoSize = true;
            receiverSendOffer.Location = new Point(89, 4);
            receiverSendOffer.Name = "receiverSendOffer";
            receiverSendOffer.Size = new Size(194, 21);
            receiverSendOffer.TabIndex = 1;
            receiverSendOffer.TabStop = true;
            receiverSendOffer.Text = "库存转入方（收货方）发送报价";
            receiverSendOffer.UseVisualStyleBackColor = true;
            // 
            // autoAccept
            // 
            autoAccept.AutoSize = true;
            autoAccept.Location = new Point(392, 5);
            autoAccept.Name = "autoAccept";
            autoAccept.Size = new Size(99, 21);
            autoAccept.TabIndex = 2;
            autoAccept.Text = "自动接受报价";
            autoAccept.UseVisualStyleBackColor = true;
            autoAccept.CheckedChanged += autoAccept_CheckedChanged;
            // 
            // label2
            // 
            label2.ForeColor = Color.FromArgb(127, 127, 127);
            label2.Location = new Point(306, 6);
            label2.Name = "label2";
            label2.Size = new Size(80, 17);
            label2.TabIndex = 1;
            label2.Text = "报价处理：";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.ForeColor = Color.FromArgb(127, 127, 127);
            label1.Location = new Point(3, 6);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 0;
            label1.Text = "报价发送方：";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(selectReceiverBtn);
            groupBox2.Controls.Add(receiverBox);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(12, 163);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(677, 49);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "库存转入方";
            // 
            // selectReceiverBtn
            // 
            selectReceiverBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectReceiverBtn.AutoSize = true;
            selectReceiverBtn.Location = new Point(605, 24);
            selectReceiverBtn.Name = "selectReceiverBtn";
            selectReceiverBtn.Size = new Size(56, 17);
            selectReceiverBtn.TabIndex = 3;
            selectReceiverBtn.TabStop = true;
            selectReceiverBtn.Text = "选择帐号";
            selectReceiverBtn.VisitedLinkColor = Color.Blue;
            selectReceiverBtn.LinkClicked += selectReceiverBtn_LinkClicked;
            // 
            // receiverBox
            // 
            receiverBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            receiverBox.AutoEllipsis = true;
            receiverBox.Location = new Point(95, 24);
            receiverBox.Name = "receiverBox";
            receiverBox.Size = new Size(514, 17);
            receiverBox.TabIndex = 2;
            // 
            // label3
            // 
            label3.ForeColor = Color.FromArgb(127, 127, 127);
            label3.Location = new Point(9, 24);
            label3.Name = "label3";
            label3.Size = new Size(80, 17);
            label3.TabIndex = 1;
            label3.Text = "库存转入方：";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(deliverersPanel);
            groupBox3.Controls.Add(selectDelivererBtn);
            groupBox3.Location = new Point(12, 228);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(677, 204);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "库存转出方";
            // 
            // label5
            // 
            label5.ForeColor = Color.FromArgb(127, 127, 127);
            label5.Location = new Point(6, 21);
            label5.Name = "label5";
            label5.Size = new Size(80, 17);
            label5.TabIndex = 6;
            label5.Text = "库存转出方：";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // deliverersPanel
            // 
            deliverersPanel.AutoScroll = true;
            deliverersPanel.BackColor = Color.White;
            deliverersPanel.Location = new Point(2, 41);
            deliverersPanel.Name = "deliverersPanel";
            deliverersPanel.Size = new Size(673, 155);
            deliverersPanel.TabIndex = 5;
            // 
            // selectDelivererBtn
            // 
            selectDelivererBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectDelivererBtn.AutoSize = true;
            selectDelivererBtn.Location = new Point(605, 19);
            selectDelivererBtn.Name = "selectDelivererBtn";
            selectDelivererBtn.Size = new Size(56, 17);
            selectDelivererBtn.TabIndex = 4;
            selectDelivererBtn.TabStop = true;
            selectDelivererBtn.Text = "选择帐号";
            selectDelivererBtn.VisitedLinkColor = Color.Blue;
            selectDelivererBtn.LinkClicked += selectDelivererBtn_LinkClicked;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.Controls.Add(okBtn);
            panel3.Location = new Point(12, 438);
            panel3.Name = "panel3";
            panel3.Size = new Size(677, 37);
            panel3.TabIndex = 3;
            // 
            // okBtn
            // 
            okBtn.Dock = DockStyle.Fill;
            okBtn.Location = new Point(0, 0);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(677, 37);
            okBtn.TabIndex = 0;
            okBtn.Text = "开始转移";
            okBtn.UseVisualStyleBackColor = true;
            okBtn.Click += okBtn_Click;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel4.Controls.Add(label4);
            panel4.Location = new Point(12, 8);
            panel4.Name = "panel4";
            panel4.Size = new Size(677, 50);
            panel4.TabIndex = 4;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label4.ForeColor = Color.Red;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(677, 50);
            label4.TabIndex = 0;
            label4.Text = "风险提示\r\n大批量饰品报价交易可能会引起Steam帐号红锁，因本工具所引起的一切后果，我们概不负责，请自行谨慎操作！！！";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TransferAssets
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(701, 478);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TransferAssets";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Steam库存转移";
            FormClosing += TransferAssets_FormClosing;
            Load += TransferAssets_Load;
            groupBox1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private Panel panel1;
        private RadioButton receiverSendOffer;
        private GroupBox groupBox2;
        private Panel panel2;
        private Label label2;
        private CheckBox autoConfirm;
        private CheckBox autoAccept;
        private Label receiverBox;
        private Label label3;
        private LinkLabel selectReceiverBtn;
        private GroupBox groupBox3;
        private LinkLabel selectDelivererBtn;
        private Panel deliverersPanel;
        private Panel panel3;
        private Button okBtn;
        private Panel panel4;
        private Label label4;
        private Label label5;
        private Panel panel5;
        private Label label6;
        private RadioButton allAssets;
        private RadioButton cs2Game;
        private Label label7;
    }
}
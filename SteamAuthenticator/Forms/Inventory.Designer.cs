namespace Steam_Authenticator.Forms
{
    partial class Inventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inventory));
            inventoryPages = new TabControl();
            defaultApp = new TabPage();
            defaultInventory = new Panel();
            SteamId = new Label();
            UserName = new Label();
            prePageBtn = new LinkLabel();
            label1 = new Label();
            inventoryTotalBox = new Label();
            nextPageBtn = new LinkLabel();
            label2 = new Label();
            pageTotalBox = new Label();
            label4 = new Label();
            label5 = new Label();
            currentPageBox = new Label();
            label7 = new Label();
            inventoryPages.SuspendLayout();
            defaultApp.SuspendLayout();
            SuspendLayout();
            // 
            // inventoryPages
            // 
            inventoryPages.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inventoryPages.Controls.Add(defaultApp);
            inventoryPages.Location = new Point(12, 38);
            inventoryPages.Name = "inventoryPages";
            inventoryPages.SelectedIndex = 0;
            inventoryPages.Size = new Size(776, 411);
            inventoryPages.TabIndex = 0;
            inventoryPages.SelectedIndexChanged += inventoryPages_SelectedIndexChanged;
            // 
            // defaultApp
            // 
            defaultApp.Controls.Add(defaultInventory);
            defaultApp.Location = new Point(4, 26);
            defaultApp.Name = "defaultApp";
            defaultApp.Padding = new Padding(3);
            defaultApp.Size = new Size(768, 381);
            defaultApp.TabIndex = 0;
            defaultApp.Text = "Steam";
            defaultApp.UseVisualStyleBackColor = true;
            // 
            // defaultInventory
            // 
            defaultInventory.AutoScroll = true;
            defaultInventory.Dock = DockStyle.Fill;
            defaultInventory.Location = new Point(3, 3);
            defaultInventory.Name = "defaultInventory";
            defaultInventory.Size = new Size(762, 375);
            defaultInventory.TabIndex = 0;
            // 
            // SteamId
            // 
            SteamId.AutoEllipsis = true;
            SteamId.ForeColor = Color.FromArgb(0, 0, 238);
            SteamId.Location = new Point(16, 9);
            SteamId.Name = "SteamId";
            SteamId.Size = new Size(133, 18);
            SteamId.TabIndex = 20;
            SteamId.Text = "---";
            SteamId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UserName
            // 
            UserName.AutoEllipsis = true;
            UserName.ForeColor = Color.Green;
            UserName.Location = new Point(160, 9);
            UserName.Name = "UserName";
            UserName.Size = new Size(300, 18);
            UserName.TabIndex = 19;
            UserName.Text = "---";
            UserName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // prePageBtn
            // 
            prePageBtn.AutoSize = true;
            prePageBtn.Location = new Point(155, 452);
            prePageBtn.Name = "prePageBtn";
            prePageBtn.Size = new Size(44, 17);
            prePageBtn.TabIndex = 21;
            prePageBtn.TabStop = true;
            prePageBtn.Text = "上一页";
            prePageBtn.LinkClicked += preBtn_LinkClicked;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(19, 452);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 22;
            label1.Text = "库存总数";
            // 
            // inventoryTotalBox
            // 
            inventoryTotalBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            inventoryTotalBox.Location = new Point(81, 452);
            inventoryTotalBox.Name = "inventoryTotalBox";
            inventoryTotalBox.Size = new Size(68, 17);
            inventoryTotalBox.TabIndex = 23;
            inventoryTotalBox.Text = "0";
            // 
            // nextPageBtn
            // 
            nextPageBtn.AutoSize = true;
            nextPageBtn.Location = new Point(393, 452);
            nextPageBtn.Name = "nextPageBtn";
            nextPageBtn.Size = new Size(44, 17);
            nextPageBtn.TabIndex = 24;
            nextPageBtn.TabStop = true;
            nextPageBtn.Text = "下一页";
            nextPageBtn.VisitedLinkColor = Color.Blue;
            nextPageBtn.LinkClicked += netBtn_LinkClicked;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(205, 452);
            label2.Name = "label2";
            label2.Size = new Size(20, 17);
            label2.TabIndex = 25;
            label2.Text = "共";
            // 
            // pageTotalBox
            // 
            pageTotalBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pageTotalBox.Location = new Point(231, 452);
            pageTotalBox.Name = "pageTotalBox";
            pageTotalBox.Size = new Size(36, 17);
            pageTotalBox.TabIndex = 26;
            pageTotalBox.Text = "1";
            pageTotalBox.TextAlign = ContentAlignment.TopCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(273, 452);
            label4.Name = "label4";
            label4.Size = new Size(20, 17);
            label4.TabIndex = 27;
            label4.Text = "页";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(367, 452);
            label5.Name = "label5";
            label5.Size = new Size(20, 17);
            label5.TabIndex = 30;
            label5.Text = "页";
            // 
            // currentPageBox
            // 
            currentPageBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            currentPageBox.Location = new Point(325, 452);
            currentPageBox.Name = "currentPageBox";
            currentPageBox.Size = new Size(36, 17);
            currentPageBox.TabIndex = 29;
            currentPageBox.Text = "1";
            currentPageBox.TextAlign = ContentAlignment.TopCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(299, 452);
            label7.Name = "label7";
            label7.Size = new Size(20, 17);
            label7.TabIndex = 28;
            label7.Text = "第";
            // 
            // Inventory
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 478);
            Controls.Add(label5);
            Controls.Add(currentPageBox);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(pageTotalBox);
            Controls.Add(label2);
            Controls.Add(nextPageBtn);
            Controls.Add(inventoryTotalBox);
            Controls.Add(label1);
            Controls.Add(prePageBtn);
            Controls.Add(SteamId);
            Controls.Add(UserName);
            Controls.Add(inventoryPages);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Inventory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Steam库存";
            Load += Inventory_Load;
            inventoryPages.ResumeLayout(false);
            defaultApp.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl inventoryPages;
        private TabPage defaultApp;
        private Label SteamId;
        private Label UserName;
        private Panel defaultInventory;
        private LinkLabel prePageBtn;
        private Label label1;
        private Label inventoryTotalBox;
        private LinkLabel nextPageBtn;
        private Label label2;
        private Label pageTotalBox;
        private Label label4;
        private Label label5;
        private Label currentPageBox;
        private Label label7;
    }
}
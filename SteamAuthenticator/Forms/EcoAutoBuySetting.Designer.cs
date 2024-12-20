namespace Steam_Authenticator.Forms
{
    partial class EcoAutoBuySetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcoAutoBuySetting));
            panel1 = new Panel();
            autoBuyList = new DataGridView();
            GameId = new DataGridViewComboBoxColumn();
            HashName = new DataGridViewTextBoxColumn();
            MaxPrice = new DataGridViewTextBoxColumn();
            BuySize = new DataGridViewTextBoxColumn();
            SteamId = new DataGridViewComboBoxColumn();
            PayType = new DataGridViewComboBoxColumn();
            NotifyAddress = new DataGridViewTextBoxColumn();
            GoodsEnabled = new DataGridViewCheckBoxColumn();
            panel2 = new Panel();
            reloadBtn = new Label();
            addGoodsBtn = new Label();
            saveBtn = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)autoBuyList).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(autoBuyList);
            panel1.Location = new Point(12, 51);
            panel1.Name = "panel1";
            panel1.Size = new Size(917, 474);
            panel1.TabIndex = 0;
            // 
            // autoBuyList
            // 
            autoBuyList.AllowUserToAddRows = false;
            autoBuyList.AllowUserToDeleteRows = false;
            autoBuyList.AllowUserToResizeColumns = false;
            autoBuyList.AllowUserToResizeRows = false;
            autoBuyList.BackgroundColor = Color.White;
            autoBuyList.BorderStyle = BorderStyle.None;
            autoBuyList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            autoBuyList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            autoBuyList.Columns.AddRange(new DataGridViewColumn[] { GameId, HashName, MaxPrice, BuySize, SteamId, PayType, NotifyAddress, GoodsEnabled });
            autoBuyList.Dock = DockStyle.Fill;
            autoBuyList.EditMode = DataGridViewEditMode.EditOnEnter;
            autoBuyList.Location = new Point(0, 0);
            autoBuyList.Name = "autoBuyList";
            autoBuyList.RowHeadersVisible = false;
            autoBuyList.Size = new Size(917, 474);
            autoBuyList.TabIndex = 0;
            autoBuyList.CellMouseDown += autoBuyList_CellMouseDown;
            autoBuyList.Leave += autoBuyList_Leave;
            // 
            // GameId
            // 
            GameId.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            GameId.HeaderText = "游戏";
            GameId.Name = "GameId";
            // 
            // HashName
            // 
            HashName.HeaderText = "物品名称 (HashName)";
            HashName.Name = "HashName";
            HashName.Width = 200;
            // 
            // MaxPrice
            // 
            MaxPrice.HeaderText = "购买价格";
            MaxPrice.Name = "MaxPrice";
            // 
            // BuySize
            // 
            BuySize.HeaderText = "单次下单数量";
            BuySize.Name = "BuySize";
            BuySize.Width = 105;
            // 
            // SteamId
            // 
            SteamId.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            SteamId.HeaderText = "收货帐号";
            SteamId.Name = "SteamId";
            // 
            // PayType
            // 
            PayType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            PayType.HeaderText = "支付方式";
            PayType.Name = "PayType";
            PayType.Width = 80;
            // 
            // NotifyAddress
            // 
            NotifyAddress.HeaderText = "通知邮箱";
            NotifyAddress.Name = "NotifyAddress";
            NotifyAddress.Resizable = DataGridViewTriState.True;
            NotifyAddress.SortMode = DataGridViewColumnSortMode.NotSortable;
            NotifyAddress.Width = 150;
            // 
            // GoodsEnabled
            // 
            GoodsEnabled.HeaderText = "是否可用";
            GoodsEnabled.Name = "GoodsEnabled";
            GoodsEnabled.Width = 80;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(255, 248, 220);
            panel2.Controls.Add(reloadBtn);
            panel2.Controls.Add(addGoodsBtn);
            panel2.Controls.Add(saveBtn);
            panel2.Location = new Point(12, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(917, 40);
            panel2.TabIndex = 1;
            // 
            // reloadBtn
            // 
            reloadBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            reloadBtn.AutoSize = true;
            reloadBtn.Cursor = Cursors.Hand;
            reloadBtn.ForeColor = Color.Green;
            reloadBtn.Location = new Point(794, 12);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(56, 17);
            reloadBtn.TabIndex = 13;
            reloadBtn.Text = "重新加载";
            reloadBtn.Click += reloadBtn_Click;
            // 
            // addGoodsBtn
            // 
            addGoodsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            addGoodsBtn.AutoSize = true;
            addGoodsBtn.Cursor = Cursors.Hand;
            addGoodsBtn.ForeColor = Color.Green;
            addGoodsBtn.Location = new Point(732, 12);
            addGoodsBtn.Name = "addGoodsBtn";
            addGoodsBtn.Size = new Size(56, 17);
            addGoodsBtn.TabIndex = 12;
            addGoodsBtn.Text = "添加商品";
            addGoodsBtn.Click += addGoodsBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            saveBtn.AutoSize = true;
            saveBtn.Cursor = Cursors.Hand;
            saveBtn.ForeColor = Color.Green;
            saveBtn.Location = new Point(856, 12);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(56, 17);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "保存配置";
            saveBtn.Click += saveBtn_Click;
            // 
            // EcoAutoBuySetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(941, 537);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EcoAutoBuySetting";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ECO 自动扫货配置";
            Load += EcoAutoBuySetting_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)autoBuyList).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView autoBuyList;
        private Panel panel2;
        private Label saveBtn;
        private Label addGoodsBtn;
        private Label reloadBtn;
        private DataGridViewComboBoxColumn GameId;
        private DataGridViewTextBoxColumn HashName;
        private DataGridViewTextBoxColumn MaxPrice;
        private DataGridViewTextBoxColumn BuySize;
        private DataGridViewComboBoxColumn SteamId;
        private DataGridViewComboBoxColumn PayType;
        private DataGridViewTextBoxColumn NotifyAddress;
        private DataGridViewCheckBoxColumn GoodsEnabled;
    }
}
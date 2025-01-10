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
            panel2 = new Panel();
            msgBox = new Label();
            selectAllBtn = new Label();
            reloadBtn = new Label();
            addGoodsBtn = new Label();
            saveBtn = new Label();
            GameId = new DataGridViewComboBoxColumn();
            HashName = new DataGridViewComboBoxColumn();
            MaxPrice = new DataGridViewTextBoxColumn();
            CurrentPrice = new DataGridViewTextBoxColumn();
            PayType = new DataGridViewComboBoxColumn();
            SteamId = new DataGridViewComboBoxColumn();
            QuerySize = new DataGridViewTextBoxColumn();
            BuySize = new DataGridViewTextBoxColumn();
            Interval = new DataGridViewTextBoxColumn();
            NotifyAddress = new DataGridViewTextBoxColumn();
            RunTimes = new DataGridViewTextBoxColumn();
            GoodsEnabled = new DataGridViewCheckBoxColumn();
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
            panel1.Size = new Size(1398, 643);
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
            autoBuyList.Columns.AddRange(new DataGridViewColumn[] { GameId, HashName, MaxPrice, CurrentPrice, PayType, SteamId, QuerySize, BuySize, Interval, NotifyAddress, RunTimes, GoodsEnabled });
            autoBuyList.Dock = DockStyle.Fill;
            autoBuyList.EditMode = DataGridViewEditMode.EditOnEnter;
            autoBuyList.Location = new Point(0, 0);
            autoBuyList.Name = "autoBuyList";
            autoBuyList.RowHeadersVisible = false;
            autoBuyList.Size = new Size(1398, 643);
            autoBuyList.TabIndex = 0;
            autoBuyList.CellMouseUp += autoBuyList_CellMouseUp;
            autoBuyList.DataError += autoBuyList_DataError;
            autoBuyList.SortCompare += autoBuyList_SortCompare;
            autoBuyList.Leave += autoBuyList_Leave;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(255, 248, 220);
            panel2.Controls.Add(msgBox);
            panel2.Controls.Add(selectAllBtn);
            panel2.Controls.Add(reloadBtn);
            panel2.Controls.Add(addGoodsBtn);
            panel2.Controls.Add(saveBtn);
            panel2.Location = new Point(12, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(1398, 40);
            panel2.TabIndex = 1;
            // 
            // msgBox
            // 
            msgBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            msgBox.AutoEllipsis = true;
            msgBox.Cursor = Cursors.Hand;
            msgBox.ForeColor = Color.Gray;
            msgBox.Location = new Point(16, 12);
            msgBox.Name = "msgBox";
            msgBox.Size = new Size(1165, 17);
            msgBox.TabIndex = 15;
            msgBox.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // selectAllBtn
            // 
            selectAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            selectAllBtn.Cursor = Cursors.Hand;
            selectAllBtn.ForeColor = Color.Green;
            selectAllBtn.Location = new Point(1275, 13);
            selectAllBtn.Name = "selectAllBtn";
            selectAllBtn.Size = new Size(38, 17);
            selectAllBtn.TabIndex = 14;
            selectAllBtn.Text = "全选";
            selectAllBtn.TextAlign = ContentAlignment.MiddleCenter;
            selectAllBtn.Click += selectAllBtn_Click;
            // 
            // reloadBtn
            // 
            reloadBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            reloadBtn.Cursor = Cursors.Hand;
            reloadBtn.ForeColor = Color.Green;
            reloadBtn.Location = new Point(1319, 13);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(56, 17);
            reloadBtn.TabIndex = 13;
            reloadBtn.Text = "重新加载";
            reloadBtn.TextAlign = ContentAlignment.MiddleCenter;
            reloadBtn.Click += reloadBtn_Click;
            // 
            // addGoodsBtn
            // 
            addGoodsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            addGoodsBtn.Cursor = Cursors.Hand;
            addGoodsBtn.ForeColor = Color.Green;
            addGoodsBtn.Location = new Point(1187, 13);
            addGoodsBtn.Name = "addGoodsBtn";
            addGoodsBtn.Size = new Size(38, 17);
            addGoodsBtn.TabIndex = 12;
            addGoodsBtn.Text = "添加";
            addGoodsBtn.TextAlign = ContentAlignment.MiddleCenter;
            addGoodsBtn.Click += addGoodsBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            saveBtn.Cursor = Cursors.Hand;
            saveBtn.ForeColor = Color.Green;
            saveBtn.Location = new Point(1231, 13);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(38, 17);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "保存";
            saveBtn.TextAlign = ContentAlignment.MiddleCenter;
            saveBtn.Click += saveBtn_Click;
            // 
            // GameId
            // 
            GameId.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            GameId.HeaderText = "游戏";
            GameId.Name = "GameId";
            GameId.Resizable = DataGridViewTriState.False;
            GameId.SortMode = DataGridViewColumnSortMode.Automatic;
            GameId.Width = 80;
            // 
            // HashName
            // 
            HashName.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            HashName.HeaderText = "物品名称";
            HashName.Name = "HashName";
            HashName.Resizable = DataGridViewTriState.False;
            HashName.SortMode = DataGridViewColumnSortMode.Automatic;
            HashName.Width = 200;
            // 
            // MaxPrice
            // 
            MaxPrice.HeaderText = "购买价格";
            MaxPrice.Name = "MaxPrice";
            MaxPrice.Resizable = DataGridViewTriState.False;
            MaxPrice.Width = 80;
            // 
            // CurrentPrice
            // 
            CurrentPrice.HeaderText = "当前价格";
            CurrentPrice.Name = "CurrentPrice";
            CurrentPrice.ReadOnly = true;
            CurrentPrice.Resizable = DataGridViewTriState.False;
            CurrentPrice.Width = 140;
            // 
            // PayType
            // 
            PayType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            PayType.HeaderText = "支付方式";
            PayType.Name = "PayType";
            PayType.Resizable = DataGridViewTriState.False;
            PayType.SortMode = DataGridViewColumnSortMode.Automatic;
            PayType.Width = 80;
            // 
            // SteamId
            // 
            SteamId.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            SteamId.HeaderText = "收货帐号";
            SteamId.Name = "SteamId";
            SteamId.Resizable = DataGridViewTriState.False;
            SteamId.SortMode = DataGridViewColumnSortMode.Automatic;
            SteamId.Width = 80;
            // 
            // QuerySize
            // 
            QuerySize.HeaderText = "单次查询数量";
            QuerySize.Name = "QuerySize";
            QuerySize.Width = 105;
            // 
            // BuySize
            // 
            BuySize.HeaderText = "单次下单数量";
            BuySize.Name = "BuySize";
            BuySize.Resizable = DataGridViewTriState.False;
            BuySize.Width = 105;
            // 
            // Interval
            // 
            Interval.HeaderText = "扫货频率 (毫秒/每次)";
            Interval.Name = "Interval";
            Interval.Resizable = DataGridViewTriState.False;
            Interval.Width = 150;
            // 
            // NotifyAddress
            // 
            NotifyAddress.HeaderText = "通知邮箱";
            NotifyAddress.Name = "NotifyAddress";
            NotifyAddress.Resizable = DataGridViewTriState.False;
            NotifyAddress.Width = 130;
            // 
            // RunTimes
            // 
            RunTimes.HeaderText = "运行时间";
            RunTimes.Name = "RunTimes";
            RunTimes.ReadOnly = true;
            RunTimes.Resizable = DataGridViewTriState.False;
            RunTimes.Width = 150;
            // 
            // GoodsEnabled
            // 
            GoodsEnabled.HeaderText = "是否可用";
            GoodsEnabled.Name = "GoodsEnabled";
            GoodsEnabled.Resizable = DataGridViewTriState.False;
            GoodsEnabled.SortMode = DataGridViewColumnSortMode.Automatic;
            GoodsEnabled.Width = 80;
            // 
            // EcoAutoBuySetting
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1422, 706);
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
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView autoBuyList;
        private Panel panel2;
        private Label saveBtn;
        private Label addGoodsBtn;
        private Label reloadBtn;
        private Label selectAllBtn;
        private Label msgBox;
        private DataGridViewComboBoxColumn GameId;
        private DataGridViewComboBoxColumn HashName;
        private DataGridViewTextBoxColumn MaxPrice;
        private DataGridViewTextBoxColumn CurrentPrice;
        private DataGridViewComboBoxColumn PayType;
        private DataGridViewComboBoxColumn SteamId;
        private DataGridViewTextBoxColumn QuerySize;
        private DataGridViewTextBoxColumn BuySize;
        private DataGridViewTextBoxColumn Interval;
        private DataGridViewTextBoxColumn NotifyAddress;
        private DataGridViewTextBoxColumn RunTimes;
        private DataGridViewCheckBoxColumn GoodsEnabled;
    }
}
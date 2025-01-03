namespace Steam_Authenticator.Forms
{
    partial class EcoOrders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcoOrders));
            panel2 = new Panel();
            selectAllBtn = new Label();
            payTypeBox = new ComboBox();
            payBtn = new Label();
            refreshBtn = new Label();
            reloadBtn = new Label();
            addGoodsBtn = new Label();
            saveBtn = new Label();
            panel1 = new Panel();
            ecoOrdersList = new DataGridView();
            GameId = new DataGridViewComboBoxColumn();
            GoodsName = new DataGridViewTextBoxColumn();
            FloatValue = new DataGridViewTextBoxColumn();
            Price = new DataGridViewTextBoxColumn();
            IsChecked = new DataGridViewCheckBoxColumn();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ecoOrdersList).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(255, 248, 220);
            panel2.Controls.Add(selectAllBtn);
            panel2.Controls.Add(payTypeBox);
            panel2.Controls.Add(payBtn);
            panel2.Controls.Add(refreshBtn);
            panel2.Controls.Add(reloadBtn);
            panel2.Controls.Add(addGoodsBtn);
            panel2.Controls.Add(saveBtn);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(722, 40);
            panel2.TabIndex = 3;
            // 
            // selectAllBtn
            // 
            selectAllBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            selectAllBtn.Cursor = Cursors.Hand;
            selectAllBtn.ForeColor = Color.Green;
            selectAllBtn.Location = new Point(620, 12);
            selectAllBtn.Name = "selectAllBtn";
            selectAllBtn.Size = new Size(38, 17);
            selectAllBtn.TabIndex = 17;
            selectAllBtn.Text = "全选";
            selectAllBtn.TextAlign = ContentAlignment.MiddleCenter;
            selectAllBtn.Click += selectAllBtn_Click;
            // 
            // payTypeBox
            // 
            payTypeBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            payTypeBox.BackColor = Color.Cornsilk;
            payTypeBox.FormattingEnabled = true;
            payTypeBox.Location = new Point(488, 9);
            payTypeBox.Name = "payTypeBox";
            payTypeBox.Size = new Size(82, 25);
            payTypeBox.TabIndex = 16;
            // 
            // payBtn
            // 
            payBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            payBtn.Cursor = Cursors.Hand;
            payBtn.ForeColor = Color.Green;
            payBtn.Location = new Point(576, 12);
            payBtn.Name = "payBtn";
            payBtn.Size = new Size(38, 17);
            payBtn.TabIndex = 15;
            payBtn.Text = "支付";
            payBtn.TextAlign = ContentAlignment.MiddleCenter;
            payBtn.Click += payBtn_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            refreshBtn.Cursor = Cursors.Hand;
            refreshBtn.ForeColor = Color.Green;
            refreshBtn.Location = new Point(664, 12);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new Size(38, 17);
            refreshBtn.TabIndex = 14;
            refreshBtn.Text = "刷新";
            refreshBtn.TextAlign = ContentAlignment.MiddleCenter;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // reloadBtn
            // 
            reloadBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            reloadBtn.Cursor = Cursors.Hand;
            reloadBtn.ForeColor = Color.Green;
            reloadBtn.Location = new Point(1730, 12);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(56, 0);
            reloadBtn.TabIndex = 13;
            reloadBtn.Text = "重新加载";
            reloadBtn.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // addGoodsBtn
            // 
            addGoodsBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            addGoodsBtn.Cursor = Cursors.Hand;
            addGoodsBtn.ForeColor = Color.Green;
            addGoodsBtn.Location = new Point(1642, 12);
            addGoodsBtn.Name = "addGoodsBtn";
            addGoodsBtn.Size = new Size(38, 0);
            addGoodsBtn.TabIndex = 12;
            addGoodsBtn.Text = "添加";
            addGoodsBtn.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            saveBtn.Cursor = Cursors.Hand;
            saveBtn.ForeColor = Color.Green;
            saveBtn.Location = new Point(1686, 12);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(38, 0);
            saveBtn.TabIndex = 10;
            saveBtn.Text = "保存";
            saveBtn.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(ecoOrdersList);
            panel1.Location = new Point(12, 58);
            panel1.Name = "panel1";
            panel1.Size = new Size(722, 403);
            panel1.TabIndex = 2;
            // 
            // ecoOrdersList
            // 
            ecoOrdersList.AllowUserToAddRows = false;
            ecoOrdersList.AllowUserToDeleteRows = false;
            ecoOrdersList.AllowUserToResizeColumns = false;
            ecoOrdersList.AllowUserToResizeRows = false;
            ecoOrdersList.BackgroundColor = Color.White;
            ecoOrdersList.BorderStyle = BorderStyle.None;
            ecoOrdersList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ecoOrdersList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ecoOrdersList.Columns.AddRange(new DataGridViewColumn[] { GameId, GoodsName, FloatValue, Price, IsChecked });
            ecoOrdersList.Dock = DockStyle.Fill;
            ecoOrdersList.EditMode = DataGridViewEditMode.EditOnEnter;
            ecoOrdersList.Location = new Point(0, 0);
            ecoOrdersList.Name = "ecoOrdersList";
            ecoOrdersList.RowHeadersVisible = false;
            ecoOrdersList.Size = new Size(722, 403);
            ecoOrdersList.TabIndex = 0;
            ecoOrdersList.Leave += ecoOrdersList_Leave;
            // 
            // GameId
            // 
            GameId.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            GameId.HeaderText = "游戏";
            GameId.Name = "GameId";
            GameId.ReadOnly = true;
            // 
            // GoodsName
            // 
            GoodsName.HeaderText = "物品名称";
            GoodsName.Name = "GoodsName";
            GoodsName.ReadOnly = true;
            GoodsName.Resizable = DataGridViewTriState.False;
            GoodsName.Width = 300;
            // 
            // FloatValue
            // 
            FloatValue.HeaderText = "磨损度";
            FloatValue.Name = "FloatValue";
            FloatValue.ReadOnly = true;
            FloatValue.Width = 150;
            // 
            // Price
            // 
            Price.HeaderText = "购买价格";
            Price.Name = "Price";
            Price.ReadOnly = true;
            Price.Resizable = DataGridViewTriState.False;
            // 
            // IsChecked
            // 
            IsChecked.HeaderText = "选择";
            IsChecked.Name = "IsChecked";
            IsChecked.Resizable = DataGridViewTriState.False;
            IsChecked.Width = 50;
            // 
            // EcoOrders
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(746, 473);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "EcoOrders";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ECO 订单";
            Load += EcoOrders_Load;
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ecoOrdersList).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private Label reloadBtn;
        private Label addGoodsBtn;
        private Label saveBtn;
        private Panel panel1;
        private DataGridView ecoOrdersList;
        private Label payBtn;
        private Label refreshBtn;
        private DataGridViewComboBoxColumn GameId;
        private DataGridViewTextBoxColumn GoodsName;
        private DataGridViewTextBoxColumn FloatValue;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewCheckBoxColumn IsChecked;
        private ComboBox payTypeBox;
        private Label selectAllBtn;
    }
}
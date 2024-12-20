using Steam_Authenticator.Model;
using static Steam_Authenticator.Model.EcoUser;

namespace Steam_Authenticator.Forms
{
    public partial class EcoAutoBuySetting : Form
    {
        private readonly ContextMenuStrip contextMenuStrip;

        private readonly EcoClient client;
        private readonly List<SelectItem> games;
        private readonly List<SelectItem> payTypes;
        private readonly List<SelectItem> steamUsers;

        public EcoAutoBuySetting(EcoClient client)
        {
            InitializeComponent();
            this.client = client;

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("添加商品").Click += addGoodsBtn_Click;
            contextMenuStrip.Items.Add("删除商品").Click += removeGoods_Click; ;

            games = new List<SelectItem> { new SelectItem { Value = "730", Name = "CS2" } };
            payTypes = new List<SelectItem> { new SelectItem { Value = Model.ECO.PayType.余额, Name = "余额" }, new SelectItem { Value = Model.ECO.PayType.支付宝, Name = "支付宝" } };

            steamUsers = client.User.SteamUsers.Select(c => new SelectItem { Value = c.SteamId, Name = c.NickName }).ToList();
        }

        private void EcoAutoBuySetting_Load(object sender, EventArgs e)
        {
            GameId.DataSource = games;
            GameId.ValueMember = nameof(SelectItem.Value);
            GameId.DisplayMember = nameof(SelectItem.Name);

            PayType.DataSource = payTypes;
            PayType.ValueMember = nameof(SelectItem.Value);
            PayType.DisplayMember = nameof(SelectItem.Name);

            SteamId.DataSource = steamUsers;
            SteamId.ValueMember = nameof(SelectItem.Value);
            SteamId.DisplayMember = nameof(SelectItem.Name);

            LoadSetting();
        }

        private void reloadBtn_Click(object sender, EventArgs e)
        {
            LoadSetting();
        }

        private void autoBuyList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < autoBuyList.Rows.Count; i++)
            {
                autoBuyList.Rows[i].Selected = false;
            }

            autoBuyList.Rows[e.RowIndex].Selected = true;
        }

        private void addGoodsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                addGoodsBtn.Enabled = false;

                int index = autoBuyList.Rows.Add();
                var row = autoBuyList.Rows[index];

                row.Cells[nameof(GameId)].Value = games[0].Value;
                row.Cells[nameof(PayType)].Value = payTypes[0].Value;
                row.Cells[nameof(BuySize)].Value = "1";

                if (steamUsers.Any())
                {
                    row.Cells[nameof(SteamId)].Value = steamUsers[0].Value;
                }

                row.ContextMenuStrip = contextMenuStrip;
            }
            finally
            {
                addGoodsBtn.Enabled = true;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                saveBtn.Enabled = false;

                if (autoBuyList.CurrentCell != null)
                {
                    autoBuyList.EndEdit();
                }

                List<AutoBuyGoods> buyGoods = new List<AutoBuyGoods>();
                AutoBuyGoods goods;
                foreach (DataGridViewRow row in autoBuyList.Rows)
                {
                    if (!decimal.TryParse(row.Cells[nameof(MaxPrice)].Value?.ToString(), out decimal maxPrice))
                    {
                        MessageBox.Show("请输入正确的价格", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(row.Cells[nameof(BuySize)].Value?.ToString(), out int buySize))
                    {
                        MessageBox.Show("请输入正确单次下单数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    goods = new AutoBuyGoods
                    {
                        GameId = row.Cells[nameof(GameId)].Value?.ToString(),
                        HashName = row.Cells[nameof(HashName)].Value?.ToString(),
                        SteamId = row.Cells[nameof(SteamId)].Value?.ToString(),

                        MaxPrice = maxPrice,
                        BuySize = buySize,

                        PayType = (Model.ECO.PayType)row.Cells[nameof(PayType)].Value,

                        NotifyAddress = row.Cells[nameof(NotifyAddress)].Value?.ToString(),
                        Enabled = (bool?)row.Cells[nameof(GoodsEnabled)].Value ?? false,
                    };

                    if (string.IsNullOrWhiteSpace(goods.GameId) || string.IsNullOrWhiteSpace(goods.HashName))
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(goods.SteamId))
                    {
                        continue;
                    }

                    buyGoods.Add(goods);
                }

                client.User.BuyGoods = buyGoods;
                Appsetting.Instance.Manifest.SaveEcoUser(client.User.UserId, client.User);
            }
            finally
            {
                saveBtn.Enabled = true;
            }

            MessageBox.Show("已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadSetting();
        }

        private void autoBuyList_Leave(object sender, EventArgs e)
        {
            if (autoBuyList.CurrentCell != null)
            {
                autoBuyList.EndEdit();
            }
        }

        private void removeGoods_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            DataGridView view = menuStrip.SourceControl as DataGridView;
            if (view.SelectedRows.Count > 0)
            {
                view.Rows.Remove(view.SelectedRows[0]);
            }
        }

        private void LoadSetting()
        {
            autoBuyList.Rows.Clear();

            foreach (var goods in client.User.BuyGoods)
            {
                int index = autoBuyList.Rows.Add();
                var row = autoBuyList.Rows[index];

                row.Cells[nameof(GameId)].Value = goods.GameId;
                row.Cells[nameof(HashName)].Value = goods.HashName;
                row.Cells[nameof(MaxPrice)].Value = goods.MaxPrice.ToString();
                row.Cells[nameof(BuySize)].Value = goods.BuySize.ToString();
                row.Cells[nameof(PayType)].Value = goods.PayType;
                row.Cells[nameof(SteamId)].Value = goods.SteamId;
                row.Cells[nameof(NotifyAddress)].Value = goods.NotifyAddress;
                row.Cells[nameof(GoodsEnabled)].Value = goods.Enabled;

                row.ContextMenuStrip = contextMenuStrip;
            }
        }
    }
}

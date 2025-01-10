using CefSharp.DevTools.IO;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using static Steam_Authenticator.Model.EcoUser;

namespace Steam_Authenticator.Forms
{
    public partial class EcoAutoBuySetting : Form
    {
        private readonly ContextMenuStrip contextMenuStrip;
        private readonly System.Threading.Timer timer;

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

            timer = new System.Threading.Timer(AutoRefresh, null, -1, -1);

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

            HashName.Items.Add(new SelectItem { Value = null, Name = "请选择商品" });
            HashName.ValueMember = nameof(SelectItem.Value);
            HashName.DisplayMember = nameof(SelectItem.Name);

            SteamId.DataSource = steamUsers;
            SteamId.ValueMember = nameof(SelectItem.Value);
            SteamId.DisplayMember = nameof(SelectItem.Name);

            MaxPrice.ValueType = typeof(decimal);
            QuerySize.ValueType = typeof(int);
            BuySize.ValueType = typeof(int);
            Interval.ValueType = typeof(int);

            LoadSetting();
        }

        private void reloadBtn_Click(object sender, EventArgs e)
        {
            LoadSetting();
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in autoBuyList.Rows)
            {
                row.Cells[nameof(GoodsEnabled)].Value = !((bool?)row.Cells[nameof(GoodsEnabled)].Value ?? false);
            }
        }

        private void autoBuyList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var row = autoBuyList.Rows[e.RowIndex];
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < autoBuyList.Rows.Count; i++)
                {
                    autoBuyList.Rows[i].Selected = false;
                }
                row.Selected = true;
            }

            if (e.Button == MouseButtons.Left)
            {
                var gameId = row.Cells[nameof(GameId)].Value?.ToString();

                var column = autoBuyList.Columns[e.ColumnIndex].Name;
                switch (column)
                {
                    case nameof(HashName):
                        {
                            if (string.IsNullOrWhiteSpace(gameId))
                            {
                                break;
                            }

                            var cell = row.Cells[e.ColumnIndex];
                            string hashName = row.Cells[nameof(HashName)].Value?.ToString();

                            List<SelectItem> defaultDatas = new List<SelectItem>();
                            SelectItem defaultValue = null;
                            if (row.Tag is AutoBuyGoods buyGoods)
                            {
                                defaultValue = new SelectItem { Name = buyGoods.Name, Value = buyGoods.HashName };
                            }

                            if (defaultValue != null)
                            {
                                defaultDatas.Add(defaultValue);
                            }

                            var goodsSearch = new Search("商品查询", "请输入关键词进行查询", defaultValue, defaultDatas, async (key) =>
                            {
                                if (string.IsNullOrWhiteSpace(key))
                                {
                                    return defaultDatas;
                                }

                                var searchResult = await EcoApi.KeyWordSearchAsync(client, gameId, key);
                                if (searchResult == null || searchResult.StatusCode != "0" || searchResult.StatusData.ResultCode != "0")
                                {
                                    MessageBox.Show("提示", searchResult?.StatusData?.ResultMsg ?? searchResult?.StatusMsg ?? "查询失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return new List<SelectItem>();
                                }

                                return searchResult.StatusData.ResultData.Select(c => new SelectItem { Name = c.GoodsName, Value = c.HashName });
                            });

                            if (goodsSearch.ShowDialog() != DialogResult.OK)
                            {
                                break;
                            }
                            var goods = goodsSearch.Value;
                            if (goods == null)
                            {
                                break;
                            }

                            if (!HashName.Items.Contains(goods))
                            {
                                HashName.Items.Add(goods);
                            }

                            row.Cells[nameof(HashName)].Value = goods.Value;
                        }
                        break;

                    case nameof(RunTimes):
                        {
                            List<(DateTime Strat, DateTime End)> values = new List<(DateTime Strat, DateTime End)>();
                            var defaultStart = new DateTime(1970, 1, 1, 0, 0, 0);
                            var defaultEnd = new DateTime(1970, 1, 1, 0, 0, 0);
                            if (row.Cells[nameof(RunTimes)].Tag is List<TimeRange> timeRanges && timeRanges.Any())
                            {
                                values = timeRanges.Select(c => (new DateTime(1970, 1, 1, c.Start.Hour, c.Start.Minute, c.Start.Second), new DateTime(1970, 1, 1, c.End.Hour, c.End.Minute, c.End.Second))).ToList();

                                defaultStart = values[0].Strat;
                                defaultEnd = values[0].End;
                            }

                            var timePicker = new TimePicker("HH:mm:ss", defaultStart, defaultEnd, values);
                            if (timePicker.ShowDialog() != DialogResult.OK)
                            {
                                break;
                            }

                            var vaules = timePicker.Values;

                            timeRanges = values.Select(c => new TimeRange
                            {
                                Start = new TimeOnly(c.Strat.Hour, c.Strat.Minute, c.Strat.Second),
                                End = new TimeOnly(c.End.Hour, c.End.Minute, c.End.Second)
                            }).ToList();
                            row.Cells[nameof(RunTimes)].Tag = timeRanges;
                            row.Cells[nameof(RunTimes)].Value = string.Join(" ; ", timeRanges.Select(c => $"{c.Start:HH:mm:ss}-{c.End:HH:mm:ss}"));
                        }
                        break;
                }
            }
        }

        private void autoBuyList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (autoBuyList.Columns[e.ColumnIndex].Name == nameof(MaxPrice))
            {
                MessageBox.Show("请输入正确购买价格", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (autoBuyList.Columns[e.ColumnIndex].Name == nameof(BuySize))
            {
                MessageBox.Show("请输入正确单次下单数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (autoBuyList.Columns[e.ColumnIndex].Name == nameof(Interval))
            {
                MessageBox.Show("请输入正确扫货频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void autoBuyList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            bool compareValue = e.Column.ValueType == typeof(int) || e.Column.ValueType == typeof(long) || e.Column.ValueType == typeof(decimal) || e.Column.ValueType == typeof(double) || e.Column.ValueType == typeof(float) || e.Column.ValueType == typeof(DateTime) || e.Column.ValueType == typeof(TimeOnly);
            if (compareValue)
            {
                e.SortResult = DataSortUtils.Compare(e.Column.ValueType, e.CellValue1, e.CellValue2);
            }
            else if (autoBuyList.Rows[e.RowIndex1].Cells[e.Column.Name].Tag != null || autoBuyList.Rows[e.RowIndex2].Cells[e.Column.Name].Tag != null)
            {
                var value1 = autoBuyList.Rows[e.RowIndex1].Cells[e.Column.Name].Tag;
                var value2 = autoBuyList.Rows[e.RowIndex2].Cells[e.Column.Name].Tag;
                var type = value1 != null ? value1.GetType() : value2.GetType();

                e.SortResult = DataSortUtils.Compare(type, value1, value2);
            }
            else
            {
                e.SortResult = DataSortUtils.Compare(e.Column.ValueType, e.CellValue1, e.CellValue2);
            }

            if (e.SortResult == 0)
            {
                e.SortResult = string.Compare(autoBuyList.Rows[e.RowIndex1].Cells[nameof(HashName)].Value?.ToString(), autoBuyList.Rows[e.RowIndex2].Cells[nameof(HashName)].Value?.ToString());
            }

            e.Handled = true;
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
                row.Cells[nameof(Interval)].Value = "500";

                if (steamUsers.Any())
                {
                    row.Cells[nameof(SteamId)].Value = steamUsers[0].Value;
                }

                row.ContextMenuStrip = contextMenuStrip;

                row.Cells[nameof(HashName)].ReadOnly = true;
            }
            finally
            {
                addGoodsBtn.Enabled = true;
            }
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                saveBtn.Enabled = false;

                if (autoBuyList.CurrentCell != null)
                {
                    if (!autoBuyList.EndEdit())
                    {
                        return;
                    }
                }

                List<AutoBuyGoods> goodsList = new List<AutoBuyGoods>();
                AutoBuyGoods goods;
                foreach (DataGridViewRow row in autoBuyList.Rows)
                {
                    if (!decimal.TryParse(row.Cells[nameof(MaxPrice)].Value?.ToString(), out decimal maxPrice))
                    {
                        MessageBox.Show("请输入正确的价格", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(row.Cells[nameof(QuerySize)].Value?.ToString(), out int querySize))
                    {
                        MessageBox.Show("请输入正确单次查询数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(row.Cells[nameof(BuySize)].Value?.ToString(), out int buySize))
                    {
                        MessageBox.Show("请输入正确单次下单数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(row.Cells[nameof(Interval)].Value?.ToString(), out int interval))
                    {
                        MessageBox.Show("请输入正确扫货频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var runTimes = row.Cells[nameof(RunTimes)].Tag as List<TimeRange> ?? new List<TimeRange>();

                    goods = new AutoBuyGoods
                    {
                        GameId = row.Cells[nameof(GameId)].Value?.ToString(),
                        HashName = row.Cells[nameof(HashName)].Value?.ToString(),
                        Name = row.Cells[nameof(HashName)].Value?.ToString(),
                        SteamId = row.Cells[nameof(SteamId)].Value?.ToString(),

                        MaxPrice = maxPrice,
                        QuerySize= querySize,
                        BuySize = buySize,

                        Interval = interval,

                        RunTimes = runTimes,

                        PayType = (Model.ECO.PayType)row.Cells[nameof(PayType)].Value,

                        NotifyAddress = row.Cells[nameof(NotifyAddress)].Value?.ToString(),
                        Enabled = (bool?)row.Cells[nameof(GoodsEnabled)].Value ?? false,
                    };

                    if (goodsList.Any(c => c.GameId == goods.GameId && c.HashName == goods.HashName))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(goods.GameId) || string.IsNullOrWhiteSpace(goods.HashName))
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(goods.SteamId))
                    {
                        continue;
                    }

                    var goodsDetailResponse = await EcoApi.QueryGoodsDetailAsync(goods.GameId, goods.HashName);
                    var goodsDetail = goodsDetailResponse?.StatusData?.ResultData;
                    if (goodsDetail == null)
                    {
                        string msg = $"在ECO市场未找到商品{goods.HashName}{Environment.NewLine}请确认商品名称是否正确";

                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    goods.Name = goodsDetail.GoodsName;

                    msgBox.Text = $"正在保存 {goods.Name}";

                    if (client.User.BuyGoods.Any(c => c.GameId == goods.GameId && c.HashName == goods.HashName))
                    {
                        int index = client.User.BuyGoods.FindIndex(c => c.GameId == goods.GameId && c.HashName == goods.HashName);
                        client.User.BuyGoods.RemoveAt(index);
                        client.User.BuyGoods.Insert(index, goods);
                    }
                    else
                    {
                        client.User.BuyGoods.Add(goods);
                    }

                    goodsList.Add(goods);

                    Appsetting.Instance.Manifest.SaveEcoUser(client.User.UserId, client.User);

                    await Task.Delay(500);
                }

                client.User.BuyGoods.RemoveAll(c => !goodsList.Any(g => c.GameId == g.GameId && c.HashName == g.HashName));
                Appsetting.Instance.Manifest.SaveEcoUser(client.User.UserId, client.User);

            }
            finally
            {
                msgBox.Text = "";
                saveBtn.Enabled = true;
            }
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
            autoBuyList.CancelEdit();
            autoBuyList.Rows.Clear();

            foreach (var goods in client.User.BuyGoods)
            {
                if (!HashName.Items.Contains(new SelectItem { Value = goods.HashName, Name = goods.Name }))
                {
                    HashName.Items.Add(new SelectItem { Value = goods.HashName, Name = goods.Name });
                }

                int index = autoBuyList.Rows.Add();
                var row = autoBuyList.Rows[index];
                row.Tag = goods;

                row.Cells[nameof(GameId)].Value = goods.GameId;
                row.Cells[nameof(HashName)].Value = goods.HashName;
                row.Cells[nameof(MaxPrice)].Value = goods.MaxPrice.ToString();
                row.Cells[nameof(QuerySize)].Value = goods.QuerySize.ToString();
                row.Cells[nameof(BuySize)].Value = goods.BuySize.ToString();
                row.Cells[nameof(Interval)].Value = goods.Interval.ToString();
                row.Cells[nameof(PayType)].Value = goods.PayType;
                row.Cells[nameof(SteamId)].Value = goods.SteamId;
                row.Cells[nameof(NotifyAddress)].Value = goods.NotifyAddress;
                row.Cells[nameof(GoodsEnabled)].Value = goods.Enabled;

                row.Cells[nameof(RunTimes)].Value = string.Join(" ; ", goods.RunTimes.OrderBy(c => c.Start).Select(c => $"{c.Start:HH:mm:ss}-{c.End:HH:mm:ss}"));
                row.Cells[nameof(RunTimes)].Tag = goods.RunTimes;

                row.Cells[nameof(GameId)].ReadOnly = true;
                row.Cells[nameof(HashName)].ReadOnly = true;

                row.ContextMenuStrip = contextMenuStrip;
            }

            timer.Change(1000, 3000);
        }

        private void AutoRefresh(object obj)
        {
            try
            {
                foreach (DataGridViewRow row in autoBuyList.Rows)
                {
                    string gameId = row.Cells[nameof(GameId)].Value?.ToString();
                    string hashName = row.Cells[nameof(HashName)].Value?.ToString();

                    var goods = client.User.BuyGoods.FirstOrDefault(c => c.GameId == gameId && c.HashName == hashName);
                    var currentPrice = goods?.GetCurrentPrice();
                    if (currentPrice == null)
                    {
                        continue;
                    }

                    row.Cells[nameof(CurrentPrice)].Value = $"[{currentPrice.Value.Time:HH:mm:ss}]  ￥{currentPrice.Value.Price:f2}";
                    row.Cells[nameof(CurrentPrice)].Tag = currentPrice.Value.Price;
                }
            }
            catch
            {

            }
            finally
            {
                timer.Change(1000, 3000);
            }
        }
    }
}

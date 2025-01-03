using QRCoder;
using Steam_Authenticator.Internal;
using Steam_Authenticator.Model;
using Steam_Authenticator.Model.ECO;

namespace Steam_Authenticator.Forms
{
    public partial class EcoOrders : Form
    {
        private readonly EcoClient client;
        private readonly List<SelectItem> games;
        private readonly List<SelectItem> payTypes;

        public EcoOrders(EcoClient client)
        {
            InitializeComponent();

            this.client = client;

            games = new List<SelectItem> { new SelectItem { Value = "730", Name = "CS2" } };
            payTypes = new List<SelectItem> { new SelectItem { Value = PayType.None, Name = "支付方式" }, new SelectItem { Value = PayType.支付宝, Name = "支付宝" }, new SelectItem { Value = PayType.余额, Name = "余额" } };
        }

        private async void EcoOrders_Load(object sender, EventArgs e)
        {
            payTypeBox.DataSource = payTypes;
            payTypeBox.ValueMember = nameof(SelectItem.Value);
            payTypeBox.DisplayMember = nameof(SelectItem.Name);
            payTypeBox.SelectedIndex = 1;

            GameId.DataSource = games;
            GameId.ValueMember = nameof(SelectItem.Value);
            GameId.DisplayMember = nameof(SelectItem.Name);

            Price.ValueType = typeof(decimal);
            FloatValue.ValueType = typeof(decimal);

            await Reload();
        }

        private void ecoOrdersList_Leave(object sender, EventArgs e)
        {
            if (ecoOrdersList.CurrentCell != null)
            {
                ecoOrdersList.EndEdit();
            }
        }


        private void ecoOrdersList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            bool compareValue = e.Column.ValueType == typeof(int) || e.Column.ValueType == typeof(long) || e.Column.ValueType == typeof(decimal) || e.Column.ValueType == typeof(double) || e.Column.ValueType == typeof(float) || e.Column.ValueType == typeof(DateTime) || e.Column.ValueType == typeof(TimeOnly);
            if (compareValue)
            {
                e.SortResult = DataSortUtils.Compare(e.Column.ValueType, e.CellValue1, e.CellValue2);
            }
            else if (ecoOrdersList.Rows[e.RowIndex1].Cells[e.Column.Name].Tag != null || ecoOrdersList.Rows[e.RowIndex2].Cells[e.Column.Name].Tag != null)
            {
                var value1 = ecoOrdersList.Rows[e.RowIndex1].Cells[e.Column.Name].Tag;
                var value2 = ecoOrdersList.Rows[e.RowIndex2].Cells[e.Column.Name].Tag;
                var type = value1 != null ? value1.GetType() : value2.GetType();

                e.SortResult = DataSortUtils.Compare(type, value1, value2);
            }
            else
            {
                e.SortResult = DataSortUtils.Compare(e.Column.ValueType, e.CellValue1, e.CellValue2);
            }

            if (e.SortResult == 0)
            {
                e.SortResult = string.Compare(ecoOrdersList.Rows[e.RowIndex1].Cells[nameof(GoodsName)].Value?.ToString(), ecoOrdersList.Rows[e.RowIndex2].Cells[nameof(GoodsName)].Value?.ToString());
            }

            e.Handled = true;
        }

        private async void refreshBtn_Click(object sender, EventArgs e)
        {
            await Reload();
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in ecoOrdersList.Rows)
            {
                row.Cells[nameof(IsChecked)].Value = !((bool?)row.Cells[nameof(IsChecked)].Value ?? false);
            }
        }

        private async void payBtn_Click(object sender, EventArgs e)
        {
            var selectPayType = payTypeBox.SelectedItem as SelectItem;
            if (selectPayType == null || (PayType)selectPayType.Value == PayType.None)
            {
                MessageBox.Show("请选择支付方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var payTpe = (PayType)selectPayType.Value;

            if (ecoOrdersList.CurrentCell != null)
            {
                ecoOrdersList.EndEdit();
            }

            List<QueryOrdersResponse> orders = new List<QueryOrdersResponse>();
            foreach (DataGridViewRow row in ecoOrdersList.Rows)
            {
                bool isChecked = (bool?)row.Cells[nameof(IsChecked)].Value ?? false;
                if (!isChecked)
                {
                    continue;
                }

                if (row.Tag is QueryOrdersResponse order && order.State == OrderState.待付款)
                {
                    orders.Add(order);
                }
            }

            if (!orders.Any())
            {
                MessageBox.Show("请选择要支付的订单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var payResult = await EcoApi.PayOrdersAsync(client, orders.Select(c => c.OrderNo), payTpe);
            if (payResult.StatusCode != "0")
            {
                MessageBox.Show(payResult.StatusMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (payResult.StatusData.ResultCode != "0")
            {
                MessageBox.Show(payResult.StatusData.ResultMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (payTpe == PayType.余额)
            {
                MessageBox.Show(payResult.StatusData.ResultMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string payUrl = payResult.StatusData.ResultData.Action;
            var qr = new QrBox("请扫码进行支付", (qrBox) =>
            {
                using (var qrGenerator = new QRCodeGenerator())
                {
                    using (var qrCodeData = qrGenerator.CreateQrCode(payUrl, QRCodeGenerator.ECCLevel.L))
                    {
                        using (var qrCode = new PngByteQRCode(qrCodeData))
                        {
                            var qrCodeBuffer = qrCode.GetGraphic(10, drawQuietZones: false);
                            qrBox.Image = Image.FromStream(new MemoryStream(qrCodeBuffer));
                        }
                    }
                }
            });
            qr.ShowDialog();

            await Reload();
        }

        public async Task Reload()
        {
            string gameId = "730";

            var ordersResponse = await EcoApi.QueryWaitPayOrdersAsync(client, gameId);
            if (ordersResponse.StatusCode != "0")
            {
                MessageBox.Show(ordersResponse.StatusMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ordersResponse.StatusData.ResultCode != "0")
            {
                MessageBox.Show(ordersResponse.StatusData.ResultMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ecoOrdersList.Rows.Clear();
            var orders = ordersResponse.StatusData.ResultData.PageResult;
            List<QueryAssetResponse> assets = new List<QueryAssetResponse>();
            if (orders.Any())
            {
                var assetIds = orders.Select(c => c.AssetId);
                var assetsResponse = await EcoApi.QueryAssetAsync(gameId, assetIds);
                assets = assetsResponse?.StatusData?.ResultData ?? new List<QueryAssetResponse>();
            }

            foreach (var order in orders)
            {
                int index = ecoOrdersList.Rows.Add();
                var row = ecoOrdersList.Rows[index];
                row.Tag = order;

                var asset = assets.FirstOrDefault(c => c.AssetId == order.AssetId);

                row.Cells[nameof(GameId)].Value = gameId;
                row.Cells[nameof(GoodsName)].Value = order.GoodsName;
                row.Cells[nameof(FloatValue)].Value = asset?.PaintWear ?? "0";
                row.Cells[nameof(Price)].Value = order.TotalMoney;
                row.Cells[nameof(IsChecked)].Value = false;
            }
        }
    }
}

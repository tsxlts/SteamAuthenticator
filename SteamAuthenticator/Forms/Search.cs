
using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class Search : Form
    {
        private readonly SelectItem defaultValue;
        private readonly IEnumerable<SelectItem> defaultDatas;
        private readonly Func<string, Task<IEnumerable<SelectItem>>> dataFactory;

        public Search(string title, string tips, SelectItem defaultValue, IEnumerable<SelectItem> defaultDatas, Func<string, Task<IEnumerable<SelectItem>>> dataFactory)
        {
            InitializeComponent();

            this.defaultValue = defaultValue;
            this.defaultDatas = defaultDatas;
            this.Text = title;
            this.tips.Text = tips;
            this.dataFactory = dataFactory;

            searchBox.ValueMember = nameof(SelectItem.Value);
            searchBox.DisplayMember = nameof(SelectItem.Name);
        }

        private void Search_Load(object sender, EventArgs e)
        {
            searchBox.Items.AddRange(defaultDatas.ToArray());
            searchBox.SelectedItem = defaultValue;

            searchBox.TextChanged += searchBox_TextChanged;
        }

        private async void searchBox_TextChanged(object sender, EventArgs e)
        {
            searchBox.TextChanged -= searchBox_TextChanged;

            await Task.Delay(300);

            string key = searchBox.Text;
            var data = await dataFactory.Invoke(key);

            searchBox.Items.Clear();
            searchBox.Items.Add(new SelectItem { Name = "", Value = "" });
            searchBox.Items.AddRange(data.ToArray());

            searchBox.SelectionStart = searchBox.Text.Length;
            searchBox.IntegralHeight = true;
            searchBox.DroppedDown = true;

            searchBox.TextChanged += searchBox_TextChanged;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        public SelectItem Value { get { return searchBox.SelectedItem as SelectItem; } }
    }
}

using SteamKit.Model;
using System.Text.RegularExpressions;

namespace Steam_Authenticator.Forms
{
    public partial class PhoneInput : Form
    {
        private readonly string title;
        private readonly string tips;
        public PhoneInput(string title, string tips)
        {
            InitializeComponent();

            this.title = title;
            this.tips = tips;
        }

        private async void Input_Load(object sender, EventArgs e)
        {
            Text = title;
            TipsLabel.Text = tips;
            PhoneBox.Text = Phone;

            CountryBox.DisplayMember = nameof(SteamKit.Model.Country.Name);
            CountryBox.ValueMember = nameof(SteamKit.Model.Country.CountryCode);
            await LoadCountry();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Phone))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            Phone = PhoneBox.Text;
            if (Phone[0] != '+')
            {
                MessageBox.Show("电话号码必须以+和国家代码开头。", "手机号", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CountryCode = (CountryBox.SelectedItem as Country)?.CountryCode;

            DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CountryBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            var regex = new Regex(@"[^a-zA-Z]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void PhoneBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            var regex = new Regex(@"[^0-9\s\+]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private async Task LoadCountry()
        {
            int selectIndex = 0;

            List<Country> list = new List<Country>();
            var countryResponse = await SteamKit.SteamApi.QueryCountryAsync();
            var countries = countryResponse.Body?.Countries ?? new List<Country> { new Country { Name = "中国", CountryCode = "CN" } };
            countries = countries.OrderBy(x => x.Name).ToList();
            for (int index = 0; index < countries.Count; index++)
            {
                var country = countries[index];
                if (country.CountryCode == CountryCode)
                {
                    selectIndex = index;
                }

                list.Add(new Country
                {
                    Name = $"{country.Name} ({country.CountryCode})",
                    CountryCode = country.CountryCode
                });
            }
            CountryBox.DataSource = list;
            CountryBox.SelectedIndex = selectIndex;
        }

        public string Phone { get; set; }

        public string CountryCode { get; set; } = "CN";
    }
}

using Steam_Authenticator.Model;

namespace Steam_Authenticator.Forms
{
    public partial class Options : Form
    {
        public Options(string title, string tips)
        {
            InitializeComponent();

            Text = title;
            msg.Text = tips;
            selectAllBtn.Tag = false;

            Selected = new List<SelectOption>();
        }

        public List<SelectOption> Datas { get; set; }

        public bool Multiselect { get; set; }

        public Size ItemSize { get; set; } = new Size(100, 20);

        public List<SelectOption> Selected { get; init; }

        private void Options_Load(object sender, EventArgs e)
        {
            int x = 10;
            int y = 10;
            foreach (var item in Datas)
            {
                if (x + ItemSize.Width > dataPanel.Width)
                {
                    x = 10;
                    y = y + ItemSize.Height + 10;
                }

                CheckBox checkBox = new CheckBox
                {
                    Name = item.Value,
                    AutoSize = false,
                    AutoEllipsis = true,
                    Size = ItemSize,
                    Text = item.Text,
                    Tag = item,
                    ForeColor = Color.Gray,
                    Location = new Point(x, y),
                };
                checkBox.CheckedChanged += (sender, e) =>
                {
                    checkBox.ForeColor = checkBox.Checked ? Color.Green : Color.Gray;

                    if (!Multiselect)
                    {
                        foreach (var item in dataPanel.Controls)
                        {
                            if (!(item is CheckBox other) || other == checkBox)
                            {
                                continue;
                            }

                            other.Enabled = !checkBox.Checked;
                        }
                    }
                };
                dataPanel.Controls.Add(checkBox);

                x = checkBox.Location.X + ItemSize.Width + 10;
            }

            foreach (var item in Datas.Where(c => c.Checked))
            {
                var checkBox = dataPanel.Controls.Find(item.Value, false)?.FirstOrDefault() as CheckBox;
                if (checkBox == null)
                {
                    continue;
                }

                checkBox.Checked = item.Checked;
            }
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            try
            {
                selectAllBtn.Enabled = false;

                bool selectAll = !(bool)selectAllBtn.Tag;
                selectAllBtn.Text = selectAll ? "取消选择" : "全选";
                selectAllBtn.Tag = selectAll;

                foreach (var item in dataPanel.Controls)
                {
                    if (!(item is CheckBox checkBox))
                    {
                        continue;
                    }

                    checkBox.Checked = selectAll;
                }
            }
            finally
            {
                selectAllBtn.Enabled = true;
            }
        }

        private void reverseSelectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                reverseSelectBtn.Enabled = false;

                selectAllBtn.Text = "全选";
                selectAllBtn.Tag = false;

                foreach (var item in dataPanel.Controls)
                {
                    if (!(item is CheckBox checkBox))
                    {
                        continue;
                    }

                    checkBox.Checked = !checkBox.Checked;
                }
            }
            finally
            {
                reverseSelectBtn.Enabled = true;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                okBtn.Enabled = false;

                Selected.Clear();

                foreach (var item in dataPanel.Controls)
                {
                    if (!(item is CheckBox checkBox) || !checkBox.Checked)
                    {
                        continue;
                    }

                    Selected.Add((SelectOption)checkBox.Tag);
                }

                DialogResult = DialogResult.OK;
            }
            finally
            {
                okBtn.Enabled = true;
            }
        }
    }
}

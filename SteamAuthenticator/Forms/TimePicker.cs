
namespace Steam_Authenticator.Forms
{
    public partial class TimePicker : Form
    {
        public readonly List<(DateTime Strat, DateTime End)> Values;

        public TimePicker(string format, DateTime defaultStart, DateTime defaultEnd, List<(DateTime Strat, DateTime End)> values)
        {
            InitializeComponent();

            startTime.Value = defaultStart;
            startTime.CustomFormat = format;

            endTime.Value = defaultEnd;
            endTime.CustomFormat = format;

            Values = values ?? new List<(DateTime Strat, DateTime End)>();
        }

        private void TimePicker_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            Values.Add((startTime.Value, endTime.Value));
            Reload();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Reload()
        {
            timesBox.Controls.Clear();

            int y = 0;
            foreach (var item in Values.OrderBy(c => c.Strat))
            {
                timesBox.Controls.Add(new Label
                {
                    Text = $"{item.Strat.ToString(startTime.CustomFormat)} - {item.End.ToString(endTime.CustomFormat)}",
                    AutoSize = false,
                    Size = new Size(300, 25),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(0, y),
                });

                var deleteBtn = new Label
                {
                    Text = $"删除",
                    AutoSize = false,
                    Size = new Size(40, 25),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(335, y),
                    Cursor = Cursors.Hand,
                    ForeColor = Color.Gray,
                };
                deleteBtn.Click += (object sender, EventArgs e) =>
                {
                    Values.Remove(item);
                    Reload();
                };
                timesBox.Controls.Add(deleteBtn);

                y = y + 25;
            }
        }
    }
}

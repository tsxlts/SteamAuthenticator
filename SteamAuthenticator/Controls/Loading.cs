
namespace Steam_Authenticator.Controls
{
    public class Loading : IDisposable
    {
        public readonly Panel loadingPanel;

        public Loading(Control parent)
        {
            loadingPanel = new Panel
            {
                Width = parent.Width,
                Height = parent.Height,
                Location = new Point(0, 0),
            };
            loadingPanel.Controls.Add(new PictureBox
            {
                Image = Properties.Resources.loading,
                Location = new Point((loadingPanel.Width - 120) / 2, (loadingPanel.Height - 120) / 2),
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom
            });

            parent.Controls.Add(loadingPanel);
            loadingPanel.BringToFront();
        }

        public void Dispose()
        {
            loadingPanel.Hide();
            loadingPanel.Dispose();
        }
    }
}

using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Controls
{
    internal class ReadonlyRichTextBox : RichTextBox
    {
        private bool readOnly = true;

        public new bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = true; }
        }

        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);

            WindowsApi.HideCaret(Handle);
        }
    }
}

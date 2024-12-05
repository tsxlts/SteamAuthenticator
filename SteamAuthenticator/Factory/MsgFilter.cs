using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Factory
{
    internal class MsgFilter : IMessageFilter
    {
        private readonly MainForm main;

        public MsgFilter(MainForm main)
        {
            this.main = main;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WindowsApi.MsgStrat + 1)
            {
                main?.ShowForm();
                return true;
            }
            return false;
        }
    }
}

using System.Runtime.InteropServices;

namespace Steam_Authenticator.Internal
{
    internal static class WindowsApi
    {
        public const uint MsgStrat = 0x8001;
        public const uint MsgEnd = 0xBFFF;

        public const int WM_SHOWWINDOW = 0x0018;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(int threadId, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "HideCaret")]
        public static extern bool HideCaret(IntPtr hWnd);

    }
}

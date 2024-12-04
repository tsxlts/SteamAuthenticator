using System.Runtime.InteropServices;

namespace Steam_Authenticator.Internal
{
    internal static class WindowsApi
    {
        public const int WM_SHOWWINDOW = 0x0018;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}

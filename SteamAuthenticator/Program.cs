using Steam_Authenticator.Forms;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using static SteamKit.SteamBulider;

namespace Steam_Authenticator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex instance = new Mutex(true, "sda", out bool createdNew))
            {
                ApplicationConfiguration.Initialize();

                if (!createdNew)
                {
                    var processs = PriorProcess();
                    if (processs == null)
                    {
                        MessageBox.Show("已经有另外一个程序在运行中");
                        return;
                    }

                    ShowWindowAsync(processs.MainWindowHandle, 1);
                    SetForegroundWindow(processs.MainWindowHandle);

                    Application.Exit();
                    return;
                }

                WithProxy((s, m) =>
                {
                    var setting = Appsetting.Instance.AppSetting.Entry;
                    var proxy = Proxy.Instance
                    .WithSteamCommunity(setting.Domain.SteamCommunity)
                    .WithSteamApi(setting.Domain.SteamApi)
                    .WithSteamStore(setting.Domain.SteamPowered)
                    .WithSteamLogin(setting.Domain.SteamLogin);

                    if (!string.IsNullOrWhiteSpace(setting.Proxy?.Address) || !string.IsNullOrWhiteSpace(setting.Proxy?.Host))
                    {
                        if (!string.IsNullOrWhiteSpace(setting.Proxy?.Address))
                        {
                            proxy.WithProxy(new WebProxy(new Uri(setting.Proxy.Address)));
                        }
                        else
                        {
                            proxy.WithProxy(new WebProxy(setting.Proxy.Host, setting.Proxy.Port));
                        }
                    }

                    return proxy;
                });

                if (Appsetting.Instance.Manifest.Encrypted)
                {
                    string tips = "请输入访问密码";
                    Input input;
                    while (true)
                    {
                        input = new Input("访问密码", tips, true);
                        if (input.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        string password = input.InputValue;
                        if (!Appsetting.Instance.Manifest.CheckPassword(password))
                        {
                            tips = "访问密码错误，请重新输入";
                            continue;
                        }

                        Appsetting.Instance.AppSetting.Password = password;
                        break;
                    }
                }

                Application.Run(new MainForm());
                instance.ReleaseMutex();
            }
        }

        public static Process PriorProcess()
        {
            try
            {
                Process curr = Process.GetCurrentProcess();
                Process[] procs = Process.GetProcessesByName(curr.ProcessName);
                foreach (Process p in procs)
                {
                    if (p.Id != curr.Id && p.MainModule.FileName == curr.MainModule.FileName)
                    {
                        return p;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);

    }
}
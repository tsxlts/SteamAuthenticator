using Steam_Authenticator.Factory;
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
                    var proxy = Proxy.Instance;
                    if (!string.IsNullOrWhiteSpace(setting.Domain.SteamCommunity))
                    {
                        proxy.WithSteamCommunity(setting.Domain.SteamCommunity);
                    }
                    if (!string.IsNullOrWhiteSpace(setting.Domain.SteamApi))
                    {
                        proxy.WithSteamApi(setting.Domain.SteamApi);
                    }
                    if (!string.IsNullOrWhiteSpace(setting.Domain.SteamPowered))
                    {
                        proxy.WithSteamStore(setting.Domain.SteamPowered);
                    }
                    if (!string.IsNullOrWhiteSpace(setting.Domain.SteamLogin))
                    {
                        proxy.WithSteamLogin(setting.Domain.SteamLogin);
                    }

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

                if (Appsetting.Instance.AppSetting.Entry.FirstUsed)
                {
                    Welcome welcome = new Welcome();
                    if (welcome.ShowDialog() != DialogResult.Continue)
                    {
                        return;
                    }

                    Appsetting.Instance.AppSetting.Entry.FirstUsed = false;
                    Appsetting.Instance.AppSetting.Save();

                    var dialogResult = MessageBox.Show($"为了你的帐号安全，建议你设置访问密码" +
                         $"{Environment.NewLine}" +
                         $"是否立即设置访问密码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.Yes)
                    {
                        goto Run;
                    }

                    Input input = new Input("设置密码", $"请输入访问密码" +
                        $"{Environment.NewLine}" +
                        $"如果你不需要设置密码，则不需要输入任何文本，直接点击确定即可", password: true);
                    if (input.ShowDialog() != DialogResult.OK)
                    {
                        goto Run;
                    }

                    string password = input.InputValue;
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        goto Run;
                    }

                    while (true)
                    {
                        input = new Input("设置密码", "请再次确认访问密码", password: true, required: true, errorMsg: "请输入密码");
                        if (input.ShowDialog() != DialogResult.OK)
                        {
                            goto Run;
                        }

                        if (password != input.InputValue)
                        {
                            MessageBox.Show("两次密码不一致", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        Appsetting.Instance.Manifest.ChangePassword("", password);
                        Appsetting.Instance.AppSetting.Password = password;
                        MessageBox.Show("密码设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }

                    goto Run;
                }

                if (Appsetting.Instance.Manifest.Encrypted)
                {
                    string tips = "请输入访问密码";
                    Input input;
                    while (true)
                    {
                        input = new Input("访问密码", tips, password: true, required: true, errorMsg: "请输入密码");
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

            Run:
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.Run(new MainForm());
                instance.ReleaseMutex();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppLogger.Instance.Error(e.Exception);

            MessageBox.Show($"程序发生未知异常{Environment.NewLine}{e.Exception.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
            {
                return;
            }

            AppLogger.Instance.Error(exception);

            MessageBox.Show($"程序发生未知异常{Environment.NewLine}{exception.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
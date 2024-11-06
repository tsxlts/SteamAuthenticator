using SteamKit;
using System.Text;

namespace Steam_Authenticator.Factory
{
    internal class AppLogger
    {
        public static readonly AppLogger Instance;

        static AppLogger()
        {
            Instance = new AppLogger();
        }

        private readonly string logPath;
        private readonly AsyncLocker asyncLocker;

        private AppLogger()
        {
            logPath = Path.Combine(AppContext.BaseDirectory, "logs");
            asyncLocker = new AsyncLocker();

            Directory.CreateDirectory(logPath);
        }

        public void Error(Exception exception)
        {
            try
            {
                int num = 1;
                StringBuilder messageBuilder = new StringBuilder();
                StringBuilder stackTraceBuilder = new StringBuilder();
                do
                {
                    messageBuilder.Append($"{num}层: {exception.Message}");
                    messageBuilder.AppendLine();

                    stackTraceBuilder.Append($"{num}层: {exception.StackTrace}");
                    stackTraceBuilder.AppendLine();

                    exception = exception.InnerException;
                    num++;
                } while (exception != null);

                using (asyncLocker.Lock())
                {
                    string log = Path.Combine(logPath, $"errors-{DateTime.Now:yyyyMMddHH}.log");
                    File.AppendAllLines(log, new[] { $"********** {DateTime.Now:yyyy-MM-dd HH:mm:ss} **********" });
                    File.AppendAllLines(log, new[] { messageBuilder.ToString(), stackTraceBuilder.ToString() });
                }
            }
            catch
            {

            }
        }
    }
}

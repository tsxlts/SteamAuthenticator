using System.Text;
using SteamKit;

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
        private readonly string errorLogPath;
        private readonly string debugLogPath;
        private readonly AsyncLocker asyncLocker;

        private readonly int logRetentionTime = 24;
        private readonly System.Threading.Timer timer;

        private AppLogger()
        {
            logPath = Path.Combine(AppContext.BaseDirectory, "logs");
            errorLogPath = Path.Combine(logPath, "error");
            debugLogPath = Path.Combine(logPath, "debug");
            asyncLocker = new AsyncLocker();

            timer = new System.Threading.Timer((obj) =>
            {
                try
                {
                    var time = DateTime.Now.AddHours(-1 * logRetentionTime);
                    var files = Directory.GetFiles(logPath, "", SearchOption.AllDirectories);
                    Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = 3 }, file =>
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.CreationTime > time)
                        {
                            return;
                        }

                        fileInfo.Delete();
                    });
                }
                catch
                {

                }
            }, this, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10));
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

                using (asyncLocker.Lock(TimeSpan.FromSeconds(1)))
                {
                    DateTime current = DateTime.Now;
                    var directory = errorLogPath;
                    Directory.CreateDirectory(directory);

                    string log = Path.Combine(directory, $"{current:yyyyMMddHH}.log");
                    File.AppendAllLines(log, new[] { $"-------------------- {current:yyyy-MM-dd HH:mm:ss} --------------------" });
                    File.AppendAllLines(log, new[] { messageBuilder.ToString(), stackTraceBuilder.ToString() });
                }
            }
            catch
            {

            }
        }

        public void Debug(string type, string path, string msg)
        {
            try
            {
                using (asyncLocker.Lock(TimeSpan.FromSeconds(1)))
                {
                    DateTime current = DateTime.Now;
                    var directory = Path.Combine(debugLogPath, path, type);
                    Directory.CreateDirectory(directory);

                    string log = Path.Combine(directory, $"{current:yyyyMMdd-HHmm}.log");
                    File.AppendAllLines(log, new[] { $"-------------------- {current:yyyy-MM-dd HH:mm:ss} --------------------" });
                    File.AppendAllLines(log, new[] { msg });
                }
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }
    }
}

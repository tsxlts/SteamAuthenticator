using SteamKit.Factory;
using System.Collections.Concurrent;

namespace Steam_Authenticator.Internal
{
    public class Logger : ILogger
    {
        private readonly ConcurrentQueue<(DateTime time, Color color, string msg)> _logs = new ConcurrentQueue<(DateTime time, Color color, string msg)>();
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
        private readonly ListView logView;

        public Logger(ListView logView)
        {
            this.logView = logView;

            ShowLog().ConfigureAwait(false);
        }

        public void LogInformation(string format, params object[] args)
        {

        }

        public void LogError(Exception exception, string format, params object[] args)
        {

        }

        public void LogWarning(Exception exception, string format, params object[] args)
        {

        }

        public void LogDebug(string format, params object[] args)
        {
            _logs.Enqueue((DateTime.Now, Color.Green, $"{string.Format(format, args)}"));
            resetEvent.Set();
        }

        public void LogDebug(Exception exception, string format, params object[] args)
        {
            _logs.Enqueue((DateTime.Now, Color.Red, $"{string.Format(format, args)}"));
            resetEvent.Set();
        }

        private async Task ShowLog()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (!_logs.TryDequeue(out var log))
                        {
                            if (!resetEvent.WaitOne(TimeSpan.FromMilliseconds(1000)))
                            {
                                continue;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(log.msg))
                        {
                            continue;
                        }

                        if (logView.InvokeRequired)
                        {
                            logView.Invoke(new Action(() =>
                            {
                                if (logView.Items.Count > 100)
                                {
                                    logView.Items.RemoveAt(100);
                                }

                                ListViewItem lstItem = new ListViewItem(new[] { $"{log.time:yyyy/MM/dd HH:mm:ss}", log.msg })
                                {
                                    ForeColor = log.color,
                                };
                                logView.Items.Insert(0, lstItem);
                            }));
                        }
                        else
                        {
                            if (logView.Items.Count > 100)
                            {
                                logView.Items.RemoveAt(100);
                            }

                            ListViewItem lstItem = new ListViewItem(new[] { $"{log.time:yyyy/MM/dd HH:mm:ss}", log.msg })
                            {
                                ForeColor = log.color,
                            };
                            logView.Items.Insert(0, lstItem);
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }
    }
}

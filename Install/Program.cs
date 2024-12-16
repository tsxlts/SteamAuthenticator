using Newtonsoft.Json;

namespace Install
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            if (args.Length != 1)
            {
                Application.Exit();
                return;
            }

            RunParams runParams = null;

            try
            {
                runParams = JsonConvert.DeserializeObject<RunParams>(args[0]);
                ArgumentException.ThrowIfNullOrWhiteSpace(runParams?.InstallPath);
                ArgumentException.ThrowIfNullOrWhiteSpace(runParams?.InstallPackage);
            }
            catch
            {
                Application.Exit();
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Install(runParams));
        }
    }
}
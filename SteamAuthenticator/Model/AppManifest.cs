using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    internal class AppManifest
    {
        private readonly string guardPath = "sda/guard";
        private readonly string userPath = "sda/user";
        private readonly Manifest manifest;

        public AppManifest()
        {
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = Path.Combine(commonAppDataPath, "SA", "Manifest", "sda.manifest");
            manifest = Manifest.FromFile(file);
        }

        public void AddGuard(string accountName, Guard entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(guardPath, accountName, password, entry);
        }

        public bool RemoveGuard(string accountName, out Guard entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(guardPath, accountName, password, out entry);
        }

        public Guard GetGuard(string accountName)
        {
            string password = GetPassword();
            return manifest.GetEntry<Guard>(guardPath, accountName, password)?.Value;
        }

        public IEnumerable<string> GetGuards()
        {
            return manifest.GetEntries(guardPath);
        }

        public void AddUser(string steamId, User entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(userPath, steamId, password, entry);
        }

        public bool RemoveUser(string steamId, out User entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(userPath, steamId, password, out entry);
        }

        public User GetUser(string name)
        {
            string password = GetPassword();
            return manifest.GetEntry<User>(userPath, name, password)?.Value;
        }

        public IEnumerable<string> GetUsers()
        {
            return manifest.GetEntries(userPath);
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return manifest.ChangePassword(oldPassword, newPassword);
        }

        public bool CheckPassword(string password)
        {
            return manifest.CheckPassword(password);
        }

        private string GetPassword()
        {
            string password = Appsetting.Instance.AppSetting.Password;
            return password;
        }

        public bool Encrypted => manifest.Encrypted;
    }
}

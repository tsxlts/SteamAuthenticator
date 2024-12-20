using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    internal class AppManifest
    {
        private readonly string guardPath = "sda/guard";
        private readonly string userPath = "sda/user";
        private readonly string buffUserPath = "sda/buffUser";
        private readonly string ecoUserPath = "sda/ecoUser";
        private readonly Manifest manifest;

        public AppManifest()
        {
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = Path.Combine(commonAppDataPath, "SA", "Manifest", "sda.manifest");
            manifest = Manifest.FromFile(file);
        }

        #region Guard
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
        #endregion

        #region User
        public void SaveSteamUser(string steamId, User entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(userPath, steamId, password, entry);
        }

        public bool RemoveSteamUser(string steamId, out User entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(userPath, steamId, password, out entry);
        }

        public User GetSteamUser(string steamId)
        {
            string password = GetPassword();
            return manifest.GetEntry<User>(userPath, steamId, password)?.Value;
        }

        public IEnumerable<string> GetSteamUsers()
        {
            return manifest.GetEntries(userPath);
        }
        #endregion

        #region BuffUser
        public void SaveBuffUser(string userId, BuffUser entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(buffUserPath, userId, password, entry);
        }

        public bool RemoveBuffUser(string userId, out BuffUser entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(buffUserPath, userId, password, out entry);
        }

        public BuffUser GetBuffUser(string userId)
        {
            string password = GetPassword();
            return manifest.GetEntry<BuffUser>(buffUserPath, userId, password)?.Value;
        }

        public IEnumerable<string> GetBuffUser()
        {
            return manifest.GetEntries(buffUserPath);
        }
        #endregion

        #region EcoUser
        public void SaveEcoUser(string userId, EcoUser entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(ecoUserPath, userId, password, entry);
        }

        public bool RemoveEcoUser(string userId, out EcoUser entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(ecoUserPath, userId, password, out entry);
        }

        public EcoUser GetEcoUser(string userId)
        {
            string password = GetPassword();
            return manifest.GetEntry<EcoUser>(ecoUserPath, userId, password)?.Value;
        }

        public IEnumerable<string> GetEcoUser()
        {
            return manifest.GetEntries(ecoUserPath);
        }
        #endregion

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

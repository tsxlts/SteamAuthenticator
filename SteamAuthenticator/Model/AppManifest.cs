using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    internal class AppManifest
    {
        private readonly string guardPath = "sda/guard";
        private readonly string userPath = "sda/user";
        private readonly string buffUserPath = "sda/buffUser";
        private readonly string ecoUserPath = "sda/ecoUser";
        private readonly string youpinUserPath = "sda/youpinUser";
        private readonly string c5UserPath = "sda/c5User";
        private readonly Manifest manifest;

        public AppManifest()
        {
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = Path.Combine(commonAppDataPath, "SA", "Manifest", "sda.manifest");
            manifest = Manifest.FromFile(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changed"></param>
        /// <returns></returns>
        public void WithChanged(Action<object, ManifestChangedEventArgs> changed)
        {
            manifest.ManifestChanged += new ManifestChangedEventHandler(changed);
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
            return manifest.GetEntry<Guard>(guardPath, accountName, password);
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
            return manifest.GetEntry<User>(userPath, steamId, password);
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
            return manifest.GetEntry<BuffUser>(buffUserPath, userId, password);
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
            return manifest.GetEntry<EcoUser>(ecoUserPath, userId, password);
        }

        public IEnumerable<string> GetEcoUser()
        {
            return manifest.GetEntries(ecoUserPath);
        }
        #endregion

        #region YouPinUser
        public void SaveYouPinUser(string userId, YouPinUser entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(youpinUserPath, userId, password, entry);
        }

        public bool RemoveYouPinUser(string userId, out YouPinUser entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(youpinUserPath, userId, password, out entry);
        }

        public YouPinUser GetYouPinUser(string userId)
        {
            string password = GetPassword();
            return manifest.GetEntry<YouPinUser>(youpinUserPath, userId, password);
        }

        public IEnumerable<string> GetYouPinUser()
        {
            return manifest.GetEntries(youpinUserPath);
        }
        #endregion

        #region C5User
        public void SaveC5User(string userId, C5User entry)
        {
            string password = GetPassword();
            manifest.SaveEntry(c5UserPath, userId, password, entry);
        }

        public bool RemoveC5User(string userId, out C5User entry)
        {
            string password = GetPassword();
            return manifest.RemoveEntry(c5UserPath, userId, password, out entry);
        }

        public C5User GetC5User(string userId)
        {
            string password = GetPassword();
            return manifest.GetEntry<C5User>(c5UserPath, userId, password);
        }

        public IEnumerable<string> GetC5User()
        {
            return manifest.GetEntries(c5UserPath);
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

using Newtonsoft.Json;
using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    internal class SettingManifest
    {
        private readonly string path = "sda/setting";
        private readonly Manifest manifest;
        private readonly Setting setting;

        public SettingManifest()
        {
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = Path.Combine(commonAppDataPath, "SA", "Manifest", "sda.setting.manifest");
            manifest = Manifest.FromFile(file);
            setting = manifest.GetEntry<Setting>(path, "setting", null)?.Value ?? new Setting();
        }

        public Setting Entry => setting;

        public string Password { get; set; }

        public bool Save()
        {
            return manifest.SaveEntry(path, "setting", null, setting);
        }

        /// <summary>
        /// 
        /// </summary>
        public class Setting : JsonStreamSerializer<Setting>
        {
            public Setting()
            {
                Domain = new Domain
                {
                    SteamCommunity = "https://steamcommunity.com",
                    SteamApi = "https://api.steampowered.com",
                    SteamPowered = "https://store.steampowered.com",
                    SteamLogin = "https://login.steampowered.com"
                };
            }

            [JsonProperty("domain")]
            public Domain Domain { get; set; }

            /// <summary>
            /// 是否自动检测确认信息
            /// </summary>
            [JsonProperty("periodic_checking_confirmation")]
            public bool PeriodicCheckingConfirmation { get; set; }

            /// <summary>
            /// 是否检测所有账号确认信息
            /// </summary>
            [JsonProperty("check_all_confirmation")]
            public bool CheckAllConfirmation { get; set; }

            /// <summary>
            /// 是否自动确认报价
            /// </summary>
            [JsonProperty("auto_confirm_trade")]
            public bool AutoConfirmTrade { get; set; }

            /// <summary>
            /// 是否自动确认市场上架
            /// </summary>
            [JsonProperty("auto_confirm_market")]
            public bool AutoConfirmMarket { get; set; }
        }
    }
}

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

                Proxy = new HostProxy
                {
                    Host = "",
                    Address = "",
                    Port = 8080
                };
            }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("current_user")]
            public string CurrentUser { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("domain")]
            public Domain Domain { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("proxy")]
            public HostProxy Proxy { get; set; }

            /// <summary>
            /// 是否自动检测确认信息
            /// </summary>
            [JsonProperty("periodic_checking_confirmation")]
            public bool PeriodicCheckingConfirmation { get; set; }

            /// <summary>
            /// 检测频率
            /// 单位：秒
            /// </summary>
            [JsonProperty("auto_refresh_internal")]
            public int AutoRefreshInternal { get; set; } = 5;

            /// <summary>
            /// 是否检测所有账号确认信息
            /// </summary>
            [JsonProperty("check_all_confirmation")]
            public bool CheckAllConfirmation { get; set; }

            /// <summary>
            /// 自动弹出确认
            /// </summary>
            [JsonProperty("confirmation_auto_popup")]
            public bool ConfirmationAutoPopup { get; set; }

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

            /// <summary>
            /// 是否自动接收赠送报价
            /// 别人送给我的
            /// </summary>
            [JsonProperty("auto_accept_receive_offer")]
            public bool AutoAcceptReceiveOffer { get; set; }

            /// <summary>
            /// 是否自动接收索取报价
            /// 别人索取我的
            /// </summary>
            [JsonProperty("auto_accept_give_offer")]
            public bool AutoAcceptGiveOffer { get; set; }
        }
    }
}

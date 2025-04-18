﻿using Newtonsoft.Json;
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
            setting = manifest.GetEntry<Setting>(path, "setting", null) ?? new Setting();
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

        public Setting Entry => setting;

        public string Password { get; set; }

        public bool Save()
        {
            return manifest.SaveEntry(path, "setting", null, setting);
        }

        /// <summary>
        /// 
        /// </summary>
        public class Setting : JsonStreamSerializer
        {
            public Setting()
            {
                Domain = new Domain
                {
                    SteamCommunity = "https://steamcommunity.com",
                    SteamApi = "https://api.steampowered.com",
                    SteamPowered = "https://store.steampowered.com",
                    SteamHelp = "https://help.steampowered.com",
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
            [JsonProperty("first_used")]
            public bool FirstUsed { get; set; } = true;

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("tip_version")]
            public int TipVersion { get; set; } = 0;

            /// <summary>
            /// 
            /// </summary>
            public string InitialDirectory { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("current_user")]
            public string CurrentUser { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool UseCustomerDomain { get; set; } = true;

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("domain")]
            public Domain Domain { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public bool UseCustomerProxy { get; set; } = true;

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("proxy")]
            public HostProxy Proxy { get; set; }

            /// <summary>
            /// 是否自动检测确认信息
            /// </summary>
            [JsonProperty("periodic_checking_confirmation")]
            public bool PeriodicCheckingConfirmation { get; set; } = true;

            /// <summary>
            /// 检测频率
            /// 单位：秒
            /// </summary>
            [JsonProperty("auto_refresh_internal")]
            public int AutoRefreshInternal { get; set; } = 5;

            /// <summary>
            /// 自动弹出确认
            /// </summary>
            [JsonProperty("confirmation_auto_popup")]
            public bool ConfirmationAutoPopup { get; set; } = true;
        }
    }
}

using SteamKit;

namespace Steam_Authenticator.Internal
{
    internal static class Configuration
    {
        public const string HelpWhyCantITrade = $"{SteamBulider.DefaultSteamHelp}/zh-cn/wizard/HelpWhyCantITrade";

        public static string DefaultTradeLinkPartner = "";

        public static string DefaultTradeLinkToken = "";
    }
}

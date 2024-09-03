using static SteamKit.SteamEnum;

namespace Steam_Authenticator.Model
{
    public class Guard : JsonStreamSerializer<Guard>
    {
        public string DeviceId { get; set; }
        public string SharedSecret { get; set; }
        public string IdentitySecret { get; set; }
        public string RevocationCode { get; set; }
        public string SerialNumber { get; set; }
        public SteamGuardScheme GuardScheme { get; set; }
        public string URI { get; set; }
        public string AccountName { get; set; }
        public string TokenGID { get; set; }
        public string Secret1 { get; set; }
    }
}

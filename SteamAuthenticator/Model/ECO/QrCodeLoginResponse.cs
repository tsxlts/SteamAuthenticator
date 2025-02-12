
namespace Steam_Authenticator.Model.ECO
{
    public class QrCodeLoginResponse
    {
        public string QrCodeId { get; set; }

        /// <summary>
        /// 未扫码 = 0， 扫码成功 = 1， 确认登陆 = 2, 二维码过期 = 3
        /// </summary>
        public int State { get; set; }

        public string ClientId { get; set; }

        public string IpAddress { get; set; }

        public LoginToken Token { get; set; }
    }

    public class LoginToken
    {
        public string Token { get; set; }

        public DateTime TokenCreateDate { get; set; }

        public DateTime TokenExpireDate { get; set; }

        /// <summary>
        /// Token相对过期时间/秒
        /// </summary>
        public int TokenRelativeExpireTime { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenCreateDate { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }

        /// <summary>
        /// 刷新Token相对过期时间/秒
        /// </summary>
        public int RefreshTokenRelativeExpireTime { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int ClientType { get; set; }

        public string LoginIp { get; set; }

        public bool? IsCertificate { get; set; }

        public bool? IsRequireVisitIpEqualLoginIp { get; set; }

        public string EquipmentCode { get; set; }
    }
}

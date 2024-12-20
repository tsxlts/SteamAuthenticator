
namespace Steam_Authenticator.Model.ECO
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }

        public DateTime TokenExpireDate { get; set; }

        public string TokenExpireDateForCST { get; set; }

        public int TokenRelativeExpireTime { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpireDate { get; set; }

        public string RefreshTokenExpireDateForCST { get; set; }

        public int RefreshTokenRelativeExpireTime { get; set; }

        public string ClientId { get; set; }
    }
}

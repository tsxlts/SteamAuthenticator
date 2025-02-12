
namespace Steam_Authenticator.Model.ECO
{
    public class LoginResponse : RefreshTokenResponse
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }
    }
}

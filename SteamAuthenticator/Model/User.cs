namespace Steam_Authenticator.Model
{
    public class User : JsonStreamSerializer<User>
    {
        public string Account { get; set; }

        public string SteamId { get; set; }

        public string NickName { get; set; }

        public string RefreshToken { get; set; }

        public string Avatar { get; set; }
    }
}

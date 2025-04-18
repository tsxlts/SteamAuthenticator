namespace Steam_Authenticator.Model
{
    public class ClientRefreshedArgs
    {
        public IUserClient Client { get; set; }

        public bool Changed { get; set; }
    }
}

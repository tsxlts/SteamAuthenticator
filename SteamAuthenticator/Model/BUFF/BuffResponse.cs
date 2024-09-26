
namespace Steam_Authenticator.Model.BUFF
{
    public class BuffResponse<T>
    {
        public string code { get; set; }

        public string msg { get; set; }

        public string error { get; set; }

        public T data { get; set; }
    }
}

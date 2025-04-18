
namespace Steam_Authenticator.Model.C5
{
    public class C5Response<T>
    {
        public bool success { get; set; }

        public T data { get; set; }

        public int errorCode { get; set; }

        public object errorMsg { get; set; }

        public object errorData { get; set; }

        public object errorCodeStr { get; set; }
    }
}



namespace Steam_Authenticator.Model.YouPin898
{
    public class YouPin898Response<T>
    {
        public int GetCode()
        {
            return Code ?? code ?? -1;
        }

        public string GetMsg()
        {
            return Msg ?? msg ?? "";
        }

        public T GetData()
        {
            return Data ?? data;
        }

        public bool IsSuccess()
        {
            return GetCode() == 0;
        }

        public int? Code { get; set; }

        public int? code { get; set; }

        public string Msg { get; set; }

        public string msg { get; set; }

        public T Data { get; set; }

        public T data { get; set; }
    }
}

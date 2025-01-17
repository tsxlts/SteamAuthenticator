
namespace Steam_Authenticator.Model.ECO
{
    public class EcoResponse<T>
    {
        public string StatusCode { get; set; }

        public string StatusMsg { get; set; }

        public Statusdata<T> StatusData { get; set; }

        public bool IsSuccess => StatusCode == "0" && StatusData?.ResultCode == "0";
    }

    public class Statusdata<T>
    {
        public string ResultCode { get; set; }

        public string ResultMsg { get; set; }

        public T ResultData { get; set; }
    }
}

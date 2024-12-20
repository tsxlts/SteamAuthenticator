
namespace Steam_Authenticator.Model.ECO
{
    public class PagesModel<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public int TotalRecord { get; set; }

        public List<T> PageResult { get; set; }
    }
}

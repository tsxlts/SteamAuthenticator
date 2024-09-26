
namespace Steam_Authenticator.Model.BUFF
{
    public class QrCodeLoginResponse
    {
        public string avatar { get; set; }
        public string id { get; set; }
        public bool is_need_steam_verify { get; set; }
        public bool is_new { get; set; }
        public int login_from { get; set; }
        public string mobile { get; set; }
        public string nickname { get; set; }
        public object partner_role_info { get; set; }
        public bool show_invitation { get; set; }
        public string steamid { get; set; }
        public string trade_url { get; set; }
        public int trade_url_state { get; set; }
    }
}

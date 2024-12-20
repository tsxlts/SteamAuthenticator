namespace Steam_Authenticator.Model.ECO
{
    public class QueryUserResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHead { get; set; }

        /// <summary>
        /// Steam头像
        /// </summary>
        public string SteamHead { get; set; }

        /// <summary>
        /// 用户绑定SteamId
        /// </summary>
        public string SteamId { get; set; }

        /// <summary>
        /// 用户绑定Steam昵称
        /// </summary>
        public string SteamNickName { get; set; }

        /// <summary>
        /// 交易链接 Trade
        /// </summary>
        public string TreadLink { get; set; }

        /// <summary>
        /// APIKey
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 绑定手机号码
        /// </summary>
        public string BindMobile { get; set; }
    }
}


namespace Steam_Authenticator.Model.BUFF
{
    public class BuffUserInfoResponse
    {
        public bool allow_auto_remark { get; set; }
        public bool allow_bargain_chat_message_push { get; set; }
        public bool allow_bargain_rejected_push { get; set; }
        public bool allow_buyer_bargain { get; set; }
        public bool allow_buyer_bargain_chat { get; set; }
        public bool allow_comment_push { get; set; }
        public bool allow_csgo_trade_up { get; set; }
        public bool allow_csgo_trade_up_community { get; set; }
        public bool allow_deliver_push { get; set; }
        public bool allow_epay { get; set; }
        public bool allow_export_bill_order { get; set; }
        public bool allow_feedback_new_entry { get; set; }
        public bool allow_image_search { get; set; }
        public bool allow_item_recommendation { get; set; }
        public bool allow_mail_notification { get; set; }
        public bool allow_nickname_sign { get; set; }
        public bool allow_preview_audit { get; set; }
        public bool allow_preview_recommend { get; set; }
        public bool allow_price_change_notify { get; set; }
        public bool allow_pubg_recycle { get; set; }
        public bool allow_purchase_premium { get; set; }
        public bool allow_shop_display { get; set; }
        public bool allow_sms_notification { get; set; }
        public bool allow_social_comment { get; set; }
        public bool allow_up_push { get; set; }
        public bool allow_wechat_trade_message { get; set; }
        public string avatar { get; set; }
        public string buff_price_currency { get; set; }
        public string buff_price_currency_desc { get; set; }
        public float buff_price_currency_rate_base_cny { get; set; }
        public float buff_price_currency_rate_base_usd { get; set; }
        public string buff_price_currency_symbol { get; set; }
        public int buyer_exam_state { get; set; }
        public bool can_unbind_steam { get; set; }
        public bool force_buyer_send_offer { get; set; }
        public string id { get; set; }
        public string inventory_price { get; set; }
        public bool is_foreigner { get; set; }
        public bool is_need_steam_verify { get; set; }
        public bool is_new { get; set; }
        public bool is_premium { get; set; }
        public string language { get; set; }
        public int login_from { get; set; }
        public string mobile { get; set; }
        public string nickname { get; set; }
        public double nickname_remaining { get; set; }
        public int seller_exam_state { get; set; }
        public int steam_api_key_state { get; set; }
        public string steam_price_currency { get; set; }
        public string steam_price_currency_desc { get; set; }
        public string steamid { get; set; }
        public string trade_url { get; set; }
        public int trade_url_state { get; set; }
    }
}

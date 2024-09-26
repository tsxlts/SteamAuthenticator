
namespace Steam_Authenticator.Model.BUFF
{
    public class SteamTradeResponse
    {
        public int appid { get; set; }
        public int bot_age { get; set; }
        public string bot_age_icon { get; set; }
        public string bot_avatar { get; set; }
        public string bot_extra_info { get; set; }
        public int bot_level { get; set; }
        public string bot_level_background_color { get; set; }
        public object bot_level_background_image { get; set; }
        public string bot_name { get; set; }
        public int bot_steam_created_at { get; set; }
        public int create_count_up { get; set; }
        public int created_at { get; set; }
        public string game { get; set; }
        public IDictionary<string, Goods_Infos> goods_infos { get; set; }
        public string id { get; set; }
        public List<Items_To_Trade> items_to_trade { get; set; }
        public int state { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public string trace_url { get; set; }
        public string tradeofferid { get; set; }
        public int type { get; set; }
        public string url { get; set; }
        public string verify_code { get; set; }
    }

    public class Goods_Infos
    {
        public int appid { get; set; }
        public object description { get; set; }
        public string game { get; set; }
        public int goods_id { get; set; }
        public string icon_url { get; set; }
        public object item_id { get; set; }
        public string market_hash_name { get; set; }
        public string market_min_price { get; set; }
        public string name { get; set; }
        public string original_icon_url { get; set; }
        public string short_name { get; set; }
        public string steam_price { get; set; }
        public string steam_price_cny { get; set; }
        public Tags tags { get; set; }
    }

    public class Tags
    {
        public Category category { get; set; }
        public Category_Group category_group { get; set; }
        public Custom custom { get; set; }
        public Quality quality { get; set; }
        public Rarity rarity { get; set; }
        public Type type { get; set; }
    }

    public class Category
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Category_Group
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Custom
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Quality
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Rarity
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Type
    {
        public string category { get; set; }
        public int id { get; set; }
        public string internal_name { get; set; }
        public string localized_name { get; set; }
    }

    public class Items_To_Trade
    {
        public int appid { get; set; }
        public string assetid { get; set; }
        public string classid { get; set; }
        public int contextid { get; set; }
        public int goods_id { get; set; }
        public string instanceid { get; set; }
    }
}

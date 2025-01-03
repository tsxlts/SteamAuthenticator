
namespace Steam_Authenticator.Model.ECO
{
    public class QueryAssetResponse
    {
        public string GameId { get; set; }

        public string HashName { get; set; }

        public string AssetId { get; set; }

        public int PaintSeed { get; set; }

        public int PaintIndex { get; set; }

        public string PaintWear { get; set; }

        public string PaintLabel { get; set; }

        public string PaintSeedLabel { get; set; }

        public decimal Fade { get; set; }

        public object[] FrauDwarnings { get; set; }

        public string InspectUrl { get; set; }

        public List<Sticker> Stickers { get; set; } = new List<Sticker>();

        public List<Sticker> Keychains { get; set; } = new List<Sticker>();
    }


    public class Sticker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Pattern { get; set; }
        public int Slot { get; set; }
        public int Wear { get; set; }
        public int Price { get; set; }
        public string GoodsId { get; set; }
        public string HashName { get; set; }
        public string GameId { get; set; }
    }
}

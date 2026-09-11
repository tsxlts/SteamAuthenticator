using SteamKit.Model;
using System.ComponentModel;

namespace Steam_Authenticator.Controls
{
    internal class OfferButton : Button
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Offer Offer { get; set; }
    }
}

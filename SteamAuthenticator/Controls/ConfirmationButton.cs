using SteamKit.Model;
using System.ComponentModel;

namespace Steam_Authenticator.Controls
{
    internal class ConfirmationButton : Button
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Confirmation Confirmation { get; set; }
    }
}

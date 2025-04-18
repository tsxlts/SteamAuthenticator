using System.Text.RegularExpressions;
using Steam_Authenticator.Factory;
using Steam_Authenticator.Model;
using SteamKit.Model;
using SteamKit.WebClient;

namespace Steam_Authenticator.Internal
{
    internal class Utils
    {
        public static async Task HandleOffer(SteamCommunityClient webClient, IEnumerable<Offer> offers, bool accept, CancellationToken cancellationToken)
        {
            if (!webClient.LoggedIn)
            {
                return;
            }
            if (offers == null || !offers.Any())
            {
                return;
            }

            List<Task<bool>> tasks = new List<Task<bool>>();
            TaskFactory taskFactory = new TaskFactory();
            foreach (var item in offers)
            {
                tasks.Add(taskFactory.StartNew((arg) =>
                {
                    bool success = false;
                    Offer offer = arg as Offer;
                    try
                    {
                        if (accept)
                        {
                            webClient.TradeOffer.AcceptOfferAsync(offer.TradeOfferId, cancellationToken).GetAwaiter().GetResult();
                        }
                        else
                        {
                            webClient.TradeOffer.DeclineOfferAsync(offer.TradeOfferId, cancellationToken).GetAwaiter().GetResult();
                        }

                        success = true;
                    }
                    catch
                    {
                        success = false;
                    }

                    AppLogger.Instance.Debug("handleOffer", webClient.SteamId, $"###{(accept ? "接受报价" : "拒绝报价")}###" +
                        $"{Environment.NewLine}{offer.TradeOfferId}: {success}");

                    return success;
                }, item));
            }

            var result = await Task.WhenAll(tasks);
        }

        public static async Task<bool> HandleConfirmation(SteamCommunityClient webClient, Guard guard, IEnumerable<Confirmation> confirmations, bool accept, CancellationToken cancellationToken)
        {
            if (confirmations == null || !confirmations.Any())
            {
                return true;
            }

            if (confirmations.Count() == 1)
            {
                goto SingleConfirm;
            }

            bool success = false;
            while (true)
            {
                if (accept)
                {
                    success = await webClient.Confirmation.AllowConfirmationAsync(confirmations, guard.DeviceId, guard.IdentitySecret);
                }
                else
                {
                    success = await webClient.Confirmation.CancelConfirmationAsync(confirmations, guard.DeviceId, guard.IdentitySecret);
                }

                AppLogger.Instance.Debug("handleConfirmation", webClient.SteamId, $"###{(accept ? "令牌确认" : "取消确认")}###" +
                    $"{Environment.NewLine}[{string.Join(",", confirmations.Select(c => $"{c.ConfTypeName}#{c.CreatorId}#{c.Id}"))}]: {success}");

                if (cancellationToken.IsCancellationRequested || success)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            if (success)
            {
                return true;
            }

        SingleConfirm:
            var tasks = confirmations.Select(confirm => HandleConfirmation(webClient, guard, confirm, accept, cancellationToken));
            var results = await Task.WhenAll(tasks);
            success = !results.All(c => c == false);

            return success;
        }

        public static async Task<bool> HandleConfirmation(SteamCommunityClient webClient, Guard guard, Confirmation confirmation, bool accept, CancellationToken cancellationToken)
        {
            bool success = false;
            while (true)
            {
                if (accept)
                {
                    success = await webClient.Confirmation.AllowConfirmationAsync(confirmation, guard.DeviceId, guard.IdentitySecret);
                }
                else
                {
                    success = await webClient.Confirmation.CancelConfirmationAsync(confirmation, guard.DeviceId, guard.IdentitySecret);
                }

                AppLogger.Instance.Debug("handleConfirmation", webClient.SteamId, $"###{(accept ? "令牌确认" : "取消确认")}###" +
                    $"{Environment.NewLine}{$"{confirmation.ConfTypeName}#{confirmation.CreatorId}#{confirmation.Id}"}: {success}");

                if (cancellationToken.IsCancellationRequested || success)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            return success;
        }

        public static void CopyText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            Clipboard.SetText(value);
        }

        public static bool CheckTradeLink(string link, out string partner, out string token)
        {
            partner = null;
            token = null;

            Regex regex = new Regex(@"partner=(.+?)&token=(.+?)$");
            Match match = regex.Match(link);
            if (!match.Success)
            {
                return false;
            }

            partner = match.Groups[1].Value;
            token = match.Groups[2].Value;

            return true;
        }

    }
}

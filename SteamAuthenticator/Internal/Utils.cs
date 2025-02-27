using Steam_Authenticator.Model;
using SteamKit.Model;
using SteamKit.WebClient;

namespace Steam_Authenticator.Internal
{
    internal class Utils
    {
        public static async Task HandleOffer(SteamCommunityClient webClient, IEnumerable<Offer> offers, bool accpet, CancellationToken cancellationToken)
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
                    try
                    {
                        Offer offer = arg as Offer;
                        if (accpet)
                        {
                            webClient.TradeOffer.AcceptOfferAsync(offer.TradeOfferId, cancellationToken).GetAwaiter().GetResult();
                        }
                        else
                        {
                            webClient.TradeOffer.DeclineOfferAsync(offer.TradeOfferId, cancellationToken).GetAwaiter().GetResult();
                        }

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
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
            if (accept)
            {
                success = await webClient.Confirmation.AllowConfirmationAsync(confirmations, guard.DeviceId, guard.IdentitySecret);
            }
            else
            {
                success = await webClient.Confirmation.CancelConfirmationAsync(confirmations, guard.DeviceId, guard.IdentitySecret);
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

                if (cancellationToken.IsCancellationRequested || success)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            return success;
        }
    }
}

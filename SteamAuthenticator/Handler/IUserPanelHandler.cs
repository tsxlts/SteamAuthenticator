
namespace Steam_Authenticator.Handler
{
    public interface IUserPanelHandler
    {
        public Task LoadUsersAsync(CancellationToken cancellationToken = default);
    }
}

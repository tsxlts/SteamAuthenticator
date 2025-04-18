using Steam_Authenticator.Controls;

namespace Steam_Authenticator.Handler
{
    internal abstract class UserPanelHandler<TItemPanel, TClient> : IUserPanelHandler where TItemPanel : ClientItemPanel<TClient> where TClient : IUserClient
    {
        private readonly System.Threading.Timer refreshUserTimer;

        protected readonly ItemCollectionPanel<TItemPanel, TClient> UsersPanel;
        protected readonly ContextMenuStrip UserMenu;

        public UserPanelHandler(ItemCollectionPanel<TItemPanel, TClient> itemCollection)
        {
            refreshUserTimer = new System.Threading.Timer(c =>
            {
                try
                {
                    RefreshUserInternal().GetAwaiter().GetResult();
                }
                catch
                {

                }
                finally
                {
                    refreshUserTimer.Change(RefreshUserInterval, RefreshUserInterval * 1.5d);
                }
            }, UsersPanel, -1, -1);

            UserMenu = new ContextMenuStrip();
            UserMenu.Items.Add("重新登录").Click += async (sender, e) =>
            {
                (TItemPanel panel, TClient client) = GetClient(sender as ToolStripMenuItem);
                await ReloginInternal(panel, client);
            };
            UserMenu.Items.Add("退出登录").Click += async (sender, e) =>
            {
                (TItemPanel panel, TClient client) = GetClient(sender as ToolStripMenuItem);
                await LogoutInternal(panel, client);

                refreshUserTimer.Change(TimeSpan.FromSeconds(0), RefreshUserInterval);
            };
            UserMenu.Items.Add("移除帐号").Click += async (sender, e) =>
            {
                (TItemPanel panel, TClient client) = GetClient(sender as ToolStripMenuItem);
                await RemoveUserInternal(panel, client);

                UsersPanel.RemoveClient(client);
            };

            var usersPanelMenu = new ContextMenuStrip();
            usersPanelMenu.Items.Add("刷新").Click += (sender, e) =>
            {
                UsersPanel.Reset();
            };
            usersPanelMenu.Items.Add("添加帐号").Click += async (sender, e) =>
            {
                await AddUser();
            };

            UsersPanel = itemCollection;
            UsersPanel.ContextMenuStrip = usersPanelMenu;
            UsersPanel.SizeChanged += (sender, e) =>
            {
                UsersPanel.Reset();
            };
        }

        public async Task LoadUsersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                UsersPanel.ClearItems();
                await LoadUsersAsyncInternal(cancellationToken);
            }
            catch (Exception)
            {
            }
            finally
            {
                refreshUserTimer.Change(TimeSpan.FromSeconds(5), RefreshUserInterval);
            }
        }

        protected async Task<TItemPanel> AddUser()
        {
            var panel = await AddUserInternal();
            if (panel == null)
            {
                return null;
            }

            panel.ContextMenuStrip = UserMenu;

            return panel;
        }

        protected virtual async Task RefreshUserInternal()
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                var controlCollection = UsersPanel.ItemPanels;
                foreach (var userPanel in controlCollection)
                {
                    if (!userPanel.HasItem)
                    {
                        continue;
                    }

                    var buffClient = userPanel.Client;
                    await buffClient.RefreshClientAsync(tokenSource.Token);
                }
            }
        }

        protected abstract Task<List<TItemPanel>> LoadUsersAsyncInternal(CancellationToken cancellationToken = default);

        protected abstract Task<TItemPanel> AddUserInternal();

        protected abstract Task RemoveUserInternal(TItemPanel panel, TClient client);

        protected abstract Task ReloginInternal(TItemPanel panel, TClient client);

        protected virtual Task LogoutInternal(TItemPanel panel, TClient client)
        {
            return client.LogoutAsync();
        }

        protected (TItemPanel Panel, TClient Client) GetClient(ToolStripMenuItem menuItem)
        {
            ContextMenuStrip menuStrip = (ContextMenuStrip)menuItem.GetCurrentParent();

            TItemPanel panel = menuStrip.SourceControl.Parent as TItemPanel;
            TClient client = panel.Client;
            return (panel, client);
        }

        protected virtual TimeSpan RefreshUserInterval => TimeSpan.FromSeconds(60);
    }
}

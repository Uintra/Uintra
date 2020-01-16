using Microsoft.AspNet.SignalR;

namespace Uintra20.Features.CentralFeed
{
    [Authorize]
    public class CentralFeedHub:Hub
    {
        public void FeedReload()
        {
            Clients.All().reloadFeed();
        }
    }
}
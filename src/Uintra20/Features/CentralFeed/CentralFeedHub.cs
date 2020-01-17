using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Uintra20.Features.CentralFeed
{
    [Authorize]
    public class CentralFeedHub:Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}
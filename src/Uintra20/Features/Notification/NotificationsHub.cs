using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Features.Notification
{
    [Authorize]
    public class NotificationsHub : Hub
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
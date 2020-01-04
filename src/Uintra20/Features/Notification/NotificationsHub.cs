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
		private readonly IUiNotificationService _uiNotificationService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public NotificationsHub(
            IUiNotificationService uiNotificationService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _uiNotificationService = uiNotificationService;
            _intranetMemberService = intranetMemberService;
        }

        public async Task<int> GetNotNotifiedCount()
        {
            var member = await _intranetMemberService.GetByEmailAsync(Context.User.Identity.Name);
            
			return await _uiNotificationService.GetNotNotifiedCountAsync(member.Id);
		}

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
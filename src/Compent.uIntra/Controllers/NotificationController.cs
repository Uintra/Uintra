using uCommunity.Core.User;
using uCommunity.Notification.Core.Services;
using uCommunity.Notification.Web;
using uCommunity.Users.Core;

namespace Compent.uIntra.Controllers
{
    public class NotificationController: NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(IUiNotifierService uiNotifierService, IIntranetUserService<IntranetUser> intranetUserService) 
            : base(uiNotifierService, intranetUserService)
        {
        }
    }
}
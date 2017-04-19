using uCommunity.Core.User;
using uCommunity.Notification.Core.Services;
using uCommunity.Notification.Web;

namespace Compent.uCommunity.Controllers
{
    public class NotificationController: NotificationControllerBase
    {
        protected override int ItemsPerPage { get; } = 10;

        public NotificationController(IUiNotifierService uiNotifierService, IIntranetUserService intranetUserService) 
            : base(uiNotifierService, intranetUserService)
        {
        }
    }
}
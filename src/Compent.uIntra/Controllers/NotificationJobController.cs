using uIntra.Notification;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers
{
    public class NotificationJobController : NotificationJobControllerBase
    {
        public NotificationJobController(IReminderJob reminderJob, IMailService mailService)
            : base(reminderJob, mailService)
        {
        }
    }
}
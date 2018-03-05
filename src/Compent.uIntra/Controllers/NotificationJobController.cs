using Uintra.Notification;
using Uintra.Notification.Web;

namespace Compent.Uintra.Controllers
{
    public class NotificationJobController : NotificationJobControllerBase
    {
        public NotificationJobController(IReminderJob reminderJob, IMailService mailService)
            : base(reminderJob, mailService)
        {
        }
    }
}
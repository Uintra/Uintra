using uIntra.Notification;
using uIntra.Notification.Web;

namespace Compent.uIntra.Controllers
{
    public class ReminderController : ReminderControllerBase
    {
        public ReminderController(IReminderJob reminderJob)
            :base(reminderJob)
        {
        }
    }
}
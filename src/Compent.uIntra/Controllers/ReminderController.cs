using uCommunity.Notification.Core.Services;
using uCommunity.Notification.Web;

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
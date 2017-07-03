using System.Web.Http;
using Umbraco.Web.WebApi;

namespace uIntra.Notification.Web
{
    public abstract class ReminderControllerBase : UmbracoApiController
    {
        private readonly IReminderJob _reminderJob;

        protected ReminderControllerBase(IReminderJob reminderJob)
        {
            _reminderJob = reminderJob;
        }

        [HttpGet, AllowAnonymous]
        public void RunReminderJob()
        {
            _reminderJob.Run();
        }
    }
}

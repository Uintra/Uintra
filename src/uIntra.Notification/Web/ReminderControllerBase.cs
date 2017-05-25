using System.Web.Http;
using uIntra.Notification.Core.Services;
using Umbraco.Web.WebApi;

namespace uIntra.Notification.Web
{
    public abstract class ReminderControllerBase: UmbracoApiController
    {
        private readonly IReminderJob ReminderJob;

        public ReminderControllerBase(IReminderJob reminderJob)
        {
            ReminderJob = reminderJob;
        }

        [HttpGet, AllowAnonymous]
        public void RunReminderJob()
        {
            ReminderJob.Run();
        }
    }
}

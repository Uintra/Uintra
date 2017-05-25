using System.Web.Http;
using uCommunity.Notification.Core.Services;
using Umbraco.Web.WebApi;

namespace uCommunity.Notification.Web
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

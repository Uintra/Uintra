using System.Web.Http;
using uCommunity.Notification.Core.Services;
using Umbraco.Web.WebApi;

namespace Compent.uCommunity.Controllers
{
    public class ReminderController : UmbracoApiController
    {
        private readonly IReminderJob ReminderJob;

        public ReminderController(IReminderJob reminderJob)
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
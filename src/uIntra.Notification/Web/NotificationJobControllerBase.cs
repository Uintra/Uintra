using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Uintra.Notification.Web
{
    public abstract class NotificationJobControllerBase : UmbracoApiController
    {
        private readonly IReminderJob _reminderJob;
        private readonly IMailService _mailService;

        protected NotificationJobControllerBase(
            IReminderJob reminderJob,
            IMailService mailService)
        {
            _reminderJob = reminderJob;
            _mailService = mailService;
        }

        [HttpGet, AllowAnonymous]
        public void RunReminderJob()
        {
            _reminderJob.Run();
        }

        [HttpGet, AllowAnonymous]
        public void RunMailJob()
        {
            _mailService.ProcessMails();
        }
    }
}

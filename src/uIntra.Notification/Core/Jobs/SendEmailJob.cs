using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class SendEmailJob : BaseIntranetJob
    {
        private readonly IMailService _mailService;

        public SendEmailJob(IMailService mailService)
        {
            _mailService = mailService;
        }
        public override void Action()
        {
            _mailService.ProcessMails();
        }
    }
}

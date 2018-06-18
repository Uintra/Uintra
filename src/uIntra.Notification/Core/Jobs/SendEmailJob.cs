using uIntra.Core.Jobs.Models;

namespace uIntra.Notification.Jobs
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

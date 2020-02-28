using Uintra20.Core.Jobs.Models;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Features.Jobs
{
    public class SendEmailJob : Uintra20BaseIntranetJob
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

using Uintra.Core.Jobs.Models;
using Uintra.Features.Notification.Services;

namespace Uintra.Features.Jobs
{
    public class SendEmailJob : UintraBaseIntranetJob
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

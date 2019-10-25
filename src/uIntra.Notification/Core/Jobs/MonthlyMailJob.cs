using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class MontlyMailJob : BaseIntranetJob
    {
        private readonly IEmailBroadcastService _emailBroadcastService;

        public MontlyMailJob(IEmailBroadcastService emailBroadcastService)
        {
            _emailBroadcastService = emailBroadcastService;
        }

        public override void Action()
        {
            _emailBroadcastService.ProcessEmail();
        }
    }
}

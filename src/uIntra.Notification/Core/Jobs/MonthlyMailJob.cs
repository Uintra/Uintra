using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class MonthlyMailJob : BaseIntranetJob
    {
        private readonly IEmailBroadcastService _emailBroadcastService;

        public MonthlyMailJob(IEmailBroadcastService emailBroadcastService)
        {
            _emailBroadcastService = emailBroadcastService;
        }

        public override void Action()
        {
            _emailBroadcastService.IsBroadcastable();
        }
    }
}

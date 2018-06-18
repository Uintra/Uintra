using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class MontlyMailJob : BaseIntranetJob
    {
        private readonly IMonthlyEmailService _monthlyEmailService;

        public MontlyMailJob(IMonthlyEmailService monthlyEmailService)
        {
            _monthlyEmailService = monthlyEmailService;
        }

        public override void Action()
        {
            _monthlyEmailService.SendEmail();
        }
    }
}

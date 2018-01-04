using uIntra.Core.Jobs.Models;

namespace uIntra.Notification.Jobs
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

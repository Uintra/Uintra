using Uintra20.Core.Jobs.Models;
using Uintra20.Features.MonthlyMail;

namespace Uintra20.Features.Jobs
{
    public class MontlyMailJob : Uintra20BaseIntranetJob
    {
        private readonly IMonthlyEmailService _monthlyEmailService;

        public MontlyMailJob(IMonthlyEmailService monthlyEmailService)
        {
            _monthlyEmailService = monthlyEmailService;
        }

        public override void Action()
        {
            _monthlyEmailService.ProcessMonthlyEmail();
        }
    }
}

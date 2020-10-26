using Uintra.Core.Jobs.Models;
using Uintra.Features.MonthlyMail;

namespace Uintra.Features.Jobs
{
    public class MontlyMailJob : UintraBaseIntranetJob
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

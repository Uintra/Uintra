namespace Uintra.Features.MonthlyMail
{
    public interface IMonthlyEmailService
    {
        void ProcessMonthlyEmail();

        void CreateAndSendMail();
    }
}

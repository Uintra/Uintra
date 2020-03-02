namespace Uintra20.Features.MonthlyMail
{
    public interface IMonthlyEmailService
    {
        void ProcessMonthlyEmail();

        void CreateAndSendMail();
    }
}

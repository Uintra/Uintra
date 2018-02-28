namespace Uintra.Notification
{
    public interface IMonthlyEmailService
    {
        void ProcessMonthlyEmail();

        void CreateAndSendMail();
    }
}

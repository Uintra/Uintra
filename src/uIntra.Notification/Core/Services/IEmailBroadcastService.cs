namespace Uintra.Notification
{
    public interface IEmailBroadcastService
    {
        void ProcessEmail();
        void CreateAndSendMail();
    }
}

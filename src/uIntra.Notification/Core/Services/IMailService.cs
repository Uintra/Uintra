using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public interface IMailService
    {
        void Send(MailBase mail);

        void ProcessMails(int? count = null, int? mailId = null);
    }
}
using System;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface IMailService
    {
        void Send(MailBase mail);

        void ProcessMails(int? count = null, int? mailId = null);

        void SendMailByTypeAndDay(MailBase mail, string email , DateTime date, NotificationTypeEnum mailTemplateTypeEnum);
    }
}
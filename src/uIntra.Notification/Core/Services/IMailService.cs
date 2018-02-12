using System;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    public interface IMailService
    {
        void Send(MailBase mail);

        void ProcessMails(int? count = null, int? mailId = null);

        void SendMailByTypeAndDay(MailBase mail, string email , DateTime date, NotificationTypeEnum mailTemplateTypeEnum);
    }
}
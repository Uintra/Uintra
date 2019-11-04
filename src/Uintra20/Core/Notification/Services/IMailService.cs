using System;
using System.Threading.Tasks;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Notification.Configuration;

namespace Uintra20.Core.Notification
{
    public interface IMailService
    {
        void Send(MailBase mail);

        void ProcessMails(int? count = null, int? mailId = null);

        void SendMailByTypeAndDay(MailBase mail, string email, DateTime date, NotificationTypeEnum mailTemplateTypeEnum);

        Task SendAsync(MailBase mail);
        Task SendMailByTypeAndDayAsync(MailBase mail, string email, DateTime date, NotificationTypeEnum mailTemplateTypeEnum);
    }
}

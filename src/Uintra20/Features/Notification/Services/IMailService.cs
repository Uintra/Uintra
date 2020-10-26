using System;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base.Mails;

namespace Uintra20.Features.Notification.Services
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

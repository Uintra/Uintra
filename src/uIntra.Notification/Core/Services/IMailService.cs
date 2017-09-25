using System;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface IMailService
    {
        void Send(MailBase mail);

        void ProcessMails(int? count = null, int? mailId = null);

        void SendOneTimePerDayMailForSpecialTypeAndDay(MailBase mail, string email , DateTime day, NotificationTypeEnum mailTemplateTypeEnum);
    }
}
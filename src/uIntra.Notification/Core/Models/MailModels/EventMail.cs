using System;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.MailModels
{
    public class EventMail : EmailBase
    {
        private readonly string xpath;

        public EventMail(string xpath)
        {
            this.xpath = xpath;
        }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.Event;
    }
}

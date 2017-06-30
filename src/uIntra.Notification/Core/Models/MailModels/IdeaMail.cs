using System;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.MailModels
{
    public class IdeaMail : EmailBase
    {
        private readonly string xpath;

        public IdeaMail(string xpath)
        {
            this.xpath = xpath;
        }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.Idea;
    }
}
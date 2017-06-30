using System;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.MailModels
{
    public class NewsMail : EmailBase
    {
        private readonly string xpath;

        public NewsMail(string xpath)
        {
            this.xpath = xpath;
        }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.News;
    }
}
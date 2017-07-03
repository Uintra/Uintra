using System;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.MailModels
{
    public class CommentLikeAddedMail : EmailBase
    {
        private readonly string xpath;

        public CommentLikeAddedMail(string xpath)
        {
            this.xpath = xpath;
        }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.CommentLikeAdded;
    }
}
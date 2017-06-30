using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class CommentEditedMail : EmailBase
    {
        private readonly string xpath;

        public CommentEditedMail(string xpath)
        {
            this.xpath = xpath;
        }

        public string ActivityTitle { get; set; }
        public string Url { get; set; }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.CommentEdited;

        protected override Dictionary<string, string> GetExtraTokens()
        {
            var result = base.GetExtraTokens();
            result.Add(EmailTokensConstants.ActivityTitle, ActivityTitle);
            result.Add(EmailTokensConstants.Url, Url);
            return result;
        }
    }
}
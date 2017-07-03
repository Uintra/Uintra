using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class ActivityLikeAddedMail : EmailBase
    {
        private readonly string _xpath;

        public ActivityLikeAddedMail(string xpath)
        {
            _xpath = xpath;
        }

        public string ActivityTitle { get; set; }
        public string ActivityType { get; set; }
        public string CreatedDate { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        protected override string GetXPath()
        {
            return _xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.ActivityLikeAdded;

        protected override Dictionary<string, string> GetExtraTokens()
        {
            var result = base.GetExtraTokens();
            result.Add(EmailTokensConstants.ActivityTitle, ActivityTitle);
            result.Add(EmailTokensConstants.ActivityType, ActivityType);
            result.Add(EmailTokensConstants.CreatedDate, CreatedDate);
            result.Add(EmailTokensConstants.Url, Url);

            result.Add(EmailTokensConstants.FullName, FullName);

            return result;
        }
    }
}
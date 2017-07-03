using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class BeforeStartMail : EmailBase
    {
        private readonly string xpath;

        public BeforeStartMail(string xpath)
        {
            this.xpath = xpath;
        }

        public string ActivityTitle { get; set; }
        public string FullName { get; set; }
        public string ActivityType { get; set; }
        public string StartDate { get; set; }
        public string Url { get; set; }

        protected override string GetXPath()
        {
            return xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.BeforeStart;

        protected override Dictionary<string, string> GetExtraTokens()
        {
            var result = base.GetExtraTokens();
            result.Add(EmailTokensConstants.ActivityTitle, ActivityTitle);
            result.Add(EmailTokensConstants.FullName, FullName);
            result.Add(EmailTokensConstants.ActivityType, ActivityType);
            result.Add(EmailTokensConstants.StartDate, StartDate);
            result.Add(EmailTokensConstants.Url, Url);
            return result;
        }
    }
}
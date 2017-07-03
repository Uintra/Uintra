using System;
using System.Collections.Generic;
using EmailWorker.Data.Model;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class EventUpdatedMail : EmailBase
    {
        private readonly string _xpath;

        public EventUpdatedMail(string xpath)
        {
            _xpath = xpath;
        }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        protected override string GetXPath()
        {
            return _xpath;
        }

        public override Enum MailTemplateTypeEnum => NotificationTypeEnum.EventUpdated;

        protected override Dictionary<string, string> GetExtraTokens()
        {
            var result = base.GetExtraTokens();
            result.Add(EmailTokensConstants.Title, Title);
            result.Add(EmailTokensConstants.Type, Type);
            result.Add(EmailTokensConstants.Url, Url);

            result.Add(EmailTokensConstants.FullName, FullName);

            return result;
        }
    }
}
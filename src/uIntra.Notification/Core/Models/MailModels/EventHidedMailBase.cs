using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class EventHidedMailBase : MailBase
    {
        public string Title { get; set; }
        public string NotifierFullName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.EventHided;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {EmailTokensConstants.Title, Title},
                {EmailTokensConstants.NotifierFullName, NotifierFullName},
                {EmailTokensConstants.Type, Type},
                {EmailTokensConstants.Url, Url},
                {EmailTokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
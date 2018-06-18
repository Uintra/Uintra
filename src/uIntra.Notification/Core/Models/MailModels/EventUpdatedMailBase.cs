using System.Collections.Generic;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;

namespace Uintra.Notification.MailModels
{
    public class EventUpdatedMailBase : MailBase
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.EventUpdated;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {TokensConstants.Title, Title},
                {TokensConstants.Type, Type},
                {TokensConstants.Url, Url},
                {TokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
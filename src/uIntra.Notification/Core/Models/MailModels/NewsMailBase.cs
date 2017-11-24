using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class NewsMailBase : MailBase
    {
        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.News;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {TokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
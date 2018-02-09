using System.Collections.Generic;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;

namespace Uintra.Notification.MailModels
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
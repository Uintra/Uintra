
using System.Collections.Generic;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;

namespace Uintra.Notification.MailModels
{
    public class MonthlyMailBase: MailBase
    {
        public string FullName { get; set; }

        public string ActivityList { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.MonthlyMail;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {TokensConstants.FullName, FullName},
                {TokensConstants.ActivityList, ActivityList}
            };

            return result;
        }
    }
}


using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
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

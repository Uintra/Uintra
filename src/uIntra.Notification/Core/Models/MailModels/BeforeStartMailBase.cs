using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class BeforeStartMailBase : MailBase
    {
        public string ActivityTitle { get; set; }
        public string ActivityType { get; set; }
        public string StartDate { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.BeforeStart;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {TokensConstants.ActivityTitle, ActivityTitle},
                {TokensConstants.ActivityType, ActivityType},
                {TokensConstants.StartDate, StartDate},
                {TokensConstants.Url, Url},
                {TokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
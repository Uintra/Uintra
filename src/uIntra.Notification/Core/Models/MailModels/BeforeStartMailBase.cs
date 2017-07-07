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
                {EmailTokensConstants.ActivityTitle, ActivityTitle},
                {EmailTokensConstants.ActivityType, ActivityType},
                {EmailTokensConstants.StartDate, StartDate},
                {EmailTokensConstants.Url, Url},
                {EmailTokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
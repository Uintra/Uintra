using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class ActivityLikeAddedMailBase : MailBase
    {
        public string ActivityTitle { get; set; }
        public string ActivityType { get; set; }
        public string CreatedDate { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.ActivityLikeAdded;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {EmailTokensConstants.ActivityTitle, ActivityTitle},
                {EmailTokensConstants.ActivityType, ActivityType},
                {EmailTokensConstants.CreatedDate, CreatedDate},
                {EmailTokensConstants.Url, Url},
                {EmailTokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
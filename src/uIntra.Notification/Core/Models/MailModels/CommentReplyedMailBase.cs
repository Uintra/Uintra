using System.Collections.Generic;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Constants;

namespace uIntra.Notification.MailModels
{
    public class CommentReplyedMailBase : MailBase
    {
        public string ActivityTitle { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.CommentReplyed;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {EmailTokensConstants.ActivityTitle, ActivityTitle},
                {EmailTokensConstants.Url, Url},
                {EmailTokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
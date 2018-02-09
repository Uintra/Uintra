using System.Collections.Generic;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;

namespace Uintra.Notification.MailModels
{
    public class CommentEditedMailBase : MailBase
    {
        public string ActivityTitle { get; set; }
        public string Url { get; set; }

        public string FullName { get; set; }

        public override NotificationTypeEnum MailTemplateType => NotificationTypeEnum.CommentEdited;

        public override IDictionary<string, string> GetExtraTokens()
        {
            var result = new Dictionary<string, string>
            {
                {TokensConstants.ActivityTitle, ActivityTitle},
                {TokensConstants.Url, Url},
                {TokensConstants.FullName, FullName}
            };

            return result;
        }
    }
}
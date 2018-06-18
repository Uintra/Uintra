using System.Collections.Generic;
using System.Linq;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Base
{
    public abstract class MailBase
    {
        public IList<MailRecipient> Recipients { get; set; } = new List<MailRecipient>();
        public IList<string> BccEmails { get; set; } = new List<string>();
        public IList<string> CcEmails { get; set; } = new List<string>();

        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string Body { get; set; }
        public string Subject { get; set; }

        public IEnumerable<MailAttachmentFile> Attachments { get; set; } = Enumerable.Empty<MailAttachmentFile>();

        public abstract NotificationTypeEnum MailTemplateType { get; }

        public abstract IDictionary<string, string> GetExtraTokens();
        public string DomainUri { get; set; }
    }
}
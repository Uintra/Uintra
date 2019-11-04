using System;
using System.Collections.Generic;
using System.Linq;
using EmailWorker.Data.Model;
using Uintra20.Core.Notification.Base;
using Uintra20.Core.Notification.Configuration;

namespace Uintra20.Core.Notification
{
    public class EmailNotificationMessage : MailBase, IEmailBase, INotificationMessage
    {
        public override NotificationTypeEnum MailTemplateType { get; }
        public string GetXPath() => MailConfiguration.MailTemplateXpath;

        public override IDictionary<string, string> GetExtraTokens() => new Dictionary<string, string>();
        public IList<IEmailRecipient> To { get; } = new List<IEmailRecipient>();
        public IEnumerable<IEmailAttachmentFile> AttachmentFiles { get; } = Enumerable.Empty<IEmailAttachmentFile>();
        public Enum MailTemplateTypeEnum { get; } = default;
    }
}
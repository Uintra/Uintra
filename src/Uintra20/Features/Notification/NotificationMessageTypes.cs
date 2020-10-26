using System;
using System.Collections.Generic;
using System.Linq;
using EmailWorker.Data.Features.EmailWorker;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base.Mails;
using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification
{
    public class EmailNotificationMessage : MailBase, IEmailBase, INotificationMessage
    {
        public override NotificationTypeEnum MailTemplateType { get; }
        public string GetXPath() => MailConfiguration.MailTemplateXpath;

        public override IDictionary<string, string> GetExtraTokens() => new Dictionary<string, string>();
        public IList<IEmailRecipient> To { get; } = new List<IEmailRecipient>();
        public IEnumerable<IEmailAttachmentFile> AttachmentFiles { get; } = Enumerable.Empty<IEmailAttachmentFile>();
        public Enum MailTemplateTypeEnum { get; } = default(Enum);
    }
}
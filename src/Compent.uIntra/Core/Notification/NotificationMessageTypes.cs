using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmailWorker.Data.Model;
using uIntra.Core.TypeProviders;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace Compent.uIntra.Core.Notification
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
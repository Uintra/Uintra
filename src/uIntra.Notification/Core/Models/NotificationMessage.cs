using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{

    public class UiNotificationMessage : INotificationMessage
    {
        public IIntranetType NotificationType { get; set; }
        public Guid ReceiverId { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }

    public class EmailNotificationMessage : MailBase, INotificationMessage
    {
        public override NotificationTypeEnum MailTemplateType { get; }
        public override IDictionary<string, string> GetExtraTokens() => new Dictionary<string, string>();
    }

    public interface INotificationMessage
    {

    }
}

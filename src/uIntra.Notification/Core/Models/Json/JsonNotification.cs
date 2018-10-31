using System;

namespace Uintra.Notification.Models.Json
{
    public class JsonNotification
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public string Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public Enum Type { get; set; }
        public dynamic Value { get; set; }

        public Guid NotifierId { get; set; }
        public string NotifierPhoto { get; set; }
        public string NotifierDisplayedName { get; set; }
        public string Message { get; set; }
        public string DesktopMessage { get; set; }
        public string DesktopTitle { get; set; }
        public string Url { get; set; }
        public bool IsDesktopNotificationEnabled { get; set; }
    }
}

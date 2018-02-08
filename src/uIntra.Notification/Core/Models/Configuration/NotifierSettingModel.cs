using System;

namespace uIntra.Notification
{
    public class NotifierSettingSaveModel<T>
        where T : INotifierTemplate
    {
        public Enum ActivityType { get; set; }
        public Enum NotificationType { get; set; }
        public Enum NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }
}
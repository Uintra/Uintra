using System;

namespace Uintra.Notification
{
    public class NotifierSettingModel<T>
        where T : INotifierTemplate
    {
        public Enum ActivityType { get; set; }
        public string ActivityTypeName { get; set; }
        public Enum NotificationType { get; set; }
        public string NotificationTypeName { get; set; }
        public Enum NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }
}
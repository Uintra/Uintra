using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class NotifierSettingSaveModel<T>
        where T : INotifierTemplate
    {
        public IntranetType ActivityType { get; set; }
        public IntranetType NotificationType { get; set; }
        public Enum NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }
}
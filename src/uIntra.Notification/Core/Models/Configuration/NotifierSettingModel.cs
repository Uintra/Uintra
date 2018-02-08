using System;
using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class NotifierSettingSaveModel<T>
        where T : INotifierTemplate
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public NotifierTypeEnum NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }
}
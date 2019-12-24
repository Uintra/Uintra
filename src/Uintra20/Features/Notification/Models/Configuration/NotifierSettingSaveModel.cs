using System;
using Uintra20.Features.Notification.Models.NotifierTemplates;

namespace Uintra20.Features.Notification.Models.Configuration
{
    public class NotifierSettingSaveModel<T>
        where T : INotifierTemplate
    {
        public int ActivityType { get; set; }
        public int NotificationType { get; set; }
        public int NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; }
        public T Template { get; set; }
    }
}
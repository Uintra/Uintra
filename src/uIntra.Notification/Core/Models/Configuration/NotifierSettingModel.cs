

namespace uIntra.Notification
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
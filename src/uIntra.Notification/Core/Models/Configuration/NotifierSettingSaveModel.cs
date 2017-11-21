using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public class NotifierSettingModel<T>
        where T : INotifierTemplate
    {
        public IIntranetType ActivityType { get; set; }
        public IIntranetType NotificationType { get; set; }
        public IIntranetType NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }
}
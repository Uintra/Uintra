using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    /// <summary>
    /// Marker
    /// </summary>
    public interface INotifierTemplate { }

    public class EmailNotifierTemplate : INotifierTemplate
    {
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    public class UiNotifierTemplate : INotifierTemplate
    {
        public string Message { get; set; }
    }


    public class NotifierSettingModel<T>
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public NotifierTypeEnum NotifierType { get; set; }
        public bool IsEnabled { get; set; }
        public string NotificationInfo { get; set; } 
        public T Template { get; set; }
    }


    public class NotifierSettingsModel
    {
        public NotifierSettingModel<EmailNotifierTemplate> EmailNotifierSetting { get; set; }
        public NotifierSettingModel<UiNotifierTemplate> UiNotifierSetting { get; set; }
    }
}
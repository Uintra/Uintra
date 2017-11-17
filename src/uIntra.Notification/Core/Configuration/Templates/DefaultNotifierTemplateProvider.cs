using uIntra.Core.Activity;

namespace uIntra.Notification.Configuration
{
    public class DefaultNotifierTemplateProvider : 
        IDefaultNotifierTemplateProvider<EmailNotifierTemplate>,
        IDefaultNotifierTemplateProvider<UiNotifierTemplate>
    {
        public EmailNotifierTemplate GetTemplate(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            throw new System.NotImplementedException();
        }

        UiNotifierTemplate IDefaultNotifierTemplateProvider<UiNotifierTemplate>.GetTemplate(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            throw new System.NotImplementedException();
        }
    }
}
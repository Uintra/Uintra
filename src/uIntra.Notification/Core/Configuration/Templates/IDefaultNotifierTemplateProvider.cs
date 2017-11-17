using uIntra.Core.Activity;

namespace uIntra.Notification.Configuration
{
    public interface IDefaultNotifierTemplateProvider<out T>
        where T : INotifierTemplate
    {
        T GetTemplate(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType);
    }
}
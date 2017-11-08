using uIntra.Core.Activity;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Services;

namespace uIntra.Notification
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        public (IntranetActivityTypeEnum activityType, NotificationTypeEnum[] notificationTypes)[] NotificationPolicies => new[]
        {
            (activityType: IntranetActivityTypeEnum.Bulletins, new[]
            {
                NotificationTypeEnum.CommentAdded,
                NotificationTypeEnum.CommentEdited
            }),
            (activityType:IntranetActivityTypeEnum.Events, new[]
            {
                NotificationTypeEnum.CommentAdded
            }),
            (activityType:IntranetActivityTypeEnum.Events, new[]
            {
                NotificationTypeEnum.CommentAdded
            })
        };
    }
}

using System;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification;

namespace Compent.uIntra.Core.Migration
{
    public class OldUiNotificationMigration
    {
        private readonly ISqlRepository<global::uIntra.Notification.Notification> _notificationRepository;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly NewNotifierDataValueProvider _newNotifierDataValueProvider;
        private readonly NewNotificationMessageService _newNotificationMessageService;

        public OldUiNotificationMigration(
            ISqlRepository<global::uIntra.Notification.Notification> notificationRepository,
            IActivitiesServiceFactory activitiesServiceFactory,
            INotificationTypeProvider notificationTypeProvider,
            NewNotifierDataValueProvider newNotifierDataValueProvider,
            NewNotificationMessageService newNotificationMessageService)
        {
            _notificationRepository = notificationRepository;
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
            _newNotifierDataValueProvider = newNotifierDataValueProvider;
            _newNotificationMessageService = newNotificationMessageService;
        }


        public void Execute()
        {
            var allNotifications = _notificationRepository.GetAll();

            var mappedNotifications =
                allNotifications.Select(n => (item: n, data: n.Value.Deserialize<OldNotifierData>()));

            var oldNotifications = mappedNotifications
                .Where(n => IsOldNotifierData(n.data))
                .ToList();

            var parsedNotifications = oldNotifications
                .Select(UpdateNotificationValue)
                .ToLookup(n => n.isValid);

            var invalidNotifications = parsedNotifications[false].Select(UnpackNotification); 
            var updatedNotifications = parsedNotifications[true].Select(UnpackNotification); // notifications to activities that do not exist

            global::uIntra.Notification.Notification UnpackNotification((bool isValid, global::uIntra.Notification.Notification notification) arg) => arg.notification;

            _notificationRepository.Update(updatedNotifications);
            _notificationRepository.Delete(invalidNotifications); // we delete notifications that could not be migrated for some reason
        }

        private (bool isValid, global::uIntra.Notification.Notification notification) UpdateNotificationValue((global::uIntra.Notification.Notification notification, OldNotifierData data) item)
        {
            var notification = item.notification;

            NotificationValue newValue;
            try
            {
                newValue = MapToNewNotificationValue(item);
                if (newValue == null)
                    throw new NullReferenceException();
            }
            catch (Exception e)
            {
                return (isValid: false, notification);
            }

            notification.Value = newValue.ToJson();
            return (isValid: true, notification);
        }

        private NotificationValue MapToNewNotificationValue((global::uIntra.Notification.Notification item, OldNotifierData data) notification)
        {
            Guid activityId = ParseActivityId(notification.data.Url);

            var activityService =
                _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(notification.data.ActivityType.Id);

            var activity = activityService.Get(activityId);

            var notificationType = _notificationTypeProvider.Get(notification.item.Type);

            var newValue = _newNotifierDataValueProvider.GetNotifierDataValue(notification.data, activity, notificationType);
            var message = _newNotificationMessageService.GetUiNotificationMessage(notification.item.ReceiverId, notification.data.ActivityType, notificationType, newValue);

            return new NotificationValue
            {
                Message = message.Message,
                Url = notification.data.Url
            };
        }

        private Guid ParseActivityId(string url) => url.ParseIdFromQueryString("id=");

        private bool IsOldNotifierData(OldNotifierData data)
        {
            return data != null && data.Title.IsNotNullOrEmpty() && data.ActivityType != null;
        }
    }
}

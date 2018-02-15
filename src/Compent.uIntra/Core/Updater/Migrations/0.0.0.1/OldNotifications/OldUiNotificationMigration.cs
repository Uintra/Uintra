using System;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;
using Uintra.Notification;

namespace Compent.Uintra.Installer.Migrations
{
    public class OldUiNotificationMigration
    {
        private readonly ISqlRepository<global::Uintra.Notification.Notification> _notificationRepository;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly NewNotifierDataValueProvider _newNotifierDataValueProvider;
        private readonly NewNotificationMessageService _newNotificationMessageService;

        public OldUiNotificationMigration(
            ISqlRepository<global::Uintra.Notification.Notification> notificationRepository,
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
            var parsedNotifications = _notificationRepository
                .GetAll()
                .Select(n => (item: n, data: n.Value.Deserialize<OldNotifierData>()))
                .Where(n => IsOldNotifierData(n.data))
                .Select(UpdateNotificationValue)
                .ToLookup(n => n.isValid);

            var invalidNotifications = parsedNotifications[false].Select(UnpackNotification); 
            var updatedNotifications = parsedNotifications[true].Select(UnpackNotification); // notifications to activities that do not exist

            Notification UnpackNotification((bool isValid, global::Uintra.Notification.Notification notification) arg) => arg.notification;

            _notificationRepository.Update(updatedNotifications);
            _notificationRepository.Delete(invalidNotifications); // we delete notifications that could not be migrated for some reason
        }

        private (bool isValid, global::Uintra.Notification.Notification notification) UpdateNotificationValue((global::Uintra.Notification.Notification notification, OldNotifierData data) item)
        {
            var notification = item.notification;

            NotificationValue newValue;
            try
            {
                newValue = MapToNewNotificationValue(item);
                if (newValue == null)
                    throw new NullReferenceException();
            }
            catch (Exception) // something has gone wrong, so return notification and mark it as invalid
            {
                return (isValid: false, notification);
            }

            notification.Value = newValue.ToJson();
            return (isValid: true, notification);
        }

        private NotificationValue MapToNewNotificationValue((Notification item, OldNotifierData data) notification)
        {
            Guid activityId = ParseActivityId(notification.data.Url);

            var activityService =
                _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(notification.data.ActivityType);

            var activity = activityService.Get(activityId);

            var notificationType = _notificationTypeProvider[notification.item.Type];

            var newValue = _newNotifierDataValueProvider.GetNotifierDataValue(notification.data, activity, notificationType);
            var message = _newNotificationMessageService.GetUiNotificationMessage(
                notification.item.ReceiverId,
                notification.data.ActivityType,
                notificationType,
                newValue);

            return new NotificationValue
            {
                Message = message.Message,
                Url = notification.data.Url
            };
        }

        private Guid ParseActivityId(string url) => url.ParseIdFromQueryString("id=");

        private bool IsOldNotifierData(OldNotifierData data)
        {
            return data?.ActivityType != null;
        }
    }
}

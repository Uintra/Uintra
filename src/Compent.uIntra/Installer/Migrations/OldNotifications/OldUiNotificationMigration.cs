using System;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Helpers;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;

namespace Compent.uIntra.Installer.Migrations
{
    public class OldUiNotificationMigration
    {
        private readonly ISqlRepository<Notification> _notificationRepository;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly NewNotifierDataValueProvider _newNotifierDataValueProvider;
        private readonly NewNotificationMessageService _newNotificationMessageService;

        public OldUiNotificationMigration(INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper, ISqlRepository<Notification> notificationRepository, INotifierDataHelper notifierDataHelper, IActivitiesServiceFactory activitiesServiceFactory, INotificationTypeProvider notificationTypeProvider, IIntranetUserService<IIntranetUser> intranetUserService, INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper1, INotificationSettingsService notificationSettingsService, INotifierTypeProvider notifierTypeProvider, NewNotifierDataValueProvider newNotifierDataValueProvider, NewNotificationMessageService newNotificationMessageService)
        {
            _notificationRepository = notificationRepository;
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
            _newNotifierDataValueProvider = newNotifierDataValueProvider;
            _newNotificationMessageService = newNotificationMessageService;
        }

        private T GetService<T>() => DependencyResolver.Current.GetService<T>();
        
        public void Execute()
        {
            var allNotifications = _notificationRepository.GetAll();

            var mappedNotifications =
                allNotifications.Select(n => (item: n, data: n.Value.Deserialize<OldNotifierData>()));

            var oldNotifications = mappedNotifications
                .Where(n => IsOldNotifierData(n.data))
                .ToList();

            var updatedNotifications = oldNotifications
                .Select(UpdateNotificationValue)
                .ToList();

           // _notificationRepository.Update(updatedNotifications);
        }

        private Notification UpdateNotificationValue((Notification item, OldNotifierData data) notification)
        {
            var item = notification.item;
            item.Value = MapToNewNotificationValue(notification).ToJson();
            return item;
        }

        private NotificationValue MapToNewNotificationValue((Notification item, OldNotifierData data) notification)
        {
            Guid activityId = ParseActivityId(notification.data.Url);

            var activityService =
                _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(notification.data.ActivityType.Id);

            var activity = activityService.Get(activityId);
            if (activity == null) return null;

            var notificationType = _notificationTypeProvider.Get(notification.item.Type);

            INotifierDataValue newValue = _newNotifierDataValueProvider.GetNotifierDataValue(notification.data, activity, notificationType);
            UiNotificationMessage message = _newNotificationMessageService.GetUiNotificationMessage(notification.item.ReceiverId, notification.data.ActivityType, notificationType, newValue);

            return new NotificationValue
            {
                Message = message.Message,
                Url = notification.data.Url
            };
        }


        private Guid ParseActivityId(string url) => url.ParseIdFromQueryString("id=");

        private bool IsOldNotifierData(OldNotifierData data)
        {
            return data.Title.IsNotNullOrEmpty() && data.ActivityType != null;
        }
    }
}

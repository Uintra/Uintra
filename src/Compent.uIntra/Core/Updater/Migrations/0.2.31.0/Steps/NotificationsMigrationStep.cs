using System.Web.Helpers;
using Localization.Umbraco.Extensions;
using Uintra.Core.Persistence;

namespace Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class NotificationsMigrationStep : IMigrationStep
    {
        private readonly ISqlRepository<global::Uintra.Notification.Notification> _notificationsRepository;

        public NotificationsMigrationStep(ISqlRepository<global::Uintra.Notification.Notification> notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public ExecutionResult Execute()
        {
            var notifications = _notificationsRepository.GetAll();
            foreach (var notification in notifications)
            {
                var notificationData = Json.Decode(notification.Value);

                var isPinned = notificationData.IsPinned ?? false;
                var isPinActual = notificationData.IsPinActual ?? false;

                notification.Value = new
                    {
                        notificationData.Message,
                        notificationData.Url,
                        notificationData.NotifierId,
                        IsPinned = isPinned,
                        IsPinActual = isPinActual
                    }
                    .ToJson();
            }

            _notificationsRepository.Update(notifications);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            var notifications = _notificationsRepository.GetAll();
            foreach (var notification in notifications)
            {
                var notificationData = Json.Decode(notification.Value);

                notification.Value = new
                    {
                        notificationData.Message,
                        notificationData.Url,
                        notificationData.NotifierId
                    }
                    .ToJson();
            }

            _notificationsRepository.Update(notifications);
        }
    }
}
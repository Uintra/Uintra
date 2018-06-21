using Compent.Extensions;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Notification
{
    public class UserTaggingNotificationService : IUserTaggingNotificationService
    {
        private readonly INotificationsService _notificationService;

        public UserTaggingNotificationService(
            INotificationsService notificationService)
        {
            _notificationService = notificationService;
        }

        public void SendNotification(UserTaggingNotificationModel model)
        {
            var notificationType = NotificationTypeEnum.UserTagging;
            foreach (var receivedId in model.ReceivedIds)
            {
                var notifierData = new NotifierData
                {
                    NotificationType = notificationType,
                    ReceiverIds = receivedId.ToEnumerable(),
                    Value = new UserTaggingNotifierDataModel
                    {
                        Title = model.Title,
                        Url = model.Url,
                        NotifierId = model.CreatorId,
                        ReceiverId = receivedId,
                        NotificationType = notificationType
                    }
                };

                _notificationService.ProcessNotification(notifierData);
            }
        }
    }
}
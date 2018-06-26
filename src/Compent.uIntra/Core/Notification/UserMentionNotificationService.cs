using Compent.Extensions;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Notification
{
    public class UserMentionNotificationService : IUserMentionNotificationService
    {
        private readonly INotificationsService _notificationService;

        public UserMentionNotificationService(
            INotificationsService notificationService)
        {
            _notificationService = notificationService;
        }

        public void SendNotification(UserMentionNotificationModel model)
        {
            var notificationType = NotificationTypeEnum.UserMention;
            foreach (var receivedId in model.ReceivedIds)
            {
                var notifierData = new NotifierData
                {
                    NotificationType = notificationType,
                    ReceiverIds = receivedId.ToEnumerable(),
                    Value = new UserMentionNotifierDataModel
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
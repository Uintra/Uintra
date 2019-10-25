using System;
using System.Linq;
using Compent.Extensions;
using Uintra.Core.Extensions;
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
            const NotificationTypeEnum notificationType = NotificationTypeEnum.UserMention;
            foreach (var receivedId in model.ReceivedIds)
            {
                if (SkipNotification(receivedId, model.MentionedSourceId))
                {
                    continue;
                }
                var notifierData = new NotifierData
                {
                    NotificationType = notificationType,
                    ActivityType = model.ActivityType,
                    ReceiverIds = receivedId.ToEnumerable(),
                    Value = new UserMentionNotifierDataModel
                    {
                        MentionedSourceId = model.MentionedSourceId,
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

        private bool SkipNotification(Guid userId, Guid sourceId) => 
            _notificationService
            .GetUserNotifications(userId, NotificationTypeEnum.UserMention.ToInt())
            .Any(mention => mention.Value.Contains(sourceId.ToString()));
    }
}
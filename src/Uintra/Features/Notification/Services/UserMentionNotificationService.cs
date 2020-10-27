using System;
using System.Linq;
using Compent.Extensions;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Entities;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Notification.Services
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
                        Url = model.Url.ToLinkModel(),
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
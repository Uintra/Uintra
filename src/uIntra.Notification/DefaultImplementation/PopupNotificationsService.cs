using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    public class PopupNotificationsService : IPopupNotificationService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;

        public PopupNotificationsService(ISqlRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }


        public void Notify(IEnumerable<PopupNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            _notificationRepository.Add(notifications);
        }

        public void ViewNotification(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            notification.IsViewed = true;
            _notificationRepository.Update(notification);
        }

        public IEnumerable<Notification> Get(Guid receiverId)
        {
            var notifications = _notificationRepository.FindAll(n => n.ReceiverId == receiverId)
                .Where(n => PopupNotificationTypeIds.Contains(n.Type));

            return notifications;
        }

        private IEnumerable<int> PopupNotificationTypeIds => new List<int>() { NotificationTypeEnum.Welcome.ToInt() };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;

namespace uIntra.Notification
{
    public class UiNotificationService : IUiNotificationService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;

        public UiNotificationService(ISqlRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public IEnumerable<Notification> GetMany(Guid receiverId, int count, out int totalCount)
        {
            var allNotifications = _notificationRepository
                .FindAll(el => el.ReceiverId == receiverId)
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date)
                .ToList();

            totalCount = allNotifications.Count;

            return allNotifications.Take(count);
        }

        public void Notify(IEnumerable<UiNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = false,
                    IsViewed = false,
                    Type = el.NotificationType.Id,
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            _notificationRepository.Add(notifications);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return (int)_notificationRepository.Count(el => el.ReceiverId == receiverId && !el.IsNotified);
        }

        public void ViewNotification(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            notification.IsViewed = true;
            _notificationRepository.Update(notification);
        }

        public void Notify(IEnumerable<Notification> notifications)
        {
            var notificationsList = notifications.AsList();
            foreach (var n in notificationsList)
            {
                n.IsNotified = true;
            }

            _notificationRepository.Update(notificationsList);
        }
    }
}

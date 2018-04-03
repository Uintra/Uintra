using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;

namespace Uintra.Notification
{
    public class UiNotificationService : IUiNotificationService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public UiNotificationService(ISqlRepository<Notification> notificationRepository, INotificationTypeProvider notificationTypeProvider)
        {
            _notificationRepository = notificationRepository;
            _notificationTypeProvider = notificationTypeProvider;
        }
        public IEnumerable<Notification> GetMany(Guid receiverId, int count, out int totalCount)
        {
            var allNotifications = GetNotifications(receiverId)
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
                    Type = el.NotificationType.ToInt(),
                    Value = new { el.Message, el.Url, el.NotifierId, el.IsPinned, el.IsPinActual }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            _notificationRepository.Add(notifications);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return GetNotifications(receiverId).Count(el => !el.IsNotified);
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

        private IEnumerable<Notification> GetNotifications(Guid receiverId)
        {
            var uiNotificationTypeIds = _notificationTypeProvider.UiNotificationTypes().Select(t => t.ToInt());
            return _notificationRepository.FindAll(el => el.ReceiverId == receiverId && uiNotificationTypeIds.Contains(el.Type));
        }
    }
}

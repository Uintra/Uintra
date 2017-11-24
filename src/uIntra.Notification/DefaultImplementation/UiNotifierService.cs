using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class UiNotifierService : IUiNotifierService
    {
        private readonly ISqlRepository<Notification> _notificationRepository;

        public NotifierTypeEnum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(ISqlRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<Notification> GetMany(Guid receiverId, int count, out int totalCount)
        {
            var allNotifications = _notificationRepository
                .FindAll(el => el.ReceiverId == receiverId)
                .OrderBy(n => n.IsNotified)
                .ThenByDescending(n => n.Date);

            totalCount = allNotifications.Count();

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
                    Value = new {el.Message, el.Url}.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            _notificationRepository.Add(notifications);
        }

        public void Notify(IEnumerable<Notification> notifications)
        {
            foreach (var n in notifications)
            {
                n.IsNotified = true;
            }

            _notificationRepository.Update(notifications);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return (int) _notificationRepository.Count(el => el.ReceiverId == receiverId && !el.IsNotified);
        }

        public void ViewNotification(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            notification.IsViewed = true;
            _notificationRepository.Update(notification);
        }
    }
}
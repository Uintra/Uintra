using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Extentions;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities;
using uCommunity.Notification.Core.Services;

namespace uCommunity.Notification
{
    public class UiNotifierService : IUiNotifierService
    {
        private readonly ISqlRepository<Core.Sql.Notification> _notificationRepository;

        public NotifierTypeEnum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(ISqlRepository<Core.Sql.Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void Notify(NotifierData data)
        {
            var notifications = data.ReceiverIds
                .Select(el=> new Core.Sql.Notification()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                IsNotified = false,
                IsViewed = false,
                Type =  data.NotificationType,
                Value = data.Value.ToJson(),
                ReceiverId = el
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

        public IEnumerable<Core.Sql.Notification> GetByReceiver(Guid receiverId, int count, out int totalCount)
        {
            var allNotifications = _notificationRepository
                                        .FindAll(el => el.ReceiverId == receiverId)
                                        .OrderBy(n => !n.IsNotified)
                                        .ThenByDescending(n => n.Date);
            totalCount = allNotifications.Count();

            var result = allNotifications.Take(count).ToList();

            if (result.Any(n => !n.IsNotified))
            {
                var notNotified = result.Where(n => !n.IsNotified).ToList();
                notNotified.ForEach(n => n.IsNotified = true);
                _notificationRepository.Update(notNotified);
            }

            return result;
        }
    }
}
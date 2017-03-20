using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Extentions;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Notification.Notifier
{
    public class UiNotifierService : IUiNotifierService
    {
        private readonly ISqlRepository<Sql.Notification> _notificationRepository;

        public NotifierTypeEnum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(ISqlRepository<Sql.Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void Notify(NotifierData data)
        {
            var notifications = new List<Sql.Notification>();

            foreach (var receiverId in data.ReceiverIds)
            {
                var notification = new Sql.Notification()
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    IsNotified = false,
                    IsViewed = false,                    
                    Type = data.NotificationType,
                    Value = data.Value.ToJson(),
                    ReceiverId = receiverId
                };

                notifications.Add(notification);
            }

            _notificationRepository.Add(notifications);
        }

        public IEnumerable<Sql.Notification> GetNotificationsByReceiver(Guid receiverId)
        {
            return _notificationRepository.FindAll(el => el.ReceiverId == receiverId).OrderBy(n => !n.IsNotified).ThenBy(n => n.Date);
        }

        public int GetNotNotifiedCount(Guid receiverId)
        {
            return _notificationRepository.FindAll(el => el.ReceiverId == receiverId && !el.IsNotified).Count();
        }

        public void ViewNotification(Guid id)
        {
            var notification = _notificationRepository.Get(id);
            notification.IsViewed = true;
            _notificationRepository.Update(notification);
        }

        public void NotifyUser(Guid userId)
        {
            var notifications = _notificationRepository.FindAll(el => el.ReceiverId == userId && !el.IsNotified).ToList();

            foreach (var n in notifications)
            {
                n.IsNotified = true;
                _notificationRepository.Update(n);


            }

        }
    }
}
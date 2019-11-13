using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Persistence;

namespace Uintra20.Core.Notification
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
                    IsNotified = true,
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    NotifierType = NotifierTypeEnum.PopupNotifier.ToInt(),
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
            var popupNotifierTypeId = NotifierTypeEnum.PopupNotifier.ToInt();
            var notifications = _notificationRepository.FindAll(n => n.ReceiverId == receiverId && !n.IsViewed && n.NotifierType == popupNotifierTypeId);

            return notifications;
        }

        public async Task NotifyAsync(IEnumerable<PopupNotificationMessage> messages)
        {
            var notifications = messages
                .Select(el => new Notification
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    IsNotified = true,
                    IsViewed = false,
                    Type = el.NotificationType.ToInt(),
                    NotifierType = NotifierTypeEnum.PopupNotifier.ToInt(),
                    Value = new { el.Message }.ToJson(),
                    ReceiverId = el.ReceiverId
                });

            await _notificationRepository.AddAsync(notifications);
        }

        public async Task ViewNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.GetAsync(id);
            notification.IsViewed = true;
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetAsync(Guid receiverId)
        {
            var popupNotifierTypeId = NotifierTypeEnum.PopupNotifier.ToInt();
            var notifications =await _notificationRepository.FindAllAsync(n => n.ReceiverId == receiverId && !n.IsViewed && n.NotifierType == popupNotifierTypeId);

            return notifications;
        }
    }
}
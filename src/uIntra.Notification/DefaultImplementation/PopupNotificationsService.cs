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
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public PopupNotificationsService(ISqlRepository<Notification> notificationRepository, INotificationTypeProvider notificationTypeProvider)
        {
            _notificationRepository = notificationRepository;
            _notificationTypeProvider = notificationTypeProvider;
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

    }
}

using System;
using System.Collections.Generic;

namespace Uintra.Notification
{
    public interface IUiNotificationService
    {
        IEnumerable<Notification> GetMany(Guid receiverId, int count, out int totalCount);
        void Notify(IEnumerable<Notification> notifications);
        void Notify(IEnumerable<UiNotificationMessage> messages);
        int GetNotNotifiedCount(Guid receiverId);
        IEnumerable<Notification> GetNotNotifiedNotifications(Guid receiverId);
        void ViewNotification(Guid id);
        void SetNotificationAsNotified(Guid id);
    }
}

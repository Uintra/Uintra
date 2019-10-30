using System;
using System.Collections.Generic;

namespace Uintra.Notification
{
    public interface IUiNotificationService
    {
        (IEnumerable<Notification> notifications, int totalCount) GetMany(Guid receiverId, int count);
        void Notify(IEnumerable<Notification> notifications);
        void Notify(IEnumerable<UiNotificationMessage> messages);
        int GetNotNotifiedCount(Guid receiverId);
        IList<Notification> GetNotNotifiedNotifications(Guid receiverId);
        void ViewNotification(Guid id);
        bool SetNotificationAsNotified(Guid id);
    }
}

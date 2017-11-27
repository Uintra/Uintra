using System;
using System.Collections.Generic;

namespace uIntra.Notification
{
    public interface IUiNotificationService
    {
        IEnumerable<Notification> GetMany(Guid receiverId, int count, out int totalCount);
        void Notify(IEnumerable<Notification> notifications);
        void Notify(IEnumerable<UiNotificationMessage> messages);
        int GetNotNotifiedCount(Guid receiverId);
        void ViewNotification(Guid id);
    }
}

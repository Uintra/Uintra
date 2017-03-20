using System;
using System.Collections.Generic;

namespace uCommunity.Notification.Notifier
{
    public interface IUiNotifierService : INotifierService
    {
        IEnumerable<Sql.Notification> GetNotificationsByReceiver(Guid receiverId);

        int GetNotNotifiedCount(Guid receiverId);

        void ViewNotification(Guid id);

        void NotifyUser(Guid userId);
    }
}

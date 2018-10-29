using System;
using System.Collections.Generic;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);

        IEnumerable<Notification> GetUserNotifications(Guid userId, int notificationTypeId);
    }
}

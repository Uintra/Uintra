using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Core.Notification.Base;

namespace Uintra20.Core.Notification
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
        IEnumerable<Notification> GetUserNotifications(Guid userId, int notificationTypeId);
        Task ProcessNotificationAsync(NotifierData data);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, int notificationTypeId);
    }
}

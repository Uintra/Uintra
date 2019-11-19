using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Entities.Base;

namespace Uintra20.Features.Notification.Services
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
        IEnumerable<Sql.Notification> GetUserNotifications(Guid userId, int notificationTypeId);
        Task ProcessNotificationAsync(NotifierData data);
        Task<IEnumerable<Sql.Notification>> GetUserNotificationsAsync(Guid userId, int notificationTypeId);
    }
}

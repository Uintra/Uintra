using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification.Services
{
    public interface IUiNotificationService
    {
        (IEnumerable<Sql.Notification> notifications, int totalCount) GetMany(Guid receiverId, int count);
        void Notify(IEnumerable<Sql.Notification> notifications);
        void Notify(IEnumerable<UiNotificationMessage> messages);
        int GetNotNotifiedCount(Guid receiverId);
        IList<Sql.Notification> GetNotNotifiedNotifications(Guid receiverId);
        void ViewNotification(Guid id);
        bool SetNotificationAsNotified(Guid id);


        Task<(IEnumerable<Sql.Notification> notifications, int totalCount)> GetManyAsync(Guid receiverId, int count);
        Task NotifyAsync(IEnumerable<Sql.Notification> notifications);
        Task NotifyAsync(IEnumerable<UiNotificationMessage> messages);
        Task<int> GetNotNotifiedCountAsync(Guid receiverId);
        Task<IList<Sql.Notification>> GetNotNotifiedNotificationsAsync(Guid receiverId);
        Task ViewNotificationAsync(Guid id);
        Task<bool> SetNotificationAsNotifiedAsync(Guid id);
    }
}

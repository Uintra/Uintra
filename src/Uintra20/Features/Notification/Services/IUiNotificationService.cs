using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification.Services
{
    public interface IUiNotificationService
    {
        IEnumerable<Sql.Notification> GetMany(Guid receiverId);
        void Notify(IEnumerable<Sql.Notification> notifications);
        void Notify(IEnumerable<UiNotificationMessage> messages);
        int GetNotNotifiedCount(Guid receiverId);
        IEnumerable<Sql.Notification> GetNotNotified(Guid receiverId);
        IEnumerable<Sql.Notification> GetNotNotifiedNotifications(Guid receiverId);
        void ViewNotification(Guid id);
        bool SetNotificationAsNotified(Guid id);


        Task<IEnumerable<Sql.Notification>> GetManyAsync(Guid receiverId);
        Task NotifyAsync(IEnumerable<Sql.Notification> notifications);
        Task NotifyAsync(IEnumerable<UiNotificationMessage> messages);
        Task<int> GetNotNotifiedCountAsync(Guid receiverId);
        Task <IEnumerable<Sql.Notification>> GetNotNotifiedAsync(Guid receiverId);
        Task<IEnumerable<Sql.Notification>> GetNotNotifiedNotificationsAsync(Guid receiverId);
        Task ViewNotificationAsync(Guid id);
        Task<bool> SetNotificationAsNotifiedAsync(Guid id);
    }
}

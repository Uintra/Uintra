using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.Notification
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


        Task<(IEnumerable<Notification> notifications, int totalCount)> GetManyAsync(Guid receiverId, int count);
        Task NotifyAsync(IEnumerable<Notification> notifications);
        Task NotifyAsync(IEnumerable<UiNotificationMessage> messages);
        Task<int> GetNotNotifiedCountAsync(Guid receiverId);
        Task<IList<Notification>> GetNotNotifiedNotificationsAsync(Guid receiverId);
        Task ViewNotificationAsync(Guid id);
        Task<bool> SetNotificationAsNotifiedAsync(Guid id);
    }
}

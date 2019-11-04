using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.Notification
{
    public interface IPopupNotificationService
    {
        void Notify(IEnumerable<PopupNotificationMessage> messages);
        void ViewNotification(Guid id);
        IEnumerable<Notification> Get(Guid receiverId);

        Task NotifyAsync(IEnumerable<PopupNotificationMessage> messages);
        Task ViewNotificationAsync(Guid id);
        Task<IEnumerable<Notification>> GetAsync(Guid receiverId);
    }
}

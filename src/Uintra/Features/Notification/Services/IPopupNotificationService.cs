using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Features.Notification.Models;

namespace Uintra.Features.Notification.Services
{
    public interface IPopupNotificationService
    {
        void Notify(IEnumerable<PopupNotificationMessage> messages);
        void ViewNotification(Guid id);
        IEnumerable<Sql.Notification> Get(Guid receiverId);

        Task NotifyAsync(IEnumerable<PopupNotificationMessage> messages);
        Task ViewNotificationAsync(Guid id);
        Task<IEnumerable<Sql.Notification>> GetAsync(Guid receiverId);
    }
}

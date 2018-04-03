using System;
using System.Collections.Generic;

namespace Uintra.Notification
{
    public interface IPopupNotificationService
    {
        void Notify(IEnumerable<PopupNotificationMessage> messages);
        void ViewNotification(Guid id);
        IEnumerable<Notification> Get(Guid receiverId);
    }
}

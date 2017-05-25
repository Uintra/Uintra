using System;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface INotifyableService
    {
        void Notify(Guid entityId, NotificationTypeEnum notificationType);
    }
}

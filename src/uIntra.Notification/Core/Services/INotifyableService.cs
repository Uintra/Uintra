using System;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Services
{
    public interface INotifyableService
    {
        void Notify(Guid entityId, NotificationTypeEnum notificationType);
    }
}

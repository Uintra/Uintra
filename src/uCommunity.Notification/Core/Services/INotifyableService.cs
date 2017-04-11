using System;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Services
{
    public interface INotifyableService
    {
        void Notify(Guid entityId, NotificationTypeEnum notificationType);
    }
}

using System;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface INotifyableService
    {
        void Notify(Guid entityId, IIntranetType notificationType);
    }
}

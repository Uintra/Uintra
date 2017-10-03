using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public interface INotifyableService
    {
        void Notify(Guid entityId, IIntranetType notificationType);
    }
}

using System;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification
{
    public interface INotifyableService : ITypedService
    {
        void Notify(Guid entityId, IIntranetType notificationType);
    }
}

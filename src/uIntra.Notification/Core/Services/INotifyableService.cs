using System;
using uIntra.Core.Activity;

namespace uIntra.Notification
{
    public interface INotifyableService : ITypedService
    {
        void Notify(Guid entityId, Enum notificationType);
    }
}

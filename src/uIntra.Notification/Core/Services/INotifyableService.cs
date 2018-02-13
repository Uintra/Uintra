using System;
using Uintra.Core.Activity;

namespace Uintra.Notification
{
    public interface INotifyableService : ITypedService
    {
        void Notify(Guid entityId, Enum notificationType);
    }
}

using System;
using Uintra20.Core.Activity;

namespace Uintra20.Core.Notification
{
    public interface INotifyableService : ITypedService
    {
        void Notify(Guid entityId, Enum notificationType);
    }
}
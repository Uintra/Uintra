using System;
using System.Threading.Tasks;
using Uintra.Core.Activity;

namespace Uintra.Features.Notification.Services
{
    public interface INotifyableService : ITypedService
    {
        void Notify(Guid entityId, Enum notificationType);
        Task NotifyAsync(Guid entityId, Enum notificationType);
    }
}
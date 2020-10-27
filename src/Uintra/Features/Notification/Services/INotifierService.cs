using System;
using System.Threading.Tasks;
using Uintra.Features.Notification.Entities.Base;

namespace Uintra.Features.Notification.Services
{
    public interface INotifierService
    {
        Enum Type { get; }
        void Notify(NotifierData data);
        Task NotifyAsync(NotifierData data);
    }
}

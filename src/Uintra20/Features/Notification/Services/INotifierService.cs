using System;
using System.Threading.Tasks;
using Uintra20.Features.Notification.Entities.Base;

namespace Uintra20.Features.Notification.Services
{
    public interface INotifierService
    {
        Enum Type { get; }
        void Notify(NotifierData data);
        Task NotifyAsync(NotifierData data);
    }
}

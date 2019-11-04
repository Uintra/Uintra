using System;
using System.Threading.Tasks;
using Uintra20.Core.Notification.Base;

namespace Uintra20.Core.Notification
{
    public interface INotifierService
    {
        Enum Type { get; }
        void Notify(NotifierData data);
        Task NotifyAsync(NotifierData data);
    }
}

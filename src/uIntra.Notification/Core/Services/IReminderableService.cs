using System;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

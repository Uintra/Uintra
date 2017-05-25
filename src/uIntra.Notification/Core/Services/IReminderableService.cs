using System;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

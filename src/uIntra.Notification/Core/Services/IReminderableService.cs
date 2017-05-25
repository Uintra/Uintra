using System;
using uIntra.Notification.Core.Entities.Base;

namespace uIntra.Notification.Core.Services
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

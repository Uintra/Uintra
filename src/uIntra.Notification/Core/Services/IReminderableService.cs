using System;
using uCommunity.Notification.Core.Entities.Base;

namespace uCommunity.Notification.Core.Services
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

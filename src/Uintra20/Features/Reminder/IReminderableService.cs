using System;
using Uintra20.Features.Notification.Entities.Base;

namespace Uintra20.Features.Reminder
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

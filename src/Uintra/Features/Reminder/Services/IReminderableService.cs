using System;
using Uintra.Features.Notification.Entities.Base;

namespace Uintra.Features.Reminder.Services
{
    public interface IReminderableService<out T> where T : IReminderable
    {
        T Get(Guid id);
        T GetActual(Guid id);
    }
}

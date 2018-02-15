using System;

namespace Uintra.Notification.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }

        DateTime StartDate { get; }
    }
}

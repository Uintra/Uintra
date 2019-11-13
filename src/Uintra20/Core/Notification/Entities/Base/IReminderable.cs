using System;

namespace Uintra20.Core.Notification.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }
        DateTime StartDate { get; }
    }
}

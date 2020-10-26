using System;

namespace Uintra.Features.Notification.Entities.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }
        DateTime StartDate { get; }
    }
}

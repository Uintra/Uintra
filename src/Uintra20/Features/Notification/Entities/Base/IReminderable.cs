using System;

namespace Uintra20.Features.Notification.Entities.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }
        DateTime StartDate { get; }
    }
}

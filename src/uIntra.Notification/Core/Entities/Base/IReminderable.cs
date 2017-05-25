using System;

namespace uIntra.Notification.Core.Entities.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }

        DateTime StartDate { get; }
    }
}

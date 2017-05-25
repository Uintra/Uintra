using System;

namespace uIntra.Notification.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }

        DateTime StartDate { get; }
    }
}

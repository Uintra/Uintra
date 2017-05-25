using System;

namespace uCommunity.Notification.Core.Entities.Base
{
    public interface IReminderable
    {
        Guid Id { get; set; }

        DateTime StartDate { get; }
    }
}

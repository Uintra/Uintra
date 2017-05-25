using System;

namespace uCommunity.Notification.Core.Entities
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

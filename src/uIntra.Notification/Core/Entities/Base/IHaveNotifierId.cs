using System;

namespace uIntra.Notification.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

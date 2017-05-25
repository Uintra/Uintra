using System;

namespace uIntra.Notification.Core.Entities.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

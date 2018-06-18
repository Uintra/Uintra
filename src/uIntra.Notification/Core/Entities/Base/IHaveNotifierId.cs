using System;

namespace Uintra.Notification.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

using System;

namespace Uintra20.Core.Notification.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

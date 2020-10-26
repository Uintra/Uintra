using System;

namespace Uintra.Features.Notification.Entities.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

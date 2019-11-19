using System;

namespace Uintra20.Features.Notification.Entities.Base
{
    public interface IHaveNotifierId
    {
        Guid NotifierId { get; set; }
    }
}

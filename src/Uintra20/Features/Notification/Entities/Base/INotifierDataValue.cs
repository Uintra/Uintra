using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Notification.Entities.Base
{
    public interface INotifierDataValue
    {
        UintraLinkModel Url { get; set; }
        bool IsPinned { get; set; }
        bool IsPinActual { get; set; }
    }
}

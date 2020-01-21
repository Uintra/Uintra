using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Groups.Helpers
{
    public interface IGroupHelper
    {
        bool IsGroupPage(IPublishedContent currentPage);
        bool IsGroupRoomPage(IPublishedContent currentPage);
    }
}

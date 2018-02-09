using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupHelper
    {
        bool IsGroupPage(IPublishedContent currentPage);
        bool IsGroupRoomPage(IPublishedContent currentPage);
    }
}
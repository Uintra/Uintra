using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupHelper
    {
        bool IsGroupPage(IPublishedContent currentPage);
        bool IsGroupRoomPage(IPublishedContent currentPage);
    }
}
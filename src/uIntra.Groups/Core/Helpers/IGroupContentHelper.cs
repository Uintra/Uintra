using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupContentHelper
    {
        IPublishedContent GetMyGroupsOverviewPage();
        IPublishedContent GetDeactivatedGroupPage();
        IPublishedContent GetGroupRoomPage();
        IPublishedContent GetCreateGroupPage();
        IPublishedContent GetOverviewPage();
        IPublishedContent GetEditPage();
    }
}
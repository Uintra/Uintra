using uIntra.CentralFeed.Providers;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupContentProvider : IFeedContentProvider
    {
        IPublishedContent GetMyGroupsOverviewPage();
        IPublishedContent GetDeactivatedGroupPage();
        IPublishedContent GetGroupRoomPage();
        IPublishedContent GetCreateGroupPage();
        IPublishedContent GetEditPage();
    }
}
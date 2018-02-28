using Uintra.CentralFeed.Providers;
using Umbraco.Core.Models;

namespace Uintra.Groups
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
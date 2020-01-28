using Uintra20.Features.CentralFeed.Providers;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Groups.ContentServices
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

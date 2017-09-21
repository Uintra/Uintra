using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupContentHelper
    {
        IPublishedContent GetGroupRoomPage();
        IPublishedContent GetCreateGroupPage();
        IPublishedContent GetOverviewPage();
        IPublishedContent GetEditPage();
        bool IsGroupRoomPage(IPublishedContent currentPage);

        IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId);
        ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentContent, Guid groupId);

        IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentContent, IIntranetUser user,  Guid groupId);

        IIntranetType GetTabType(IPublishedContent content);
        bool IsGroupPage(IPublishedContent currentPage);
        IPublishedContent GetMyGroupsOverviewPage();
        IPublishedContent GetDeactivatedGroupPage();
    }
}
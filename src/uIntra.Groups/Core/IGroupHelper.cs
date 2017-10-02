using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupHelper
    {
        bool IsGroupRoomPage(IPublishedContent currentPage);

        IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId);
        ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentContent, Guid groupId);

        IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentContent, IIntranetUser user,  Guid groupId);

        IIntranetType GetActivityTypeFromPlugin(IPublishedContent content, string pluginAlias);
        IIntranetType GetGroupFeedTabType(IPublishedContent content);
        IIntranetType GetCreateActivityType(IPublishedContent content);

        bool IsGroupPage(IPublishedContent currentPage);
    }

    public interface IGroupContentHelper
    {
        IPublishedContent GetMyGroupsOverviewPage();
        IPublishedContent GetDeactivatedGroupPage();
        IPublishedContent GetGroupRoomPage();
        IPublishedContent GetCreateGroupPage();
        IPublishedContent GetOverviewPage();
        IPublishedContent GetEditPage();
    }

    public interface IGroupLinkProvider
    {
        string GetGroupLink(Guid groupId);
        string GetDeactivatedGroupLink(Guid groupId);

        string GetGroupsOverviewLink();
        string GetCreateGroupLink();
    }
}
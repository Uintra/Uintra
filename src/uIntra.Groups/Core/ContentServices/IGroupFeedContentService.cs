using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.User;
using Umbraco.Core.Models;

namespace uIntra.Groups
{
    public interface IGroupFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentPage, IIntranetUser user, Guid groupId);
        ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentPage, Guid groupId);
        IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentPage, IIntranetUser user, Guid groupId);
    }
}
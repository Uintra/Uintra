using System;
using System.Collections.Generic;
using Uintra.CentralFeed;
using Uintra.Core.User;
using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentPage, IIntranetUser user, Guid groupId);
        ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentPage, Guid groupId);
        IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentPage, IIntranetUser user, Guid groupId);
    }
}
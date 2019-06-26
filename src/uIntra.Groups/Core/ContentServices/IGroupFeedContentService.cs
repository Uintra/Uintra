using System;
using System.Collections.Generic;
using Uintra.CentralFeed;
using Uintra.CentralFeed.Navigation.Models;
using Uintra.Core.User;
using Umbraco.Core.Models;

namespace Uintra.Groups
{
    public interface IGroupFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentPage, IIntranetMember member, Guid groupId);
        ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentPage, Guid groupId);
        IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentPage, Guid groupId);
    }
}
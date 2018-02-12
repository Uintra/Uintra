using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
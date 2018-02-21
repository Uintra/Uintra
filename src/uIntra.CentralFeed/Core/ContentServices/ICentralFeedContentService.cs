using System.Collections.Generic;
using Uintra.CentralFeed.Navigation.Models;
using Umbraco.Core.Models;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
using System.Collections.Generic;
using Uintra20.Features.Navigation.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
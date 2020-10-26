using System.Collections.Generic;
using Uintra.Core.Feed.Services;
using Uintra.Features.Navigation.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.CentralFeed.Services
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
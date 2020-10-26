using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.CentralFeed.Providers
{
    public interface IFeedContentProvider
    {
        IPublishedContent GetOverviewPage();
        IEnumerable<IPublishedContent> GetRelatedPages();
    }
}

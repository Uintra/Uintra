using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.CentralFeed.Providers
{
    public interface IFeedContentProvider
    {
        IPublishedContent GetOverviewPage();
        IEnumerable<IPublishedContent> GetRelatedPages();
    }
}
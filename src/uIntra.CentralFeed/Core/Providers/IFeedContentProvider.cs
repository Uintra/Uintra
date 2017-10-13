using Umbraco.Core.Models;

namespace uIntra.CentralFeed.Providers
{
    public interface IFeedContentProvider
    {
        IPublishedContent GetOverviewPage();
    }
}
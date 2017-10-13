using Umbraco.Core.Models;

namespace uIntra.CentralFeed.Providers
{
    public interface ICentralFeedContentProvider
    {
        IPublishedContent GetOverviewPage();
    }
}
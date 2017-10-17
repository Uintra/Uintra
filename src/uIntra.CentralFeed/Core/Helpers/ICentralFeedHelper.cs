using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedHelper
    {
        bool IsCentralFeedPage(IPublishedContent page);
    }
}
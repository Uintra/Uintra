using Umbraco.Core.Models;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedHelper
    {
        bool IsCentralFeedPage(IPublishedContent page);
    }
}
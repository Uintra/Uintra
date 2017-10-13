using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface IFeedContentHelper
    {
        IIntranetType GetCreateActivityType(IPublishedContent content);
        IIntranetType GetFeedTabType(IPublishedContent content);
    }
}
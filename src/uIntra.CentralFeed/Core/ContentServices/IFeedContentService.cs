using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface IFeedContentService
    {
        IIntranetType GetCreateActivityType(IPublishedContent content);
        IIntranetType GetFeedTabType(IPublishedContent content);
    }
}
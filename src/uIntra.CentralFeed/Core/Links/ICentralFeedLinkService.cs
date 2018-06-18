using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(IIntranetType activityType);
    }
}

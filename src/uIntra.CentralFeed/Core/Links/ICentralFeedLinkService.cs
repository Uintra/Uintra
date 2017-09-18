using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedLinkService : IActivityLinkService
    {
        ActivityCreateLinks GetCreateLinks(IIntranetType activityType);
    }
}

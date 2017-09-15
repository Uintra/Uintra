using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
        public interface ICentralFeedLinkService
        {
            ActivityLinks GetLinks(IFeedItem item);
            ActivityCreateLinks GetCreateLinks(IIntranetType activityType);
        }
}

using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
        public interface ICentralFeedLinkService
        {
            ActivityLinks GetLinks(IFeedItem item);
            ActivityLinks GetCreateLinks();
        }
}

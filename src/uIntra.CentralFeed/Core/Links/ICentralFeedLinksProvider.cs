using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedLinksProvider
    {
        IActivityLinks GetLinks(ActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

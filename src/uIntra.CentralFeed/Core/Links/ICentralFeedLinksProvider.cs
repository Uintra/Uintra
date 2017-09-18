using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedLinksProvider
    {
        ActivityLinks GetLinks(ActivityTransferModel activity);
        ActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

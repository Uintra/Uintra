using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedLinkProvider
    {
        IActivityLinks GetLinks(ActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

using Uintra20.Core.Links;

namespace Uintra20.Core.CentralFeed
{
    public interface ICentralFeedLinkProvider
    {
        IActivityLinks GetLinks(ActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

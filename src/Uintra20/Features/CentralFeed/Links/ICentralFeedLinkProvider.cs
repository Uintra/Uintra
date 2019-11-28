using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.CentralFeed.Links
{
    public interface ICentralFeedLinkProvider
    {
        IActivityLinks GetLinks(ActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

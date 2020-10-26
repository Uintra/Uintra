using Uintra.Features.CentralFeed.Models;
using Uintra.Features.Links.Models;

namespace Uintra.Features.CentralFeed.Links
{
    public interface ICentralFeedLinkProvider
    {
        IActivityLinks GetLinks(ActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model);
    }
}

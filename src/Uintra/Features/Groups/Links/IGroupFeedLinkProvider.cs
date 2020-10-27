using Uintra.Features.Links.Models;

namespace Uintra.Features.Groups.Links
{
    public interface IGroupFeedLinkProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}

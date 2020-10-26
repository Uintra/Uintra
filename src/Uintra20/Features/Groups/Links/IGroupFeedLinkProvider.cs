using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Groups.Links
{
    public interface IGroupFeedLinkProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}

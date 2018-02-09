using Uintra.Core.Links;

namespace Uintra.Groups
{
    public interface IGroupFeedLinkProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}
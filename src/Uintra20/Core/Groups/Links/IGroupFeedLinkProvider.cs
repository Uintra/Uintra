using Uintra20.Core.Links;

namespace Uintra20.Core.Groups
{
    public interface IGroupFeedLinkProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}

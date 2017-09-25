using uIntra.Core.Links;

namespace uIntra.Groups
{
    public interface IGroupFeedLinksProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}
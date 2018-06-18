using uIntra.Core.Links;

namespace uIntra.Groups
{
    public interface IGroupFeedLinkProvider
    {
        IActivityLinks GetLinks(GroupActivityTransferModel activity);
        IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}
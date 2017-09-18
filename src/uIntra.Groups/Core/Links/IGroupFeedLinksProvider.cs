using uIntra.Core.Links;

namespace uIntra.Groups
{
    public interface IGroupFeedLinksProvider
    {
        ActivityLinks GetLinks(GroupActivityTransferModel activity);
        ActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model);
    }
}
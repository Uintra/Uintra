using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Feed;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Links
{
    public class GroupFeedLinkProvider : FeedLinkProvider, IGroupFeedLinkProvider
    {
        private readonly IGroupLinkProvider _groupLinkProvider;

        public GroupFeedLinkProvider(
            IActivityPageHelper activityPageHelper,
            IProfileLinkProvider profileLinkProvider,
            IGroupLinkProvider groupLinkProvider)
            : base(activityPageHelper, profileLinkProvider)
        {
            _groupLinkProvider = groupLinkProvider;
        }

        public IActivityLinks GetLinks(GroupActivityTransferModel activity)
        {
            return new ActivityLinks
            {
                Feed = _groupLinkProvider.GetGroupRoomLink(activity.GroupId),
                Create = _activityPageHelper.GetCreatePageUrl(activity.Type)?.AddGroupId(activity.GroupId),
                Details = _activityPageHelper.GetDetailsPageUrl(activity.Type, activity.Id).AddGroupId(activity.GroupId),
                Edit = _activityPageHelper.GetEditPageUrl(activity.Type, activity.Id).AddGroupId(activity.GroupId),
                Owner = GetProfileLink(activity.OwnerId),
            };
        }

        public IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model)
        {

            return new ActivityCreateLinks
            {
                Feed = _groupLinkProvider.GetGroupRoomLink(model.GroupId),
                Create = _activityPageHelper.GetCreatePageUrl(model.Type)?.AddGroupId(model.GroupId),
                Owner = GetProfileLink(model.OwnerId),
            };
        }
    }
}
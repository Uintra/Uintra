using System;
using Uintra20.Features.CentralFeed.Models.GroupFeed;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Services;

namespace Uintra20.Core.Activity
{
    public class FeedActivityHelper : IFeedActivityHelper
    {
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;
        private readonly IGroupLinkProvider _groupLinkProvider;

        public FeedActivityHelper(IGroupActivityService groupActivityService, IGroupService groupService, IGroupLinkProvider groupLinkProvider)
        {
            _groupActivityService = groupActivityService;
            _groupService = groupService;
            _groupLinkProvider = groupLinkProvider;
        }

        public GroupInfo? GetGroupInfo(Guid activityId)
        {
            var groupId = _groupActivityService
                .GetGroupId(activityId);

            if (!groupId.HasValue) return null;

            return GetInfoForGroup(groupId.Value);
        }

        private GroupInfo GetInfoForGroup(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            var groupOverviewUrl = _groupLinkProvider.GetGroupRoomLink(groupId);

            return new GroupInfo(
                title: group.Title,
                url: groupOverviewUrl);
        }
    }
}

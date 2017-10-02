using System;
using uIntra.Groups;

namespace Compent.uIntra.Core.Activity
{
    class FeedActivityHelper : IFeedActivityHelper
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

        public GroupInfo? GetGroupInfo(Guid itemId)
        {
            Guid? groupId = _groupActivityService.GetGroupId(itemId);
            GroupInfo? result;

            if (groupId.HasValue)
                result = GetInfoForGroup(groupId.Value);
            else result = null;

            return result;
        }

        private GroupInfo GetInfoForGroup(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            var groupOverviewUrl = _groupLinkProvider.GetGroupLink(groupId);

            return new GroupInfo(
                title: group.Title,
                url: groupOverviewUrl);
        }
    }
}
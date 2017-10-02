using System;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Groups;

namespace Compent.uIntra.Core.Activity
{
    class FeedActivityHelper : IFeedActivityHelper
    {
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;
        private readonly IGroupHelper _groupHelper;

        public FeedActivityHelper(IGroupActivityService groupActivityService, IGroupService groupService, IGroupHelper groupHelper)
        {
            _groupActivityService = groupActivityService;
            _groupService = groupService;
            _groupHelper = groupHelper;
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
            var groupOverviewUrl = _groupHelper
                .GetGroupRoomPage()
                .Url
                .AddGroupId(groupId);

            return new GroupInfo(
                title: group.Title,
                url: groupOverviewUrl);
        }
    }
}
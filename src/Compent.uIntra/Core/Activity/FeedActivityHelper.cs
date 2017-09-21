using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Groups;

namespace Compent.uIntra.Core.Activity
{
    class FeedActivityHelper : IFeedActivityHelper
    {
        private readonly IGroupActivityService _groupActivityService;
        private readonly IGroupService _groupService;
        private readonly IGroupContentHelper _groupContentHelper;

        public FeedActivityHelper(IGroupActivityService groupActivityService, IGroupService groupService, IGroupContentHelper groupContentHelper)
        {
            _groupActivityService = groupActivityService;
            _groupService = groupService;
            _groupContentHelper = groupContentHelper;
        }

        public GroupInfo? GetGroupInfo(IFeedItem item)
        {
            Guid? groupId = _groupActivityService.GetGroupId(item.Id);
            GroupInfo? result;

            if (groupId.HasValue)
                result = GetGroupInfo(groupId.Value);
            else result = null;

            return result;
        }

        private GroupInfo GetGroupInfo(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            var groupOverviewUrl = _groupContentHelper
                .GetGroupRoomPage()
                .Url
                .AddGroupId(groupId);

            return new GroupInfo(
                title: group.Title,
                url: groupOverviewUrl);
        }
    }
}
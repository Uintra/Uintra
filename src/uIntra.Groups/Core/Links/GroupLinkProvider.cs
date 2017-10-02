using System;
using uIntra.Core.Extentions;

namespace uIntra.Groups
{
    public class GroupLinkProvider : IGroupLinkProvider
    {
        private readonly IGroupContentHelper _contentHelper;

        public GroupLinkProvider(IGroupContentHelper contentHelper)
        {
            _contentHelper = contentHelper;
        }

        public string GetGroupLink(Guid groupId)
        {
           return _contentHelper.GetGroupRoomPage().Url.AddGroupId(groupId);
        }

        public string GetDeactivatedGroupLink(Guid groupId)
        {
            return _contentHelper.GetDeactivatedGroupPage().Url.AddGroupId(groupId);
        }

        public string GetGroupsOverviewLink()
        {
            return _contentHelper.GetOverviewPage().Url;
        }

        public string GetCreateGroupLink()
        {
            return _contentHelper.GetCreateGroupPage().Url;
        }
    }
}
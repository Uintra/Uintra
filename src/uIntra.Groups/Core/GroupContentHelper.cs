using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Groups
{
    public class GroupContentHelper : IGroupContentHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGridHelper _gridHelper;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;

        private const string HomePageTypeAlias = "homePage";
        private const string GroupOverviewTypeAlias = "groupOverview";
        private const string GroupRoomTypeAlias = "";
        private const string GroupEditTypeAlias = "";
        private const string MyGroupEditTypeAlias = "";


        public string NewsCreateTypeAlias { get; set; }

        public string EventsCreateTypeAlias { get; set; }

        public GroupContentHelper(
            UmbracoHelper umbracoHelper,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IGridHelper gridHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider)
        {
            _umbracoHelper = umbracoHelper;
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _gridHelper = gridHelper;
            _centralFeedTypeProvider = centralFeedTypeProvider;
        }

        public IPublishedContent GetGroupRoomPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias, GroupRoomTypeAlias));
        }

        public IPublishedContent GetCreateGroupPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias, GroupRoomTypeAlias));
        }

        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias));
        }

        public IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias, GroupRoomTypeAlias, GroupEditTypeAlias));
        }

        public IPublishedContent GetMyGroupsOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias, MyGroupEditTypeAlias));
        }

        public IPublishedContent GetDeactivatedGroupPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePageTypeAlias, GroupOverviewTypeAlias, GroupRoomTypeAlias, GroupRoomTypeAlias));
        }

        public bool IsGroupPage(IPublishedContent currentPage)
        {
            return GetOverviewPage().IsAncestorOrSelf(currentPage);
        }

        public bool IsGroupRoomPage(IPublishedContent currentPage)
        {
            return GetGroupRoomPage().IsAncestorOrSelf(currentPage);
        }

        public IEnumerable<GroupNavigationTab> GetTabs(Guid groupId, IIntranetUser user, IPublishedContent currentContent)
        {
            var groupRoom = GetGroupRoomPage();
            yield return new GroupNavigationTab
            {
                Content = groupRoom,
                Type = GetTabType(groupRoom),
                IsActive = groupRoom.Id == currentContent.Id
            };

            var canEdit = _groupService.CanEdit(groupId, user);
            var memberOfGroup = _groupMemberService.IsGroupMember(groupId, user.Id);
            var editGroupPage = GetEditPage();
            foreach (var content in GetContents())
            {
                if (!canEdit && editGroupPage.Id == content.Id )
                //if (!canEdit && editGroupPage.Id == content.Id || content.IsHideFromSubNavigation())
                {
                        continue;
                }

                var tab = new GroupNavigationTab
                {
                    Content = content,
                    Type = GetTabType(content),
                    IsActive = content.IsAncestorOrSelf(currentContent)
                };

                if (tab.Type != null && memberOfGroup)
                {
                    tab.CreateUrl = content.Children.SingleOrDefault(n => n.DocumentTypeAlias.In(NewsCreateTypeAlias, EventsCreateTypeAlias))?.Url;
                }

                yield return tab;
            }
        }


        public IIntranetType GetTabType(IPublishedContent content)
        {
            var value = _gridHelper.GetValue(content, "custom.GroupCentralFeedOverview");

            if (value == null || value.tabType == null)
            {
                return default(IIntranetType);
            }

            int tabType;
            if (int.TryParse(value.tabType.ToString(), out tabType))
            {
                return _centralFeedTypeProvider.Get(tabType);
            }
            return default(IIntranetType);
        }

        private IEnumerable<IPublishedContent> GetContents()
        {
            return GetGroupRoomPage().Children;
        }
    }
}

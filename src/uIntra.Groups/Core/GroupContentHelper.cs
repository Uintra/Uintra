using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Activity;
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
        private readonly IGridHelper _gridHelper;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupFeedLinkService _groupFeedLinkService;
        private readonly IFeedTypeProvider _feedTypeProvider;

        public GroupContentHelper(
            UmbracoHelper umbracoHelper,
            IGroupService groupService,
            IGridHelper gridHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGroupFeedLinkService groupFeedLinkService,
            IFeedTypeProvider feedTypeProvider)
        {
            _umbracoHelper = umbracoHelper;
            _groupService = groupService;
            _gridHelper = gridHelper;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupFeedLinkService = groupFeedLinkService;
            _feedTypeProvider = feedTypeProvider;
        }

        public IPublishedContent GetGroupRoomPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage(), _documentTypeAliasProvider.GetGroupRoomPage()));
        }

        public IPublishedContent GetCreateGroupPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage(), _documentTypeAliasProvider.GetGroupCreatePage()));
        }

        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage()));
        }

        public IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage(), _documentTypeAliasProvider.GetGroupRoomPage(), _documentTypeAliasProvider.GetGroupEditPage()));
        }

        public IPublishedContent GetMyGroupsOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage(), _documentTypeAliasProvider.GetGroupMyGroupsOverviewPage()));
        }

        public IPublishedContent GetDeactivatedGroupPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(),
                _documentTypeAliasProvider.GetGroupOverviewPage(), _documentTypeAliasProvider.GetGroupRoomPage(), _documentTypeAliasProvider.GetGroupDeactivatedPage()));
        }

        public bool IsGroupPage(IPublishedContent currentPage)
        {
            return GetOverviewPage().IsAncestorOrSelf(currentPage);
        }

        public bool IsGroupRoomPage(IPublishedContent currentPage)
        {
            return GetGroupRoomPage().IsAncestorOrSelf(currentPage);
        }

        public IEnumerable<FeedTabModel> GetTabs(Guid groupId, IIntranetUser user, IPublishedContent currentContent)
        {
            var groupRoom = GetGroupRoomPage();
            var type = GetTabType(groupRoom);
            yield return new FeedTabModel
            {
                Content = groupRoom,
                Type = type,
                IsActive = groupRoom.Id == currentContent.Id,
                Links = _groupFeedLinkService.GetCreateLinks(type, groupId)
            };

            var canEdit = _groupService.CanEdit(groupId, user);
            var editGroupPage = GetEditPage();

            foreach (var content in GetContents())
            {
                //if (!canEdit && editGroupPage.Id == content.Id || content.IsHideFromSubNavigation())
                if (!canEdit && editGroupPage.Id == content.Id)
                    continue;

                var tabType = GetTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                    continue;

                var tab = new FeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    IsActive = content.IsAncestorOrSelf(currentContent),
                    Links = _groupFeedLinkService.GetCreateLinks(tabType, groupId)
                };

                yield return tab;
            }
        }

        // TODO : this method is called in a loop. EACH time we parse grid. That decrease performance a lot, young man!
        public IIntranetType GetTabType(IPublishedContent content)
        {
            var value = _gridHelper.GetValue(content, "custom.GroupCentralFeedOverview");

            if (value == null || value.tabType == null)
            {
                return _feedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());
            }

            int tabType;
            if (int.TryParse(value.tabType.ToString(), out tabType))
            {
                return _centralFeedTypeProvider.Get(tabType);
            }
            return _feedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());
        }

        private IEnumerable<IPublishedContent> GetContents()
        {
            return GetGroupRoomPage().Children;
        }
    }
}

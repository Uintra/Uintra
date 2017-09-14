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
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public GroupContentHelper(
            UmbracoHelper umbracoHelper,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IGridHelper gridHelper,
            IFeedTypeProvider centralFeedTypeProvider, 
            IDocumentTypeAliasProvider documentTypeAliasProvider, IActivityTypeProvider activityTypeProvider)
        {
            _umbracoHelper = umbracoHelper;
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _gridHelper = gridHelper;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activityTypeProvider = activityTypeProvider;
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
            yield return new FeedTabModel
            {
                Content = groupRoom,
                Type = GetTabType(groupRoom),
                IsActive = groupRoom.Id == currentContent.Id
            };

            var canEdit = _groupService.CanEdit(groupId, user);
            var memberOfGroup = _groupMemberService.IsGroupMember(groupId, user.Id);
            var editGroupPage = GetEditPage();

            var activityTypes = _activityTypeProvider.GetAll();
            var activitiesList = activityTypes.Select(_documentTypeAliasProvider.GetCreatePage).ToArray();

            foreach (var content in GetContents())
            {
                if (!canEdit && editGroupPage.Id == content.Id)
                //if (!canEdit && editGroupPage.Id == content.Id || content.IsHideFromSubNavigation())
                {
                        continue;
                }

                var type = GetTabType(content);

                if (type != null && memberOfGroup)
                {
                    yield return new FeedTabModel
                    {
                        Content = content,
                        Type = type,
                        IsActive = content.IsAncestorOrSelf(currentContent),
                        CreateUrl = content.Children.SingleOrDefault(n => n.DocumentTypeAlias.In(activitiesList))?.Url.AddGroupId(groupId)
                    };
                }
            }
        }

        // TODO : this method is called in a loop. EACH time we parse grid. That decrease performance a lot, young man!
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

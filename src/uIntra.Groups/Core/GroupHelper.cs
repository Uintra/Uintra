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
    public class GroupHelper : IGroupHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IGroupService _groupService;
        private readonly IGridHelper _gridHelper;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupFeedLinkService _groupFeedLinkService;
        private readonly IFeedTypeProvider _feedTypeProvider;

        public GroupHelper(
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

        public ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentContent, Guid groupId)
        {
            var groupRoom = GetGroupRoomPage();
            var type = GetGroupFeedTabType(groupRoom);
            var result = new ActivityFeedTabModel
            {
                Content = groupRoom,
                Type = type,
                IsActive = groupRoom.Id == currentContent.Id,
                Links = _groupFeedLinkService.GetCreateLinks(type, groupId)
            };
            return result;
        }

        // ULTRA TODO : use tuples to return all tabs at once!
        public IEnumerable<ActivityFeedTabModel> GetActivityTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId)
        {
            yield return GetMainFeedTab(currentContent, groupId);



            foreach (var content in GetContent())
            {
                var tabType = GetGroupFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                    continue;

                var tab = new ActivityFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    IsActive = content.IsAncestorOrSelf(currentContent),
                    Links = _groupFeedLinkService.GetCreateLinks(tabType, groupId)
                };

                yield return tab;
            }
        }

        public IEnumerable<PageTabModel> GetPageTabs(IPublishedContent currentContent, IIntranetUser user, Guid groupId)
        {
            Func<IPublishedContent, bool> skipPage = GetPageSkipResolver(user, groupId);

            foreach (var content in GetContent())
            {
                if (skipPage(content))
                    continue;
                var tabType = GetGroupFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();
                if (activityType == null)
                    yield return GetPageTab(currentContent, content, groupId);
            }
        }

        private Func<IPublishedContent, bool> GetPageSkipResolver(IIntranetUser user, Guid groupId)
        {
            var canEdit = _groupService.CanEdit(groupId, user);
            var editGroupPage = GetEditPage();

            var deactivatedPage = GetDeactivatedGroupPage();

            Func<IPublishedContent, bool> skipPage = (content) =>
                    (!canEdit && AreSamePages(editGroupPage, content)
                     || AreSamePages(deactivatedPage, content));
            return skipPage;
        }

        private PageTabModel GetPageTab(IPublishedContent currentContent, IPublishedContent content, Guid groupId)
        {
            return new PageTabModel()
            {
                Content = content,
                IsActive = content.IsAncestorOrSelf(currentContent),
                Title = content.Name,
                Link = content.Url.AddGroupId(groupId)
            };
        }

        private static bool AreSamePages(IPublishedContent first, IPublishedContent second)
        {
            return first.Id == second.Id;
        }

        // TODO : this method is called in a loop. EACH time we parse grid. That decrease performance a lot, young man!
        public IIntranetType GetActivityTypeFromPlugin(IPublishedContent content, string gridPluginAlias)
        {
            var value = _gridHelper.GetValue(content, gridPluginAlias);

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

        private IEnumerable<IPublishedContent> GetContent()
        {
            return GetGroupRoomPage().Children;
        }

        public IIntranetType GetGroupFeedTabType(IPublishedContent content)
        {
            return GetActivityTypeFromPlugin(content, "custom.GroupCentralFeedOverview");
        }

        public IIntranetType GetCreateActivityType(IPublishedContent content)
        {
            return GetActivityTypeFromPlugin(content, "custom.GroupActivityCreate");
        }
    }
}

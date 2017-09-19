using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Extentions;
using Compent.uIntra.Core.Users;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Extentions;
using uIntra.Navigation;
using uIntra.Navigation.SystemLinks;
using uIntra.Navigation.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string TopNavigationViewPath { get; } = "~/Views/Navigation/TopNavigation/Navigation.cshtml";

        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
        protected override string SystemLinkNodePropertyAlias { get; } = "links";
        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
        protected override string SystemLinksContentXPath { get; }

        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupService _groupService;
        private readonly IGroupContentHelper _groupContentHelper;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;


        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ICentralFeedContentHelper centralFeedContentHelper,
            ISystemLinksModelBuilder systemLinksModelBuilder, IDocumentTypeAliasProvider documentTypeAliasProvider, IGroupService groupService, IGroupContentHelper groupContentHelper, IFeedTypeProvider feedTypeProvider, IIntranetUserService<IntranetUser> intranetUserService) :
            base(leftSideNavigationModelBuilder, subNavigationModelBuilder, topNavigationModelBuilder, systemLinksModelBuilder)

        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupService = groupService;
            _groupContentHelper = groupContentHelper;
            _feedTypeProvider = feedTypeProvider;
            _intranetUserService = intranetUserService;

            SystemLinksContentXPath = $"root/{_documentTypeAliasProvider.GetDataFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLinkFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLink()}[@isDoc]";
        }

        public override ActionResult SubNavigation()
        {
            if (_centralFeedContentHelper.IsCentralFeedPage(CurrentPage))
            {
                return new EmptyResult();
            }

            if (_groupContentHelper.IsGroupRoomPage(CurrentPage))
            {
                var groupId = Request.QueryString.GetGroupId();
                var group = _groupService.Get(groupId.Value);
                var groupNavigationModel = new GroupNavigationViewModel { GroupTitle = @group.Title };

                if (!group.IsHidden)
                {
                    groupNavigationModel.GroupUrl = _groupContentHelper.GetGroupRoomPage().UrlWithGroupId(groupId);
                    var allFeedTabType = _feedTypeProvider.Get(CentralFeedTypeEnum.All.ToInt());

                    var allTabs = _groupContentHelper.GetTabs(groupId.Value, _intranetUserService.GetCurrentUser(), CurrentPage).ToList();
                    var tabs = allTabs.FindAll(t => t.Type == null && t.Content.IsShowPageInSubNavigation() || t.Type?.Id == allFeedTabType.Id);
                    var allTab = tabs.Single(t => t.Type?.Id == allFeedTabType.Id);
                    allTab.IsActive = allTabs.Exists(t => t.Type != null && t.IsActive);

                    var groupEditPage = _groupContentHelper.GetEditPage();
                    groupNavigationModel.Tabs = tabs.Select(tab =>
                    {
                        var tabModel = tab.Map<GroupNavigationTabViewModel>();
                        tabModel.AlignRight = tab.Content.Id == groupEditPage.Id;
                        tabModel.Title = tab.Content.GetNavigationName();
                        return tabModel;
                    });
                }


                return PartialView("~/App_Plugins/Groups/GroupNavigation.cshtml", groupNavigationModel);
            }

            var model = new SubNavigationMenuViewModel
            {
                Items = GetContentForSubNavigation(CurrentPage).Where(c => c.IsShowPageInSubNavigation()).Select(MapSubNavigationItem).ToList(),
                Parent = IsHomePage(CurrentPage.Parent) ? null : MapSubNavigationItem(CurrentPage.Parent),
                Title = CurrentPage.GetNavigationName()
            };

            return PartialView(SubNavigationViewPath, model);
        }

        public ContentResult GetTitle()
        {
            var currentPage = CurrentPage;
            var isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());
            var result = isPageHasNavigation ? currentPage.GetNavigationName() : currentPage.Name;

            while (currentPage.Parent != null && !currentPage.Parent.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()))
            {
                currentPage = currentPage.Parent;
                isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());

                result = isPageHasNavigation ? $"{currentPage.GetNavigationName()} - {result}" : $"{currentPage.Name} - {result}";
            }

            return Content($" - {result}");
        }

        private IEnumerable<IPublishedContent> GetContentForSubNavigation(IPublishedContent content)
        {
            if (content.Children.Any() || IsHomePage(content.Parent))
            {
                return content.Children;
            }

            return content.Parent.Children;
        }

        private bool IsHomePage(IPublishedContent content)
        {
            return content.DocumentTypeAlias == _documentTypeAliasProvider.GetHomePage();
        }

        private MenuItemViewModel MapSubNavigationItem(IPublishedContent content)
        {
            return new MenuItemViewModel
            {
                Id = content.Id,
                Name = content.GetNavigationName(),
                Url = content.Url,
                IsActive = content.IsAncestorOrSelf(CurrentPage)
            };
        }
    }
}
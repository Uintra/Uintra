using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Users;
using Extensions;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups;
using uIntra.Groups.Extensions;
using uIntra.Groups.Navigation.Models;
using uIntra.Navigation;
using uIntra.Navigation.SystemLinks;
using uIntra.Navigation.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
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
        private string GroupNavigationViewPath { get; } = "~/App_Plugins/Groups/GroupNavigation.cshtml";

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupService _groupService;
        private readonly IGroupFeedContentService _groupFeedContentService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupContentProvider _groupContentProvider;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ICentralFeedHelper _centralFeedHelper;
        private readonly IGroupHelper _groupHelper;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ICentralFeedContentService centralFeedContentService,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGroupService groupService,
            IGroupFeedContentService groupFeedContentService,
            IIntranetUserService<IntranetUser> intranetUserService,
            IGroupLinkProvider groupLinkProvider,
            IGroupContentProvider groupContentProvider,
            IGroupHelper groupHelper,
            ICentralFeedHelper centralFeedHelper,
            IProfileLinkProvider profileLinkProvider,
            IPermissionsService permissionsService,
            IUserService userService)
            : base(
                leftSideNavigationModelBuilder,
                subNavigationModelBuilder,
                topNavigationModelBuilder,
                systemLinksModelBuilder,
                intranetUserService,
                profileLinkProvider,
                permissionsService,
                userService)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupService = groupService;
            _groupFeedContentService = groupFeedContentService;
            _intranetUserService = intranetUserService;
            _groupLinkProvider = groupLinkProvider;
            _groupContentProvider = groupContentProvider;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _groupHelper = groupHelper;
            _centralFeedHelper = centralFeedHelper;

            SystemLinksContentXPath = $"root/{_documentTypeAliasProvider.GetDataFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLinkFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLink()}[@isDoc]";
        }

        public override ActionResult SubNavigation()
        {
            if (_centralFeedHelper.IsCentralFeedPage(CurrentPage))
            {
                return new EmptyResult();
            }

            if (_groupHelper.IsGroupRoomPage(CurrentPage))
            {
                return RenderGroupNavigation();
            }

            var subNavigation = _subNavigationModelBuilder.GetMenu();
            var result = subNavigation.Map<SubNavigationMenuViewModel>();

            return PartialView(SubNavigationViewPath, result);
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

        private ActionResult RenderGroupNavigation()
        {
            var groupId = Request.QueryString.GetGroupId();
            var group = _groupService.Get(groupId.Value);
            var groupNavigationModel = new GroupNavigationViewModel { GroupTitle = @group.Title };

            if (!group.IsHidden)
            {
                groupNavigationModel.GroupUrl = _groupLinkProvider.GetGroupLink(group.Id);

                groupNavigationModel.ActivityTabs = _groupFeedContentService
                    .GetMainFeedTab(CurrentPage, groupId.Value)
                    .ToEnumerable()
                    .Map<IEnumerable<GroupNavigationActivityTabViewModel>>();

                var currentUser = _intranetUserService.GetCurrentUser();
                var groupEditPage = _groupContentProvider.GetEditPage();
                groupNavigationModel.PageTabs = _groupFeedContentService
                    .GetPageTabs(CurrentPage, currentUser, groupId.Value)
                    .Select(t => MapToGroupPageTabViewModel(t, groupEditPage));
            }


            return PartialView(GroupNavigationViewPath, groupNavigationModel);
        }

        private GroupNavigationPageTabViewModel MapToGroupPageTabViewModel(PageTabModel tab, IPublishedContent editPage)
        {
            var result = tab.Map<GroupNavigationPageTabViewModel>();
            result.AlignRight = IsGroupEditPage(tab.Content, editPage);
            return result;
        }

        private bool IsGroupEditPage(IPublishedContent tab, IPublishedContent editPage)
        {
            return tab.Id == editPage.Id;
        }
    }
}
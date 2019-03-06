using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Users;
using Compent.Extensions;
using Compent.Uintra.Core.Navigation;
using Uintra.CentralFeed;
using Uintra.CentralFeed.Navigation.Models;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Extentions;
using Uintra.Groups.Navigation.Models;
using Uintra.Navigation;
using Uintra.Navigation.SystemLinks;
using Uintra.Navigation.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Uintra.Core.Permissions.Interfaces;

namespace Compent.Uintra.Controllers
{
    public class NavigationController : NavigationControllerBase
    {
        protected override string TopNavigationViewPath { get; } = "~/Views/Navigation/TopNavigation/Navigation.cshtml";
        private string GroupNavigationViewPath { get; } = "~/Views/Groups/GroupNavigation.cshtml";

        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
        protected override string SystemLinkNodePropertyAlias { get; } = "links";
        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
        protected override string SystemLinksContentXPath { get; }

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupService _groupService;
        private readonly IGroupFeedContentService _groupFeedContentService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupContentProvider _groupContentProvider;
        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
        private readonly ICentralFeedHelper _centralFeedHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
        private readonly IUintraInformationService _uintraInformationService;

        public NavigationController(
            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
            ISubNavigationModelBuilder subNavigationModelBuilder,
            ITopNavigationModelBuilder topNavigationModelBuilder,
            ISystemLinksModelBuilder systemLinksModelBuilder,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGroupService groupService,
            IGroupFeedContentService groupFeedContentService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IGroupLinkProvider groupLinkProvider,
            IGroupContentProvider groupContentProvider,
            IGroupHelper groupHelper,
            ICentralFeedHelper centralFeedHelper,
            IProfileLinkProvider profileLinkProvider,
            IPermissionsService permissionsService,
            IUserService userService,
            IUintraInformationService uintraInformationService)
            : base(
                leftSideNavigationModelBuilder,
                subNavigationModelBuilder,
                topNavigationModelBuilder,
                systemLinksModelBuilder,
                intranetMemberService,
                profileLinkProvider,
                permissionsService,
                userService)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupService = groupService;
            _groupFeedContentService = groupFeedContentService;
            _groupLinkProvider = groupLinkProvider;
            _groupContentProvider = groupContentProvider;
            _subNavigationModelBuilder = subNavigationModelBuilder;
            _groupHelper = groupHelper;
            _centralFeedHelper = centralFeedHelper;
            _uintraInformationService = uintraInformationService;
            _topNavigationModelBuilder = topNavigationModelBuilder;

            SystemLinksContentXPath = $"root/{_documentTypeAliasProvider.GetDataFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLinkFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLink()}[@isDoc]";
        }


        public override ActionResult TopNavigation()
        {
            var topNavigation = _topNavigationModelBuilder.Get();
            var result = new TopNavigationExtendedViewModel
            {
                CurrentMember = topNavigation.CurrentMember.Map<MemberViewModel>(),
                CentralUserListUrl = topNavigation.CentralUserListUrl,
                UintraDocumentationLink = _uintraInformationService.DocumentationLink,
                UintraDocumentationVersion = _uintraInformationService.Version
            };

            return PartialView(TopNavigationViewPath, result);
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

        protected override List<BreadcrumbItemViewModel> GetBreadcrumbsItems()
        {
            var result = base.GetBreadcrumbsItems();
            return _groupHelper.IsGroupRoomPage(CurrentPage) ? GetGroupBreadcrumbsItems(result) : result;
        }

        private List<BreadcrumbItemViewModel> GetGroupBreadcrumbsItems(List<BreadcrumbItemViewModel> items)
        {
            var groupId = Request.QueryString.GetGroupId();
            var group = _groupService.Get(groupId.Value);
            var groupRoomPageUrl = _groupContentProvider.GetGroupRoomPage().Url;

            var groupRoomItem = items.Single(item => item.Url.Equals(groupRoomPageUrl));
            groupRoomItem.Name = group.Title;

            var groupRoomItems = items.SkipWhile(item => item != groupRoomItem).Where(item => item.IsClickable);
            foreach (var item in groupRoomItems)
            {
                item.Url = item.Url.AddGroupId(groupId.Value);
            }

            return items;
        }

        private ActionResult RenderGroupNavigation()
        {
            var groupId = Request.QueryString.GetGroupId();
            if (!groupId.HasValue)
            {
                return new EmptyResult();
            }
            var group = _groupService.Get(groupId.Value);

            if (group == null)
            {
                return new EmptyResult();
            }

            var groupNavigationModel = new GroupNavigationViewModel { GroupTitle = @group.Title };

            if (!group.IsHidden)
            {
                groupNavigationModel.GroupUrl = _groupLinkProvider.GetGroupLink(group.Id);

                groupNavigationModel.ActivityTabs = _groupFeedContentService
                    .GetMainFeedTab(CurrentPage, groupId.Value)
                    .ToEnumerable()
                    .Map<IEnumerable<GroupNavigationActivityTabViewModel>>();

                var groupEditPage = _groupContentProvider.GetEditPage();
                groupNavigationModel.PageTabs = _groupFeedContentService
                    .GetPageTabs(CurrentPage, groupId.Value)
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

        private bool IsGroupEditPage(IPublishedContent tab, IPublishedContent editPage) => tab.Id == editPage.Id;
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Uintra20.Core.Member.Abstractions;
//using Uintra20.Core.Member.Entities;
//using Uintra20.Core.Member.Models;
//using Uintra20.Core.Member.Services;
//using Uintra20.Features.Groups.ContentServices;
//using Uintra20.Features.Groups.Helpers;
//using Uintra20.Features.Groups.Services;
//using Uintra20.Features.Links;
//using Uintra20.Features.Navigation;
//using Uintra20.Features.Navigation.ModelBuilders.LeftSideMenu;
//using Uintra20.Features.Navigation.ModelBuilders.SubMenu;
//using Uintra20.Features.Navigation.ModelBuilders.SystemLinks;
//using Uintra20.Features.Navigation.ModelBuilders.TopMenu;
//using Uintra20.Features.Navigation.Models;
//using Uintra20.Features.Navigation.Web;
//using Uintra20.Features.UintraPanels.LastActivities.Helpers;
//using Uintra20.Infrastructure;
//using Uintra20.Infrastructure.Extensions;
//using Uintra20.Infrastructure.Providers;
//using Umbraco.Core.Models.PublishedContent;
//using Umbraco.Web;

//namespace Uintra20.Controllers
//{
//    public class NavigationsController : NavigationsControllerBase
//    {
//        protected override string SystemLinkTitleNodePropertyAlias { get; } = "linksGroupTitle";
//        protected override string SystemLinkNodePropertyAlias { get; } = "links";
//        protected override string SystemLinkSortOrderNodePropertyAlias { get; } = "sort";
//        protected override IEnumerable<string> SystemLinksContentAliasPath { get; }

//        protected override string DefaultRedirectUrl { get; } = "/";
//        protected override string UmbracoRedirectUrl { get; } = "/umbraco";

//        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
//        private readonly IGroupService _groupService;
//        private readonly IGroupFeedContentService _groupFeedContentService;
//        private readonly IGroupLinkProvider _groupLinkProvider;
//        private readonly IGroupContentProvider _groupContentProvider;
//        private readonly ISubNavigationModelBuilder _subNavigationModelBuilder;
//        private readonly ICentralFeedHelper _centralFeedHelper;
//        private readonly IGroupHelper _groupHelper;
//        private readonly ITopNavigationModelBuilder _topNavigationModelBuilder;
//        private readonly IUintraInformationService _uintraInformationService;

//        public NavigationsController(
//            ILeftSideNavigationModelBuilder leftSideNavigationModelBuilder,
//            ISubNavigationModelBuilder subNavigationModelBuilder,
//            ITopNavigationModelBuilder topNavigationModelBuilder,
//            ISystemLinksModelBuilder systemLinksModelBuilder,
//            IDocumentTypeAliasProvider documentTypeAliasProvider,
//            IGroupService groupService,
//            IGroupFeedContentService groupFeedContentService,
//            IIntranetMemberService<IntranetMember> intranetMemberService,
//            IGroupLinkProvider groupLinkProvider,
//            IGroupContentProvider groupContentProvider,
//            IGroupHelper groupHelper,
//            ICentralFeedHelper centralFeedHelper,
//            IProfileLinkProvider profileLinkProvider,
//            IUintraInformationService uintraInformationService,
//            INavigationModelsBuilder navigationModelsBuilder,
//            UmbracoContext umbracoContext)
//            : base(
//                leftSideNavigationModelBuilder,
//                subNavigationModelBuilder,
//                topNavigationModelBuilder,
//                systemLinksModelBuilder,
//                intranetMemberService,
//                profileLinkProvider,
//                navigationModelsBuilder,
//                umbracoContext)
//        {
//            _documentTypeAliasProvider = documentTypeAliasProvider;
//            _groupService = groupService;
//            _groupFeedContentService = groupFeedContentService;
//            _groupLinkProvider = groupLinkProvider;
//            _groupContentProvider = groupContentProvider;
//            _subNavigationModelBuilder = subNavigationModelBuilder;
//            _groupHelper = groupHelper;
//            _centralFeedHelper = centralFeedHelper;
//            _uintraInformationService = uintraInformationService;
//            _topNavigationModelBuilder = topNavigationModelBuilder;

//            SystemLinksContentAliasPath = $"root/{_documentTypeAliasProvider.GetDataFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLinkFolder()}[@isDoc]/{_documentTypeAliasProvider.GetSystemLink()}[@isDoc]";
//            SystemLinksContentAliasPath = new[] { _documentTypeAliasProvider.GetDataFolder(), _documentTypeAliasProvider.GetSystemLinkFolder(), _documentTypeAliasProvider.GetSystemLink() };
//        }


//        public override TopNavigationViewModel TopNavigation()
//        {
//            var topNavigation = _topNavigationModelBuilder.Get();
//            var result = new TopNavigationExtendedViewModel
//            {
//                CurrentMember = topNavigation.CurrentMember.Map<MemberViewModel>(),
//                CentralUserListUrl = topNavigation.CentralUserListUrl,
//                UintraDocumentationLink = _uintraInformationService.DocumentationLink,
//                UintraDocumentationVersion = _uintraInformationService.Version
//            };

//            return result;
//        }


//        public override SubNavigationMenuViewModel SubNavigation()
//        {
//            if (_centralFeedHelper.IsCentralFeedPage(CurrentPage))
//            {
//                return new EmptyResult();
//            }

//            if (_groupHelper.IsGroupRoomPage(CurrentPage))
//            {
//                return RenderGroupNavigation();
//            }

//            var subNavigation = _subNavigationModelBuilder.GetMenu();
//            var result = subNavigation.Map<SubNavigationMenuViewModel>();

//            return PartialView(SubNavigationViewPath, result);
//        }

//        public ContentResult GetTitle()
//        {
//            var currentPage = CurrentPage;
//            var isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());
//            var result = isPageHasNavigation ? currentPage.GetNavigationName() : currentPage.Name;

//            while (currentPage.Parent != null && !currentPage.Parent.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()))
//            {
//                currentPage = currentPage.Parent;
//                isPageHasNavigation = currentPage.IsComposedOf(_documentTypeAliasProvider.GetNavigationComposition());

//                result = isPageHasNavigation ? $"{currentPage.GetNavigationName()} - {result}" : $"{currentPage.Name} - {result}";
//            }

//            return Content($" - {result}");
//        }

//        protected override IEnumerable<BreadcrumbItemViewModel> GetBreadcrumbsItems()
//        {
//            var result = base.GetBreadcrumbsItems();
//            return _groupHelper.IsGroupRoomPage(CurrentPage) ? GetGroupBreadcrumbsItems(result) : result;
//        }

//        private IEnumerable<BreadcrumbItemViewModel> GetGroupBreadcrumbsItems(IEnumerable<BreadcrumbItemViewModel> items)
//        {
//            var itemsList = items.ToList();
//            var groupId = Request.QueryString.GetGroupId();
//            var group = _groupService.Get(groupId);
//            var groupRoomPageUrl = _groupContentProvider.GetGroupRoomPage().Url;

//            var groupRoomItem = itemsList.Single(item => item.Url.Equals(groupRoomPageUrl));
//            groupRoomItem.Name = group.Title;

//            var groupRoomItems = itemsList.SkipWhile(item => item != groupRoomItem).Where(item => item.IsClickable);
//            foreach (var item in groupRoomItems)
//            {
//                item.Url = item.Url.AddGroupId(groupId);
//            }

//            return itemsList;
//        }

//        private ActionResult RenderGroupNavigation()
//        {
//            var groupId = Request.QueryString.GetGroupIdOrNone();
//            var validGroup = groupId.Map(_groupService.Get);

//            return validGroup.Match(
//                Some: group =>
//                {
//                    var groupNavigationModel = group.IsHidden
//                        ? new GroupNavigationViewModel { GroupTitle = group.Title }
//                        : new GroupNavigationViewModel
//                        {
//                            GroupTitle = group.Title,
//                            GroupUrl = _groupLinkProvider.GetGroupLink(group.Id),
//                            ActivityTabs = _groupFeedContentService
//                                .GetMainFeedTab(CurrentPage, group.Id)
//                                .ToEnumerable()
//                                .Map<IEnumerable<GroupNavigationActivityTabViewModel>>(),
//                            PageTabs = _groupFeedContentService
//                                .GetPageTabs(CurrentPage, group.Id)
//                                .Select(t => MapToGroupPageTabViewModel(t, _groupContentProvider.GetEditPage()))
//                                .ToList()
//                        };

//                    return PartialView(GroupNavigationViewPath, groupNavigationModel);
//                },
//                None: () => (ActionResult)new EmptyResult());
//        }

//        private GroupNavigationPageTabViewModel MapToGroupPageTabViewModel(PageTabModel tab, IPublishedContent editPage)
//        {
//            var result = tab.Map<GroupNavigationPageTabViewModel>();
//            result.AlignRight = IsGroupEditPage(tab.Content, editPage);
//            return result;
//        }

//        private static bool IsGroupEditPage(IPublishedContent tab, IPublishedContent editPage) => tab.Id == editPage.Id;
//    }
//}
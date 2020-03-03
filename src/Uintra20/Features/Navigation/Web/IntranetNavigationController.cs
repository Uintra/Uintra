using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Core.HomePage;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Models.MyLinks;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly INodeModelService _nodeModelService;
        private readonly IMyLinksHelper _myLinksHelper;
        private readonly IPermissionsService _permissionsService;

        public IntranetNavigationController(
            INavigationModelsBuilder navigationModelsBuilder,
            INodeModelService nodeModelService,
            IMyLinksHelper myLinksHelper,
            IPermissionsService permissionsService)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
            _nodeModelService = nodeModelService;
            _myLinksHelper = myLinksHelper;
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public virtual TopNavigationViewModel MobileNavigation()
        {
            var model = _navigationModelsBuilder.GetMobileNavigation();
            var viewModel = model.Map<TopNavigationViewModel>();

            return viewModel;
        }

        [HttpGet]
        public virtual TopNavigationViewModel TopNavigation()
        {
            var model = _navigationModelsBuilder.GetTopNavigationModel();
            var viewModel = model.Map<TopNavigationViewModel>();
            viewModel.CurrentMember = model.CurrentMember.ToViewModel();
            return viewModel;
        }

        [HttpGet]
        public virtual async Task<LeftNavigationViewModel> LeftNavigation()
        {
            var result = new LeftNavigationViewModel
            {
                MenuItems = GetMenuItems(),
                GroupItems = GroupItems(),
                MyLinks = await GetMyLinksAsync(),
                SharedLinks = GetSharedLinks()
            };

            return result;
        }

        private IEnumerable<SharedLinkApiViewModel> GetSharedLinks()
        {
            var sharedLinks = _nodeModelService.AsEnumerable().OfType<SharedLinkItemModel>().Where(sl => sl.Links.Value != null);

            var result = sharedLinks.OrderBy(sl => sl.Sort).Select(MapSharedLinkItemModel);
            return result;
        }

        private IEnumerable<MenuItemViewModel> GetMenuItems()
        {
            return _navigationModelsBuilder.GetLeftSideNavigation().Select(MapMenuItem);
        }

        protected virtual GroupLeftNavigationMenuViewModel GroupItems()
        {
            var rootGroupPage = _nodeModelService.AsEnumerable().OfType<UintraGroupsPageModel>().First();

            var groupPageChildren = _nodeModelService.AsEnumerable().Where(x =>
                x is IGroupNavigationComposition navigation && navigation.GroupNavigation.ShowInMenu &&
                x.ParentId == rootGroupPage.Id);

            groupPageChildren = groupPageChildren.Where(x =>
            {
                if (x is UintraGroupsCreatePageModel)
                {
                    return _permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Create,
                        PermissionResourceTypeEnum.Groups));
                }

                return true;
            });

            var menuItems = groupPageChildren.OrderBy(x => ((IGroupNavigationComposition)x).GroupNavigation.SortOrder.Value).Select(x => new GroupLeftNavigationItemViewModel
            {
                Title = ((IGroupNavigationComposition)x).GroupNavigation.NavigationTitle,
                Link = x.Url.ToLinkModel()
            });

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupPageItem = new GroupLeftNavigationItemViewModel
                {
                    Link = rootGroupPage.Url.ToLinkModel(),
                    Title = ((IGroupNavigationComposition)rootGroupPage).GroupNavigation.NavigationTitle
                }
            };

            return result;
        }

        protected virtual async Task<IEnumerable<MyLinkItemViewModel>> GetMyLinksAsync()
        {
            var linkModels = await _myLinksHelper.GetMenuAsync();
            return linkModels.Map<IEnumerable<MyLinkItemViewModel>>();
        }

        protected virtual SharedLinkApiViewModel MapSharedLinkItemModel(SharedLinkItemModel model)
        {
            return new SharedLinkApiViewModel
            {
                LinksGroupTitle = model.LinksGroupTitle,
                Links = model.Links.Value.Select(sharedLink => sharedLink.ToViewModel())
            };
        }

        protected virtual MenuItemViewModel MapMenuItem(TreeNavigationItemModel model)
        {
            var item = _nodeModelService.Get(model.Id);

            var result = model.Map<MenuItemViewModel>();
            result.IsHeading = item is HeadingPageModel;
            result.IsHomePage = item is HomePageModel;
            result.IsClickable = !result.IsHeading && result.IsActive;

            return result;
        }
    }
}
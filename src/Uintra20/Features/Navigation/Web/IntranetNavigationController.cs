using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Core.HomePage;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Models.MyLinks;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly INodeModelService _nodeModelService;
        private readonly IMyLinksHelper _myLinksHelper;
        private readonly IGroupHelper _groupHelper;

        public IntranetNavigationController(
            INavigationModelsBuilder navigationModelsBuilder,
            INodeModelService nodeModelService,
            IMyLinksHelper myLinksHelper,
            IGroupHelper groupHelper)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
            _nodeModelService = nodeModelService;
            _myLinksHelper = myLinksHelper;
            _groupHelper = groupHelper;
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
                GroupItems = _groupHelper.GroupNavigation(),
                MyLinks = await GetMyLinksAsync(),
                SharedLinks = GetSharedLinks()
            };

            return result;
        }

        [HttpGet]
        public virtual IEnumerable<BreadcrumbItemViewModel> Breadcrumbs()
        {
            return _navigationModelsBuilder.GetBreadcrumbsItems().ToList();
        }

        private IEnumerable<SharedLinkApiViewModel> GetSharedLinks()
        {
            var sharedLinks = _nodeModelService.AsEnumerable().OfType<SharedLinkItemModel>().Where(sl => sl.Links.Value != null);

            sharedLinks = sharedLinks.ToArray();

            var result = sharedLinks.OrderBy(sl => sl.Sort.Value).Select(MapSharedLinkItemModel);
            return result;
        }

        private IEnumerable<MenuItemViewModel> GetMenuItems()
        {
            return _navigationModelsBuilder.GetLeftSideNavigation().Select(MapMenuItem);
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
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Core.HomePage;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly INodeModelService _nodeModelService;

        public IntranetNavigationController(INavigationModelsBuilder navigationModelsBuilder,
            INodeModelService nodeModelService)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
            _nodeModelService = nodeModelService;
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

            return viewModel;
        }

        [HttpGet]
        public virtual MenuViewModel LeftNavigation()
        {
            IEnumerable<TreeNavigationItemModel> leftNavigation = _navigationModelsBuilder.GetLeftSideNavigation();
            var result = new MenuViewModel { MenuItems = leftNavigation.Select(MapMenuItem) };

            return result;
        }

        [HttpGet]
        public virtual IEnumerable<SharedLinkApiViewModel> SystemLinks()
        {
            var sharedLinks = _nodeModelService.AsEnumerable().OfType<SharedLinkItemModel>().Where(sl => sl.Links.Value != null);

            var result = sharedLinks.Select(MapSharedLinkItemModel).OrderBy(sl => sl.Sort);
            return result;
        }

        private SharedLinkApiViewModel MapSharedLinkItemModel(SharedLinkItemModel model)
        {
            return new SharedLinkApiViewModel
            {
                LinksGroupTitle = model.LinksGroupTitle,
                Sort = model.Sort,
                Links = model.Links.Value.Select(sharedLink => sharedLink.ToViewModel())
            };
        }

        private MenuItemViewModel MapMenuItem(TreeNavigationItemModel model)
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
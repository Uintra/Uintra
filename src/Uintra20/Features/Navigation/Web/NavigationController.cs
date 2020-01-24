using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using Uintra20.Core.HomePage;
using Uintra20.Features.Navigation.ModelBuilders.SystemLinks;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;
        private readonly INodeModelService _nodeModelService;
        private readonly ISystemLinksModelBuilder _systemLinksModelBuilder;

        public IntranetNavigationController(INavigationModelsBuilder navigationModelsBuilder, 
            INodeModelService nodeModelService, 
            ISystemLinksModelBuilder systemLinksModelBuilder)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
            _nodeModelService = nodeModelService;
            _systemLinksModelBuilder = systemLinksModelBuilder;
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
        public virtual IEnumerable<SharedLinkItemViewModel> SystemLinks()
        {
            return _systemLinksModelBuilder.Get();
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
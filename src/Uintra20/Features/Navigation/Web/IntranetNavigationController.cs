using System.Collections.Generic;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.Web
{
    public class IntranetNavigationController : UBaselineApiController
    {
        private readonly INavigationModelsBuilder _navigationModelsBuilder;

        public IntranetNavigationController(INavigationModelsBuilder navigationModelsBuilder)
        {
            _navigationModelsBuilder = navigationModelsBuilder;
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
            var leftNavigation = _navigationModelsBuilder.GetLeftSideNavigation();
            var result = new MenuViewModel { MenuItems = leftNavigation.Map<IEnumerable<MenuItemViewModel>>() };

            return result;
        }
    }
}
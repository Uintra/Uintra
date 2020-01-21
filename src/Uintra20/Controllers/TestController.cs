using System.Linq;
using System.Web.Http;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Controllers;
using UBaseline.Core.Navigation;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.Navigation;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Controllers
{
    public class TestController : UBaselineApiController
    {
        private readonly INavigationService _navigationService;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;
        private readonly INodeModelService _nodeModelService;
        private readonly INavigationBuilder _navigationBuilder;
        private readonly INodeDirectAccessValidator _nodeDirectAccessValidator;

        public TestController(
            INavigationService navigationService,
            IUBaselineRequestContext uBaselineRequestContext,
            INodeModelService nodeModelService,
            INavigationBuilder navigationBuilder,
            INodeDirectAccessValidator nodeDirectAccessValidator)

        {
            _navigationService = navigationService;
            _uBaselineRequestContext = uBaselineRequestContext;
            _nodeModelService = nodeModelService;
            _navigationBuilder = navigationBuilder;
            _nodeDirectAccessValidator = nodeDirectAccessValidator;
        }

        [HttpGet]
        public void Menu()
        {
            var navigationNodes = _nodeModelService.AsEnumerable()
                .Where(i => i.Level >= 2 && _nodeDirectAccessValidator.HasAccess(i))
                .OfType<IUintraNavigationComposition>()
                .OrderBy(i => i.SortOrder)
                .Where(i => i.Navigation.ShowInMenu.Value && i.Url.HasValue());

            var items = _navigationBuilder.GetTreeNavigation(navigationNodes);
        }
    }
}
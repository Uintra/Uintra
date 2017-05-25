using System.Web.Http;
using Umbraco.Web.WebApi;

namespace uIntra.Navigation.Dashboard
{
    public class UNavigationApiController : UmbracoAuthorizedApiController
    {
        private readonly IHomeNavigationCompositionService _homeNavigationCompositionService;
        private readonly INavigationCompositionService _navigationCompositionService;

        public UNavigationApiController(
            IHomeNavigationCompositionService homeNavigationCompositionService, 
            INavigationCompositionService navigationCompositionService
            )
        {
            _homeNavigationCompositionService = homeNavigationCompositionService;
            _navigationCompositionService = navigationCompositionService;
        }

        [HttpGet]
        public NavigationState GetInitialState()
        {
            var result = new NavigationState
            {
                IsDocumentTypesAlreadyExists = _navigationCompositionService.IsExists() && _homeNavigationCompositionService.IsExists()
            };

            return result;
        }

        [HttpPost]
        public NavigationState CreateNavigationCompositions(CreateNavigationCompositionsModel model)
        {
            var navigationCompositionState = _navigationCompositionService.Create(model.ParentIdOrAlias);
            var homeNavigationCompositionState = _homeNavigationCompositionService.Create(model.ParentIdOrAlias);

            var result = new NavigationState
            {
                IsDocumentTypesAlreadyExists = navigationCompositionState.IsExists || homeNavigationCompositionState.IsExists,
                IsUnknownParent = navigationCompositionState.IsUnknownParent || homeNavigationCompositionState.IsUnknownParent
            };

            return result;
        }

        [HttpPost]
        public NavigationState DeleteNavigationCompositions()
        {
            var navigationCompositionState = _navigationCompositionService.Delete();
            var homeNavigationCompositionState = _homeNavigationCompositionService.Delete();

            var result = new NavigationState
            {
                IsDocumentTypesAlreadyExists = navigationCompositionState.IsExists || homeNavigationCompositionState.IsExists
            };

            return result;
        }
    }
}

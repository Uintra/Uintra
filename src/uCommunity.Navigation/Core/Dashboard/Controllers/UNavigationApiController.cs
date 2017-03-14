using System.Web.Http;
using Umbraco.Web.WebApi;

namespace uCommunity.Navigation.Core.Dashboard
{
    public class UNavigationApiController : UmbracoAuthorizedApiController
    {
        private readonly INavigationService _navigationService;

        public UNavigationApiController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet]
        public NavigationInitialState GetInitialState()
        {
            var result = new NavigationInitialState
            {
                IsDocumentTypesAlreadyExists = _navigationService.IsNavigationCompositionExist() && _navigationService.IsHomeNavigationCompositionExist()
            };

            return result;
        }

        [HttpPost]
        public void CreateNavigationCompositions(CreateNavigationCompositionsModel model)
        {
            _navigationService.CreateNavigationComposition(model.FolderId);
            _navigationService.CreateHomeNavigationComposition(model.FolderId);
        }
    }
}

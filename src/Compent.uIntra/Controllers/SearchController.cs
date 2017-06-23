using System.Collections.Generic;
using uIntra.Core.Localization;
using uIntra.Search.Core;
using uIntra.Search.Web;

namespace Compent.uIntra.Controllers
{
    public class SearchController : SearchControllerBase
    {
        public SearchController(
            ElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper)
            : base(elasticIndex, searchableServices, localizationService, searchUmbracoHelper)
        {
        }
    }
}
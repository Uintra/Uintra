using System.Collections.Generic;
using Localization.Umbraco.Attributes;
using uIntra.Core.Localization;
using uIntra.Search;
using uIntra.Search.Web;

namespace Compent.uIntra.Controllers
{
    [ThreadCulture]
    public class SearchController : SearchControllerBase
    {
        public SearchController(ElasticIndex elasticIndex, IEnumerable<IIndexer> searchableServices, IIntranetLocalizationService localizationService, ISearchUmbracoHelper searchUmbracoHelper, ISearchableTypeProvider searchableTypeProvider)
            : base(elasticIndex, searchableServices, localizationService, searchUmbracoHelper, searchableTypeProvider)
        {
        }
    }
}
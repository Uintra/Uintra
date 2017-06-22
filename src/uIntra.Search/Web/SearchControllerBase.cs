using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Core.Localization;
using uIntra.Search.Core;
using Umbraco.Web.Mvc;

namespace uIntra.Search.Web
{
    public abstract class SearchControllerBase : SurfaceController
    {
        protected const int SuggestionSearchCount = 10;

        private readonly ElasticIndex _elasticIndex;
        private readonly IIntranetLocalizationService _localizationService;

        protected SearchControllerBase(ElasticIndex elasticIndex, IIntranetLocalizationService localizationService)
        {
            _elasticIndex = elasticIndex;
            _localizationService = localizationService;
        }

        public virtual ActionResult Index(string query)
        {
            if (query.IsNullOrEmpty())
            {
                return PartialView("~/App_Plugins/Search/Result/Index.cshtml", new SearchViewModel());
            }

            return PartialView("~/App_Plugins/Search/Result/Index.cshtml", new SearchViewModel
            {
                Query = query
            });
        }

        public virtual JsonResult Autocomplete(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = SuggestionSearchCount
            });

            var result = searchResult.Documents.Select(d =>
            {
                var model = d.Map<SearchAutocompleteResultModel>();
                model.Type = _localizationService.Translate(d.Type.GetLocalizeKey());
                return model;
            });

            return Json(new { Documents = result }, JsonRequestBehavior.AllowGet);
        }

        public virtual PartialViewResult Search(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = 1000,//todo change after paging will be added
                ApplyHighlights = true
            });

            var result = searchResult.Documents.Select(d =>
            {
                var model = d.Map<SearchTextResultModel>();
                model.Type = _localizationService.Translate(d.Type.GetLocalizeKey());
                return model;
            });

            return PartialView("~/App_Plugins/Search/Result/SearchResult.cshtml", result);
        }
    }
}

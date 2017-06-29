using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Core.Localization;
using Umbraco.Web.Mvc;

namespace uIntra.Search.Web
{
    public abstract class SearchControllerBase : SurfaceController
    {
        protected virtual string IndexViewPath { get; } = "~/App_Plugins/Search/Result/Index.cshtml";
        protected virtual string SearchResultViewPath { get; } = "~/App_Plugins/Search/Result/SearchResult.cshtml";
        protected virtual string SearchBoxViewPath { get; } = "~/App_Plugins/Search/Controls/SearchBox.cshtml";

        protected virtual int SuggestionCount { get; } = 10;
        protected virtual int ResultsPerPage { get; } = 20;

        private readonly ElasticIndex _elasticIndex;
        private readonly IEnumerable<IIndexer> _searchableServices;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;

        protected SearchControllerBase(
            ElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper)
        {
            _elasticIndex = elasticIndex;
            _searchableServices = searchableServices;
            _localizationService = localizationService;
            _searchUmbracoHelper = searchUmbracoHelper;
        }

        public virtual PartialViewResult Index(string query)
        {
            var result = new SearchViewModel
            {
                Query = query
            };

            return PartialView(IndexViewPath, result);
        }

        [HttpPost]
        public virtual PartialViewResult Search(string query, int page = 1)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = ResultsPerPage * page,
                ApplyHighlights = true
            });

            var results = searchResult.Documents.Select(d =>
            {
                var model = d.Map<SearchResultViewModel>();
                model.Type = _localizationService.Translate(d.Type.GetLocalizeKey());
                return model;
            }).ToList();

            var resultModel = new SearchResultsOverviewViewModel
            {
                Query = query,
                Results = results,
                ResultsCount = results.Count
            };

            return PartialView(SearchResultViewPath, resultModel);
        }

        public virtual PartialViewResult SearchBox()
        {
            var result = new SearchBoxViewModel
            {
                SearchResultsUrl = _searchUmbracoHelper.GetSearchPage()?.Url
            };

            return PartialView(SearchBoxViewPath, result);
        }

        public virtual JsonResult Autocomplete(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = SuggestionCount
            });

            var result = searchResult.Documents.Select(d =>
            {
                var model = d.Map<SearchAutocompleteResultViewModel>();
                model.Type = _localizationService.Translate(d.Type.GetLocalizeKey());
                return model;
            });

            return Json(new { Documents = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void RebuildIndex()
        {
            _elasticIndex.RecreateIndex();
            foreach (var service in _searchableServices)
            {
                service.FillIndex();
            }
        }
    }
}

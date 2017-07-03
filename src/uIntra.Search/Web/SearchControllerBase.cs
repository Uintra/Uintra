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

        protected virtual int AutocompleteSuggestionCount { get; } = 10;
        protected virtual int ResultsPerPage { get; } = 20;

        private readonly ElasticIndex _elasticIndex;
        private readonly IEnumerable<IIndexer> _searchableServices;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly ISearchableTypeProvider _searchableTypeProvider;

        protected SearchControllerBase(
            ElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper,
            ISearchableTypeProvider searchableTypeProvider)
        {
            _elasticIndex = elasticIndex;
            _searchableServices = searchableServices;
            _localizationService = localizationService;
            _searchUmbracoHelper = searchUmbracoHelper;
            _searchableTypeProvider = searchableTypeProvider;
        }

        public virtual PartialViewResult Index(string query)
        {
            var filterItems = _searchableTypeProvider.GetAll().Select(el => new SearchFilterItemViewModel
            {
                Id = el.Id,
                Name = _localizationService.Translate($"Search.Filter.{el.Name}")
            });

            var result = new SearchViewModel
            {
                Query = query,
                FilterItems = filterItems
            };

            return PartialView(IndexViewPath, result);
        }

        [HttpPost]
        public virtual PartialViewResult Search(SearchFilterModel model)
        {
            var searchableTypeIds = model.Types.Count > 0 ? model.Types : _searchableTypeProvider.GetAll().Select(el => el.Id);

            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = model.Query,
                Take = ResultsPerPage * model.Page,
                SearchableTypeIds = searchableTypeIds,
                ApplyHighlights = true
            });

            var results = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<SearchResultViewModel>();
                resultItem.Type = _localizationService.Translate(_searchableTypeProvider.Get(d.Type).Name);
                return resultItem;
            }).ToList();

            var resultModel = new SearchResultsOverviewViewModel
            {
                Query = model.Query,
                Results = results,
                ResultsCount = (int)searchResult.TotalHits
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
                Take = AutocompleteSuggestionCount
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

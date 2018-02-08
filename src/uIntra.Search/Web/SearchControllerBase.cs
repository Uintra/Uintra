using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Extensions;
using uIntra.Core.Localization;
using uIntra.Core.TypeProviders;
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
        protected virtual string SearchTranslationPrefix { get; } = "Search.";

        private readonly IElasticIndex _elasticIndex;
        private readonly IEnumerable<IIndexer> _searchableServices;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly ISearchableTypeProvider _searchableTypeProvider;

        protected SearchControllerBase(
            IElasticIndex elasticIndex,
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
            var result = GetSearchViewModel();
            result.Query = query;

            return PartialView(IndexViewPath, result);
        }

        [HttpPost]
        public virtual PartialViewResult Search(SearchFilterModel model)
        {
            var searchableTypeIds = model.Types.Count > 0 ? model.Types : GetSearchableTypes().Select(t => t.ToInt());

            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = model.Query,
                Take = ResultsPerPage * model.Page,
                SearchableTypeIds = searchableTypeIds,
                OnlyPinned = model.OnlyPinned,
                ApplyHighlights = true
            });

            var resultModel = GetSearchResultsOverviewModel(searchResult);
            resultModel.Query = model.Query;

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
                Take = AutocompleteSuggestionCount,
                SearchableTypeIds = GetAutoCompleteSearchableTypes().Select(t => t.ToInt())
            });

            var result = GetAutocompleteResultModels(searchResult.Documents);
            return Json(new { Documents = result }, JsonRequestBehavior.AllowGet);
        }

        protected virtual IEnumerable<Enum> GetSearchableTypes()
        {
            return _searchableTypeProvider.All;
        }

        protected virtual IEnumerable<Enum> GetAutoCompleteSearchableTypes()
        {
            return _searchableTypeProvider.All;
        }

        protected virtual SearchViewModel GetSearchViewModel()
        {
            var filterItems = GetFilterItemTypes().Select(el => new SearchFilterItemViewModel
            {
                Id = el.ToInt(),
                Name = _localizationService.Translate($"{SearchTranslationPrefix}{el.ToString()}")
            });

            var result = new SearchViewModel
            {
                FilterItems = filterItems
            };

            return result;
        }

        protected virtual IEnumerable<Enum> GetFilterItemTypes()
        {
            return _searchableTypeProvider.All;
        }

        protected virtual SearchResultsOverviewViewModel GetSearchResultsOverviewModel(SearchResult<SearchableBase> searchResult)
        {
            var searchResultViewModels = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<SearchResultViewModel>();
                resultItem.Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[d.Type].ToString()}");
                return resultItem;
            }).ToList();

            var filterItems = GetSearchableTypes().GroupJoin(
                searchResult.TypeFacets,
                type => type.ToInt(),
                facet => int.Parse(facet.Name),
                (type, facets) =>
                {
                    var facet = facets.FirstOrDefault();
                    return new SearchFilterItemViewModel
                    {
                        Id = type.ToInt(),
                        Name = GetLabelWithCount($"{SearchTranslationPrefix}{type.ToString()}", facet != null ? (int)facet.Count : default)
                    };
                }
               );

            var result = new SearchResultsOverviewViewModel
            {
                Results = searchResultViewModels,
                ResultsCount = (int)searchResult.TotalHits,
                FilterItems = filterItems,
                AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", (int)searchResult.TotalHits),
                BlockScrolling = searchResult.TotalHits <= searchResultViewModels.Count
            };

            return result;
        }

        protected virtual IEnumerable<SearchAutocompleteResultViewModel> GetAutocompleteResultModels(IEnumerable<SearchableBase> searchResults)
        {
            var result = searchResults.Select(d =>
            {
                var model = d.Map<SearchAutocompleteResultViewModel>();
                model.Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[d.Type].ToString()}");
                return model;
            });

            return result;
        }

        protected virtual string GetLabelWithCount(string label, int count)
        {
            return $"{_localizationService.Translate(label)} ({count})";
        }

        [HttpPost]
        public virtual void RebuildIndex()
        {
            _elasticIndex.RecreateIndex();
            foreach (var service in _searchableServices)
            {
                service.FillIndex();
            }
        }
    }
}

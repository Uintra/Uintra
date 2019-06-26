using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LanguageExt;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Localization;
using Umbraco.Web.Mvc;

namespace Uintra.Search.Web
{
    public abstract class SearchControllerBase : SurfaceController
    {
        protected virtual string IndexViewPath { get; } = "~/App_Plugins/Search/Result/Index.cshtml";
        protected virtual string SearchResultViewPath { get; } = "~/App_Plugins/Search/Result/SearchResult.cshtml";
        protected virtual string SearchBoxViewPath { get; } = "~/App_Plugins/Search/Controls/SearchBox.cshtml";
        protected virtual string SearchBoxAutocompleteItemViewPath { get; } = "~/App_Plugins/Search/Controls/SearchBoxAutocompleteItem.cshtml";

        protected virtual int AutocompleteSuggestionCount { get; } = 10;
        protected virtual int ResultsPerPage { get; } = 20;
        protected virtual string SearchTranslationPrefix { get; } = "Search.";

        private readonly IElasticIndex _elasticIndex;
        private readonly IEnumerable<IIndexer> _searchableServices;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly ViewRenderer _viewRenderer;

        protected SearchControllerBase(
            IElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper,
            ISearchableTypeProvider searchableTypeProvider,
            ViewRenderer viewRenderer)
        {
            _elasticIndex = elasticIndex;
            _searchableServices = searchableServices;
            _localizationService = localizationService;
            _searchUmbracoHelper = searchUmbracoHelper;
            _searchableTypeProvider = searchableTypeProvider;
            _viewRenderer = viewRenderer;
        }

        public virtual PartialViewResult Index(SearchRequest searchRequest)
        {
            var result = GetSearchViewModel();
            result.Query = searchRequest.Query;

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

        public virtual JsonResult Autocomplete(SearchRequest searchRequest)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = searchRequest.Query,
                Take = AutocompleteSuggestionCount,
                SearchableTypeIds = GetAutoCompleteSearchableTypes().Select(EnumExtensions.ToInt)
            });

            var result = GetAutocompleteResultModels(searchResult.Documents).ToList();
            if (result.Count > 0)
            {
                var seeAll = GetAllSearchAutocompleteResultModel(searchRequest.Query);
                result.Add(seeAll);
            }

            return Json(new { Documents = result }, JsonRequestBehavior.AllowGet);
        }

        protected virtual IEnumerable<Enum> GetSearchableTypes()
        {
            return _searchableTypeProvider.All;
        }

        protected virtual Lst<Enum> GetAutoCompleteSearchableTypes() => 
            new Lst<Enum>(_searchableTypeProvider.All);

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
                        Name = GetLabelWithCount($"{SearchTranslationPrefix}{type.ToString()}", facet != null ? (int)facet.Count : default(int))
                    };
                });

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

                var searchAutocompleteItem = new SearchBoxAutocompleteItemViewModel
                {
                    Title = model.Title,
                    Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[d.Type].ToString()}")
                };

                model.Html = _viewRenderer.Render(SearchBoxAutocompleteItemViewPath, searchAutocompleteItem);

                return model;
            });

            return result;
        }

        protected virtual string GetLabelWithCount(string label, int count)
        {
            return $"{_localizationService.Translate(label)} ({count})";
        }

        protected virtual SearchAutocompleteResultViewModel GetAllSearchAutocompleteResultModel(string query)
        {
            var seeAll = new SearchAutocompleteResultViewModel
            {
                Title = _localizationService.Translate("SearchBox.SeeAll.lnk"),
                Url = _searchUmbracoHelper.GetSearchPage()?.Url.AddQueryParameter(query)
            };

            var searchAutocompleteItem = GetSeeAllSearchAutocompleteItemModel(seeAll.Title);
            seeAll.Html = _viewRenderer.Render(SearchBoxAutocompleteItemViewPath, searchAutocompleteItem);

            return seeAll;
        }

        protected virtual SearchBoxAutocompleteItemViewModel GetSeeAllSearchAutocompleteItemModel(string title)
        {
            return new SearchBoxAutocompleteItemViewModel { Title = title, Type = SearchConstants.AutocompleteType.All };
        }

        [HttpPost]
        public virtual ActionResult RebuildIndex()
        {
            var response = new
            {
                success = _elasticIndex.RecreateIndex(out var error),
                error
            };
            if (response.success)
                foreach (var service in _searchableServices)
                {
                    service.FillIndex();
                }
            return Json(response);
        }
    }
}

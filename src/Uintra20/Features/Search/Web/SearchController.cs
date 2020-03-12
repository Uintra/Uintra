using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UBaseline.Core.Controllers;
using Uintra20.Core.Localization;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Search.Indexes;
using Uintra20.Features.Search.Models;
using Uintra20.Features.Search.Queries;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Search.Web
{
    public class SearchController : UBaselineApiController
    {
        protected virtual int AutocompleteSuggestionCount { get; } = 10;
        protected virtual int ResultsPerPage { get; } = 20;
        protected virtual string SearchTranslationPrefix { get; } = "Search.";

        private readonly IElasticIndex _elasticIndex;
        private readonly IEnumerable<IIndexer> _searchableServices;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly ISearchableTypeProvider _searchableTypeProvider;

        public SearchController(
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

        public SearchViewModel Index(SearchRequest searchRequest)
        {
            var result = GetSearchViewModel();
            result.Query = searchRequest.Query;
            return result;
        }

        [HttpPost]
        public SearchResultsOverviewViewModel Search(SearchFilterModel model)
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

            return resultModel;
        }

        public SearchBoxViewModel SearchBox()
        {
            var result = new SearchBoxViewModel
            {
                SearchResultsUrl = _searchUmbracoHelper.GetSearchPage()?.Url
            };

            return result;
        }

        public IEnumerable<SearchAutocompleteResultViewModel> Autocomplete(SearchRequest searchRequest)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = searchRequest.Query,
                Take = AutocompleteSuggestionCount,
                SearchableTypeIds = GetUintraSearchableTypes().Select(u=>u.ToInt())
            });

            var result = GetAutocompleteResultModels(searchResult.Documents).ToList();
            if (result.Count > 0)
            {
                var seeAll = GetAllSearchAutocompleteResultModel(searchRequest.Query);
                result.Add(seeAll);
            }

            return result;
        }

        protected virtual IEnumerable<Enum> GetSearchableTypes()
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
            var result = searchResults.Select(searchResult =>
            {
                var model = searchResult.Map<SearchAutocompleteResultViewModel>();

                var searchAutocompleteItem = new SearchBoxAutocompleteItemViewModel
                {
                    Title = model.Title,
                    Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[searchResult.Type].ToString()}")
                };

                if (searchResult is SearchableMember user)
                {
                    searchAutocompleteItem.Email = user.Email;
                    searchAutocompleteItem.Photo = user.Photo;
                }

                model.Item = searchAutocompleteItem;
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
            seeAll.Item = searchAutocompleteItem;

            return seeAll;
        }

        protected virtual SearchBoxAutocompleteItemViewModel GetSeeAllSearchAutocompleteItemModel(string title)
        {
            return new SearchBoxAutocompleteItemViewModel { Title = title, Type = SearchConstants.AutocompleteType.All };
        }

        [HttpPost]
        public RebuildIndexStatusModel RebuildIndex()
        {

            var success = _elasticIndex.RecreateIndex(out var error);

            if (success)
            {
                foreach (var service in _searchableServices)
                {
                    service.FillIndex();
                }
            }

            var status = new RebuildIndexStatusModel()
            {
                Success = success,
                Message = error
            };


            return status;
        }

        private static List<UintraSearchableTypeEnum> GetUintraSearchableTypes() => new
            List<UintraSearchableTypeEnum>()
            {
                UintraSearchableTypeEnum.News,
                UintraSearchableTypeEnum.Events,
                UintraSearchableTypeEnum.Socials,
                UintraSearchableTypeEnum.Content,
                UintraSearchableTypeEnum.Document,
                UintraSearchableTypeEnum.Member,
                UintraSearchableTypeEnum.Tag
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UBaseline.Core.Controllers;
using Uintra20.Core.Localization;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Helpers;
using Uintra20.Core.Search.Indexers;
using Uintra20.Core.Search.Indexes;
using Uintra20.Core.Search.Providers;
using Uintra20.Features.Search.Models;
using Uintra20.Features.Search.Queries;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Search.Web
{
    //todo refactor duplicated code in SearchPageConverter
    //todo after refactor remove unused models and methods
    public class SearchController : UBaselineApiController
    {
        private string SearchTranslationPrefix { get; } = "Search.";
        private int AutocompleteSuggestionCount { get; } = 10;
        private  int ResultsPerPage { get; } = 20;

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

        [HttpPost]
        public  SearchPageViewModel Search (SearchFilterModel model)
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

            var resultModel = GetSearchPage(searchResult);
            resultModel.Query = model.Query;

            return resultModel;
        }

        [HttpPost]
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
        

        protected virtual IEnumerable<Enum> GetFilterItemTypes()
        {
            return _searchableTypeProvider.All;
        }
        
        protected virtual SearchPageViewModel GetSearchPage(SearchResult<SearchableBase> searchResult)
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

            var result = new SearchPageViewModel()
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

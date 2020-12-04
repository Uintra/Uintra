using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Compent.Shared.Extensions.Bcl;
using Compent.Shared.Search.Contract;
using UBaseline.Core.Controllers;
using UBaseline.Search.Core;
using Uintra.Core.Localization;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers.Diagnostics.Models;
using Uintra.Core.Search.Repository;
using Uintra.Features.Search.Models;
using Uintra.Infrastructure.Extensions;
using ISearchableTypeProvider = Uintra.Core.Search.Providers.ISearchableTypeProvider;
using SearchAutocompleteResultViewModel = Uintra.Features.Search.Models.SearchAutocompleteResultViewModel;
using SearchByTextQuery = Uintra.Core.Search.Queries.SearchByTextQuery;
using SearchFilterModel = Uintra.Features.Search.Models.SearchFilterModel;
using SearchResultViewModel = Uintra.Features.Search.Models.SearchResultViewModel;

namespace Uintra.Features.Search.Web
{
    //todo refactor duplicated code in SearchPageConverter|
    //todo after refactor remove unused models and methods
    public class SearchController : UBaselineApiController
    {
        private string SearchTranslationPrefix { get; } = "Search.";
        private int AutocompleteSuggestionCount { get; } = 10;
        private int ResultsPerPage { get; } = 20;

        private readonly IIntranetLocalizationService _localizationService;
        private readonly ISearchUmbracoHelper _searchUmbracoHelper;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IEnumerable<ISearchDocumentIndexer> indexers;
        private readonly IUintraSearchRepository _searchRepository;
        
        public SearchController(
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper,
            ISearchableTypeProvider searchableTypeProvider,
            IEnumerable<ISearchDocumentIndexer> indexers)
        {
            _localizationService = localizationService;
            _searchUmbracoHelper = searchUmbracoHelper;
            _searchableTypeProvider = searchableTypeProvider;
            this.indexers = indexers;
        }

        [HttpPost]
        public async Task<SearchPageViewModel> Search(SearchFilterModel model)
        {
            var searchableTypeIds = model.Types.Count > 0 ? model.Types : GetSearchableTypes().Select(t => t.ToInt());
            var searchByTextQuery = new SearchByTextQuery
            {
                Text = model.Query,
                Take = ResultsPerPage * model.Page,
                SearchableTypeIds = searchableTypeIds,
                OnlyPinned = model.OnlyPinned,
                ApplyHighlights = true
            };

            var searchResult = await _searchRepository.SearchAsyncTyped(searchByTextQuery);
            
            var resultModel = GetSearchPage(searchResult);
            resultModel.Query = model.Query;

            return resultModel;
        }

        [HttpPost]
        public async Task<IEnumerable<SearchAutocompleteResultViewModel>> Autocomplete(SearchRequest searchRequest)
        {
            var searchByTextQuery = new SearchByTextQuery
            {
                Text = searchRequest.Query,
                Take = AutocompleteSuggestionCount,
                SearchableTypeIds = GetAutocompleteSearchableTypes().Select(u => u.ToInt())
            };

            var searchResult = await _searchRepository.SearchAsync(searchByTextQuery, String.Empty);

            var result = GetAutocompleteResultModels(searchResult.Documents).ToList();
            if (result.Count > 0)
            {
                var seeAll = GetAllSearchAutocompleteResultModel(searchRequest.Query);
                result.Add(seeAll);
            }

            return result;
        }

        protected virtual SearchPageViewModel GetSearchPage(Core.Search.Entities.SearchResult<SearchableBase> searchResult)
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
                ResultsCount = (int)searchResult.TotalCount,
                FilterItems = filterItems,
                AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", (int)searchResult.TotalCount),
                BlockScrolling = searchResult.TotalCount <= searchResultViewModels.Count
            };

            return result;
        }

        protected virtual IEnumerable<SearchAutocompleteResultViewModel> GetAutocompleteResultModels(
            IEnumerable<ISearchDocument> searchResults)
        {
            var result = searchResults.OfType<SearchableBase>().Select(searchResult =>
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
                Url = _searchUmbracoHelper.GetSearchPage()?.Url.AddQueryParameter(query).ToLinkModel()
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
        public async Task<RebuildIndexStatusModel> RebuildIndex()
        {
            // TODO: Search. Add wrapper with detailed result?
            // TODO: Search. Adjust FE
            var indexRebuildResults = indexers.Select(i =>
                (IndexType: i.Type, Task: i.RebuildIndex()))
                .AsList();

            await Task.WhenAll(indexRebuildResults.Select(i => i.Task));
            var res = indexRebuildResults.Select(i => new IndexedModelResult()
            {
                IndexedName = i.IndexType.ToString(),
                Success = i.Task.Result
            });

            var status = new RebuildIndexStatusModel
            {
                Success = indexRebuildResults.All(i => i.Task.Result),
                //Message = error,
                Index = res
            };

            return status;
        }

        private static List<UintraSearchableTypeEnum> GetAutocompleteSearchableTypes() => new
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

        private static IEnumerable<UintraSearchableTypeEnum> GetSearchableTypes()
        {
            return GetAutocompleteSearchableTypes().Except(UintraSearchableTypeEnum.Tag.ToEnumerable());
        }
    }
}

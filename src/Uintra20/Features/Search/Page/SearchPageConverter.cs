using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Extensions;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.SearchPage;
using Uintra20.Core.Localization;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexes;
using Uintra20.Core.Search.Providers;
using Uintra20.Features.Search.Models;
using Uintra20.Features.Search.Queries;
using Uintra20.Infrastructure.Extensions;
using SearchPageViewModel = Uintra20.Features.Search.Models.SearchPageViewModel;

namespace Uintra20.Features.Search.Page
{
    public class SearchPageConverter : INodeViewModelConverter<SearchPageModel, SearchPageViewModel>
    {
        private  int ResultsPerPage { get; } = 20;
        private string SearchTranslationPrefix { get; } = "Search.";
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IElasticIndex _elasticIndex;
        private readonly IUBaselineRequestContext _requestContext;

        public SearchPageConverter(
            IIntranetLocalizationService intranetLocalizationService,
            ISearchableTypeProvider searchableTypeProvider,
            IElasticIndex elasticIndex,
            IUBaselineRequestContext requestContext)
        {
            _intranetLocalizationService = intranetLocalizationService;
            _searchableTypeProvider = searchableTypeProvider;
            _elasticIndex = elasticIndex;
            _requestContext = requestContext;
        }

        public void Map(SearchPageModel node, SearchPageViewModel viewModel)
        {
            var query = System.Web.HttpUtility.ParseQueryString(_requestContext.NodeRequestParams.NodeUrl.Query).TryGetQueryValue<string>("query");
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = ResultsPerPage * 1,
                SearchableTypeIds = GetSearchableTypes().Select(t => t.ToInt()),
                OnlyPinned = false,
                ApplyHighlights = true
            });

            var resultModel = ExtendSearchPage(searchResult,viewModel);
            resultModel.Query = query;
        }
        

        protected virtual SearchPageViewModel ExtendSearchPage(
            SearchResult<SearchableBase> searchResult,SearchPageViewModel viewModel)
        {
            var searchResultViewModels = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<SearchResultViewModel>();
                resultItem.Type =
                    _intranetLocalizationService.Translate(
                        $"{SearchTranslationPrefix}{_searchableTypeProvider[d.Type].ToString()}");
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
                        Name = GetLabelWithCount($"{SearchTranslationPrefix}{type.ToString()}",
                            facet != null ? (int) facet.Count : default(int))
                    };
                });

            viewModel.Results = searchResultViewModels;
            viewModel.ResultsCount = (int) searchResult.TotalHits;
            viewModel.FilterItems = filterItems;
            viewModel.AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", (int) searchResult.TotalHits);
            viewModel.BlockScrolling = searchResult.TotalHits <= searchResultViewModels.Count;
        
            return viewModel;
        }

        protected virtual string GetLabelWithCount(string label, int count)
        {
            return $"{_intranetLocalizationService.Translate(label)} ({count})";
        }

        protected virtual IEnumerable<Enum> GetSearchableTypes()
        {
            return _searchableTypeProvider.All;
        }

        private static List<Enum> GetFilterItemTypes() => new
            List<Enum>()
            {
                UintraSearchableTypeEnum.News,
                UintraSearchableTypeEnum.Events,
                UintraSearchableTypeEnum.Socials,
                UintraSearchableTypeEnum.Content,
                UintraSearchableTypeEnum.Document,
                UintraSearchableTypeEnum.Member,
                UintraSearchableTypeEnum.Tag,
            };
    }
}
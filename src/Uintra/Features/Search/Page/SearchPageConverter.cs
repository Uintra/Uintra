﻿using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Extensions;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.SearchPage;
using Uintra.Core.Localization;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Providers;
using Uintra.Core.Search.Queries;
using Uintra.Core.Search.Repository;
using Uintra.Features.Search.Models;
using Uintra.Infrastructure.Extensions;
using SearchableBase = Uintra.Core.Search.Entities.SearchableBase;
using SearchPageViewModel = Uintra.Features.Search.Models.SearchPageViewModel;

namespace Uintra.Features.Search.Page
{
    public class SearchPageConverter : INodeViewModelConverter<SearchPageModel, SearchPageViewModel>
    {
        private  int ResultsPerPage { get; } = 20;
        private string SearchTranslationPrefix { get; } = "Search.";
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly IUintraSearchRepository _searchRepository;

        public SearchPageConverter(
            IIntranetLocalizationService intranetLocalizationService,
            ISearchableTypeProvider searchableTypeProvider,
            IUBaselineRequestContext requestContext,
            IUintraSearchRepository searchRepository)
        {
            _intranetLocalizationService = intranetLocalizationService;
            _searchableTypeProvider = searchableTypeProvider;
            _requestContext = requestContext;
            _searchRepository = searchRepository;
        }

        public void Map(SearchPageModel node, SearchPageViewModel viewModel)
        {
            var query = System.Web.HttpUtility.ParseQueryString(_requestContext.NodeRequestParams.NodeUrl.Query).TryGetQueryValue<string>("query");
            var searchBytTextQuery = new SearchByTextQuery
            {
                Text = query,
                Take = ResultsPerPage * 1,
                SearchableTypeIds = GetSearchableTypes().Select(t => t.ToInt()),
                OnlyPinned = false,
                ApplyHighlights = true
            };
            var searchResult = AsyncHelpers.RunSync(() =>_searchRepository.SearchAsyncTyped(searchBytTextQuery));

            var resultModel = ExtendSearchPage(searchResult,viewModel);
            resultModel.Query = query;
        }
        

        protected virtual SearchPageViewModel ExtendSearchPage(Core.Search.Entities.SearchResult<SearchableBase> searchResult,SearchPageViewModel viewModel)
        {
            var searchResultViewModels = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<SearchResultViewModel>();
                resultItem.Type =
                    _intranetLocalizationService.Translate(
                        $"{SearchTranslationPrefix}{_searchableTypeProvider[d.Type]}");
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
            viewModel.ResultsCount = searchResult.TotalCount;
            viewModel.FilterItems = filterItems;
            viewModel.AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", searchResult.TotalCount);
            viewModel.BlockScrolling = searchResult.TotalCount <= searchResultViewModels.Count;
        
            return viewModel;
        }

        protected virtual string GetLabelWithCount(string label, int count)
        {
            return $"{_intranetLocalizationService.Translate(label)} ({count})";
        }

        protected virtual IEnumerable<Enum> GetSearchableTypes()
        {
            return _searchableTypeProvider.All.Except(((Enum)UintraSearchableTypeEnum.Tag).ToEnumerable());
        }
    }
}
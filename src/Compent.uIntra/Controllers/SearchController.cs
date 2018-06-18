using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Search;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.Search.Models;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Localization;
using Uintra.Search;
using Uintra.Search.Web;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    public class SearchController : SearchControllerBase
    {
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IElasticIndex _elasticIndex;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly ViewRenderer _viewRenderer;

        protected override string SearchResultViewPath { get; } = "~/Views/Search/SearchResult.cshtml";
        protected override string SearchBoxAutocompleteItemViewPath { get; } = "~/Views/Search/SearchBoxAutocompleteItem.cshtml";

        public SearchController(
            IElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper,
            ISearchableTypeProvider searchableTypeProvider,
            ViewRenderer viewRenderer)
            : base(elasticIndex, searchableServices, localizationService, searchUmbracoHelper, searchableTypeProvider, viewRenderer)
        {
            _localizationService = localizationService;
            _elasticIndex = elasticIndex;
            _searchableTypeProvider = searchableTypeProvider;
            _viewRenderer = viewRenderer;
        }

        [HttpPost]
        public override PartialViewResult Search(SearchFilterModel model)
        {
            var searchableTypeIds = model.Types.Count > 0 ? model.Types : GetSearchableTypes().Select(t => t.ToInt());

            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = model.Query,
                OnlyPinned = model.OnlyPinned,
                Take = ResultsPerPage * model.Page,
                SearchableTypeIds = searchableTypeIds,
                ApplyHighlights = true
            });

            var resultModel = GetUintraSearchResultsOverviewModel(searchResult);
            resultModel.Query = model.Query;

            return PartialView(SearchResultViewPath, resultModel);
        }

        protected override IEnumerable<Enum> GetAutoCompleteSearchableTypes()
        {
            var types = GetUintraSearchableTypes().ToList();
            types.Add(UintraSearchableTypeEnum.Tag);

            return types;
        }

        protected override IEnumerable<Enum> GetFilterItemTypes() => GetSearchableTypes();

        protected override IEnumerable<Enum> GetSearchableTypes() => GetUintraSearchableTypes();

        private UintraSearchResultsOverviewViewModel GetUintraSearchResultsOverviewModel(SearchResult<SearchableBase> searchResult)
        {
            var searchResultViewModels = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<UintraSearchResultViewModel>();
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
                }
            );

            var result = new UintraSearchResultsOverviewViewModel
            {
                Results = searchResultViewModels,
                ResultsCount = (int)searchResult.TotalHits,
                FilterItems = filterItems,
                AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", (int)searchResult.TotalHits),
                BlockScrolling = searchResult.TotalHits <= searchResultViewModels.Count
            };

            return result;
        }

        protected override IEnumerable<SearchAutocompleteResultViewModel> GetAutocompleteResultModels(IEnumerable<SearchableBase> searchResults)
        {
            var result = searchResults.Select(searchResult =>
            {
                var model = searchResult.Map<SearchAutocompleteResultViewModel>();

                var searchAutocompleteItem = new SearchBoxAutocompleteItemExtendedViewModel
                {
                    Title = model.Title,
                    Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[searchResult.Type].ToString()}")
                };

                if (searchResult is SearchableUser user)
                {
                    searchAutocompleteItem.Email = user.Email;
                    searchAutocompleteItem.Photo = user.Photo;
                }

                model.Html = _viewRenderer.Render(SearchBoxAutocompleteItemViewPath, searchAutocompleteItem);
                return model;
            });

            return result;
        }

        private static IEnumerable<Enum> GetUintraSearchableTypes()
        {
            return new Enum[]
            {
                UintraSearchableTypeEnum.News,
                UintraSearchableTypeEnum.Events,
                UintraSearchableTypeEnum.Bulletins,
                UintraSearchableTypeEnum.Content,
                UintraSearchableTypeEnum.Document,
                UintraSearchableTypeEnum.User
            };
        }

        protected override SearchBoxAutocompleteItemViewModel GetSeeAllSearchAutocompleteItemModel(string title)
        {
            var seeAllSearchAutocompleteItem = base.GetSeeAllSearchAutocompleteItemModel(title);
            return new SearchBoxAutocompleteItemExtendedViewModel { Title = seeAllSearchAutocompleteItem.Title, Type = seeAllSearchAutocompleteItem.Type };
        }
    }
}
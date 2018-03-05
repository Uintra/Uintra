using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Search;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.Search.Models;
using Localization.Umbraco.Attributes;
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

        protected override string SearchResultViewPath { get; } = "~/Views/Search/SearchResult.cshtml";

        public SearchController(
            IElasticIndex elasticIndex,
            IEnumerable<IIndexer> searchableServices,
            IIntranetLocalizationService localizationService,
            ISearchUmbracoHelper searchUmbracoHelper,
            ISearchableTypeProvider searchableTypeProvider)
            : base(elasticIndex, searchableServices, localizationService, searchUmbracoHelper, searchableTypeProvider)
        {
            _localizationService = localizationService;
            _elasticIndex = elasticIndex;
            _searchableTypeProvider = searchableTypeProvider;
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
                var model = searchResult.Map<UintraSearchAutocompleteResultViewModel>();
                model.Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider[searchResult.Type].ToString()}");
                if (searchResult is SearchableUser user)
                {
                    var email = new SearchInfoListItemModel { Name = "Email", Value = user.Email };
                    var photo = new SearchInfoListItemModel { Name = "Photo", Value = user.Photo };
                    model.AdditionalInfo = new List<SearchInfoListItemModel> { email, photo };
                }

                return model;
            });

            return result;
        }

        private IEnumerable<Enum> GetUintraSearchableTypes()
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
    }
}
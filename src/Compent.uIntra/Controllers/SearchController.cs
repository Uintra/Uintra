using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Search;
using Compent.uIntra.Core.Search.Entities;
using Compent.uIntra.Core.Search.Models;
using Localization.Umbraco.Attributes;
using uIntra.Core.Extensions;
using uIntra.Core.Localization;
using uIntra.Core.TypeProviders;
using uIntra.Search;
using uIntra.Search.Web;

namespace Compent.uIntra.Controllers
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
            var searchableTypeIds = model.Types.Count > 0 ? model.Types : GetSearchableTypes().Select(t => t.Id);

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

        protected override IEnumerable<IIntranetType> GetAutoCompleteSearchableTypes()
        {
            var types = GetUintraSearchableTypes().ToList();
            types.Add(new IntranetType()
            {
                Id = (int) UintraSearchableTypeEnum.Tag,
                Name = UintraSearchableTypeEnum.Tag.ToString()
            });

            return types;
        }

        protected override IEnumerable<IIntranetType> GetFilterItemTypes()
        {
            return GetSearchableTypes();
        }

        protected override IEnumerable<IIntranetType> GetSearchableTypes()
        {
            return GetUintraSearchableTypes();
        }

        private UintraSearchResultsOverviewViewModel GetUintraSearchResultsOverviewModel(SearchResult<SearchableBase> searchResult)
        {
            var searchResultViewModels = searchResult.Documents.Select(d =>
            {
                var resultItem = d.Map<UintraSearchResultViewModel>();
                resultItem.Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider.Get(d.Type).Name}");
                return resultItem;
            }).ToList();

            var filterItems = GetSearchableTypes().GroupJoin(
                searchResult.TypeFacets,
                type => type.Id,
                facet => int.Parse(facet.Name),
                (type, facets) =>
                {
                    var facet = facets.FirstOrDefault();
                    return new SearchFilterItemViewModel
                    {
                        Id = type.Id,
                        Name = GetLabelWithCount($"{SearchTranslationPrefix}{type.Name}", facet != null ? (int) facet.Count : default)
                    };
                }
            );

            var result = new UintraSearchResultsOverviewViewModel
            {
                Results = searchResultViewModels,
                ResultsCount = (int) searchResult.TotalHits,
                FilterItems = filterItems,
                AllTypesPlaceholder = GetLabelWithCount("Search.Filter.All.lbl", (int) searchResult.TotalHits)
            };

            return result;
        }

        protected override IEnumerable<SearchAutocompleteResultViewModel> GetAutocompleteResultModels(IEnumerable<SearchableBase> searchResults)
        {
            var result = searchResults.Select(searchResult =>
            {
                var model = searchResult.Map<UintraSearchAutocompleteResultViewModel>();
                model.Type = _localizationService.Translate($"{SearchTranslationPrefix}{_searchableTypeProvider.Get(searchResult.Type).Name}");
                if (searchResult is SearchableUser user)
                {
                    var email = new SearchInfoListItemModel {Name = "Email", Value = user.Email};
                    var phone = new SearchInfoListItemModel {Name = "Phone", Value = user.Phone};
                    var photo = new SearchInfoListItemModel {Name = "Photo", Value = user.Photo};
                    model.AdditionalInfo = new List<SearchInfoListItemModel> {email, photo, phone};
                }

                return model;
            });

            return result;
        }

        private IEnumerable<IIntranetType> GetUintraSearchableTypes()
        {
            return new[]
            {
                UintraSearchableTypeEnum.News,
                UintraSearchableTypeEnum.Events,
                UintraSearchableTypeEnum.Bulletins,
                UintraSearchableTypeEnum.Content,
                UintraSearchableTypeEnum.Document,
                UintraSearchableTypeEnum.User
            }.Select(t => new IntranetType
            {
                Id = (int) t,
                Name = t.ToString()
            });
        }
    }
}
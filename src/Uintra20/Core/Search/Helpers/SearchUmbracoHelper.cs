using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using UBaseline.Shared.SearchPage;
using Uintra20.Core.Search.Entities;
using Uintra20.Features.Search.Web;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Search.Helpers
{
    public class SearchUmbracoHelper : ISearchUmbracoHelper
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ISearchContentPanelConverterProvider _searchContentPanelConverterProvider;

        public SearchUmbracoHelper(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            INodeModelService nodeModelService,
            IUBaselineRequestContext requestContext,
            ISearchContentPanelConverterProvider searchContentPanelConverterProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _nodeModelService = nodeModelService;
            _requestContext = requestContext;
            _searchContentPanelConverterProvider = searchContentPanelConverterProvider;
        }

        public SearchPageModel GetSearchPage()
        {
            return _nodeModelService.GetByAlias<SearchPageModel>("searchPage", _requestContext.HomeNode.RootId);
        }

        public bool IsSearchable(IPublishedContent content)
        {
            var isContentPage = content.ContentType.Alias.Equals(_documentTypeAliasProvider.GetArticlePage());
            return isContentPage && content.GetProperty("includeInSearch").Value<bool>();
        }

        public string GetSearchLink(string searchQuery)
        {
            var searchPage = GetSearchPage();
            var searchLink = searchPage?.Url.AddParameter("query", searchQuery);

            return searchLink;
        }

        public SearchableContent GetContent(IPublishedContent publishedContent)
        {
            var panelsComposition = _nodeModelService.Get<NodeModel>(publishedContent.Id) as IPanelsComposition;
            return new SearchableContent
            {
                Id = publishedContent.Id,
                Type = SearchableTypeEnum.Content.ToInt(),
                Url = publishedContent.Url.ToLinkModel(),
                Title = publishedContent.Name,
                Panels = panelsComposition == null
                    ? Enumerable.Empty<SearchablePanel>()
                    : _searchContentPanelConverterProvider.Convert(panelsComposition)
            };
        }
    }
}
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.SearchPage;
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

        public SearchUmbracoHelper(
            IDocumentTypeAliasProvider documentTypeAliasProvider, 
            INodeModelService nodeModelService,
            IUBaselineRequestContext requestContext)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _nodeModelService = nodeModelService;
            _requestContext = requestContext;
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
    }
}
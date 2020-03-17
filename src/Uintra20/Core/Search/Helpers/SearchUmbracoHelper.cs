using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Core.Search.Helpers
{
    public class SearchUmbracoHelper : ISearchUmbracoHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public SearchUmbracoHelper(UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetSearchPage()
        {
            //todo-search rework to ubaseline approach
            //return _umbracoHelper.ContentAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetSearchResultPage()));
            return null;
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
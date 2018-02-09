using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Search;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.Uintra.Core.Search
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
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetSearchResultPage()));
        }

        public bool IsSearchable(IPublishedContent content)
        {
            var isContentPage = content.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetContentPage());
            return isContentPage && content.GetPropertyValue<bool>("useInSearch"); 
        }

        public string GetSearchLink(string searchQuery)
        {
            var searchPage = GetSearchPage();
            var searchLink = searchPage?.Url.AddParameter("query", searchQuery);

            return searchLink;
        }
    }
}
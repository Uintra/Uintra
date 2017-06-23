using uIntra.Core;
using uIntra.Search.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core.Search
{
    public class SearchUmbracoHelper: ISearchUmbracoHelper
    {
        private readonly UmbracoHelper _umbracoHelper;

        public SearchUmbracoHelper(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public IPublishedContent GetSearchPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias, SearchResultPage.ModelTypeAlias));
        }

        public bool IsSearchable(IPublishedContent content)
        {
            return content.ContentType == ContentPage.GetModelContentType() && ((ContentPage)content).UseInSearch;
        }
    }
}
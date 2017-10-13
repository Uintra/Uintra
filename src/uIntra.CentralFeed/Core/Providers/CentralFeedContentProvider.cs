using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.CentralFeed.Providers
{
    public class CentralFeedContentProvider : ContentProviderBase, ICentralFeedContentProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public CentralFeedContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider, UmbracoHelper umbracoHelper)
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetOverviewPage()
        {
            var xPath = _documentTypeAliasProvider.GetHomePage().ToEnumerableOfOne();
            return GetContent(xPath);
        }
    }
}
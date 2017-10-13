using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Extentions;
using Umbraco.Web;

namespace uIntra.CentralFeed.Providers
{
    public class CentralFeedContentProvider : FeedContentProviderBase, ICentralFeedContentProvider
    {
        protected override IEnumerable<string> OverviewXPath { get; }

        public CentralFeedContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper)
            : base(umbracoHelper)
        {
            OverviewXPath = documentTypeAliasProvider.GetHomePage().ToEnumerableOfOne();
        }
    }
}
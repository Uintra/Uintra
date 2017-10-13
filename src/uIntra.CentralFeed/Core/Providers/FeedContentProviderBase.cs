using System.Collections.Generic;
using uIntra.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.CentralFeed.Providers
{
    public abstract class FeedContentProviderBase : ContentProviderBase, IFeedContentProvider
    {
        protected abstract IEnumerable<string> OverviewXPath { get; }

        protected FeedContentProviderBase(UmbracoHelper umbracoHelper) : base(umbracoHelper)
        {}

        public virtual IPublishedContent GetOverviewPage() =>
            GetContent(OverviewXPath);

        public abstract IEnumerable<IPublishedContent> GetRelatedPages();
    }
}
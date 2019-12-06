using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Providers
{
    public abstract class FeedContentProviderBase : ContentProviderBase, IFeedContentProvider
    {
        protected abstract IEnumerable<string> OverviewAliasPath { get; }

        protected FeedContentProviderBase(UmbracoHelper umbracoHelper) : base(umbracoHelper)
        { }

        public virtual IPublishedContent GetOverviewPage() =>
            GetContent(OverviewAliasPath);

        public abstract IEnumerable<IPublishedContent> GetRelatedPages();
    }
}
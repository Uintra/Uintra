using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed.Providers
{
    public abstract class FeedContentProviderBase : ContentProviderBase, IFeedContentProvider
    {
        protected abstract IEnumerable<string> OverviewAliasPath { get; }

        protected FeedContentProviderBase() : base()
        { }

        public virtual IPublishedContent GetOverviewPage() =>
            GetContent(OverviewAliasPath);

        public abstract IEnumerable<IPublishedContent> GetRelatedPages();
    }
}
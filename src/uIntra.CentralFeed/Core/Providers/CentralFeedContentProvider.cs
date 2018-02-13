using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core;
using Uintra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.CentralFeed.Providers
{
    public class CentralFeedContentProvider : FeedContentProviderBase, ICentralFeedContentProvider
    {
        protected override IEnumerable<string> OverviewXPath { get; }

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public CentralFeedContentProvider(IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper, IActivityTypeProvider activityTypeProvider)
            : base(umbracoHelper)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activityTypeProvider = activityTypeProvider;

            OverviewXPath = documentTypeAliasProvider.GetHomePage().ToEnumerable();
        }

        public override IEnumerable<IPublishedContent> GetRelatedPages()
        {
            var activityAliases = _activityTypeProvider
                .All
                .Select(_documentTypeAliasProvider.GetOverviewPage)
                .ToArray();
            return GetOverviewPage().Children.Where(c => c.DocumentTypeAlias.In(activityAliases));
        }
    }
}
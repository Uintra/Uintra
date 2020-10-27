﻿using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using Uintra.Infrastructure.Providers;
using Uintra.Infrastructure.TypeProviders;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Features.CentralFeed.Providers
{
    public class CentralFeedContentProvider : FeedContentProviderBase, ICentralFeedContentProvider
    {
        protected override IEnumerable<string> OverviewAliasPath { get; }

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public CentralFeedContentProvider(
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IActivityTypeProvider activityTypeProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _activityTypeProvider = activityTypeProvider;

            OverviewAliasPath = documentTypeAliasProvider.GetHomePage().ToEnumerable();
        }
        
        public override IEnumerable<IPublishedContent> GetRelatedPages()
        {
            var activityAliases = _activityTypeProvider
                .All
                .Select(_documentTypeAliasProvider.GetOverviewPage)
                .ToArray();

            var children = GetOverviewPage().Children.ToArray();

            var result = children.Where(c => c.ContentType.Alias.In(activityAliases)).ToArray();

            return result;
        }
    }
}
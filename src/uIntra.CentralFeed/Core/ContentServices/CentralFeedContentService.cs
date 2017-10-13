using System.Collections.Generic;
using uIntra.CentralFeed.Providers;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;
using Umbraco.Web;
using static uIntra.CentralFeed.CentralFeedConstants;

namespace uIntra.CentralFeed
{
    public class CentralFeedContentService : FeedContentServiceBase, ICentralFeedContentService
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly ICentralFeedLinkService _centralFeedLinkService;
        private readonly ICentralFeedContentProvider _contentProvider;

        protected override string FeedPluginAlias { get; } = CentralFeedPluginAlias;
        protected override string ActivityCreatePluginAlias { get; } = FeedActivityCreatePluginAlias;

        public CentralFeedContentService(
            ICentralFeedService centralFeedService,
            ICentralFeedLinkService centralFeedLinkService,
            ICentralFeedContentProvider contentProvider)
        {
            _centralFeedService = centralFeedService;
            _centralFeedLinkService = centralFeedLinkService;
            _contentProvider = contentProvider;
        }

        public IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            var overviewPage = _contentProvider.GetOverviewPage();
            var type = GetFeedTabType(overviewPage);
            yield return new ActivityFeedTabModel
            {
                Content = overviewPage,
                Type = type,
                IsActive = overviewPage.Id == currentPage.Id,
                Links = _centralFeedLinkService.GetCreateLinks(type)
            };

            foreach (var content in GetRelatedContent())
            {
                var tabType = GetFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                {
                    continue;
                }
                var settings = _centralFeedService.GetSettings(tabType);
                yield return new ActivityFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    HasSubscribersFilter = settings.HasSubscribersFilter,
                    HasPinnedFilter = settings.HasPinnedFilter,
                    IsActive = content.IsAncestorOrSelf(currentPage),
                    Links = _centralFeedLinkService.GetCreateLinks(tabType),
                };
            }
        }
    }
}
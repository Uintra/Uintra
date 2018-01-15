using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed.Providers;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
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
        private readonly IActivityTypeProvider _activityTypeProvider;

        protected override string FeedPluginAlias { get; } = CentralFeedPluginAlias;
        protected override string ActivityCreatePluginAlias { get; } = FeedActivityCreatePluginAlias;

        public CentralFeedContentService(
            IFeedTypeProvider feedTypeProvider,
            IGridHelper gridHelper,
            ICentralFeedService centralFeedService,
            ICentralFeedLinkService centralFeedLinkService,
            ICentralFeedContentProvider contentProvider, IActivityTypeProvider activityTypeProvider)
                : base(feedTypeProvider, gridHelper)
        {
            _centralFeedService = centralFeedService;
            _centralFeedLinkService = centralFeedLinkService;
            _contentProvider = contentProvider;
            _activityTypeProvider = activityTypeProvider;
        }

        private ActivityFeedTabModel GetMainFeedTab(IPublishedContent currentPage)
        {
            var overviewPage = _contentProvider.GetOverviewPage();
            var type = GetFeedTabType(overviewPage);
            return new ActivityFeedTabModel
            {
                Content = overviewPage,
                Type = type,
                IsActive = overviewPage.Id == currentPage.Id,
                Links = _centralFeedLinkService.GetCreateLinks(type)
            };
        }

        public IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            yield return GetMainFeedTab(currentPage);

            var allActivityTypes = _activityTypeProvider.GetAll().ToList();

            foreach (var content in _contentProvider.GetRelatedPages())
            {
                var tabType = GetFeedTabType(content);
                var activityType = allActivityTypes.SingleOrDefault(a => a.Id == tabType.Id);

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
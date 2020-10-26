using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Features.Links;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Grid;
using Uintra20.Infrastructure.TypeProviders;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.CentralFeed.Services
{
    public class CentralFeedContentService : FeedContentServiceBase, ICentralFeedContentService
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly ICentralFeedContentProvider _contentProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public CentralFeedContentService(
            IFeedTypeProvider feedTypeProvider,
            IGridHelper gridHelper,
            ICentralFeedService centralFeedService,
            IFeedLinkService feedLinkService,
            ICentralFeedContentProvider contentProvider,
            IActivityTypeProvider activityTypeProvider) : base(feedTypeProvider, gridHelper)
        {
            _centralFeedService = centralFeedService;
            _feedLinkService = feedLinkService;
            _contentProvider = contentProvider;
            _activityTypeProvider = activityTypeProvider;
        }
        protected override string FeedPluginAlias => "custom.CentralFeed";
        protected override string ActivityCreatePluginAlias => "custom.ActivityCreate";

        public IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
           
            yield return GetMainFeedTab(currentPage);

            var allActivityTypes = _activityTypeProvider.All;

            foreach (var content in _contentProvider.GetRelatedPages())
            {
                var tabType = GetFeedTabType(content);
                var activityType = allActivityTypes.SingleOrDefault(a => a.ToInt() == tabType.ToInt());

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
                    Links = _feedLinkService.GetCreateLinks(tabType)
                };
            }
        }

        public Enum GetCreateActivityType(IPublishedContent content)
        {
            throw new NotImplementedException();
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
                Links = _feedLinkService.GetCreateLinks(type)
            };
        }

    }
}
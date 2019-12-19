using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Links;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.UintraPanels.LastActivities.Helpers
{
    public class LatestActivitiesPanelHelper : ILatestActivitiesPanelHelper
    {
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public LatestActivitiesPanelHelper(
            ICentralFeedService centralFeedService,
            IFeedTypeProvider centralFeedTypeProvider,
            IFeedLinkService feedLinkService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _centralFeedService = centralFeedService;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _feedLinkService = feedLinkService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }
       

        public (bool isShowMore, IEnumerable<FeedItemViewModel> feedItems) GetFeedItems(LatestActivitiesPanelModel node)
        {
            var settings = _centralFeedService.GetAllSettings();
            var centralFeedType = _centralFeedTypeProvider[node.ActivityType.Value.Id];

            var latestActivities = GetLatestActivities(centralFeedType, node.CountToDisplay.Value);
            var feedItems = GetFeedItems(latestActivities.activities, settings).ToArray();

            return (latestActivities.activities.Count() < latestActivities.totalCount, feedItems);
        }

        private (IEnumerable<IFeedItem> activities, int totalCount) GetLatestActivities(Enum activityType, int activityAmount)
        {
            var items = GetCentralFeedItems(activityType).ToList();
            var filteredItems = FilterLatestActivities(items).Take(activityAmount);
            var sortedItems = Sort(filteredItems, activityType);

            return (sortedItems, items.Count);
        }

        private IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type)
        {
            IEnumerable<IFeedItem> result;
            switch (type)
            {
                case CentralFeedTypeEnum.All:
                    result = sortedItems.OrderBy(i => i, new CentralFeedItemComparer());
                    break;
                default:
                    result = sortedItems.OrderByDescending(el => el.PublishDate);
                    break;
            }
            return result;
        }
        private IEnumerable<FeedItemViewModel> GetFeedItems(IEnumerable<IFeedItem> items, IEnumerable<FeedSettings> settings)
        {
            var activitySettings = settings
                .ToDictionary(s => s.Type.ToInt());

            var result = items
                .Select(i => MapFeedItemToViewModel(i, activitySettings));

            return result;
        }
        private FeedItemViewModel MapFeedItemToViewModel(IFeedItem i, Dictionary<int, FeedSettings> settings)
        {
            var options = GetActivityFeedOptions(i.Id);

            var activity = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(i.Type).GetPreviewModel(i.Id);

            return new FeedItemViewModel
            {
                Activity = activity,
                Options = options,
                ControllerName = settings[i.Type.ToInt()].Controller
            };
        }
        private ActivityFeedOptions GetActivityFeedOptions(Guid activityId)
        {
            return new ActivityFeedOptions()
            {
                Links = _feedLinkService.GetLinks(activityId)
            };
        }
        private IEnumerable<IFeedItem> GetCentralFeedItems(Enum type)
        {
            if (IsTypeForAllActivities(type))
            {
                var items = _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate);
                return items;
            }
            return _centralFeedService.GetFeed(type);
        }
        private IEnumerable<IFeedItem> FilterLatestActivities(IEnumerable<IFeedItem> activities)
        {
            var settings = _centralFeedService.GetAllSettings().Where(s => !s.ExcludeFromLatestActivities).Select(s => s.Type);
            var items = activities.Join(settings, item => item.Type.ToInt(), type => type.ToInt(), (item, _) => item);

            return items;
        }
        private static bool IsTypeForAllActivities(Enum type) =>
            type is CentralFeedTypeEnum.All;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Links;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IFeedLinkService _feedLinkService;

        public LatestActivitiesPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            ICentralFeedService centralFeedService,
            IFeedTypeProvider centralFeedTypeProvider,
            IFeedLinkService feedLinkService)
        {
            _requestContext = requestContext;
            _centralFeedService = centralFeedService;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _feedLinkService = feedLinkService;
        }
        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var settings = _centralFeedService.GetAllSettings();
            var centralFeedType = _centralFeedTypeProvider[node.ActivityType.Value.Id];

            //var test = DependencyResolver.Current.GetService<ICentralFeedContentService>();

            var latestActivities = GetLatestActivities(centralFeedType, node.CountToDisplay.Value);
            var feedItems = GetFeedItems(latestActivities.activities, settings);
            //var tab = GetTabForActivityType(centralFeedType);

            viewModel.Feed = feedItems;
            viewModel.Tab = null;
            viewModel.ShowSeeAllButton = latestActivities.activities.Count() < latestActivities.totalCount;
        }

        private (IEnumerable<IFeedItem> activities, int totalCount) GetLatestActivities(Enum activityType, int activityAmount)
        {
            var items = GetCentralFeedItems(activityType).ToList();
            var filteredItems = FilterLatestActivities(items).Take(activityAmount);
            var sortedItems = Sort(filteredItems, activityType);

            return (sortedItems, items.Count);
        }

        //private ActivityFeedTabViewModel GetTabForActivityType(Enum activitiesType)
        //{
        //    var result = _centralFeedContentService
        //       .GetTabs(Current.UmbracoContext.Content.GetById(_requestContext.Node.Id))
        //       .Single(el => Equals(el.Type, activitiesType))
        //       .Map<ActivityFeedTabViewModel>();

        //    return result;
        //}

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
            return new FeedItemViewModel
            {
                Activity = i,
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
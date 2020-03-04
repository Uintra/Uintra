using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.Links;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Logging;

namespace Uintra20.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IActivityLinkService _linkService;
        private readonly ILogger _logger;

        public LatestActivitiesPanelViewModelConverter(
            IFeedTypeProvider feedTypeProvider,
            ICentralFeedService centralFeedService,
            IIntranetLocalizationService intranetLocalizationService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IActivityLinkService linkService,
            ILogger logger)
        {
            _feedTypeProvider = feedTypeProvider;
            _centralFeedService = centralFeedService;
            _intranetLocalizationService = intranetLocalizationService;
            _intranetMemberService = intranetMemberService;
            _linkService = linkService;
            _logger = logger;
        }

        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {

            var centralFeedType = _feedTypeProvider[node.ActivityType.Value.Id];

            var allItems = GetCentralFeedItems(centralFeedType).ToList();
            var filteredItems = FilterLatestActivities(allItems).Take(node.CountToDisplay.Value);
            var sortedItems = Sort(filteredItems, centralFeedType).Select(Convert).ToList();

            viewModel.Feed = sortedItems;
            viewModel.ShowSeeAllButton = sortedItems.Count < allItems.Count;

        }


        public LatestActivitiesItemViewModel Convert(IFeedItem item)
        {
            if (item is IntranetActivity activity)
            {
                var latestActivityModel = new LatestActivitiesItemViewModel()
                {
                    Id = activity.Id,
                    Type = _intranetLocalizationService.Translate(activity.Type.ToString()),
                    Title = activity.Title,
                    Description = activity.Description,
                    Owner = _intranetMemberService.Get(item.OwnerId).ToViewModel(),
                    Links = _linkService.GetLinks(activity.Id),
                    Dates = item.PublishDate.ToDateTimeFormat().ToEnumerable()
                };
                return latestActivityModel;
            }

            _logger.Warn<LatestActivitiesPanelViewModelConverter>("Feed item is not IntranetActivity (id={0};type={1})", item.Id, item.Type.ToInt());
            return null;
        }

        private IEnumerable<IFeedItem> GetCentralFeedItems(Enum centralFeedType)
        {
            return centralFeedType is CentralFeedTypeEnum.All
                ? _centralFeedService.GetFeed().OrderByDescending(item => item.PublishDate)
                : _centralFeedService.GetFeed(centralFeedType);
        }

        private IEnumerable<IFeedItem> FilterLatestActivities(IEnumerable<IFeedItem> activities)
        {
            var settings = _centralFeedService.GetAllSettings()
                .Where(s => !s.ExcludeFromLatestActivities)
                .Select(s => s.Type);

            var items = activities.Join(settings, item => item.Type.ToInt(), type => type.ToInt(), (item, _) => item);

            return items;
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
    }
}
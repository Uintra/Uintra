using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using UBaseline.Core.Node;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Feed;
using Uintra.Core.Feed.Models;
using Uintra.Core.Localization;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.CentralFeed.Helpers;
using Uintra.Features.CentralFeed.Services;
using Uintra.Features.Links;
using Uintra.Features.UintraPanels.LastActivities.Models;
using Uintra.Infrastructure.Extensions;
using Umbraco.Core.Logging;

namespace Uintra.Features.UintraPanels.LastActivities.Converters
{
    public class LatestActivitiesPanelViewModelConverter : INodeViewModelConverter<LatestActivitiesPanelModel, LatestActivitiesPanelViewModel>
    {
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IIntranetLocalizationService _intranetLocalizationService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IActivityLinkService _linkService;
        private readonly ICentralFeedHelper _centralFeedHelper;
        private readonly ILogger _logger;

        public LatestActivitiesPanelViewModelConverter(
            IFeedTypeProvider feedTypeProvider,
            ICentralFeedService centralFeedService,
            IIntranetLocalizationService intranetLocalizationService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IActivityLinkService linkService,
            ICentralFeedHelper centralFeedHelper,
            ILogger logger)
        {
            _feedTypeProvider = feedTypeProvider;
            _centralFeedService = centralFeedService;
            _intranetLocalizationService = intranetLocalizationService;
            _intranetMemberService = intranetMemberService;
            _linkService = linkService;
            _centralFeedHelper = centralFeedHelper;
            _logger = logger;
        }

        public void Map(LatestActivitiesPanelModel node, LatestActivitiesPanelViewModel viewModel)
        {
            var centralFeedType = _feedTypeProvider[node.ActivityType.Value.Id];

            var allItems = _centralFeedHelper.GetCentralFeedItems(centralFeedType).ToList();
            var filteredItems = FilterLatestActivities(allItems).Take(node.CountToDisplay.Value);
            var sortedItems = _centralFeedHelper.Sort(filteredItems, centralFeedType).Select(Convert).ToList();

            viewModel.Feed = sortedItems;
            viewModel.ShowSeeAllButton = sortedItems.Count < allItems.Count;

        }

        public LatestActivitiesItemViewModel Convert(IFeedItem item)
        {
            if (item is IntranetActivity activity)
            {
                var latestActivityModel = new LatestActivitiesItemViewModel
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

        private IEnumerable<IFeedItem> FilterLatestActivities(IEnumerable<IFeedItem> activities)
        {
            var settings = _centralFeedService.GetAllSettings()
                .Where(s => !s.ExcludeFromLatestActivities)
                .Select(s => s.Type);

            var items = activities.Join(settings, item => item.Type.ToInt(), type => type.ToInt(), (item, _) => item);

            return items;
        }
    }
}
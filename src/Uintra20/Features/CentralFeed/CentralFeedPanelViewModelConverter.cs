using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.State;
using Uintra20.Core.Localization;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Models;

namespace Uintra20.Features.CentralFeed
{
	public class CentralFeedPanelViewModelConverter : INodeViewModelConverter<CentralFeedPanelModel, CentralFeedPanelViewModel>
	{
		private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
		//private readonly IPermissionsService _permissionsService;
		//private readonly IPermissionResourceTypeProvider _permissionResourceTypeProvider;
		private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IIntranetLocalizationService _localizationService;

		public CentralFeedPanelViewModelConverter(
			IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
			//IPermissionsService permissionsService,
			//IPermissionResourceTypeProvider permissionResourceTypeProvider,
			IFeedTypeProvider feedTypeProvider,
            IIntranetLocalizationService localizationService)
		{
			_feedFilterStateService = feedFilterStateService;
			//_permissionsService = permissionsService;
			//_permissionResourceTypeProvider = permissionResourceTypeProvider;
			_feedTypeProvider = feedTypeProvider;
            _localizationService = localizationService;

        }

		public void Map(CentralFeedPanelModel node, CentralFeedPanelViewModel viewModel)
		{
			var centralFeedState = _feedFilterStateService.GetFiltersState();

			viewModel.Type = _feedTypeProvider[node.TabType];
			viewModel.Tabs = GetActivityTabs();
			//viewModel.TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs).Where(tab => _permissionsService.Check(_permissionResourceTypeProvider[tab.Type.ToInt()], PermissionActionEnum.Create));
			viewModel.IsFiltersOpened = centralFeedState.IsFiltersOpened;
		}


		private IEnumerable<ActivityFeedTabViewModel> GetTabsWithCreateUrl(IEnumerable<ActivityFeedTabViewModel> tabs) =>
			tabs.Where(t => !IsTypeForAllActivities(t.Type) && t.Links.Create.HasValue());

		private bool IsTypeForAllActivities(Enum type) => type is CentralFeedTypeEnum.All;

		private List<ActivityFeedTabViewModel> GetActivityTabs()
		{
            var filter = new FeedFilterStateModel();

			return new List<ActivityFeedTabViewModel>
			{
				new ActivityFeedTabViewModel
				{
					IsActive = true,
					Type = CentralFeedTypeEnum.Social,
					Title = CentralFeedTypeEnum.Social.ToString()
				},
				new ActivityFeedTabViewModel
				{
					IsActive = false,
					Type = CentralFeedTypeEnum.News,
                    Title = CentralFeedTypeEnum.News.ToString(),
                    Filters = new []
                    {
                        new ActivityFeedTabFiltersViewModel(nameof(filter.ShowPinned), _localizationService.Translate("CentralFeedList.ShowPinned.chkbx"), false)
                    }
				},
				new ActivityFeedTabViewModel
				{
					IsActive = false,
					Type = CentralFeedTypeEnum.Events,
                    Title = CentralFeedTypeEnum.Events.ToString(),
                    Filters = new []
                    {
                        new ActivityFeedTabFiltersViewModel(nameof(filter.ShowPinned), _localizationService.Translate("CentralFeedList.ShowPinned.chkbx"), false),
                        new ActivityFeedTabFiltersViewModel(nameof(filter.ShowSubscribed), _localizationService.Translate("CentralFeedList.ShowSubscribed.chkbx"), false)
                    }
                }
			};
		}
	}
}
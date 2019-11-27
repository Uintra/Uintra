using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Node;
using Uintra20.Core.Feed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed
{
	public class CentralFeedPanelViewModelConverter : INodeViewModelConverter<CentralFeedPanelModel, CentralFeedPanelViewModel>
	{
		private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
		//private readonly IPermissionsService _permissionsService;
		//private readonly IPermissionResourceTypeProvider _permissionResourceTypeProvider;
		private readonly IFeedTypeProvider _feedTypeProvider;

		public CentralFeedPanelViewModelConverter(
			IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
			//IPermissionsService permissionsService,
			//IPermissionResourceTypeProvider permissionResourceTypeProvider,
			IFeedTypeProvider feedTypeProvider)
		{
			_feedFilterStateService = feedFilterStateService;
			//_permissionsService = permissionsService;
			//_permissionResourceTypeProvider = permissionResourceTypeProvider;
			_feedTypeProvider = feedTypeProvider;
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
			return new List<ActivityFeedTabViewModel>
			{
				new ActivityFeedTabViewModel
				{
					IsActive = true,
					Type = CentralFeedTypeEnum.Bulletins,
				},
				new ActivityFeedTabViewModel
				{
					IsActive = false,
					Type = CentralFeedTypeEnum.News,
				},
				new ActivityFeedTabViewModel
				{
					IsActive = false,
					Type = CentralFeedTypeEnum.Events,
				}
			};
		}
	}
}
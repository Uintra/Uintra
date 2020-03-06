using Compent.Shared.Extensions.Bcl;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Builders;
using Uintra20.Features.CentralFeed.Commands;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Helpers;

namespace Uintra20.Features.CentralFeed
{
    public class CentralFeedPanelViewModelConverter :
        INodeViewModelConverter<CentralFeedPanelModel, CentralFeedPanelViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IActivityTabsBuilder _activityTabsBuilder;

        public CentralFeedPanelViewModelConverter(
            IPermissionsService permissionsService,
            IFeedTypeProvider feedTypeProvider,
            IActivityTabsBuilder activityTabsBuilder)
        {
            _permissionsService = permissionsService;
            _feedTypeProvider = feedTypeProvider;
            _activityTabsBuilder = activityTabsBuilder;
        }

        public void Map(CentralFeedPanelModel node, CentralFeedPanelViewModel viewModel)
        {
            var command = GetPermissionCommand();
            viewModel.Type = _feedTypeProvider[node.TabType];
            viewModel.Tabs = _activityTabsBuilder.Build(command);
            //viewModel.TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs).Where(tab => _permissionsService.Check(_permissionResourceTypeProvider[tab.Type.ToInt()], PermissionActionEnum.Create));
            viewModel.ItemsPerRequest = node.ItemsPerRequest.Value == 0 ? 10 : node.ItemsPerRequest.Value;
            viewModel.GroupId = HttpContext.Current.Request.GetRequestQueryValue("groupId").TryParseGuid();
        }

        private IEnumerable<ActivityFeedTabViewModel> GetTabsWithCreateUrl(IEnumerable<ActivityFeedTabViewModel> tabs) =>
            tabs.Where(t => !(t.Type is CentralFeedTypeEnum.All) && t.Links.Create.OriginalUrl.HasValue());

        private CentralFeedFilterCommand GetPermissionCommand()
        {
            var activities = EnumHelper.ToEnumerable<PermissionResourceTypeEnum>();

            var list = new List<CentralFeedFilterPermissionModel>();

            foreach (var activity in activities)
            {
                var canView = _permissionsService.Check(activity, PermissionActionEnum.View);
                
                list.Add(new CentralFeedFilterPermissionModel(canView, activity));
            }

            return new CentralFeedFilterCommand(list);
        }
    }
}
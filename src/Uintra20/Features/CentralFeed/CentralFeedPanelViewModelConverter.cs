using Compent.Extensions;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
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
        private readonly IActivityTabsBuilder _activityTabsBuilder;
        private readonly IUBaselineRequestContext _context;

        public CentralFeedPanelViewModelConverter(
            IPermissionsService permissionsService,
            IActivityTabsBuilder activityTabsBuilder,
            IUBaselineRequestContext context)
        {
            _permissionsService = permissionsService;
            _activityTabsBuilder = activityTabsBuilder;
            _context = context;
        }

        public void Map(CentralFeedPanelModel node, CentralFeedPanelViewModel viewModel)
        {
            var command = GetPermissionCommand();
            viewModel.Tabs = _activityTabsBuilder.Build(command);
            viewModel.ItemsPerRequest = node.ItemsPerRequest.Value == 0 ? 10 : node.ItemsPerRequest.Value;
            viewModel.GroupId = _context.ParseQueryString("groupId").TryParseGuid();
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
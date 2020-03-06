using Compent.Shared.Extensions.Bcl;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Builders;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.CentralFeed
{
    public class CentralFeedPanelViewModelConverter :
        INodeViewModelConverter<CentralFeedPanelModel, CentralFeedPanelViewModel>
    {
        //private readonly IPermissionsService _permissionsService;
        //private readonly IPermissionResourceTypeProvider _permissionResourceTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IActivityTabsBuilder _activityTabsBuilder;

        public CentralFeedPanelViewModelConverter(
            //IPermissionsService permissionsService,
            //IPermissionResourceTypeProvider permissionResourceTypeProvider,
            IFeedTypeProvider feedTypeProvider,
            IActivityTabsBuilder activityTabsBuilder)
        {
            //_permissionsService = permissionsService;
            //_permissionResourceTypeProvider = permissionResourceTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _activityTabsBuilder = activityTabsBuilder;
        }

        public void Map(CentralFeedPanelModel node, CentralFeedPanelViewModel viewModel)
        {
            viewModel.Type = _feedTypeProvider[node.TabType];

            viewModel.Tabs = _activityTabsBuilder
                .BuildSocialTab()
                .BuildEventsTab()
                .BuildNewsTab()
                .Build();

            //viewModel.TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs).Where(tab => _permissionsService.Check(_permissionResourceTypeProvider[tab.Type.ToInt()], PermissionActionEnum.Create));
            viewModel.ItemsPerRequest = node.ItemsPerRequest.Value == 0 ? 10 : node.ItemsPerRequest.Value;
            viewModel.GroupId = HttpContext.Current.Request.GetRequestQueryValue("groupId").TryParseGuid();
        }

        private IEnumerable<ActivityFeedTabViewModel> GetTabsWithCreateUrl(IEnumerable<ActivityFeedTabViewModel> tabs) =>
            tabs.Where(t => !(t.Type is CentralFeedTypeEnum.All) && t.Links.Create.OriginalUrl.HasValue());
    }
}
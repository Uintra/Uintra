using Compent.Extensions;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity;
using Uintra.Features.Links;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Social.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.HomePage
{
    public class HomePageViewModelConverter : INodeViewModelConverter<HomePageModel, HomePageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INodeModelService _nodeModelService;
        private readonly IUBaselineRequestContext _context;

        public HomePageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INodeModelService nodeModelService,
            IUBaselineRequestContext context)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _nodeModelService = nodeModelService;
            _context = context;
        }

        public void Map(HomePageModel node, HomePageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            viewModel.CreateEventsLink =
                _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create)
                    ? _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create
                    : null;
            viewModel.CreateNewsLink =
                _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create)
                    ? _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create
                    : null;

            if (node.UserListPage.Value.HasValue)
            {
                var userListPage = _nodeModelService.Get(node.UserListPage.Value.Value);
                viewModel.UserListPage = userListPage.Url.ToLinkModel();
            }


            if (groupId.HasValue)
            {
                viewModel.CreateNewsLink = viewModel.CreateNewsLink?.AddGroupId(groupId.Value);
                viewModel.CreateEventsLink = viewModel.CreateEventsLink?.AddGroupId(groupId.Value);
            }

            var socialCreateModel = _nodeModelService.AsEnumerable().OfType<SocialCreatePageModel>().First();
            viewModel.SocialCreateModel = _nodeModelService.GetViewModel<SocialCreatePageViewModel>(socialCreateModel);
        }
    }
}
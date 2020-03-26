using Compent.Extensions;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Features.Links;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.HomePage
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

            var userListPage = _nodeModelService.Get(node.UserListPage.Value);
            viewModel.UserListPage = userListPage.Url.ToLinkModel();

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
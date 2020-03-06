using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
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

        public HomePageViewModelConverter(
            IPermissionsService permissionsService, 
            IFeedLinkService feedLinkService,
            INodeModelService nodeModelService)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _nodeModelService = nodeModelService;
        }

        public void Map(HomePageModel node, HomePageViewModel viewModel)
        {
            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            Guid? groupId = null;
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
            {
                groupId = parsedGroupId;
            }

            //TODO: Uncomment when events create will be done
            //viewModel.CreateEventsLink = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create) ? 
            //    _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create
            //    : null;
            viewModel.CreateNewsLink = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create
                : null;

            if (groupId.HasValue)
            {
                viewModel.CreateNewsLink = viewModel.CreateNewsLink?.AddGroupId(groupId.Value);
                //viewModel.CreateEventsLink = viewModel.CreateEventsLink?.AddGroupId(groupId);
            }

            var socialCreateModel = _nodeModelService.AsEnumerable().OfType<SocialCreatePageModel>().First();
            viewModel.SocialCreateModel = _nodeModelService.GetViewModel<SocialCreatePageViewModel>(socialCreateModel);
        }
    }
}
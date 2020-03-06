using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter : INodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INodeModelService _nodeModelService;
        private readonly IGroupHelper _groupHelper;

        public UintraGroupsRoomPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INodeModelService nodeModelService,
            IGroupHelper groupHelper)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _nodeModelService = nodeModelService;
            _groupHelper = groupHelper;
        }

        public void Map(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.GroupId = id;
            viewModel.GroupInfo = _groupHelper.GetGroupViewModel(id);
            viewModel.GroupHeader = _groupHelper.GetHeader(id);

            //TODO: Uncomment when events create will be done
            //viewModel.CreateEventsLink = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create) ? 
            //    _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create?.AddGroupId(id)
            //    : null;
            viewModel.CreateNewsLink = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create.AddGroupId(id)
                : null;

            var socialCreateModel = _nodeModelService.AsEnumerable().OfType<SocialCreatePageModel>().First();
            viewModel.SocialCreateModel = _nodeModelService.GetViewModel<SocialCreatePageViewModel>(socialCreateModel);
        }
    }
}
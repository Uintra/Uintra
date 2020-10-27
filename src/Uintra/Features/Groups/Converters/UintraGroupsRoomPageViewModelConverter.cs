using Compent.Extensions;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Models;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Social.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INodeModelService _nodeModelService;
        private readonly IGroupHelper _groupHelper;
        private readonly IGroupService _groupService;
        private readonly IUBaselineRequestContext _context;

        public UintraGroupsRoomPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            INodeModelService nodeModelService,
            IGroupHelper groupHelper,
            IGroupService groupService,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context)
            : base(errorLinksService)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _nodeModelService = nodeModelService;
            _groupHelper = groupHelper;
            _groupService = groupService;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!groupId.HasValue) return NotFoundResult();

            var group = _groupService.Get(groupId.Value);

            if (group == null || group.IsHidden) return NotFoundResult();

            viewModel.GroupId = groupId.Value;
            viewModel.GroupInfo = _groupHelper.GetGroupViewModel(groupId.Value);
            viewModel.GroupHeader = _groupHelper.GetHeader(groupId.Value);

            viewModel.CreateEventsLink = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create?.AddGroupId(groupId.Value)
                : null;
            viewModel.CreateNewsLink = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create.AddGroupId(groupId.Value)
                : null;

            var socialCreateModel = _nodeModelService.AsEnumerable().OfType<SocialCreatePageModel>().First();
            viewModel.SocialCreateModel = _nodeModelService.GetViewModel<SocialCreatePageViewModel>(socialCreateModel);

            return OkResult();
        }
    }
}
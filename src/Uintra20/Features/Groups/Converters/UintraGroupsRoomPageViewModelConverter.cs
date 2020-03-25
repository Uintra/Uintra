using Compent.Extensions;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
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

            if (group == null) return NotFoundResult();

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
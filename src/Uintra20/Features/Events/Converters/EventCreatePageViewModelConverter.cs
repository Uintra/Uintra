using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Events.Models;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Events.Converters
{
    public class EventCreatePageViewModelConverter : UintraRestrictedNodeViewModelConverter<EventCreatePageModel, EventCreatePageViewModel>
    {
        private const PermissionResourceTypeEnum PermissionType = PermissionResourceTypeEnum.Events;
        private const IntranetActivityTypeEnum ActivityType = IntranetActivityTypeEnum.Events;

        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;

        public EventCreatePageViewModelConverter(
            IIntranetMemberService<IntranetMember> memberService,
            IPermissionsService permissionsService,
            IUserTagProvider tagProvider,
            IFeedLinkService feedLinkService,
            IGroupMemberService groupMemberService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context) 
            : base(errorLinksService)
        {
            _memberService = memberService;
            _permissionsService = permissionsService;
            _tagProvider = tagProvider;
            _feedLinkService = feedLinkService;
            _groupMemberService = groupMemberService;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(EventCreatePageModel node, EventCreatePageViewModel viewModel)
        {
            var groupId = _context.ParseQueryString("groupId").TryParseGuid();

            if (!HasPermission(groupId))
            {
                return ForbiddenResult();
            }

            viewModel.Data = GetData(groupId);
            viewModel.GroupHeader = groupId.HasValue ? _groupHelper.GetHeader(groupId.Value) : null;

            return OkResult();
        }

        private EventCreateDataViewModel GetData(Guid? groupId)
        {
            var model = new EventCreateDataViewModel();

            var currentMember = _memberService.GetCurrentMember();

            model.CanEditOwner = _permissionsService.Check(PermissionType, PermissionActionEnum.EditOwner);
            model.PinAllowed = _permissionsService.Check(PermissionType, PermissionActionEnum.CanPin);

            if (model.CanEditOwner)
                model.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionType));

            model.Links = model.GroupId.HasValue ?
                _feedLinkService.GetCreateLinks(ActivityType, model.GroupId.Value)
                : _feedLinkService.GetCreateLinks(ActivityType);

            //TODO: Uncomment when eventService is ready
            //var mediaSettings = "";_eventsService.GetMediaSettings();

            model.AllowedMediaExtensions = "";//mediaSettings.AllowedMediaExtensions;
            model.Tags = _tagProvider.GetAll();
            model.Creator = currentMember.ToViewModel();
            model.GroupId = groupId;

            var now = DateTime.UtcNow;

            model.PublishDate = now;
            model.StartDate = now;
            model.EndDate = now.AddHours(8);

            return model;
        }
        
        private bool HasPermission(Guid? groupId)
        {
            var hasPermission = _permissionsService.Check(PermissionType, PermissionActionEnum.Create);

            if (groupId.HasValue)
            {
                hasPermission = hasPermission &&
                                _groupMemberService.IsGroupMember(groupId.Value, _memberService.GetCurrentMemberId());
            }

            return hasPermission;
        }

        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Events.Entities;
using Uintra.Features.Events.Models;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Strategies.Preset;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Permissions.Models;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Events.Converters
{
    public class EventEditPageViewModelConverter : UintraRestrictedNodeViewModelConverter<EventEditPageModel, EventEditPageViewModel>
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IEventsService<Event> _eventService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        private readonly IGroupService _groupService;

        public EventEditPageViewModelConverter(
            IPermissionsService permissionsService,
            IFeedLinkService feedLinkService,
            IEventsService<Event> eventService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            IUserTagProvider userTagProvider,
            ILightboxHelper lightboxHelper,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context,
            IGroupService groupService)
            : base(errorLinksService)
        {
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _eventService = eventService;
            _memberService = memberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
            _lightboxHelper = lightboxHelper;
            _groupHelper = groupHelper;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(EventEditPageModel node, EventEditPageViewModel viewModel)
        {
            var id = _context.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var @event = _eventService.Get(id.Value);

            if (@event == null || @event.IsHidden)
            {
                return NotFoundResult();
            }

            if (@event.GroupId.HasValue)
            {
                var group = _groupService.Get(@event.GroupId.Value);
                if (group != null && group.IsHidden)
                    return NotFoundResult();
            }

            if (!_eventService.CanEdit(id.Value)) return ForbiddenResult();

            viewModel.Details = GetDetails(@event);
            viewModel.Links = _feedLinkService.GetLinks(@event.Id);
            viewModel.CanEditOwner = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.EditOwner);
            viewModel.AllowedMediaExtensions = _eventService.GetMediaSettings().AllowedMediaExtensions;
            viewModel.PinAllowed = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.CanPin);

            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionResourceTypeEnum.Events));

            var requestGroupId = HttpContext.Current.Request["groupId"];

            if (Guid.TryParse(requestGroupId, out var groupId) && @event.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }

        //TODO Refactor this code. Method is duplicated in ActivityCreatePanelConverter
        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();

        private EventViewModel GetDetails(Event @event)
        {
            var details = @event.Map<EventViewModel>();

            details.Media = MediaHelper.GetMediaUrls(@event.MediaIds);
            details.CanEdit = _eventService.CanEdit(@event);
            details.Links = _feedLinkService.GetLinks(@event.Id);
            details.IsReadOnly = false;
            details.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = @event.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(@event).ToViewModel();
            details.Tags = _userTagService.Get(@event.Id);
            details.AvailableTags = _userTagProvider.GetAll();
            details.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(@event.MediaIds, PresetStrategies.ForActivityDetails);
            details.CanSubscribe = _eventService.CanSubscribe(@event.Id);
            
            var subscribe = @event.Subscribers.FirstOrDefault(x => x.UserId == _memberService.GetCurrentMemberId());

            details.IsSubscribed = subscribe != null;
            details.IsNotificationsDisabled = subscribe?.IsNotificationDisabled ?? false;

            return details;
        }
    }
}
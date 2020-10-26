using System;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.Extensions;
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
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Events.Converters
{
    public class EventDetailsPageViewModelConverter : UintraRestrictedNodeViewModelConverter<EventDetailsPageModel,
        EventDetailsPageViewModel>
    {
        private readonly IUserTagService _userTagService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IEventsService<Event> _eventsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightBoxHelper;
        private readonly IPermissionsService _permissionsService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _baselineRequestContext;
        private readonly IGroupService _groupService;

        public EventDetailsPageViewModelConverter(
            IUserTagService userTagService,
            IFeedLinkService feedLinkService,
            IEventsService<Event> eventsService,
            IIntranetMemberService<IntranetMember> memberService,
            ILightboxHelper lightBoxHelper,
            IPermissionsService permissionsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext baselineRequestContext,
            IGroupService groupService)
            : base(errorLinksService)
        {
            _userTagService = userTagService;
            _feedLinkService = feedLinkService;
            _eventsService = eventsService;
            _memberService = memberService;
            _lightBoxHelper = lightBoxHelper;
            _permissionsService = permissionsService;
            _groupHelper = groupHelper;
            _baselineRequestContext = baselineRequestContext;
            _groupService = groupService;
        }

        public override ConverterResponseModel MapViewModel(EventDetailsPageModel node,
            EventDetailsPageViewModel viewModel)
        {
            var idStr = HttpUtility.ParseQueryString(_baselineRequestContext.NodeRequestParams.NodeUrl.Query)
                .TryGetQueryValue<string>("id");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();

            var @event = _eventsService.Get(id);

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

            if (!_permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.View))
            {
                return ForbiddenResult();
            }

            var member = _memberService.GetCurrentMember();

            viewModel.Details = GetDetails(@event);
            viewModel.Tags = _userTagService.Get(id);
            viewModel.CanEdit = _eventsService.CanEdit(id);
            viewModel.IsGroupMember = !@event.GroupId.HasValue || member.GroupIds.Contains(@event.GroupId.Value);

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var groupId) && @event.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }

        private EventViewModel GetDetails(Event @event)
        {
            var details = @event.Map<EventViewModel>();

            details.Media = MediaHelper.GetMediaUrls(@event.MediaIds);

            details.LightboxPreviewModel =
                _lightBoxHelper.GetGalleryPreviewModel(@event.MediaIds, PresetStrategies.ForActivityDetails);
            details.CanEdit = _eventsService.CanEdit(@event);
            details.Links = _feedLinkService.GetLinks(@event.Id);
            details.IsReadOnly = false;
            details.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = @event.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(@event).ToViewModel();
            details.HeaderInfo.Links = _feedLinkService.GetLinks(@event.Id);
            details.CanSubscribe = _eventsService.CanSubscribe(@event.Id);

            var subscribe = @event.Subscribers.FirstOrDefault(x => x.UserId == _memberService.GetCurrentMemberId());

            details.IsSubscribed = subscribe != null;
            details.IsNotificationsDisabled = subscribe?.IsNotificationDisabled ?? true;

            return details;
        }
    }
}
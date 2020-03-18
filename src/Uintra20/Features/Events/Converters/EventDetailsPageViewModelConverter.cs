using System;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Events.Models;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Events.Converters
{
    public class EventDetailsPageViewModelConverter : UintraRestrictedNodeViewModelConverter<EventDetailsPageModel, EventDetailsPageViewModel>
    {
        private readonly IUserTagService _userTagService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IEventsService<Event> _eventsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightBoxHelper;
        private readonly IPermissionsService _permissionsService;
        private readonly IGroupHelper _groupHelper;

        public EventDetailsPageViewModelConverter(
            IUserTagService userTagService,
            IFeedLinkService feedLinkService,
            IEventsService<Event> eventsService,
            IIntranetMemberService<IntranetMember> memberService,
            ILightboxHelper lightBoxHelper,
            IPermissionsService permissionsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService)
            : base(errorLinksService)
        {
            _userTagService = userTagService;
            _feedLinkService = feedLinkService;
            _eventsService = eventsService;
            _memberService = memberService;
            _lightBoxHelper = lightBoxHelper;
            _permissionsService = permissionsService;
            _groupHelper = groupHelper;
        }

        public override ConverterResponseModel MapViewModel(EventDetailsPageModel node, EventDetailsPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(idStr, out var id))
                return NotFoundResult();

            var @event = _eventsService.Get(id);

            if (@event == null)
            {
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

            details.LightboxPreviewModel = _lightBoxHelper.GetGalleryPreviewModel(@event.MediaIds, PresetStrategies.ForActivityDetails);
            details.CanEdit = _eventsService.CanEdit(@event);
            details.Links = _feedLinkService.GetLinks(@event.Id);
            details.IsReadOnly = false;
            details.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = @event.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberService.Get(@event).ToViewModel();
            details.HeaderInfo.Links = _feedLinkService.GetLinks(@event.Id);

            var currentUserId = _memberService.GetCurrentMemberId();
            var subscribe = @event.Subscribers.FirstOrDefault(x => x.UserId == currentUserId);

            details.IsSubscribed = subscribe != null;
            details.IsNotificationsDisabled = subscribe?.IsNotificationDisabled ?? false;

            return details;
        }
    }
}
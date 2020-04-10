using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Events.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Reminder.Services;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Events.Controllers
{
    [ValidateModel]
    public class EventsController : UBaselineApiController
    {
        private const PermissionResourceTypeEnum ActivityType = PermissionResourceTypeEnum.Events;

        private readonly IEventsService<Event> _eventsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IReminderService _reminderService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IPermissionsService _permissionsService;

        public EventsController(
            IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IReminderService reminderService,
            IGroupActivityService groupActivityService,
            IActivityLinkService activityLinkService,
            IActivityTagsHelper activityTagsHelper,
            IMentionService mentionService,
            IPermissionsService permissionsService)
        {
            _eventsService = eventsService;
            _intranetMemberService = intranetMemberService;
            _reminderService = reminderService;
            _groupActivityService = groupActivityService;
            _activityLinkService = activityLinkService;
            _activityTagsHelper = activityTagsHelper;
            _mentionService = mentionService;
            _mediaHelper = mediaHelper;
            _permissionsService = permissionsService;
        }

        [HttpPost]
        public IHttpActionResult Create(EventCreateModel createModel)
        {
            var @event = MapToEvent(createModel);
            var activityId = _eventsService.Create(@event);

            _reminderService.CreateIfNotExists(activityId, ReminderTypeEnum.OneDayBefore);

            @event = _eventsService.Get(activityId);

            if (createModel.GroupId.HasValue)
            {
                _groupActivityService.AddRelation(createModel.GroupId.Value, activityId);
                @event.GroupId = createModel.GroupId;
            }

            _activityTagsHelper.ReplaceTags(activityId, createModel.TagIdsData);

            ResolveMentions(createModel.Description, @event);

            var redirectUrl = _activityLinkService.GetLinks(activityId).Details;

            return Ok(redirectUrl);
        }

        [HttpPut]
        public IHttpActionResult Edit(EventEditModel editModel)
        {
            var cachedActivityMedias = _eventsService.Get(editModel.Id).MediaIds;

            var activity = MapToEvent(editModel);
            _eventsService.Save(activity);

            _mediaHelper.DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            if (!_eventsService.IsActual(activity)) return BadRequest("Event is not actual'");

            if (editModel.NotifyAllSubscribers)
            {
                var notificationType = NotificationTypeEnum.EventUpdated;
                ((INotifyableService)_eventsService).Notify(activity.Id, notificationType);
            }

            _activityTagsHelper.ReplaceTags(activity.Id, editModel.TagIdsData);

            _reminderService.CreateIfNotExists(activity.Id, ReminderTypeEnum.OneDayBefore);

            ResolveMentions(editModel.Description, activity);

            var redirectUrl = _activityLinkService.GetLinks(activity.Id).Details;

            return Ok(redirectUrl);
        }

        [HttpPost]
        public virtual void Hide(Guid id, bool isNotificationNeeded)
        {
            if (_eventsService.CanHide(id))
            {
                _eventsService.Hide(id);

                if (isNotificationNeeded)
                {
                    ((INotifyableService)_eventsService).Notify(id, NotificationTypeEnum.EventHidden);
                }
            }
        }

        private Event MapToEvent(EventCreateModel createModel)
        {
            var @event = createModel.Map<Event>();

            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(createModel, MediaFolderTypeEnum.NewsContent));
            @event.StartDate = createModel.StartDate.ToUniversalTime();
            @event.PublishDate = createModel.PublishDate.ToUniversalTime();
            @event.EndDate = createModel.EndDate.ToUniversalTime();
            @event.EndPinDate = createModel.EndPinDate?.ToUniversalTime();
            @event.CreatorId = _intranetMemberService.GetCurrentMemberId();

            if (!_permissionsService.Check(ActivityType, PermissionActionEnum.CanPin))
            {
                @event.EndPinDate = null;
                @event.IsPinned = false;

            }

            return @event;
        }

        private Event MapToEvent(EventEditModel editModel)
        {
            var @event = _eventsService.Get(editModel.Id);
            @event = Mapper.Map(editModel, @event);

            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(editModel, MediaFolderTypeEnum.EventsContent));
            @event.StartDate = editModel.StartDate.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.StartDate);
            @event.PublishDate = editModel.PublishDate.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.PublishDate);
            @event.EndDate = editModel.EndDate.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.EndDate);
            @event.EndPinDate = editModel.EndPinDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.EndPinDate.Value);

            if (!_permissionsService.Check(ActivityType, PermissionActionEnum.CanPin))
            {
                @event.EndPinDate = null;
                @event.IsPinned = false;

            }

            @event.SubscribeNotes = editModel.SubscribeNotes;

            // not allow change CanSubscribe, if someone subscribes while event was editing
            if (_eventsService.CanEditSubscribe(@event.Id))
            {
                @event.CanSubscribe = editModel.CanSubscribe;
            }

            return @event;
        }

        private void ResolveMentions(string text, Event @event)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var links = _activityLinkService.GetLinks(@event.Id);
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = @event.Id,
                    CreatorId = _intranetMemberService.GetCurrentMemberId(),
                    MentionedUserIds = mentionIds,
                    Title = @event.Title.StripMentionHtml(),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Events
                });

            }
        }
    }
}
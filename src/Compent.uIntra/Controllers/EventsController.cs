using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Uintra.Core.Activity.Models;
using Compent.Uintra.Core.Events;
using Compent.Uintra.Core.Feed;
using Compent.Uintra.Core.UserTags;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Grid;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.Events.Web;
using Uintra.Groups;
using Uintra.Groups.Extentions;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Uintra.Search;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Controllers
{
    public class EventsController : EventsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Events/DetailsView.cshtml";
        protected override string CreateViewPath => "~/Views/Events/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/Events/EditView.cshtml";
        protected override string ItemViewPath => "~/Views/Events/ItemView.cshtml";
        private string DetailsHeaderViewPath => "~/Views/Events/DetailsHeader.cshtml";      

        private readonly IEventsService<Event> _eventsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IReminderService _reminderService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IGroupMemberService _groupMemberService;

        public EventsController(
            IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IReminderService reminderService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IGridHelper gridHelper,
            IActivityTypeProvider activityTypeProvider,
            IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService,
            IActivityLinkService activityLinkService,
            UserTagService userTagService,
            IActivityTagsHelper activityTagsHelper,
            IGroupMemberService groupMemberService)
            : base(eventsService, mediaHelper, intranetUserService, activityTypeProvider, activityLinkService)
        {
            _eventsService = eventsService;
            _intranetUserService = intranetUserService;
            _reminderService = reminderService;
            _documentIndexer = documentIndexer;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _groupMemberService = groupMemberService;
        }

        public virtual ActionResult DetailsHeader(IntranetActivityDetailsHeaderViewModel header)
        {
            return PartialView(DetailsHeaderViewPath, header);
        }

        [HttpPost]
        public ActionResult CreateExtended(EventExtendedCreateModel createModel) => Create(createModel);

        [HttpPost]
        public ActionResult EditExtended(EventExtendedEditModel editModel) => Edit(editModel);

        protected override IEnumerable<EventBase> GetComingEvents(DateTime endDate)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var events = _eventsService.GetComingEvents(endDate);

            bool IsNotGroupEventOrUserInGroup(Event @event) =>
                !@event.GroupId.HasValue ||
                _groupMemberService.IsGroupMember(@event.GroupId.Value, currentUser.Id);

            return events.Where(IsNotGroupEventOrUserInGroup);
        }

        public ActionResult FeedItem(Event item, ActivityFeedOptionsWithGroups options)
        {
            EventExtendedItemModel extendedModel = GetItemViewModel(item, options);
            return PartialView(ItemViewPath, extendedModel);
        }

        public override JsonResult HasConfirmation(Guid id)
        {
            var @event = _eventsService.Get(id);
            return Json(new { HasConfirmation = @event.Subscribers.Any() }, JsonRequestBehavior.AllowGet);
        }

        protected override EventEditModel GetEditViewModel(EventBase @event, ActivityLinks links)
        {
            var eventExtended = (Event)@event;
            var model = base.GetEditViewModel(@event, links).Map<EventExtendedEditModel>();

            model.CanSubscribe = eventExtended.CanSubscribe;
            model.SubscribeNotes = eventExtended.SubscribeNotes;
            model.CanEditSubscribe = _eventsService.CanEditSubscribe(@event.Id);

            return model;
        }

        protected override EventCreateModel GetCreateModel(IActivityCreateLinks links)
        {
            var extendedCreateModel = base.GetCreateModel(links).Map<EventExtendedCreateModel>();
            extendedCreateModel.CanSubscribe = true;
            extendedCreateModel.CanEditSubscribe = true;

            return extendedCreateModel;
        }

        private EventExtendedItemModel GetItemViewModel(Event item, ActivityFeedOptionsWithGroups options)
        {
            var model = GetItemViewModel(item, options.Links);
            var extendedModel = model.Map<EventExtendedItemModel>();

            extendedModel.HeaderInfo = model.HeaderInfo.Map<ExtendedItemHeaderViewModel>();
            extendedModel.HeaderInfo.GroupInfo = options.GroupInfo;

            var userId = _intranetUserService.GetCurrentUser();
            extendedModel.LikesInfo = item;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            extendedModel.IsSubscribed = item.Subscribers.Any(s => s.UserId == userId.Id);

            return extendedModel;
        }

        public ActionResult PreviewItem(Event item, ActivityLinks links)
        {
            EventPreviewViewModel viewModel = GetPreviewViewModel(item, links);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override void DeleteMedia(IEnumerable<int> mediaIds)
        {
            base.DeleteMedia(mediaIds);
            _documentIndexer.DeleteFromIndex(mediaIds);
        }

        protected override void OnEventCreated(Guid activityId, EventCreateModel model)
        {

            _reminderService.CreateIfNotExists(activityId, ReminderTypeEnum.OneDayBefore);

            var groupId = Request.QueryString.GetGroupId();
            if (groupId.HasValue)
            {
                _groupActivityService.AddRelation(groupId.Value, activityId);
                var @event = _eventsService.Get(activityId);
                @event.GroupId = groupId;
            }
            if (model is EventExtendedCreateModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(activityId, extendedModel.TagIdsData);
            }
        }

        protected override void OnEventEdited(EventBase @event, EventEditModel model)
        {
            if (!_eventsService.IsActual(@event)) return;

            if (model.NotifyAllSubscribers)
            {
                var notificationType = NotificationTypeEnum.EventUpdated;
                ((INotifyableService)_eventsService).Notify(@event.Id, notificationType);
            }

            if (model is EventExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(@event.Id, extendedModel.TagIdsData);
            }

            _reminderService.CreateIfNotExists(@event.Id, ReminderTypeEnum.OneDayBefore);
        }

        protected override void OnEventHidden(Guid id, bool isNotificationNeeded)
        {
            if (isNotificationNeeded)
            {
                ((INotifyableService)_eventsService).Notify(id, NotificationTypeEnum.EventHided);
            }
        }

        protected override EventBase MapToEvent(EventCreateModel createModel)
        {
            var @event = (Event)base.MapToEvent(createModel);
            var extendedCreateModel = (EventExtendedCreateModel)createModel;

            @event.CanSubscribe = extendedCreateModel.CanSubscribe;
            @event.SubscribeNotes = extendedCreateModel.SubscribeNotes;

            return @event;
        }

        protected override EventBase MapToEvent(EventEditModel editModel)
        {
            var @event = (Event)base.MapToEvent(editModel);
            var extendedEditModel = (EventExtendedEditModel)editModel;

            @event.SubscribeNotes = extendedEditModel.SubscribeNotes;

            // not allow change CanSubscribe, if someone subscribes while event was editing
            if (_eventsService.CanEditSubscribe(@event.Id))
            {
                @event.CanSubscribe = extendedEditModel.CanSubscribe;
            }

            return @event;
        }
    }
}
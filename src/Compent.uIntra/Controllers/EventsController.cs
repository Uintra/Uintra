using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Activity.Models;
using Compent.uIntra.Core.Events;
using Compent.uIntra.Core.Feed;
using Compent.uIntra.Core.UserTags;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Web;
using uIntra.Groups;
using uIntra.Groups.Extensions;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Controllers
{
    public class EventsController : EventsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Events/DetailsView.cshtml";
        protected override string CreateViewPath => "~/Views/Events/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/Events/EditView.cshtml";
        protected override string ItemViewPath => "~/Views/Events/ItemView.cshtml";

        private readonly IEventsService<Event> _eventsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IReminderService _reminderService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;

        public EventsController(
            IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IReminderService reminderService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IGridHelper gridHelper,
            IActivityTypeProvider activityTypeProvider,
            IDocumentIndexer documentIndexer,
            INotificationTypeProvider notificationTypeProvider,
            IGroupActivityService groupActivityService,
            IActivityLinkService activityLinkService,
            UserTagService userTagService,
            IActivityTagsHelper activityTagsHelper)
            : base(eventsService, mediaHelper, intranetUserService, activityTypeProvider, activityLinkService)
        {
            _eventsService = eventsService;
            _intranetUserService = intranetUserService;
            _reminderService = reminderService;
            _documentIndexer = documentIndexer;
            _notificationTypeProvider = notificationTypeProvider;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
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

        [HttpPost]
        public ActionResult EditExtended(EventExtendedEditModel editModel)
        {
            return Edit(editModel);
        }

        protected override EventEditModel GetEditViewModel(EventBase @event, ActivityLinks links)
        {
            var baseModel = base.GetEditViewModel(@event, links);
            var extendedModel = baseModel.Map<EventExtendedEditModel>();
            return extendedModel;
        }

        protected override EventCreateModel GetCreateModel(IActivityCreateLinks links)
        {
            var baseModel = base.GetCreateModel(links);
            var extendedModel = baseModel.Map<EventExtendedCreateModel>();
            return extendedModel;
        }

        [HttpPost]
        public ActionResult CreateExtended(EventExtendedCreateModel createModel)
        {
            return Create(createModel);
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
            if (!_eventsService.IsActual(@event))
            {
                return;
            }

            if (model.NotifyAllSubscribers)
            {
                var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.EventUpdated.ToInt());
                ((INotifyableService) _eventsService).Notify(@event.Id, notificationType);
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
                var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.EventHided.ToInt());
                ((INotifyableService)_eventsService).Notify(id, notificationType);
            }
        }
    }
}
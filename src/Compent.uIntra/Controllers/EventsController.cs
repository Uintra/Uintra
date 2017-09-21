using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uIntra.Core.Activity.Models;
using Compent.uIntra.Core.Events;
using Compent.uIntra.Core.Extentions;
using Compent.uIntra.Core.Feed;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Web;
using uIntra.Groups;
using uIntra.Groups.Extentions;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Search;

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

        public EventsController(
            IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IReminderService reminderService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IGridHelper gridHelper,
            IActivityTypeProvider activityTypeProvider,
            IDocumentIndexer documentIndexer,
            INotificationTypeProvider notificationTypeProvider,
            IGroupActivityService groupActivityService,
            IActivityLinkService activityLinkService)
            : base(eventsService, mediaHelper, intranetUserService, activityTypeProvider, activityLinkService)
        {
            _eventsService = eventsService;
            _intranetUserService = intranetUserService;
            _reminderService = reminderService;
            _documentIndexer = documentIndexer;
            _notificationTypeProvider = notificationTypeProvider;
            _groupActivityService = groupActivityService;
        }

        public ActionResult FeedItem(Event item, ActivityFeedOptionsWithGroups options)
        {
            EventExtendedItemModel extendedModel = GetItemViewModel(item, options);
            return PartialView(ItemViewPath, extendedModel);
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
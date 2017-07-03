using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Events;
using uIntra.CentralFeed;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Web;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class EventsController : EventsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/Events/DetailsView.cshtml";
        protected override string CreateViewPath => "~/Views/Events/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/Events/EditView.cshtml";
        protected override string ItemViewPath => "~/Views/Events/ItemView.cshtml";
        protected override int ShortDescriptionLength { get; } = 500;

        private readonly IEventsService<Event> _eventsService;
        private readonly IReminderService _reminderService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public EventsController(IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            IReminderService reminderService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IGridHelper gridHelper,
            IActivityTypeProvider activityTypeProvider,
            IDocumentIndexer documentIndexer, INotificationTypeProvider notificationTypeProvider)
            : base(eventsService, mediaHelper, intranetUserService, intranetUserContentHelper, gridHelper, activityTypeProvider)
        {
            _eventsService = eventsService;
            _reminderService = reminderService;
            _documentIndexer = documentIndexer;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as Event;
            var extendedModel = GetItemViewModel(activity).Map<EventExtendedItemModel>();
            extendedModel.LikesInfo = activity;
            extendedModel.SubscribeInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        protected override EventViewModel GetViewModel(EventBase @event)
        {
            var eventExtended = (Event)@event;
            var extendedModel = base.GetViewModel(@event).Map<EventExtendedViewModel>();
            extendedModel = Mapper.Map(eventExtended, extendedModel);
            return extendedModel;
        }

        protected override void DeleteMedia(IEnumerable<int> mediaIds)
        {
            base.DeleteMedia(mediaIds);
            _documentIndexer.DeleteFromIndex(mediaIds);
        }

        protected override void OnEventCreated(Guid activityId, EventCreateModel model)
        {
            _reminderService.CreateIfNotExists(activityId, ReminderTypeEnum.OneDayBefore);
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

        protected override void OnEventHidden(Guid id)
        {
            var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.EventHided.ToInt());
            ((INotifyableService)_eventsService).Notify(id, notificationType);
        }
    }
}
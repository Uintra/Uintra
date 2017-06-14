using System;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Events;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Web;
using uIntra.Notification;
using uIntra.Notification.Configuration;
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

        public EventsController(IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            IReminderService reminderService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IGridHelper gridHelper)
            : base(eventsService, mediaHelper, intranetUserService, intranetUserContentHelper, gridHelper)
        {
            _eventsService = eventsService;
            _reminderService = reminderService;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
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
                ((INotifyableService)_eventsService).Notify(@event.Id, NotificationTypeEnum.EventUpdated);
            }

            _reminderService.CreateIfNotExists(@event.Id, ReminderTypeEnum.OneDayBefore);
        }

        protected override void OnEventHidden(Guid id)
        {
            ((INotifyableService)_eventsService).Notify(id, NotificationTypeEnum.EventHided);
        }
    }
}
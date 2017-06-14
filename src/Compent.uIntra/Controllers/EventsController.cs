using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.Events;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Core.Models;
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
        private readonly IGridHelper _gridHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

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
            _gridHelper = gridHelper;
            _intranetUserService = intranetUserService;
        }

        public override ActionResult ComingEvents()
        {
            var eventsAmount = _gridHelper.GetContentProperty<int>(CurrentPage, "custom.ComingEvents", "eventsAmount");
            var title = _gridHelper.GetContentProperty<string>(CurrentPage, "custom.ComingEvents", "displayTitle");
            var currentDate = DateTime.UtcNow;

            var events = _eventsService.GetComingEvents(currentDate).Take(eventsAmount);
            var eventsList = events as IList<Event> ?? events.ToList();
            var creatorsDictionary = _intranetUserService.GetMany(eventsList.Select(e => e.CreatorId)).ToDictionary(c => c.Id);
            var comingEvents = new List<ComingEventViewModel>();
            foreach (var e in eventsList)
            {
                var viewModel = e.Map<ComingEventViewModel>();
                viewModel.Creator = creatorsDictionary[e.CreatorId];
                comingEvents.Add(viewModel);
            }

            var model = new ComingEventsPanelViewModel()
            {
                Title = title,
                Events = comingEvents
            };
            return PartialView(ComingEventsViewPath, model);
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
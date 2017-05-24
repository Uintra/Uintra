﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.uCommunity.Core.Events;
using uCommunity.CentralFeed;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions.Web;
using uCommunity.Events;
using uCommunity.Events.Web;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;
using uCommunity.Tagging;
using uCommunity.Users.Core;

namespace Compent.uCommunity.Controllers
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
        private readonly ITagsService _tagsService;

        public EventsController(IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            IReminderService reminderService,
            ITagsService tagsService)
            : base(eventsService, mediaHelper, intranetUserService)
        {
            _eventsService = eventsService;
            _reminderService = reminderService;
            _tagsService = tagsService;
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

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public ActionResult Create(EventExtendedActivityCreateModel activityCreateModel)
        {
            return base.Create(activityCreateModel);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public ActionResult Edit(EventExtendedEditModel saveModel)
        {
            return base.Edit(saveModel);
        }

        [HttpPost]
        public JsonResult HasConfirmation(EventExtendedEditModel model)
        {
            var @event = _eventsService.Get(model.Id);
            @event = Mapper.Map(model, @event);
            return Json(new { HasConfirmation = _eventsService.IsActual(@event) && @event.Subscribers.Any() });
        }

        protected override EventCreateModel GetCreateModel()
        {
            var extendedModel = base.GetCreateModel().Map<EventExtendedActivityCreateModel>();
            return extendedModel;
        }

        protected override EventViewModel GetViewModel(EventBase @event)
        {
            var eventExtended = (Event)@event;
            var extendedModel = base.GetViewModel(@event).Map<EventExtendedViewModel>();
            extendedModel = Mapper.Map(eventExtended, extendedModel);
            return extendedModel;
        }

        protected override EventEditModel GetEditViewModel(EventBase @event)
        {
            var model = base.GetEditViewModel(@event).Map<EventExtendedEditModel>();

            var eventExtended = (IHaveTags)@event;
            _tagsService.FillTags(@eventExtended);
            model.Tags = eventExtended.Tags.Map<List<TagEditModel>>();
            return model;
        }

        protected override void OnEventCreated(Guid activityId, EventCreateModel model)
        {
            var extendedModel = (EventExtendedActivityCreateModel)model;
            _tagsService.Save(activityId, extendedModel.Tags.Map<IEnumerable<TagDTO>>());

            _reminderService.CreateIfNotExists(activityId, ReminderTypeEnum.OneDayBefore);
        }

        protected override void OnEventEdited(EventBase @event, EventEditModel model)
        {
            var extendedModel = (EventExtendedEditModel)model;
            _tagsService.Save(@event.Id, extendedModel.Tags.Map<IEnumerable<TagDTO>>());

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

        #region Restricted actions

        [NonAction]
        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public override ActionResult Create(EventCreateModel createModel)
        {
            return base.Create(createModel);
        }

        [NonAction]
        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public override ActionResult Edit(EventEditModel saveModel)
        {
            return base.Edit(saveModel);
        }

        [NonAction]
        [HttpPost]
        public override JsonResult HasConfirmation(EventEditModel model)
        {
            return base.HasConfirmation(model);
        }

        #endregion
    }
}
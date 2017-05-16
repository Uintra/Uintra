using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.uCommunity.Core.Events;
using uCommunity.CentralFeed;
using uCommunity.Core;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Controls.LightboxGallery;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Events;
using uCommunity.Events.Web;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;
using uCommunity.Users.Core;

namespace Compent.uCommunity.Controllers
{
    public class EventsController : EventsControllerBase
    {
        public override string OverviewViewPath => "~/Views/Events/OverView.cshtml";
        public override string ListViewPath => "~/Views/Events/ListView.cshtml";
        public override string DetailsViewPath => "~/Views/Events/DetailsView.cshtml";
        public override string CreateViewPath => "~/Views/Events/CreateView.cshtml";
        public override string EditViewPath => "~/Views/Events/EditView.cshtml";
        public override string ItemViewPath => "~/Views/Events/ItemView.cshtml";

        private readonly IEventsService<Event> _eventsService;
        private readonly IReminderService _reminderService;

        public EventsController(IEventsService<Event> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            IReminderService reminderService)
            : base(eventsService, mediaHelper, intranetUserService)
        {
            _eventsService = eventsService;
            _reminderService = reminderService;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as Event;
            return PartialView(ItemViewPath, GetOverviewItems(Enumerable.Repeat(activity, 1)).Single());
        }

        public override ActionResult List(EventType type, bool showOnlySubscribed)
        {
            var events = type == EventType.Actual ?
                 _eventsService.GetManyActual().OrderBy(IsPinActual).ThenBy(item => item.StartDate).ThenBy(item => item.EndDate) :
                 _eventsService.GetPastEvents().OrderBy(IsPinActual).ThenByDescending(item => item.StartDate).ThenByDescending(item => item.EndDate);

            FillLinks();
            return PartialView(ListViewPath, GetOverviewItems(events));
        }

        public override ActionResult Details(Guid id)
        {
            var @event = _eventsService.Get(id);

            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(_eventsService.GetOverviewPage().Url);
            }

            var model = @event.Map<IntranetEventViewModel>();
            model.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = new List<string> { @event.StartDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat), @event.EndDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat) };
            model.EditPageUrl = _eventsService.GetEditPage().Url;
            model.OverviewPageUrl = _eventsService.GetOverviewPage().Url;
            model.CanEdit = _eventsService.CanEdit(@event);
            model.CanSubscribe = _eventsService.CanSubscribe(@event);

            return PartialView(DetailsViewPath, model);
        }

        [HttpPost]
        public override JsonResult HasConfirmation(EventEditModel model)
        {
            var @event = MapModel(model);
            return Json(new { HasConfirmation = _eventsService.IsActual(@event) && @event.Subscribers.Any() });
        }

        public ActionResult ItemView(EventOverviewItemModel model)
        {
            return PartialView(ItemViewPath, model);
        }

        protected Event MapModel(EventEditModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            return @event;
        }

        protected IEnumerable<EventOverviewItemModel> GetOverviewItems(IEnumerable<Event> events)
        {
            var detailsPageUrl = _eventsService.GetDetailsPage().Url;
            foreach (var @event in events)
            {
                var model = @event.Map<EventOverviewItemModel>();
                model.MediaIds = @event.MediaIds;
                model.CanSubscribe = _eventsService.CanSubscribe(@event);

                model.HeaderInfo = @event.Map<IntranetActivityItemHeaderViewModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", @event.Id.ToString());

                model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
                {
                    MediaIds = @event.MediaIds,
                    Url = detailsPageUrl.UrlWithQueryString("id", @event.Id.ToString())
                };
                yield return model;
            }
        }

        protected override void OnEventCreated(Guid activityId)
        {
            _reminderService.CreateIfNotExists(activityId, ReminderTypeEnum.OneDayBefore);
        }

        protected override void OnEventEdited(Guid id, bool isActual, bool notifySubscribers)
        {
            if (isActual)
            {
                if (notifySubscribers)
                {
                    ((INotifyableService)_eventsService).Notify(id, NotificationTypeEnum.EventUpdated);
                }

                _reminderService.CreateIfNotExists(id, ReminderTypeEnum.OneDayBefore);
            }
        }

        protected override void OnEventHidden(Guid id)
        {
            ((INotifyableService)_eventsService).Notify(id, NotificationTypeEnum.EventHided);
        }

        private bool IsPinActual(EventBase item)
        {
            if (!item.IsPinned) return false;

            if (item.EndPinDate.HasValue)
            {
                return DateTime.Compare(item.EndPinDate.Value, DateTime.Now) > 0;
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.uCommunity.Core.Events;
using uCommunity.CentralFeed;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.Events;
using uCommunity.Events.Web;

namespace Compent.uCommunity.Controllers
{
    public class EventsController : EventsControllerBase
    {
        private readonly IEventsService<EventBase, Compent.uCommunity.Core.Events.Event> _eventsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService _intranetUserService;

        public EventsController(IEventsService<EventBase, Compent.uCommunity.Core.Events.Event> eventsService, IMediaHelper mediaHelper, IIntranetUserService intranetUserService)
            : base(eventsService, mediaHelper, intranetUserService)
        {
            _eventsService = eventsService;
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as Compent.uCommunity.Core.Events.Event;
            return PartialView("~/App_Plugins/Events/List/ItemView.cshtml", GetOverviewItems(Enumerable.Repeat(activity, 1)).Single());
        }

        public override ActionResult List(EventType type, bool showOnlySubscribed)
        {
            IEnumerable<Compent.uCommunity.Core.Events.Event> events = type == EventType.Actual ?
                _eventsService.GetManyActual().OrderBy(item => item.StartDate).ThenBy(item => item.EndDate) :
                _eventsService.GetPastEvents().OrderByDescending(item => item.StartDate).ThenByDescending(item => item.EndDate);

            FillLinks();
            return PartialView("~/App_Plugins/Events/List/ListView.cshtml", GetOverviewItems(events));
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
             
            return PartialView("~/App_Plugins/Events/Details/DetailsView.cshtml", model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public override ActionResult Create(EventCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView("~/App_Plugins/Events/Create/CreateView.cshtml", createModel);
            }

            var @event = createModel.Map<Compent.uCommunity.Core.Events.Event>();
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            @event.CreatorId = _intranetUserService.GetCurrentUserId();

            var activityId = _eventsService.Create(@event);

            return RedirectToUmbracoPage(_eventsService.GetDetailsPage(), new NameValueCollection { { "id", activityId.ToString() } });
        }

        [HttpPost]
        public override JsonResult HasConfirmation(EventEditModel model)
        {
            var @event = MapEditModel(model);
            return Json(new { HasConfirmation = _eventsService.IsActual(@event) && @event.Subscribers.Any() });
        }

        public ActionResult ItemView(Compent.uCommunity.Core.Events.EventOverviewItemModel model)
        {
            return PartialView("~/App_Plugins/Events/List/ItemView.cshtml", model);
        }

        protected Compent.uCommunity.Core.Events.Event MapEditModel(EventEditModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            return @event;
        }

        protected IEnumerable<Compent.uCommunity.Core.Events.EventOverviewItemModel> GetOverviewItems(IEnumerable<Compent.uCommunity.Core.Events.Event> events)
        {
            var detailsPageUrl = _eventsService.GetDetailsPage().Url;
            foreach (var @event in events)
            {
                var model = @event.Map<Compent.uCommunity.Core.Events.EventOverviewItemModel>();
                model.MediaIds = @event.MediaIds.Take(ImageConstants.DefaultActivityOverviewImagesCount).JoinToString(",");
                model.CanSubscribe = _eventsService.CanSubscribe(@event);

                model.HeaderInfo = @event.Map<IntranetActivityItemHeaderViewModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", @event.Id.ToString());

                yield return model;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.Events.Web
{
    [ActivityController(IntranetActivityTypeEnum.Events)]
    public abstract class EventsControllerBase : SurfaceController
    {
        public virtual string OverviewViewPath { get; } = "~/App_Plugins/Events/List/OverView.cshtml";
        public virtual string ListViewPath { get; } = "~/App_Plugins/Events/List/ListView.cshtml";
        public virtual string DetailsViewPath { get; } = "~/App_Plugins/Events/Details/DetailsView.cshtml";
        public virtual string CreateViewPath { get; } = "~/App_Plugins/Events/Create/CreateView.cshtml";
        public virtual string EditViewPath { get; } = "~/App_Plugins/Events/Edit/EditView.cshtml";
        public virtual string ItemViewPath { get; } = "~/App_Plugins/Events/List/ItemView.cshtml";

        private readonly IEventsService _eventsService;
        protected readonly IMediaHelper _mediaHelper;
        protected readonly IIntranetUserService _intranetUserService;

        protected EventsControllerBase(
            IEventsService eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService intranetUserService)
        {
            _eventsService = eventsService;
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
        }

        public virtual ActionResult OverView()
        {
            FillLinks();
            return PartialView(OverviewViewPath);
        }

        public virtual ActionResult List(EventType type, bool showOnlySubscribed)
        {
            var events = type == EventType.Actual ?
                _eventsService.GetManyActual().OrderBy(item => item.StartDate).ThenBy(item => item.EndDate) :
                _eventsService.GetPastEvents().OrderByDescending(item => item.StartDate).ThenByDescending(item => item.EndDate);

            FillLinks();
            return PartialView(ListViewPath, GetOverviewItems(events));
        }

        public virtual ActionResult Details(Guid id)
        {
            var @event = _eventsService.Get(id);

            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(_eventsService.GetOverviewPage().Url);
            }

            var model = @event.Map<EventViewModel>();
            model.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = new List<string> { @event.StartDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat), @event.EndDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat) };
            model.EditPageUrl = _eventsService.GetEditPage().Url;
            model.OverviewPageUrl = _eventsService.GetOverviewPage().Url;
            model.CanEdit = _eventsService.CanEdit(@event);
            model.CanSubscribe = _eventsService.CanSubscribe(@event);

            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = new EventCreateModel
            {
                StartDate = DateTime.Now.Date.AddHours(8),
                EndDate = DateTime.Now.Date.AddHours(8),
                CanSubscribe = true
            };
            FillCreateEditData(model);
            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(EventCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView(CreateViewPath, createModel);
            }

            var @event = createModel.Map<EventBase>();
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            @event.CreatorId = _intranetUserService.GetCurrentUserId();

            var activityId = _eventsService.Create(@event);
            OnEventCreated(activityId);

            return RedirectToUmbracoPage(_eventsService.GetDetailsPage(), new NameValueCollection { { "id", activityId.ToString() } });
        }

        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id)
        {
            var @event = _eventsService.Get(id);
            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(_eventsService.GetOverviewPage().Url);
            }

            if (!_eventsService.CanEdit(@event))
            {
                HttpContext.Response.Redirect(_eventsService.GetDetailsPage().Url.UrlWithQueryString("id", id.ToString()));
            }

            var model = @event.Map<EventEditModel>();
            model.CanEditSubscribe = _eventsService.CanEditSubscribe(@event.Id);
            FillCreateEditData(model);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(EventEditModel saveModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(saveModel);
                return PartialView(EditViewPath, saveModel);
            }

            var @event = MapEditModel(saveModel);
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(saveModel));
            @event.CreatorId = _intranetUserService.GetCurrentUserId();

            if (_eventsService.CanEditSubscribe(@event.Id))
            {
                @event.CanSubscribe = saveModel.CanSubscribe;
            }
            var isActual = _eventsService.IsActual(@event);
            _eventsService.Save(@event);

            OnEventEdited(@event.Id, isActual, saveModel.NotifyAllSubscribers);

            return RedirectToUmbracoPage(_eventsService.GetDetailsPage(), new NameValueCollection { { "id", @event.Id.ToString() } });
        }

        [HttpPost]
        public virtual JsonResult Hide(Guid id)
        {
            OnEventHidden(id);
            _eventsService.Hide(id);

            return Json(new { _eventsService.GetOverviewPage().Url });
        }

        [HttpPost]
        public virtual JsonResult HasConfirmation(EventEditModel model)
        {
            var @event = MapEditModel(model);
            return Json(new { HasConfirmation = _eventsService.IsActual(@event) });
        }

        public virtual ActionResult ItemView(EventsOverviewItemViewModel model)
        {
            return PartialView(ItemViewPath, model);
        }

        protected virtual EventBase MapEditModel(EventEditModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            return @event;
        }

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            FillLinks();

            var mediaSettings = _eventsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual void FillLinks()
        {
            ViewData["CreatePageUrl"] = _eventsService.GetCreatePage().Url;
            ViewData["DetailsPageUrl"] = _eventsService.GetDetailsPage().Url;
            ViewData["OverviewPageUrl"] = _eventsService.GetOverviewPage().Url;
        }

        protected virtual IEnumerable<EventsOverviewItemViewModel> GetOverviewItems(IEnumerable<EventBase> events)
        {
            var detailsPageUrl = _eventsService.GetDetailsPage().Url;
            foreach (var @event in events)
            {
                var model = @event.Map<EventsOverviewItemViewModel>();
                model.MediaIds = @event.MediaIds.Take(ImageConstants.DefaultActivityOverviewImagesCount).JoinToString(",");
                model.CanSubscribe = _eventsService.CanSubscribe(@event);

                model.HeaderInfo = @event.Map<IntranetActivityItemHeaderViewModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", @event.Id.ToString());

                yield return model;
            }
        }

        protected virtual void OnEventCreated(Guid activityId)
        {
        }

        protected virtual void OnEventEdited(Guid id, bool isActual, bool notifySubscribers)
        {
        }

        protected virtual void OnEventHidden(Guid id)
        {
        }
    }
}
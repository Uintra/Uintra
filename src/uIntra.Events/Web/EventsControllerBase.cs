using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Activity.Models;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Core.User.Permissions.Web;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace uIntra.Events.Web
{
    [ActivityController(IntranetActivityTypeEnum.Events)]
    public abstract class EventsControllerBase : SurfaceController
    {
        protected virtual string DetailsViewPath => "~/App_Plugins/Events/Details/DetailsView.cshtml";
        protected virtual string CreateViewPath => "~/App_Plugins/Events/Create/CreateView.cshtml";
        protected virtual string EditViewPath => "~/App_Plugins/Events/Edit/EditView.cshtml";
        protected virtual string ItemViewPath => "~/App_Plugins/Events/List/ItemView.cshtml";
        protected virtual int ShortDescriptionLength { get; } = 500;
        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly IEventsService<EventBase> _eventsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected EventsControllerBase(
            IEventsService<EventBase> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _eventsService = eventsService;
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
        }

        public virtual ActionResult Details(Guid id)
        {
            FillLinks();
            var @event = _eventsService.Get(id);
            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(IntranetActivityTypeEnum.Events));
            }

            var model = GetViewModel(@event);

            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = GetCreateModel();

            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(EventCreateModel createModel)
        {
            FillLinks();
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView(CreateViewPath, createModel);
            }

            var @event = createModel.Map<EventBase>();
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            @event.CreatorId = _intranetUserService.GetCurrentUserId();

            var activityId = _eventsService.Create(@event);
            OnEventCreated(activityId, createModel);

            return Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, activityId));
        }

        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id)
        {
            FillLinks();

            var @event = _eventsService.Get(id);
            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(IntranetActivityTypeEnum.Events));
            }

            if (!_eventsService.CanEdit(@event))
            {
                HttpContext.Response.Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, id));
            }

            var model = GetEditViewModel(@event);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(EventEditModel saveModel)
        {
            FillLinks();

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
            _eventsService.Save(@event);

            OnEventEdited(@event, saveModel);

            return Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, @event.Id));
        }

        [HttpPost]
        public virtual JsonResult Hide(Guid id)
        {
            _eventsService.Hide(id);
            OnEventHidden(id);

            FillLinks();
            return Json(new { Url = ViewData.GetActivityOverviewPageUrl(IntranetActivityTypeEnum.Events) });
        }

        [HttpPost]
        public virtual JsonResult HasConfirmation(EventEditModel model)
        {
            var @event = MapEditModel(model);
            return Json(new { HasConfirmation = _eventsService.IsActual(@event) });
        }

        protected virtual EventCreateModel GetCreateModel()
        {
            FillLinks();
            var model = new EventCreateModel
            {
                StartDate = DateTime.Now.Date.AddHours(8),
                EndDate = DateTime.Now.Date.AddHours(8),
                CanSubscribe = true
            };
            FillCreateEditData(model);
            return model;
        }

        protected virtual EventBase MapEditModel(EventEditModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            return @event;
        }

        protected virtual EventEditModel GetEditViewModel(EventBase @event)
        {
            var model = @event.Map<EventEditModel>();
            model.CanEditSubscribe = _eventsService.CanEditSubscribe(@event.Id);
            FillCreateEditData(model);
            return model;
        }

        protected virtual EventViewModel GetViewModel(EventBase @event)
        {
            var model = @event.Map<EventViewModel>();
            model.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = new[] { @event.StartDate.ToDateTimeFormat(), @event.EndDate.ToDateTimeFormat() };
            model.CanEdit = _eventsService.CanEdit(@event);
            model.CanSubscribe = _eventsService.CanSubscribe(@event);
            return model;
        }

        protected virtual EventItemViewModel GetItemViewModel(EventBase @event)
        {
            var model = @event.Map<EventItemViewModel>();

            model.ShortDescription = @event.Description.Truncate(ShortDescriptionLength);
            model.MediaIds = @event.MediaIds;
            model.CanSubscribe = _eventsService.CanSubscribe(@event);

            model.HeaderInfo = @event.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.DetailsPageUrl = ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, @event.Id);

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = @event.MediaIds,
                Url = ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, @event.Id),
                DisplayedImagesCount = DisplayedImagesCount
            };
            return model;
        }

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            var mediaSettings = _eventsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual void FillLinks()
        {
            var overviewPageUrl = _eventsService.GetOverviewPage(CurrentPage).Url;
            var createPageUrl = _eventsService.GetCreatePage(CurrentPage).Url;
            var detailsPageUrl = _eventsService.GetDetailsPage(CurrentPage).Url;
            var editPageUrl = _eventsService.GetEditPage(CurrentPage).Url;

            ViewData.SetActivityOverviewPageUrl(IntranetActivityTypeEnum.Events, overviewPageUrl);
            ViewData.SetActivityDetailsPageUrl(IntranetActivityTypeEnum.Events, detailsPageUrl);
            ViewData.SetActivityCreatePageUrl(IntranetActivityTypeEnum.Events, createPageUrl);
            ViewData.SetActivityEditPageUrl(IntranetActivityTypeEnum.Events, editPageUrl);
        }

        protected virtual void OnEventCreated(Guid activityId, EventCreateModel model)
        {
        }

        protected virtual void OnEventEdited(EventBase @event, EventEditModel model)
        {
        }

        protected virtual void OnEventHidden(Guid id)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions.Web;
using uIntra.Events.Core.Models;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace uIntra.Events.Web
{
    [ActivityController(ActivityTypeId)]
    public abstract class EventsControllerBase : SurfaceController 
    {
        protected virtual string ComingEventsViewPath => "~/App_Plugins/Events/ComingEvents/ComingEventsView.cshtml";
        protected virtual string DetailsViewPath => "~/App_Plugins/Events/Details/DetailsView.cshtml";
        protected virtual string CreateViewPath => "~/App_Plugins/Events/Create/CreateView.cshtml";
        protected virtual string EditViewPath => "~/App_Plugins/Events/Edit/EditView.cshtml";
        protected virtual string ItemViewPath => "~/App_Plugins/Events/List/ItemView.cshtml";
        protected virtual string PreviewItemViewPath => "~/App_Plugins/Events/PreviewItem/PreviewItemView.cshtml";
        protected virtual int ShortDescriptionLength { get; } = 500;
        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly IEventsService<EventBase> _eventsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IGridHelper _gridHelper;
        private readonly IActivityTypeProvider _activityTypeProvider;

        private const int ActivityTypeId = (int) IntranetActivityTypeEnum.Events;

        protected EventsControllerBase(
            IEventsService<EventBase> eventsService,
            IMediaHelper mediaHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IGridHelper gridHelper, 
            IActivityTypeProvider activityTypeProvider)
        {
            _eventsService = eventsService;
            _mediaHelper = mediaHelper;
            _intranetUserService = intranetUserService;
            _intranetUserContentHelper = intranetUserContentHelper;
            _gridHelper = gridHelper;
            _activityTypeProvider = activityTypeProvider;
        }

        public virtual ActionResult Details(Guid id)
        {
            FillLinks();
            var @event = _eventsService.Get(id);
            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetViewModel(@event);

            return PartialView(DetailsViewPath, model);
        }

        [HttpGet]
        public virtual ActionResult ComingEvents(ComingEventsPanelModel panelModel)
        {
            var viewModel = GetComingEventsViewModel(panelModel);
            return PartialView(ComingEventsViewPath, viewModel);
        }

        protected virtual ComingEventsPanelViewModel GetComingEventsViewModel(ComingEventsPanelModel panelModel)
        {
            string overviewUrl = _eventsService.GetOverviewPage().Url;
            var viewModel = new ComingEventsPanelViewModel()
            {
                OverviewUrl = overviewUrl,
                Title = panelModel.DisplayTitle,
                Events = GetComingEvents(panelModel.EventsAmount)
            };
            return viewModel;
        }

        protected virtual IList<ComingEventViewModel> GetComingEvents(int eventsAmount)
        {
            var events = _eventsService.GetComingEvents(DateTime.UtcNow).Take(eventsAmount);
            var eventsList = events as IList<EventBase> ?? events.ToList();

            var creatorsDictionary = _intranetUserService.GetMany(eventsList.Select(e => e.CreatorId)).ToDictionary(c => c.Id);

            var detailsPage = _eventsService.GetDetailsPage(CurrentPage);
            var comingEvents = new List<ComingEventViewModel>();

            foreach (var e in eventsList)
            {
                var viewModel = e.Map<ComingEventViewModel>();
                viewModel.DetailsPageUrl = detailsPage.Url.AddIdParameter(e.Id);
                viewModel.Creator = creatorsDictionary[e.CreatorId];
                comingEvents.Add(viewModel);
            }

            return comingEvents;
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = GetCreateModel();

            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(EventCreateModel createModel)
        {
            FillLinks();
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var @event = MapToEvent(createModel);
            var activityId = _eventsService.Create(@event);
            OnEventCreated(activityId, createModel);

            return Redirect(ViewData.GetActivityDetailsPageUrl(ActivityTypeId, activityId));
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id)
        {
            FillLinks();

            var @event = _eventsService.Get(id);
            if (@event.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetEditViewModel(@event);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(EventEditModel editModel)
        {
            FillLinks();

            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var cachedActivityMedias = _eventsService.Get(editModel.Id).MediaIds;

            var activity = MapToEvent(editModel);
            _eventsService.Save(activity);

            DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            OnEventEdited(activity, editModel);

            return Redirect(ViewData.GetActivityDetailsPageUrl(ActivityTypeId, activity.Id));
        }

        [HttpPost]
        public virtual void Hide(Guid id, bool isNotificationNeeded)
        {
            _eventsService.Hide(id);
            OnEventHidden(id, isNotificationNeeded);
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
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(8),
                CanSubscribe = true,
                Creator = _intranetUserService.GetCurrentUser(),
                ActivityType = _activityTypeProvider.Get(ActivityTypeId)
            };
            FillCreateEditData(model);
            return model;
        }

        protected virtual EventPreviewViewModel GetPreviewViewModel(EventBase @event)
        {
            var creator = _intranetUserService.Get(@event);
            return new EventPreviewViewModel()
            {
                Id = @event.Id,
                Title = @event.Title,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Creator = creator,
                ActivityType = @event.Type
            };
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
            model.Creator = _intranetUserService.Get(@event);
            FillCreateEditData(model);
            return model;
        }

        protected virtual EventViewModel GetViewModel(EventBase @event)
        {
            var model = @event.Map<EventViewModel>();
            model.HeaderInfo = @event.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = new[] { @event.StartDate.ToDateTimeFormat(), @event.EndDate.ToDateTimeFormat() };
            model.HeaderInfo.Creator = _intranetUserService.Get(@event);
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
            model.HeaderInfo.Creator = _intranetUserService.Get(@event);

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = @event.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = @event.Id,
                ActivityType = @event.Type
            };
            return model;
        }

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            var mediaSettings = _eventsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual EventBase MapToEvent(EventCreateModel createModel)
        {
            var @event = createModel.Map<EventBase>();
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            @event.StartDate = createModel.StartDate.ToUniversalTime();
            @event.EndDate = createModel.EndDate.ToUniversalTime();
            @event.EndPinDate = createModel.EndPinDate?.ToUniversalTime();

            return @event;
        }

        protected virtual EventBase MapToEvent(EventEditModel editModel)
        {
            var @event = MapEditModel(editModel);
            @event.MediaIds = @event.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            @event.UmbracoCreatorId = _intranetUserService.Get(editModel.CreatorId).UmbracoId;
            @event.StartDate = editModel.StartDate.ToUniversalTime();
            @event.EndDate = editModel.EndDate.ToUniversalTime();
            @event.EndPinDate = editModel.EndPinDate?.ToUniversalTime();

            if (_eventsService.CanEditSubscribe(@event.Id))
            {
                @event.CanSubscribe = editModel.CanSubscribe;
            }

            return @event;
        }

        protected virtual void FillLinks()
        {
            var overviewPageUrl = _eventsService.GetOverviewPage(CurrentPage).Url;
            var createPageUrl = _eventsService.GetCreatePage(CurrentPage).Url;
            var detailsPageUrl = _eventsService.GetDetailsPage(CurrentPage).Url;
            var editPageUrl = _eventsService.GetEditPage(CurrentPage).Url;
            var profilePageUrl = _intranetUserContentHelper.GetProfilePage().Url;

            ViewData.SetActivityOverviewPageUrl(ActivityTypeId, overviewPageUrl);
            ViewData.SetActivityDetailsPageUrl(ActivityTypeId, detailsPageUrl);
            ViewData.SetActivityCreatePageUrl(ActivityTypeId, createPageUrl);
            ViewData.SetActivityEditPageUrl(ActivityTypeId, editPageUrl);
            ViewData.SetProfilePageUrl(profilePageUrl);
        }

        protected virtual void DeleteMedia(IEnumerable<int> mediaIds)
        {
            _mediaHelper.DeleteMedia(mediaIds);
        }

        protected virtual void OnEventCreated(Guid activityId, EventCreateModel model)
        {
        }

        protected virtual void OnEventEdited(EventBase @event, EventEditModel model)
        {
        }

        protected virtual void OnEventHidden(Guid id, bool isNotificationNeeded)
        {
        }
    }
}
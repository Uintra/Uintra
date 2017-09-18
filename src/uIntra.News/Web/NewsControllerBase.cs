using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions.Web;
using Umbraco.Web.Mvc;

namespace uIntra.News.Web
{
    [ActivityController(ActivityTypeId)]
    public abstract class NewsControllerBase : SurfaceController
    {
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/News/List/ItemView.cshtml";
        protected virtual string PreviewItemViewPath { get; } = "~/App_Plugins/News/PreviewItem/PreviewItemView.cshtml";
        protected virtual string DetailsViewPath { get; } = "~/App_Plugins/News/Details/DetailsView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/News/Create/CreateView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/News/Edit/EditView.cshtml";
        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly INewsService<NewsBase> _newsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IActivityTypeProvider _activityTypeProvider;
         
        private const int ActivityTypeId = (int)IntranetActivityTypeEnum.News;

        protected NewsControllerBase(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<NewsBase> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
            _intranetUserContentHelper = intranetUserContentHelper;
            _activityTypeProvider = activityTypeProvider;
        }

        public virtual ActionResult Details(Guid id, ActivityLinks links)
        {
            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetViewModel(news, links);

            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(ActivityCreateLinks links)
        {
            var model = GetCreateModel(links);
            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }
            var newsBaseCreateModel = MapToNews(createModel);
            var activityId = _newsService.Create(newsBaseCreateModel);
             
            OnNewsCreated(activityId, createModel);
            return Redirect(ViewData.GetActivityDetailsPageUrl(ActivityTypeId, activityId));
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id, ActivityLinks links)
        {
            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetEditViewModel(news, links);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(NewsEditModel editModel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var cachedActivityMedias = _newsService.Get(editModel.Id).MediaIds;

            var activity = MapToNews(editModel);
            _newsService.Save(activity);

            DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            OnNewsEdited(activity, editModel);
            return Redirect(ViewData.GetActivityDetailsPageUrl(ActivityTypeId, editModel.Id));
        }


        protected virtual NewsCreateModel GetCreateModel(ActivityCreateLinks links)
        {
            var mediaSettings = _newsService.GetMediaSettings();
            var model = new NewsCreateModel
            {
                PublishDate = DateTime.UtcNow,
                Creator = _intranetUserService.GetCurrentUser(),
                ActivityType = _activityTypeProvider.Get(ActivityTypeId),
                Links = links,
                MediaRootId = mediaSettings.MediaRootId
            };
            return model;
        }

        protected virtual NewsEditModel GetEditViewModel(NewsBase news, ActivityLinks links)
        {
            var model = news.Map<NewsEditModel>();
            var mediaSettings = _newsService.GetMediaSettings();
            model.MediaRootId = mediaSettings.MediaRootId;
            FillMediaSettingsData(mediaSettings);

            model.Creator = _intranetUserService.Get(news);

            model.Links = links;

            return model;
        }

        protected virtual void FillMediaSettingsData(MediaSettings settings)
        {
            ViewData["AllowedMediaExtentions"] = settings.AllowedMediaExtentions;
        }

        protected virtual NewsViewModel GetViewModel(NewsBase news)
        {
            var model = news.Map<NewsViewModel>();
            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerableOfOne();
            model.HeaderInfo.Creator = _intranetUserService.Get(news);
            model.CanEdit = _newsService.CanEdit(news);
            return model;
        }

        protected virtual NewsViewModel GetViewModel(NewsBase news, ActivityLinks links)
        {
            var model = news.Map<NewsViewModel>();

            model.CanEdit = _newsService.CanEdit(news);
            model.Links = links;

            // TODO : try to move this logic smwhere to avoid duplication
            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerableOfOne();
            model.HeaderInfo.Creator = _intranetUserService.Get(news);
            model.HeaderInfo.Links = links;

            return model;
        }

        protected virtual NewsItemViewModel GetItemViewModel(NewsBase news, ActivityLinks links)
        {
            var model = news.Map<NewsItemViewModel>();
            model.MediaIds = news.MediaIds;
            model.Expired = _newsService.IsExpired(news);
            model.Links = links;

            model.HeaderInfo = news.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Creator = _intranetUserService.Get(news);
            model.HeaderInfo.Links = links;

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = news.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = news.Id,
                ActivityType = news.Type
            };
            return model;
        }


        protected virtual NewsPreviewViewModel GetPreviewViewModel(NewsBase news, ActivityLinks links)
        {
            IIntranetUser creator = _intranetUserService.Get(news);
            return new NewsPreviewViewModel()
            {
                Id = news.Id,
                Title = news.Title,
                PublishDate = news.PublishDate,
                Creator = creator,
                ActivityType = news.Type,
                Links = links
            };
        }

        protected virtual NewsBase MapToNews(NewsCreateModel createModel)
        {
            var news = createModel.Map<NewsBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            news.PublishDate = createModel.PublishDate.ToUniversalTime();
            news.UnpublishDate = createModel.UnpublishDate?.ToUniversalTime();
            news.EndPinDate = createModel.EndPinDate?.ToUniversalTime();

            return news;
        }

        protected virtual NewsBase MapToNews(NewsEditModel editModel)
        {
            var activity = _newsService.Get(editModel.Id);
            activity = Mapper.Map(editModel, activity);
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            activity.UmbracoCreatorId = _intranetUserService.Get(editModel.CreatorId).UmbracoId;
            activity.PublishDate = editModel.PublishDate.ToUniversalTime();
            activity.UnpublishDate = editModel.UnpublishDate?.ToUniversalTime();
            activity.EndPinDate = editModel.EndPinDate?.ToUniversalTime();

            return activity;
        }

        protected virtual void DeleteMedia(IEnumerable<int> mediaIds)
        {
            _mediaHelper.DeleteMedia(mediaIds);
        }

        protected virtual void OnNewsCreated(Guid activityId, NewsCreateModel model)
        {
        }

        protected virtual void OnNewsEdited(NewsBase news, NewsEditModel model)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extentions;
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

        public virtual ActionResult Details(Guid id)
        {
            FillLinks();

            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetViewModel(news);

            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = GetCreateModel();
            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            FillLinks();
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
        public virtual ActionResult Edit(Guid id)
        {
            FillLinks();

            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(ActivityTypeId));
            }

            var model = GetEditViewModel(news);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(ActivityTypeId, IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(NewsEditModel editModel)
        {
            FillLinks();

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

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            var mediaSettings = _newsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual NewsCreateModel GetCreateModel()
        {
            FillLinks();
            var model = new NewsCreateModel
            {
                PublishDate = DateTime.UtcNow,
                Creator = _intranetUserService.GetCurrentUser(),
                ActivityType = _activityTypeProvider.Get(ActivityTypeId)
            };

            FillCreateEditData(model);
            return model;
        }

        protected virtual NewsEditModel GetEditViewModel(NewsBase news)
        {
            var model = news.Map<NewsEditModel>();
            model.Creator = _intranetUserService.Get(news);
            FillCreateEditData(model);
            return model;
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

        protected virtual NewsItemViewModel GetItemViewModel(NewsBase news)
        {
            var model = news.Map<NewsItemViewModel>();
            model.MediaIds = news.MediaIds;
            model.Expired = _newsService.IsExpired(news);

            model.HeaderInfo = news.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.Creator = _intranetUserService.Get(news);

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = news.MediaIds,
                DisplayedImagesCount = DisplayedImagesCount,
                ActivityId = news.Id,
                ActivityType = news.Type
            };
            return model;
        }


        protected virtual NewsPreviewViewModel GetPreviewViewModel(NewsBase news)
        {
            IIntranetUser creator = _intranetUserService.Get(news);
            return new NewsPreviewViewModel()
            {
                Id = news.Id,
                Title = news.Title,
                PublishDate = news.PublishDate,
                Creator = creator,
                ActivityType = news.Type
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

        protected virtual void FillLinks()
        {
            var overviewPageUrl = _newsService.GetOverviewPage(CurrentPage).Url;
            var createPageUrl = _newsService.GetCreatePage(CurrentPage).Url;
            var detailsPageUrl = _newsService.GetDetailsPage(CurrentPage).Url;
            var editPageUrl = _newsService.GetEditPage(CurrentPage).Url;
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

        protected virtual void OnNewsCreated(Guid activityId, NewsCreateModel model)
        {
        }

        protected virtual void OnNewsEdited(NewsBase news, NewsEditModel model)
        {
        }
    }
}
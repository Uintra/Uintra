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

namespace uIntra.News.Web
{
    [ActivityController(IntranetActivityTypeEnum.News)]
    public abstract class NewsControllerBase : SurfaceController
    {
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/News/List/ItemView.cshtml";
        protected virtual string DetailsViewPath { get; } = "~/App_Plugins/News/Details/DetailsView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/News/Create/CreateView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/News/Edit/EditView.cshtml";
        protected virtual int ShortDescriptionLength { get; } = 500;
        protected virtual int DisplayedImagesCount { get; } = 3;

        private readonly INewsService<NewsBase> _newsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected NewsControllerBase(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<NewsBase> newsService,
            IMediaHelper mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
        }

        public virtual ActionResult Details(Guid id)
        {
            FillLinks();
            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(IntranetActivityTypeEnum.News));
            }

            var model = GetViewModel(news);

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
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            FillLinks();
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView(CreateViewPath, createModel);
            }

            var activityId = CreateNews(createModel);
            OnNewsCreated(activityId, createModel);
            return Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, activityId));
        }

        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id)
        {
            FillLinks();

            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(ViewData.GetActivityOverviewPageUrl(IntranetActivityTypeEnum.News));
            }

            if (!_newsService.CanEdit(news))
            {
                HttpContext.Response.Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, id));
            }

            var model = GetEditViewModel(news);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(NewsEditModel editModel)
        {
            FillLinks();

            if (!ModelState.IsValid)
            {
                FillCreateEditData(editModel);
                return PartialView(EditViewPath, editModel);
            }

            var activity = UpdateNews(editModel);
            OnNewsEdited(activity, editModel);
            return Redirect(ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, editModel.Id));
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
                PublishDate = DateTime.Now.Date
            };
            FillCreateEditData(model);
            return model;
        }

        protected virtual NewsEditModel GetEditViewModel(NewsBase news)
        {
            var model = news.Map<NewsEditModel>();
            FillCreateEditData(model);
            return model;
        }

        protected virtual NewsViewModel GetViewModel(NewsBase news)
        {
            var model = news.Map<NewsViewModel>();
            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerableOfOne();
            model.CanEdit = _newsService.CanEdit(news);
            return model;
        }

        protected virtual NewsItemViewModel GetItemViewModel(NewsBase news)
        {
            var model = news.Map<NewsItemViewModel>();
            model.ShortDescription = news.Description.Truncate(ShortDescriptionLength);
            model.MediaIds = news.MediaIds;
            model.HeaderInfo = news.Map<IntranetActivityItemHeaderViewModel>();
            model.HeaderInfo.DetailsPageUrl = ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, news.Id);
            model.Expired = _newsService.IsExpired(news);

            model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
            {
                MediaIds = news.MediaIds,
                Url = ViewData.GetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, news.Id),
                DisplayedImagesCount = DisplayedImagesCount
            };
            return model;
        }

        protected virtual Guid CreateNews(NewsCreateModel createModel)
        {
            var news = createModel.Map<NewsBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            news.CreatorId = _intranetUserService.GetCurrentUserId();            

            return _newsService.Create(news);
        }

        protected virtual NewsBase UpdateNews(NewsEditModel editModel)
        {
            var activity = _newsService.Get(editModel.Id);
            activity = Mapper.Map(editModel, activity);
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            activity.CreatorId = _intranetUserService.GetCurrentUserId();

            _newsService.Save(activity);
            return activity;
        }

        protected virtual void FillLinks()
        {
            var overviewPageUrl = _newsService.GetOverviewPage(CurrentPage).Url;
            var createPageUrl = _newsService.GetCreatePage(CurrentPage).Url;
            var detailsPageUrl = _newsService.GetDetailsPage(CurrentPage).Url;
            var editPageUrl = _newsService.GetEditPage(CurrentPage).Url;

            ViewData.SetActivityOverviewPageUrl(IntranetActivityTypeEnum.News, overviewPageUrl);
            ViewData.SetActivityDetailsPageUrl(IntranetActivityTypeEnum.News, detailsPageUrl);
            ViewData.SetActivityCreatePageUrl(IntranetActivityTypeEnum.News, createPageUrl);
            ViewData.SetActivityEditPageUrl(IntranetActivityTypeEnum.News, editPageUrl);
        }

        protected virtual void OnNewsCreated(Guid activityId, NewsCreateModel model)
        {
        }

        protected virtual void OnNewsEdited(NewsBase news, NewsEditModel model)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Controls.LightboxGallery;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions.Web;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace uCommunity.News.Web
{
    [ActivityController(IntranetActivityTypeEnum.News)]
    public abstract class NewsControllerBase : SurfaceController
    {
        protected virtual string ListViewPath { get; } = "~/App_Plugins/News/List/ListView.cshtml";
        protected virtual string ItemViewPath { get; } = "~/App_Plugins/News/List/ItemView.cshtml";
        protected virtual string DetailsViewPath { get; } = "~/App_Plugins/News/Details/DetailsView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/News/Create/CreateView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/News/Edit/EditView.cshtml";
        protected virtual int ShortDescriptionLength { get; } = 500;

        private readonly INewsService<NewsBase> _newsService;
        protected readonly IMediaHelper _mediaHelper;
        protected readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        protected NewsControllerBase(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<NewsBase> newsService,
            IMediaHelper mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
        }

        public virtual ActionResult List()
        {
            var news = _newsService.GetManyActual().OrderByDescending(item => item.PublishDate);
            var model = new NewsOverviewViewModel
            {
                CreatePageUrl = _newsService.GetCreatePage().Url,
                DetailsPageUrl = _newsService.GetDetailsPage().Url,
                Items = GetOverviewItems(news)
            };

            FillLinks();
            return PartialView(ListViewPath, model);
        }

        public virtual ActionResult ItemView(NewsOverviewItemViewModel model)
        {
            return PartialView(ItemViewPath, model);
        }

        public virtual ActionResult Details(Guid id)
        {
            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(_newsService.GetOverviewPage().Url);
            }

            var model = news.Map<NewsViewModel>();
            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = new List<string> { news.PublishDate.ToDateTimeFormat() };
            model.EditPageUrl = _newsService.GetEditPage().Url;
            model.OverviewPageUrl = _newsService.GetOverviewPage().Url;
            model.CanEdit = _newsService.CanEdit(news);

            return PartialView(DetailsViewPath, model);
        }

        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = new NewsCreateModel { PublishDate = DateTime.Now.Date };
            FillCreateEditData(model);
            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView(CreateViewPath, createModel);
            }

            var activityId = CreateNews(createModel);
            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activityId.ToString() } });
        }

        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(Guid id)
        {
            var news = _newsService.Get(id);
            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(_newsService.GetOverviewPage().Url);
            }

            if (!_newsService.CanEdit(news))
            {
                HttpContext.Response.Redirect(_newsService.GetDetailsPage().Url.UrlWithQueryString("id", id.ToString()));
            }

            var model = news.Map<NewsEditModel>();
            FillCreateEditData(model);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(NewsEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(editModel);
                return PartialView(EditViewPath, editModel);
            }

            UpdateNews(editModel);

            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", editModel.Id.ToString() } });
        }

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            FillLinks();

            var mediaSettings = _newsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            ViewData.SetDateTimeFormats();
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual IEnumerable<NewsOverviewItemViewModel> GetOverviewItems(IEnumerable<NewsBase> news)
        {
            var detailsPageUrl = _newsService.GetDetailsPage().Url;
            foreach (var item in news)
            {
                var model = item.Map<NewsOverviewItemViewModel>();
                model.ShortDescription = item.Description.Truncate(ShortDescriptionLength);
                model.MediaIds = item.MediaIds;
                model.HeaderInfo = item.Map<IntranetActivityItemHeaderViewModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", item.Id.ToString());
                model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
                {
                    MediaIds = item.MediaIds,
                    Url = detailsPageUrl.UrlWithQueryString("id", item.Id.ToString()),
                    MaxImagesCount = 2
                };
                model.Expired = _newsService.IsExpired(item);

                yield return model;
            }
        }

        protected virtual Guid CreateNews(NewsCreateModel createModel)
        {
            var news = createModel.Map<NewsBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            news.CreatorId = _intranetUserService.GetCurrentUserId();
            if (createModel.IsPinned && createModel.PinDays > 0)
            {
                news.EndPinDate = DateTime.Now.AddDays(createModel.PinDays);
            }

            return _newsService.Create(news);
        }

        protected virtual void UpdateNews(NewsEditModel editModel)
        {
            var activity = editModel.Map<NewsBase>();
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            activity.CreatorId = _intranetUserService.GetCurrentUserId();

            if (editModel.IsPinned && editModel.PinDays > 0 && activity.PinDays != editModel.PinDays)
            {
                activity.EndPinDate = DateTime.Now.AddDays(editModel.PinDays);
            }

            _newsService.Save(activity);
        }

        protected virtual void FillLinks()
        {
            ViewData["DetailsPageUrl"] = _newsService.GetDetailsPage().Url;
            ViewData["OverviewPageUrl"] = _newsService.GetOverviewPage().Url;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions.Web;
using Umbraco.Web.Mvc;

namespace uCommunity.News.Web
{
    [ActivityController(IntranetActivityTypeEnum.News)]
    public abstract class NewsControllerBase : SurfaceController
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService _intranetUserService;
        private readonly INewsService<NewsBase, NewsModelBase> _newsService;

        protected NewsControllerBase(
            IIntranetUserService intranetUserService,
            INewsService<NewsBase, NewsModelBase> newsService,
            IMediaHelper mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
        }

        public virtual ActionResult List()
        {
            var news = _newsService.GetManyActual();
            var model = new NewsOverviewViewModel
            {
                CreatePageUrl = _newsService.GetCreatePage().Url,
                DetailsPageUrl = _newsService.GetDetailsPage().Url,
                Items = GetOverviewItems(news).OrderByDescending(item => item.PublishDate)
            };

            FillLinks();
            return PartialView("~/App_Plugins/News/List/ListView.cshtml", model);
        }

        public virtual ActionResult ItemView(NewsOverviewItemViewModel model)
        {
            return PartialView("~/App_Plugins/News/List/ItemView.cshtml", model);
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
            model.HeaderInfo.Dates = new List<string> { news.PublishDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat) };
            model.EditPageUrl = _newsService.GetEditPage().Url;
            model.OverviewPageUrl = _newsService.GetOverviewPage().Url;
            model.CanEdit = _newsService.CanEdit(news);

            return PartialView("~/App_Plugins/News/Details/DetailsView.cshtml", model);
        }

        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create()
        {
            var model = new NewsCreateModel { PublishDate = DateTime.Now.Date };
            FillCreateEditData(model);
            return PartialView("~/App_Plugins/News/Create/CreateView.cshtml", model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView("~/App_Plugins/News/Create/CreateView.cshtml", createModel);
            }

            var news = createModel.Map<NewsModelBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            news.CreatorId = _intranetUserService.GetCurrentUserId();

            var activityId = _newsService.Create(news);
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
            return PartialView("~/App_Plugins/News/Edit/EditView.cshtml", model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public virtual ActionResult Edit(NewsEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(editModel);
                return PartialView("~/App_Plugins/News/Edit/EditView.cshtml", editModel);
            }

            var activity = editModel.Map<NewsModelBase>();
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            activity.CreatorId = _intranetUserService.GetCurrentUserId();

            _newsService.Save(activity);
            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activity.Id.ToString() } });
        }

        protected virtual void FillCreateEditData(IContentWithMediaCreateEditModel model)
        {
            FillLinks();

            var mediaSettings = _newsService.GetMediaSettings();
            ViewData["AllowedMediaExtentions"] = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        protected virtual IEnumerable<NewsOverviewItemViewModel> GetOverviewItems(IEnumerable<NewsModelBase> news)
        {
            var detailsPageUrl = _newsService.GetDetailsPage().Url;
            foreach (var item in news)
            {
                var model = item.Map<NewsOverviewItemViewModel>();
                model.MediaIds = item.MediaIds.Take(ImageConstants.DefaultActivityOverviewImagesCount).JoinToString(",");

                model.HeaderInfo = item.Map<IntranetActivityItemHeaderViewModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", item.Id.ToString());

                yield return model;
            }
        }

        protected virtual void FillLinks()
        {
            ViewData["DetailsPageUrl"] = _newsService.GetDetailsPage().Url;
            ViewData["OverviewPageUrl"] = _newsService.GetOverviewPage().Url;
        }
    }
}
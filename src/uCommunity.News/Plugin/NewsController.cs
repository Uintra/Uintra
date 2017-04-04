using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.News
{
    public class NewsController : SurfaceController
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService _intranetUserService;
        private readonly INewsService<NewsBase, NewsModelBase> _newsService;

        public NewsController(
            IIntranetUserService intranetUserService,
            INewsService<NewsBase, NewsModelBase> newsService,
            IMediaHelper mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
        }

        public ActionResult List()
        {
            var news = _newsService.GetManyActual();
            var model = new NewsOverviewModel
            {
                CreatePageUrl = _newsService.GetCreatePage().Url,
                DetailsPageUrl = _newsService.GetDetailsPage().Url,
                Items = GetOverviewItems(news).OrderByDescending(item => item.PublishDate)
            };

            FillLinks();
            return PartialView("~/App_Plugins/News/List/ListView.cshtml", model);
        }

        public ActionResult ItemView(NewsOverviewItemModelBase model)
        {
            return PartialView("~/App_Plugins/News/List/ItemView.cshtml", model);
        }

        public ActionResult Details(Guid id)
        {
            var news = _newsService.Get(id);

            if (news.IsHidden)
            {
                HttpContext.Response.Redirect(_newsService.GetOverviewPage().Url);
            }

            var model = news.Map<NewsViewModelBase>();
            model.HeaderInfo = news.Map<IntranetActivityHeaderBase>();
            model.EditPageUrl = _newsService.GetEditPage().Url;
            model.OverviewPageUrl = _newsService.GetOverviewPage().Url;
            model.CanEdit = _newsService.CanEdit(news);

            return PartialView("~/App_Plugins/News/Details/DetailsView.cshtml", model);
        }

        public ActionResult Create()
        {
            var model = new NewsCreateModel { PublishDate = DateTime.Now.Date };
            FillCreateEditModel(model);
            return PartialView("~/App_Plugins/News/Create/CreateView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(NewsCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditModel(createModel);
                return PartialView("~/App_Plugins/News/Create/CreateView.cshtml", createModel);
            }

            var news = createModel.Map<NewsModelBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));

            var activityId = _newsService.Create(news);
            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activityId.ToString() } });
        }

        public ActionResult Edit(Guid id)
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
            FillCreateEditModel(model);
            return PartialView("~/App_Plugins/News/Edit/EditView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(NewsEditModel saveModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditModel(saveModel);
                return PartialView("~/App_Plugins/News/Edit/EditView.cshtml", saveModel);
            }

            var activity = saveModel.Map<NewsModelBase>();
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(saveModel));

            _newsService.Save(activity);
            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activity.Id.ToString() } });
        }

        public ActionResult ActivityDetailsHeader(IntranetActivityHeaderBase header)
        {
            return PartialView("~/App_Plugins/Core/Activity/ActivityDetailsHeader.cshtml", header);
        }

        public ActionResult ActivityItemHeader(IntranetActivityHeaderModel header)
        {
            return PartialView("~/App_Plugins/Core/Activity/ActivityItemHeader.cshtml", header);
        }

        private void FillCreateEditModel(NewsCreateModel model)
        {
            FillLinks();
            model.Users = _intranetUserService.GetAll().OrderBy(user => user.DisplayedName);

            var mediaSettings = _newsService.GetMediaSettings();
            model.AllowedMediaExtentions = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        private IEnumerable<NewsOverviewItemModelBase> GetOverviewItems(IEnumerable<NewsModelBase> news)
        {
            var detailsPageUrl = _newsService.GetDetailsPage().Url;
            foreach (var item in news)
            {
                var model = item.Map<NewsOverviewItemModelBase>();
                model.MediaIds = item.MediaIds.Take(ImageConstants.DefaultActivityOverviewImagesCount).JoinToString(",");

                model.HeaderInfo = item.Map<IntranetActivityHeaderModel>();
                model.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", item.Id.ToString());

                yield return model;
            }
        }

        private void FillLinks()
        {
            ViewData["DetailsPageUrl"] = _newsService.GetDetailsPage().Url;
            ViewData["OverviewPageUrl"] = _newsService.GetOverviewPage().Url;
        }
    }
}
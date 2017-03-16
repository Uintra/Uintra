using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.News
{
    public class NewsController : SurfaceController
    {
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IntranetUserBase> _intranetUserService;
        private readonly INewsService<NewsBase, NewsModelBase> _newsService;

        public NewsController(
            IIntranetUserService<IntranetUserBase> intranetUserService,
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

        public ActionResult ItemView(NewsOverviewItemModel model)
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

            var model = news.Map<NewsViewModel>();
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

            var news = createModel.Map<NewsBase>();
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

        private void FillCreateEditModel(NewsCreateModel model)
        {
            FillLinks();
            model.Users = _intranetUserService.GetAll().OrderBy(user => user.Name);

            var mediaSettings = _newsService.GetMediaSettings();
            model.AllowedMediaExtentions = mediaSettings.AllowedMediaExtentions;
            model.MediaRootId = mediaSettings.MediaRootId;
        }

        private IEnumerable<NewsOverviewItemModel> GetOverviewItems(IEnumerable<NewsBase> news)
        {
            foreach (var item in news)
            {
                var model = item.Map<NewsOverviewItemModel>();
                model.MediaIds = item.MediaIds.Take(ImageConstants.DefaultActivityOverviewImagesCount).JoinToString(",");
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
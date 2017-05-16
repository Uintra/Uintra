using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Compent.uCommunity.Core.News.Entities;
using Compent.uCommunity.Core.News.Models;
using uCommunity.CentralFeed;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions.Web;
using uCommunity.News;
using uCommunity.News.Web;
using uCommunity.Tagging;
using uCommunity.Users.Core;

namespace Compent.uCommunity.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";

        private readonly INewsService<NewsEntity> _newsService;
        private readonly ITagsService _tagsService;

        public NewsController(IIntranetUserService<IntranetUser> intranetUserService, INewsService<NewsEntity> newsService, IMediaHelper mediaHelper, ITagsService tagsService)
            : base(intranetUserService, newsService, mediaHelper)
        {
            _newsService = newsService;
            _tagsService = tagsService;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as NewsBase;

            var model = GetOverviewItems(Enumerable.Repeat(activity, 1)).Single();
            return PartialView(ItemViewPath, model);
        }

        public override ActionResult List()
        {
            var news = _newsService.GetManyActual().OrderBy(IsPinActual).ThenByDescending(item => item.PublishDate);
            var model = new NewsOverviewViewModel
            {
                CreatePageUrl = _newsService.GetCreatePage().Url,
                DetailsPageUrl = _newsService.GetDetailsPage().Url,
                Items = GetOverviewItems(news)
            };

            FillLinks();
            return PartialView(ListViewPath, model);
        }

        public override ActionResult Details(Guid id)
        {
            var newsModelBase = _newsService.Get(id);
            if (newsModelBase.IsHidden)
            {
                HttpContext.Response.Redirect(_newsService.GetOverviewPage().Url);
            }

            var newsViewModel = newsModelBase.Map<NewsExtendedViewModel>();
            newsViewModel.HeaderInfo = newsModelBase.Map<IntranetActivityDetailsHeaderViewModel>();
            newsViewModel.HeaderInfo.Dates = new List<string> { newsModelBase.PublishDate.ToString("dd.MM.yyyy HH:mm") };
            newsViewModel.EditPageUrl = _newsService.GetEditPage().Url;
            newsViewModel.OverviewPageUrl = _newsService.GetOverviewPage().Url;
            newsViewModel.CanEdit = _newsService.CanEdit(newsModelBase);

            return PartialView(DetailsViewPath, newsViewModel);
        }

        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public override ActionResult Create()
        {
            var model = new NewsExtendedCreateModel { PublishDate = DateTime.Now.Date };
            FillCreateEditData(model);
            return PartialView(CreateViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Create)]
        public ActionResult CreateExtendedNews(NewsExtendedCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(createModel);
                return PartialView(CreateViewPath, createModel);
            }

            var news = createModel.Map<NewsBase>();
            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
            news.CreatorId = _intranetUserService.GetCurrentUserId();
            if (createModel.IsPinned && createModel.PinDays > 0)
            {
                news.EndPinDate = DateTime.Now.AddDays(createModel.PinDays);
            }

            var activityId = _newsService.Create(news);
           _tagsService.SaveTags(activityId, createModel.Tags);

            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activityId.ToString() } });
        }

        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public override ActionResult Edit(Guid id)
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

            _tagsService.FillTags(news);

            var model = news.Map<NewsExtendedEditModel>();
            FillCreateEditData(model);
            return PartialView(EditViewPath, model);
        }

        [HttpPost]
        [RestrictedAction(IntranetActivityActionEnum.Edit)]
        public ActionResult EditExtendedNews(NewsExtendedEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                FillCreateEditData(editModel);
                return PartialView(EditViewPath, editModel);
            }

            var activity = editModel.Map<NewsBase>();
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
            activity.CreatorId = _intranetUserService.GetCurrentUserId();

            _newsService.Save(activity);
            _tagsService.SaveTags(editModel.Id, editModel.Tags);

            return RedirectToUmbracoPage(_newsService.GetDetailsPage(), new NameValueCollection { { "id", activity.Id.ToString() } });
        }

        protected new IEnumerable<NewsOverviewItemExtendedViewModel> GetOverviewItems(IEnumerable<NewsBase> news)
        {
            string detailsPageUrl = _newsService.GetDetailsPage().Url;
            foreach (var newsModelBase in news)
            {
                var overviewItemViewModel = newsModelBase.Map<NewsOverviewItemExtendedViewModel>();
                overviewItemViewModel.MediaIds = newsModelBase.MediaIds.Take(3).JoinToString(",");
                overviewItemViewModel.HeaderInfo = newsModelBase.Map<IntranetActivityItemHeaderViewModel>();
                overviewItemViewModel.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", newsModelBase.Id.ToString());
                overviewItemViewModel.Expired = _newsService.IsExpired(newsModelBase);
                yield return overviewItemViewModel;
            }
        }

        private bool IsPinActual(NewsBase item)
        {
            if (!item.IsPinned) return false;

            if (item.EndPinDate.HasValue)
            {
                return DateTime.Compare(item.EndPinDate.Value, DateTime.Now) > 0;
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.uCommunity.Core.News.Entities;
using Compent.uCommunity.Core.News.Models;
using uCommunity.CentralFeed;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.News;
using uCommunity.News.Web;

namespace Compent.uCommunity.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";

        private readonly INewsService<NewsEntity> _newsService;

        public NewsController(IIntranetUserService intranetUserService, INewsService<NewsEntity> newsService, IMediaHelper mediaHelper)
            : base(intranetUserService, newsService, mediaHelper)
        {
            _newsService = newsService;
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
            var news = _newsService.GetManyActual();
            var model = new NewsOverviewViewModel
            {
                CreatePageUrl = _newsService.GetCreatePage().Url,
                DetailsPageUrl = _newsService.GetDetailsPage().Url,
                Items = GetOverviewItems(news).OrderByDescending(item => item.PublishDate)
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

        protected new IEnumerable<NewsOverviewItemExtendedViewModel> GetOverviewItems(IEnumerable<NewsBase> news)
        {
            string detailsPageUrl = _newsService.GetDetailsPage().Url;
            foreach (var newsModelBase in news)
            {
                var overviewItemViewModel = newsModelBase.Map<NewsOverviewItemExtendedViewModel>();
                overviewItemViewModel.MediaIds = newsModelBase.MediaIds.Take(3).JoinToString(",");
                overviewItemViewModel.HeaderInfo = newsModelBase.Map<IntranetActivityItemHeaderViewModel>();
                overviewItemViewModel.HeaderInfo.DetailsPageUrl = detailsPageUrl.UrlWithQueryString("id", newsModelBase.Id.ToString());
                yield return overviewItemViewModel;
            }
        }
    }
}
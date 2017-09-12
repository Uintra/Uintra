using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.News.Entities;
using Compent.uIntra.Core.News.Models;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.News;
using uIntra.News.Web;
using uIntra.Search;

namespace Compent.uIntra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";        

        private readonly IDocumentIndexer _documentIndexer;

        public NewsController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IDocumentIndexer documentIndexer)
            : base(intranetUserService, newsService, mediaHelper, intranetUserContentHelper, activityTypeProvider)
        {
            _documentIndexer = documentIndexer;
        }

        public ActionResult CentralFeedItem(News item, ActivityLinks links)
        {
            var activity = item;

            var extendedModel = GetItemViewModel(activity, links).Map<NewsExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(News item, ActivityLinks links)
        {
            NewsPreviewViewModel viewModel = GetPreviewViewModel(item, links);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override NewsViewModel GetViewModel(NewsBase news)
        {
            var extendedNews = (News)news;
            var extendedModel = base.GetViewModel(news).Map<NewsExtendedViewModel>();
            extendedModel = Mapper.Map(extendedNews, extendedModel);
            return extendedModel;
        }

        protected override void DeleteMedia(IEnumerable<int> mediaIds)
        {
            base.DeleteMedia(mediaIds);
            _documentIndexer.DeleteFromIndex(mediaIds);
        }
    }
}
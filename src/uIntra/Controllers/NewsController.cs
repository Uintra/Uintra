using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.News.Models;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.News;
using uIntra.News.Web;
using uIntra.Search;
using uIntra.Users;

namespace uIntra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";
        protected override int ShortDescriptionLength { get; } = 500;

        private readonly IDocumentIndexer _documentIndexer;

        public NewsController(
            IIntranetUserService<IntranetUser> intranetUserService,
            INewsService<Core.News.Entities.News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IDocumentIndexer documentIndexer)
            : base(intranetUserService, newsService, mediaHelper, intranetUserContentHelper, activityTypeProvider)
        {
            _documentIndexer = documentIndexer;
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as Core.News.Entities.News;
            var extendedModel = GetItemViewModel(activity).Map<NewsExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        protected override NewsViewModel GetViewModel(NewsBase news)
        {
            var extendedNews = (Core.News.Entities.News)news;
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
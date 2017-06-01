using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.News.Entities;
using Compent.uIntra.Core.News.Models;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.News;
using uIntra.News.Web;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";
        protected override int ShortDescriptionLength { get; } = 500;

        public NewsController(
            IIntranetUserService<IntranetUser> intranetUserService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IPermissionsService permissionsService)
            : base(intranetUserService, newsService, mediaHelper, intranetUserContentHelper, permissionsService)
        {
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            var activity = item as News;
            var extendedModel = GetItemViewModel(activity).Map<NewsExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        protected override NewsViewModel GetViewModel(NewsBase news)
        {
            var extendedNews = (News)news;
            var extendedModel = base.GetViewModel(news).Map<NewsExtendedViewModel>();
            extendedModel = Mapper.Map(extendedNews, extendedModel);
            return extendedModel;
        }
    }
}
using System.Linq;
using System.Web.Mvc;
using uCommunity.CentralFeed;
using uCommunity.Core.Activity;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.News;
using uCommunity.News.Web;

namespace Compent.uCommunity.Controllers
{
    public class NewsController : NewsControllerBase
    {
        public NewsController(IIntranetUserService intranetUserService, INewsService<NewsBase, NewsModelBase> newsService, IMediaHelper mediaHelper) 
            : base(intranetUserService, newsService, mediaHelper)
        {
        }

        public ActionResult CentralFeedItem(ICentralFeedItem item)
        {
            FillLinks();
            var activity = item as NewsModelBase;

            var model = GetOverviewItems(Enumerable.Repeat(activity, 1)).Single();
            return PartialView("~/App_Plugins/News/List/ItemView.cshtml", model);
        }

    }
}
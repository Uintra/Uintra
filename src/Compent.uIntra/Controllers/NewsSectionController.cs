using System;
using System.Web.Http;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Navigation;
using uIntra.News;
using uIntra.News.Dashboard;

namespace Compent.uIntra.Controllers
{
    public class NewsSectionController : NewsSectionControllerBase
    {
        private readonly IMyLinksService _myLinksService;

        public NewsSectionController(INewsService<NewsBase> newsService, IIntranetUserService<IIntranetUser> intranetUserService, IMediaHelper mediaHelper, IMyLinksService myLinksService) 
            : base(newsService, intranetUserService, mediaHelper)
        {
            _myLinksService = myLinksService;
        }

        [HttpDelete]
        public override void Delete(Guid id)
        {
            base.Delete(id);
            _myLinksService.DeleteByActivityId(id);
        }
    }
}
using System;
using System.Web.Http;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Navigation;
using Uintra.News;
using Uintra.News.Dashboard;

namespace Compent.Uintra.Controllers
{
    public class NewsSectionController : NewsSectionControllerBase
    {
        private readonly IMyLinksService _myLinksService;

        public NewsSectionController(INewsService<NewsBase> newsService, IIntranetMemberService<IIntranetMember> intranetMemberService, IMediaHelper mediaHelper, IMyLinksService myLinksService) 
            : base(newsService, intranetMemberService, mediaHelper)
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
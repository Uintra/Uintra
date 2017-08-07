using System;
using System.Web.Http;
using uIntra.Bulletins;
using uIntra.Core.User;
using uIntra.Navigation;

namespace Compent.uIntra.Controllers
{
    public class BulletinsSectionController : BulletinsSectionControllerBase
    {
        private readonly IMyLinksService _myLinksService;
        public BulletinsSectionController(IBulletinsService<BulletinBase> bulletinsService, IIntranetUserService<IIntranetUser> intranetUserService, IMyLinksService myLinksService)
            : base(bulletinsService, intranetUserService)
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
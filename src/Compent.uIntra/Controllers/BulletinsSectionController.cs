using System;
using System.Web.Http;
using Uintra.Bulletins;
using Uintra.Core.User;
using Uintra.Navigation;

namespace Compent.Uintra.Controllers
{
    public class BulletinsSectionController : BulletinsSectionControllerBase
    {
        private readonly IMyLinksService _myLinksService;
        public BulletinsSectionController(IBulletinsService<BulletinBase> bulletinsService, IIntranetMemberService<IIntranetMember> intranetMemberService, IMyLinksService myLinksService)
            : base(bulletinsService, intranetMemberService)
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
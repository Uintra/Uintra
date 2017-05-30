using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.Core.User;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class BulletinsApiController : BulletinsBaseApiController
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IBulletinsService<Bulletin> _bulletinService;

        public BulletinsApiController(
            IIntranetUserService<IntranetUser> intranetUserService,
            IBulletinsService<Bulletin> bulletinService)
            : base(intranetUserService, bulletinService)
        {
            _intranetUserService = intranetUserService;
            _bulletinService = bulletinService;
        }
    }
}
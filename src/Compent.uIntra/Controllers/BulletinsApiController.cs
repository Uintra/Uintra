using Compent.uIntra.Core.Bulletins;
using uIntra.Bulletins;
using uIntra.Bulletins.Web;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class BulletinsApiController : BulletinsBaseApiController
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IBulletinsService<Bulletin> _bulletinService;
        private readonly IMediaHelper _mediaHelper;

        public BulletinsApiController(
            IIntranetUserService<IntranetUser> intranetUserService,
            IBulletinsService<Bulletin> bulletinService, 
            IMediaHelper mediaHelper)
            : base(intranetUserService, bulletinService, mediaHelper)
        {
            _intranetUserService = intranetUserService;
            _bulletinService = bulletinService;
            _mediaHelper = mediaHelper;
        }
    }
}